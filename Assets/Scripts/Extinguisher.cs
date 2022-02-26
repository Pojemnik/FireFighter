using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField]
    private ExtingiushingZone _extinguishingZone;

    private void Start()
    {
        if(_extinguishingZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
    }

    void Update()
    {
        this.Extinguish();
    }

    public void Extinguish()
    {
        foreach(Fire fire in _extinguishingZone.FiresInZone)
        {
            fire.Extinguish();
        }
    }
}
