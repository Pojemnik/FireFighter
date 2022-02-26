using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private ExtingiushingZone _extinguishingZone;

    [Header("Fuel")]
    [SerializeField]
    private float _maxFuel;
    [SerializeField]
    private float _fuelUsage;

    public UnityEngine.Events.UnityEvent<float> FuelPercentageUpdatedEvent;

    private bool _extinguishing = false;
    private float _currentFuel;


    private void Start()
    {
        if (_extinguishingZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
        _currentFuel = _maxFuel;
        FuelPercentageUpdatedEvent.Invoke(100);
    }

    public void RefillFuel()
    {
        _currentFuel = _maxFuel;
        FuelPercentageUpdatedEvent.Invoke(100);
    }

    public void StartExtinguishing()
    {
        _extinguishing = true;
    }

    public void StopExtingiushing()
    {
        _extinguishing = false;
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
            if(_extinguishingZone.ChargerInZone)
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
            fire.Extinguish();
        }
    }
}
