using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{

    public event EventHandler SourceFireExtinguished;

    private float _smokeDamageRate;
    public float DamageRate {get {return _smokeDamageRate;}}
    private Fire _fire;
    // Start is called before the first frame update
    void Start()
    {
        _fire = gameObject.GetComponentInParent<Fire>();
        _smokeDamageRate = _fire.InitialDamageRate;

        _fire.FireStateChanged += c_onFireStateChanged;
        _fire.FireExtinguished += c_onSourceFireExtinguished;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void c_onFireStateChanged(object sender, EventArgs eventArgs)
    {
        Fire.FireState newFireState = ((Fire.FireStateChangedEventArgs)eventArgs).fireState;
        switch (newFireState)
        {
            case Fire.FireState.Small:
                _smokeDamageRate = _fire.InitialDamageRate / 3;
                break;
            case Fire.FireState.Medium:
                _smokeDamageRate = _fire.InitialDamageRate * 2 / 3;
                break;
            case Fire.FireState.Large:
                _smokeDamageRate = _fire.InitialDamageRate;
                break;
        }

        Debug.LogFormat("Fire state changed: {0}, dmg: {1}", newFireState, _smokeDamageRate);
    }

    private void c_onSourceFireExtinguished(object sender, EventArgs e)
    {
        OnFireExtinguished(null);
    }

    protected virtual void OnFireExtinguished(EventArgs e)
    {
        EventHandler handler = SourceFireExtinguished;
        handler?.Invoke(this, e);
        _fire.FireStateChanged -= c_onFireStateChanged;
        _fire.FireExtinguished -= c_onSourceFireExtinguished;
    }
    
}
