using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AxeSoundPlayer))]
public class Axe : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private AxeZone _axeZone;

    private AxeSoundPlayer _soundPlayer;

    private void Awake()
    {
        if (_axeZone == null)
        {
            throw new System.NullReferenceException("No extinguishing zone in fire extinguisher");
        }
        _soundPlayer = GetComponent<AxeSoundPlayer>();
        if(_soundPlayer == null)
        {
            Debug.LogError("No sound player component in the axe");
        }
    }   

    public void Use()
    {
        HashSet<DestructibleObject> objectsInZone = new HashSet<DestructibleObject>(_axeZone.DestructibleObjectsInZone);
        foreach (DestructibleObject destructible in objectsInZone)
        {
            Vector3 hitPoint = destructible.collider.ClosestPoint(transform.position);
            Vector3 normal = transform.position - hitPoint;
            destructible.Damage(hitPoint, normal);
        }
        if (objectsInZone.Count > 0 || _axeZone.OtherObjectsInZone)
        {
            _soundPlayer.PlayHitSound();
        }
    }


}
