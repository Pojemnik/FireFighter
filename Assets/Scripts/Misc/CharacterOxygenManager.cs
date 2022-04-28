using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterOxygenManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _oxygenRegenerationSpeed = 5f;
    [SerializeField]
    private float _maxOxygen;
    [SerializeField]
    private float _maximumOxygenLoss = 10f;

    [Header("Events")]
    public UnityEvent m_EventDeath;
    public UnityEvent<float> m_EventChangeOxygenState;

    [HideInInspector]
    public HashSet<Smoke> SmokesWhichAffectPlayer { get; private set; }

    public float MaxOxygen { get => _maxOxygen; }
    private float _currentOxygen;

    private static float _radiusOfOxygenLoss = 9f;
    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        if (_maxOxygen > 0) 
        {
            isAlive = true;
        }

        SmokesWhichAffectPlayer = new HashSet<Smoke>();
        _currentOxygen = _maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return;
        }

        if(SmokesWhichAffectPlayer.Count == 0)
        {
            _currentOxygen = _currentOxygen + _oxygenRegenerationSpeed * Time.deltaTime > _maxOxygen
                ? _maxOxygen
                : _currentOxygen + _oxygenRegenerationSpeed * Time.deltaTime;
        }

        foreach(Smoke smoke in SmokesWhichAffectPlayer)
        {
            _currentOxygen -= Mathf.Clamp(smoke.DamageRate * (_radiusOfOxygenLoss - Vector3.Distance(transform.position, smoke.transform.position) * Time.deltaTime),
                                          0f,
                                          _maximumOxygenLoss * Time.deltaTime);
        }

        m_EventChangeOxygenState.Invoke(_currentOxygen);
        
        if (_currentOxygen <= 0f) 
        {
            m_EventDeath.Invoke();
            isAlive = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        Smoke smoke = other.gameObject.GetComponent<Smoke>();
        if(smoke == null)
        {
            return;
        }
        smoke.SourceFireExtinguished += c_onSmokeDestroyed;
        if (!SmokesWhichAffectPlayer.Add(smoke))
        {
            Debug.LogErrorFormat("Smoke {0} added twice to the zone of smokes which affect character", other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Smoke smoke = other.gameObject.GetComponent<Smoke>();
        if (smoke == null)
        {
            return;
        }
        if (!SmokesWhichAffectPlayer.Remove(smoke))
        {
            Debug.LogErrorFormat("Smoke has {0} left the zone of smokes affected by character, but it was never inside", other.gameObject.name);
        }
        smoke.SourceFireExtinguished -= c_onSmokeDestroyed;
    }

    private void c_onSmokeDestroyed(object sender, EventArgs e)
    {
        SmokesWhichAffectPlayer.Remove((Smoke) sender);
        ((Smoke)sender).SourceFireExtinguished -= c_onSmokeDestroyed;
    }
}
