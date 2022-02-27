using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public enum FireState 
    {
        Large,
        Medium,
        Small
    }

    public event EventHandler FireExtinguished;
    public event EventHandler FireStateChanged;

    private static float _fireStrengthRegainSpeed = 2f;
    private static float _fireStrengthLoseSpeed = 10f;

    [SerializeField]
    private float _damageRate = 2f;
    public float InitialDamageRate {get {return _damageRate;}}

    [SerializeField]
    private float _maxFireStrength;
    private float _currentFireStrength;

    private ParticleSystem _particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        this._particleSystem = GetComponentInChildren<ParticleSystem>();
        _currentFireStrength = _maxFireStrength;
    }

    // Update is called once per frame
    void Update()
    {
        float fireStrengthFromBeginningOfFrame = _currentFireStrength;

        if (_currentFireStrength < _maxFireStrength) 
        {
            _currentFireStrength += Time.deltaTime * _fireStrengthRegainSpeed;
        }
        if (_currentFireStrength > _maxFireStrength) 
        {
            _currentFireStrength = _maxFireStrength;
        }

        checkForFireStateChanged(fireStrengthFromBeginningOfFrame);

        if (_currentFireStrength <= 0) 
        {
            OnFireExtinguished(null);
            Destroy(gameObject);   
        }

    }

    public void Extinguish() 
    {
        Debug.LogFormat("Tried to extinguish fire {0}, {1}", _currentFireStrength, _maxFireStrength);
        float fireStrengthFromBeforeUpdate = _currentFireStrength;
        _currentFireStrength -= _fireStrengthLoseSpeed * Time.deltaTime;
        checkForFireStateChanged(fireStrengthFromBeforeUpdate);
    }

    protected virtual void OnFireExtinguished(EventArgs e)
    {
        EventHandler handler = FireExtinguished;
        handler?.Invoke(this, e);
    }

    protected virtual void OnFireStateChanged(EventArgs e)
    {
        EventHandler handler = FireStateChanged;
        handler?.Invoke(this, e);
    }

    public class FireStateChangedEventArgs : EventArgs
    {
        public FireState fireState { get; set; }
    }

    private void checkForFireStateChanged(float fireStrengthFromStartOfUpdating) 
    {
        FireStateChangedEventArgs fireStateChangedEventArgs = new FireStateChangedEventArgs();
        if (fireStrengthFromStartOfUpdating > 0.7 * _maxFireStrength && _currentFireStrength <= 0.7 * _maxFireStrength) 
        {
            fireStateChangedEventArgs.fireState = FireState.Medium;
            OnFireStateChanged(fireStateChangedEventArgs);
        }
        else if (fireStrengthFromStartOfUpdating <= 0.7 * _maxFireStrength && _currentFireStrength > 0.7 * _maxFireStrength)
        {
            fireStateChangedEventArgs.fireState = FireState.Large;
            OnFireStateChanged(fireStateChangedEventArgs);
        }
        else if (fireStrengthFromStartOfUpdating > 0.4 * _maxFireStrength && _currentFireStrength <= 0.4 * _maxFireStrength)
        {
            fireStateChangedEventArgs.fireState = FireState.Small;
            OnFireStateChanged(fireStateChangedEventArgs);
        }
        else if (fireStrengthFromStartOfUpdating <= 0.4 * _maxFireStrength && _currentFireStrength > 0.4 * _maxFireStrength)
        {
            fireStateChangedEventArgs.fireState = FireState.Medium;
            OnFireStateChanged(fireStateChangedEventArgs);
        }
    }

}
