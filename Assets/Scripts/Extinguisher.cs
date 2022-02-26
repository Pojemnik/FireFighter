using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField]
    private ExtingiushingZone _extinguishingZone;

    private bool _extinguishing = false;

    private void Start()
    {
        if(_extinguishingZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
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
            Extinguish();
        }
    }

    public void Extinguish()
    {
        foreach(Fire fire in _extinguishingZone.FiresInZone)
        {
            fire.Extinguish();
        }
    }
}
