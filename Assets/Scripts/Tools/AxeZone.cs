using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeZone : MonoBehaviour
{
    [HideInInspector]
    public HashSet<DestructibleObject> ObjectsInZone { get; private set; }

    private void Awake()
    {
        ObjectsInZone = new HashSet<DestructibleObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        DestructibleObject obj = other.gameObject.GetComponent<DestructibleObject>();
        obj.ObjectDestroyed += c_onDestroy;
        if(obj == null)
        {
            return;
        }
        if (!ObjectsInZone.Add(obj))
        {
            Debug.LogErrorFormat("Destructible object {0} added twice to the axe zone", other.gameObject.name);
        }
        Debug.Log("Destructible object added to zone");
    }

    private void OnTriggerExit(Collider other)
    {
        DestructibleObject obj = other.gameObject.GetComponent<DestructibleObject>();
        obj.ObjectDestroyed -= c_onDestroy;
        if (obj == null)
        {
            return;
        }
        if (!ObjectsInZone.Remove(obj))
        {
            Debug.LogErrorFormat("Destructible object has {0} left the axe zone, but it was never inside", other.gameObject.name);
        }
        Debug.Log("Destructible object removed from zone");
    }

    private void c_onDestroy(object sender, EventArgs e)
    {
        ObjectsInZone.Remove((DestructibleObject)sender);
    }
}
