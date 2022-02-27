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
    private readonly struct ParticleSystemInformation
    {
        public float startSize { get; }
        public float startLifetime { get; }
        public float rateOverTime { get; }

        public ParticleSystemInformation(float startSize, float startLifeTime, float rateOverTime)
        {
            this.startSize = startSize;
            this.startLifetime = startLifeTime;
            this.rateOverTime = rateOverTime;
        }
    }

    private ParticleSystemInformation _particleSystemInitialInformation;
    private ParticleSystemInformation _particleSystemLethalInformation = new ParticleSystemInformation(0.6f, 0.3f, 20f);

    void Start()
    {
        _currentFireStrength = _maxFireStrength;

        _particleSystem = GetComponentInChildren<ParticleSystem>();
        var main =_particleSystem.main;
        var emission = _particleSystem.emission;

        _particleSystemInitialInformation = new ParticleSystemInformation(main.startSize.Evaluate(1), main.startLifetime.Evaluate(1), emission.rateOverTime.Evaluate(1));
    }

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
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(gameObject, 2);
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

        var main = _particleSystem.main;
        main.startSize = _particleSystemLethalInformation.startSize + (_particleSystemInitialInformation.startSize - _particleSystemLethalInformation.startSize) * (_currentFireStrength / _maxFireStrength);
        main.startLifetime = _particleSystemLethalInformation.startLifetime + (_particleSystemInitialInformation.startLifetime - _particleSystemLethalInformation.startLifetime) * (_currentFireStrength / _maxFireStrength);
        var emission = _particleSystem.emission;
        emission.rateOverTime = _particleSystemLethalInformation.rateOverTime + (_particleSystemInitialInformation.rateOverTime - _particleSystemLethalInformation.rateOverTime) * (_currentFireStrength / _maxFireStrength);
    }

    private void OnDestroy()
    {
        Debug.LogFormat("File {0} extingushed", gameObject.name);
    }
}
