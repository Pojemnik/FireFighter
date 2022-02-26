using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOxygenManager : MonoBehaviour
{

    private static float _speedOfLoosingOxygen = 1f;
    private static float _speedOfRegainingOxygen = 5f;

    private static float _maximumOxygenLoss = 3f;
    private static float _radiusOfOxygenLoss = 9f;

    [HideInInspector]
    public HashSet<Fire> FiresWhichAffectPlayer { get; private set; }

    [SerializeField]
    private float _maxOxygen;

    private float _currentOxygen;

    // Start is called before the first frame update
    void Start()
    {
        FiresWhichAffectPlayer = new HashSet<Fire>();
        _currentOxygen = _maxOxygen;
    }

    // Update is called once per frame
    void Update()
    {
        if(FiresWhichAffectPlayer.Count == 0)
        {
            _currentOxygen = _currentOxygen + _speedOfRegainingOxygen * Time.deltaTime > _maxOxygen
                ? _maxOxygen
                : _currentOxygen + _speedOfRegainingOxygen * Time.deltaTime;
        }

        foreach(Fire fire in FiresWhichAffectPlayer)
        {
            _currentOxygen -= Mathf.Clamp(_speedOfLoosingOxygen * (_radiusOfOxygenLoss - Vector3.Distance(transform.position, fire.transform.position) * Time.deltaTime),
                                          0f,
                                          _maximumOxygenLoss * Time.deltaTime);
            Debug.LogFormat("Oxygen of the player {0} out of {1} after loosing it", _currentOxygen, _maxOxygen);
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
            Debug.LogErrorFormat("Fire {0} added twice to the zone of fires affected by player", other.gameObject.name);
        }
        Debug.Log("Fire affected a Player");
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
            Debug.LogErrorFormat("Fire has {0} left the zone of fires affected by player, but it was never inside", other.gameObject.name);
        }
        fire.FireExtinguished -= c_onDestroy;
        Debug.Log("Fire removed from fires by which player is affected");
    }

    private void c_onDestroy(object sender, EventArgs e)
    {
        FiresWhichAffectPlayer.Remove((Fire)sender);
        ((Fire)sender).FireExtinguished -= c_onDestroy;
    }
}
