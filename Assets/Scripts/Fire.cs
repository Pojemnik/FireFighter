using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    private static float _fireStrengthRegainSpeed = 2f;

    [SerializeField]
    private float _maxFireStrength;
    private float _currentFireStrength;

    // Start is called before the first frame update
    void Start()
    {
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
            ParticleSystem thisParticleSystem = GetComponentInChildren<ParticleSystem>();
            thisParticleSystem.Stop();
        }

        
    }
}
