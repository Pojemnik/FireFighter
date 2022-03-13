using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeZone : MonoBehaviour
{
    [HideInInspector]
    public HashSet<DestructibleObject> DestructibleObjectsInZone { get; private set; }
    [HideInInspector]
    public bool OtherObjectsInZone { get => _otherObjectsCount != 0; }

    private int _otherObjectsCount = 0;

    private void Awake()
    {
        DestructibleObjectsInZone = new HashSet<DestructibleObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        DestructibleObject obj = other.gameObject.GetComponent<DestructibleObject>();
        if (obj == null)
        {
            _otherObjectsCount++;
            return;
        }
        obj.ObjectDestroyed += c_onDestroy;
        if (!DestructibleObjectsInZone.Add(obj))
        {
            Debug.LogErrorFormat("Destructible object {0} added twice to the axe zone", other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DestructibleObject obj = other.gameObject.GetComponent<DestructibleObject>();
        if (obj == null)
        {
            _otherObjectsCount--;
            if (_otherObjectsCount < 0)
            {
                Debug.LogError("Less than zero other objects in the axe zone. This sould never hapen");
            }
            return;
        }
        obj.ObjectDestroyed -= c_onDestroy;
        if (!DestructibleObjectsInZone.Remove(obj))
        {
            Debug.LogErrorFormat("Destructible object has {0} left the axe zone, but it was never inside", other.gameObject.name);
        }
    }

    private void c_onDestroy(object sender, EventArgs e)
    {
        DestructibleObjectsInZone.Remove((DestructibleObject)sender);
    }
}
