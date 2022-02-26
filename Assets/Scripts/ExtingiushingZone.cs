using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtingiushingZone : MonoBehaviour
{
    [HideInInspector]
    public HashSet<Fire> FiresInZone { get; private set; }

    private void Awake()
    {
        FiresInZone = new HashSet<Fire>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        fire.FireExtinguished += c_onDestroy;
        if(fire == null)
        {
            return;
        }
        if (!FiresInZone.Add(fire))
        {
            Debug.LogErrorFormat("Fire {0} added twice to the extinguishing zone", other.gameObject.name);
        }
        Debug.Log("Fire added to zone");
    }

    private void OnTriggerExit(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        if (fire == null)
        {
            return;
        }
        if (!FiresInZone.Remove(fire))
        {
            Debug.LogErrorFormat("Fire has {0} left the extinguishing zone, but it was never inside", other.gameObject.name);
        }
        Debug.Log("Fire removed from zone");
    }

    private void c_onDestroy(object sender, EventArgs e)
    {
        FiresInZone.Remove((Fire)sender);
    }
}
