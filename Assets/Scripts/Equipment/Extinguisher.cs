using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Extinguisher : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private ExtingiushingZone _extinguishingZone;
    [SerializeField]
    private ParticleSystem _particles;
    [SerializeField]
    private GameObject _forceField;

    [Header("Fuel")]
    [SerializeField]
    private float _maxFuel;
    [SerializeField]
    private float _fuelUsage;

    private AudioSource _audioSource;

    public UnityEngine.Events.UnityEvent<float> FuelPercentageUpdatedEvent;

    private bool _extinguishing = false;
    private float _currentFuel;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_extinguishingZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
        _currentFuel = _maxFuel;
        FuelPercentageUpdatedEvent.Invoke(100);
        _particles.Stop();
        _audioSource.Stop();
        _forceField.SetActive(false);
    }

    public void RefillFuel()
    {
        _currentFuel = _maxFuel;
        FuelPercentageUpdatedEvent.Invoke(100);
    }

    public void StartExtinguishing()
    {
        _extinguishing = true;
        if (_currentFuel > 0)
        {
            _particles.Play();
            _audioSource.Play();
            _forceField.SetActive(true);
        }
    }

    public void StopExtingiushing()
    {
        _extinguishing = false;
        _particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _forceField.SetActive(false);
        _audioSource.Stop();
    }

    void Update()
    {
        if (_extinguishing)
        {
            if (_currentFuel > 0)
            {
                Extinguish();
                UpdateFuelLevel();
            }
            else
            {
                StopExtingiushing();
            }
            if (_extinguishingZone.ChargerInZone)
            {
                RefillFuel();
            }
        }
    }

    private void UpdateFuelLevel()
    {
        _currentFuel -= _fuelUsage * Time.deltaTime;
        if (_currentFuel < 0)
        {
            _currentFuel = 0;
        }
        FuelPercentageUpdatedEvent.Invoke((_currentFuel / _maxFuel) * 100f);
    }

    public void Extinguish()
    {
        foreach (Fire fire in _extinguishingZone.FiresInZone)
        {
            Vector3 raycastDirection = fire.transform.position - transform.position;
            Debug.DrawLine(transform.position, fire.transform.position, Color.magenta);
            if (!Physics.Raycast(transform.position, raycastDirection, raycastDirection.magnitude, LayerMask.GetMask("Environment")))
            {
                fire.Extinguish();
            }
        }
    }
}
