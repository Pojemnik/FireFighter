using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private static float _fireStrengthRegainSpeed = 2f;
    private static float _fireStrengthLoseSpeed = 4f;

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
        if (_currentFireStrength < _maxFireStrength) 
        {
            _currentFireStrength += Time.deltaTime * _fireStrengthRegainSpeed;
        }
        if (_currentFireStrength > _maxFireStrength) 
        {
            _currentFireStrength = _maxFireStrength;
        }
        if (_currentFireStrength <= 0) 
        {
            _particleSystem.Stop();
        }

    }

    public void Extinguish() {
        Debug.LogErrorFormat("Tried to extinguish fire {0}, {1}", _currentFireStrength, _maxFireStrength);
        _currentFireStrength -= _fireStrengthLoseSpeed * Time.deltaTime;
    }

}
