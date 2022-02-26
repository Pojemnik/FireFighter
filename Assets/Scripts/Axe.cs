using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField]
    private AxeZone _axeZone;

    private void Start()
    {
        if (_axeZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            Use();
        }
    }    

    public void Use()
    {
        HashSet<DestructibleObject> objectsInZone = new HashSet<DestructibleObject>(_axeZone.ObjectsInZone);
        foreach (DestructibleObject destructible in objectsInZone)
        {
            Vector3 hitPoint = destructible.collider.ClosestPoint(transform.position);
            Vector3 normal = transform.position - hitPoint;
            destructible.Damage(hitPoint, normal);
        }
    }
}
