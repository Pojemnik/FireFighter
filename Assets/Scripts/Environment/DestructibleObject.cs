using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth;

    [Header("Config")]
    [SerializeField]
    private List<GameObject> _damageLevels;
    [SerializeField]
    private GameObject _fragments;
    [SerializeField]
    private GameObject _hitParticles;
    [SerializeField]
    private GameObject _model;

    private int _currentHealth;

    public event System.EventHandler ObjectDestroyed;

    [HideInInspector]
    public Collider collider;

    private void Start()
    {
        _currentHealth = _maxHealth;
        collider = GetComponent<Collider>();
    }

    public void Damage(Vector3 hitPosition, Vector3 hitNormal)
    {
        _currentHealth--;
        Instantiate(_hitParticles, hitPosition, Quaternion.LookRotation(hitNormal));
        if (_currentHealth == 0)
        {
            Instantiate(_fragments, transform.position, transform.rotation);
            ObjectDestroyed?.Invoke(this, null);
            Destroy(gameObject);
        }
        else
        {
            if (_damageLevels.Count >= _currentHealth)
            {
                ChangeModel(_damageLevels[_currentHealth - 1]);
            }
        }
    }

    private void ChangeModel(GameObject newPrefab)
    {
        Destroy(_model);
        _model = Instantiate(newPrefab, transform);
    }
}
