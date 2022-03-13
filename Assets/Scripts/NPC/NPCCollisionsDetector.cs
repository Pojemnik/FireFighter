using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NPCCollisionsDetector : MonoBehaviour
{
    public event System.EventHandler<bool> CollisionStatusChanged;

    private int _collisionsCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor"))
        {
            _collisionsCount++;
            CollisionStatusChanged?.Invoke(this, true);
            Debug.Log("Collides with something");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Floor"))
        {
            _collisionsCount--;
            if (_collisionsCount == 0)
            {
                CollisionStatusChanged?.Invoke(this, false);
                Debug.Log("Collides with floor only");
            }
        }
    }
}
