using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterOxygenManager : MonoBehaviour
{

    private static float _speedOfRegainingOxygen = 5f;

    private static float _maximumOxygenLoss = 3f;
    private static float _radiusOfOxygenLoss = 9f;

    public UnityEvent m_EventDeath;
    public UnityEvent<float> m_EventChangeOxygenState;

    [HideInInspector]
    public HashSet<Fire> FiresWhichAffectPlayer { get; private set; }

    [SerializeField]
    private float _maxOxygen;
    private float _currentOxygen;

    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        if (_maxOxygen > 0) 
        {
            isAlive = true;
        }

        FiresWhichAffectPlayer = new HashSet<Fire>();
        _currentOxygen = _maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {
            return;
        }

        if(FiresWhichAffectPlayer.Count == 0)
        {
            _currentOxygen = _currentOxygen + _speedOfRegainingOxygen * Time.deltaTime > _maxOxygen
                ? _maxOxygen
                : _currentOxygen + _speedOfRegainingOxygen * Time.deltaTime;
        }

        foreach(Fire fire in FiresWhichAffectPlayer)
        {
            _currentOxygen -= Mathf.Clamp(fire.DamageRate * (_radiusOfOxygenLoss - Vector3.Distance(transform.position, fire.transform.position) * Time.deltaTime),
                                          0f,
                                          _maximumOxygenLoss * Time.deltaTime);
            Debug.LogFormat("Oxygen of the character {0}: {1} out of {2} after loosing it", gameObject.name, _currentOxygen, _maxOxygen);
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
        Fire fire = other.gameObject.GetComponent<Fire>();
        fire.FireExtinguished += c_onDestroy;
        if(fire == null)
        {
            return;
        }
        if (!FiresWhichAffectPlayer.Add(fire))
        {
            Debug.LogErrorFormat("Fire {0} added twice to the zone of fires which affect character", other.gameObject.name);
        }
        Debug.Log("Fire affected a Character");
    }

    private void OnTriggerExit(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        if (fire == null)
        {
            return;
        }
        if (!FiresWhichAffectPlayer.Remove(fire))
        {
            Debug.LogErrorFormat("Fire has {0} left the zone of fires affected by character, but it was never inside", other.gameObject.name);
        }
        fire.FireExtinguished -= c_onDestroy;
        Debug.Log("Fire removed from fires by which character is affected");
    }

    private void c_onDestroy(object sender, EventArgs e)
    {
        FiresWhichAffectPlayer.Remove((Fire)sender);
        ((Fire)sender).FireExtinguished -= c_onDestroy;
    }
}
