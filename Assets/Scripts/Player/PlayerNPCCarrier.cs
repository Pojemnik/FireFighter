using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerNPCCarrier : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private Vector3 _npcDropOffset;
    [SerializeField]
    private Vector3 _npcMeshOffset;
    [SerializeField]
    private Vector3 _boxHalfExtend;

    [HideInInspector]
    public bool IsCarrying { get => _isCarrying; }

    private bool _isCarrying = false;
    private NPCController _npc;
    private bool _testCollision = false;
    private Vector3 _placeTarget;
    private PlayerMovement _movement;

    private void Start()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    public void PickUpNPC(NPCController npc)
    {
        if (!_isCarrying)
        {
            _npc = npc;
            _npc.OnPickup();
            _isCarrying = true;
            _movement.SetCarrying(true);
        }
    }

    public void DropNPC()
    {
        if (_isCarrying && _npc.CanDrop)
        {
            _npc.OnDrop();
            _npc = null;
            _isCarrying = false;
            _movement.SetCarrying(false);
        }
    }

    public void SetOnFloor(bool state, Vector3 floorHit)
    {
        _testCollision = state;
        _npc.transform.position = floorHit + _npcMeshOffset.z * transform.forward + _npcMeshOffset.y * Vector3.up;
        _npc.transform.rotation = Quaternion.LookRotation(Vector3.up, transform.forward);
        if (state)
        {
            _placeTarget = floorHit + _npcDropOffset.z * transform.forward + _npcDropOffset.y * Vector3.up;
        }
        else
        {
            _npc.SetStatusHidden(true);
        }
    }

    private void LateUpdate()
    {
        if (_isCarrying && _testCollision)
        {
            CheckNpcCollision();
        }
    }

    private void CheckNpcCollision()
    {
        if(CheckFloorCollision())
        {
            Debug.Log("Collides with floor");
            if (IsPlaceClearToDrop())
            {
                if (IsPlaceSafe())
                {
                    Debug.Log("Safe place");
                    _npc.SetStatusSafe();
                }
                else
                {
                    Debug.Log("Unsafe place");
                    _npc.SetStatusUnsafe();
                }
            }
            else
            {
                _npc.SetStatusCantDrop();
                Debug.Log("Collides with something");
            }
        }
        else
        {
            _npc.SetStatusHidden(false);
            Debug.Log("Doesn't collide with floor");
        }
    }

    private bool CheckFloorCollision()
    {
        Collider[] floorOverlap = Physics.OverlapBox(_placeTarget, _boxHalfExtend, Quaternion.LookRotation(transform.forward, Vector3.up), LayerMask.GetMask("Environment"));
        if (floorOverlap.Length == 0)
        {
            return false;
        }
        else
        {
            foreach (Collider collider in floorOverlap)
            {
                if (collider.gameObject.CompareTag("Floor"))
                {
                    return true;
                }
            }
            return false;
        }
    }

    private bool IsPlaceClearToDrop()
    {
        Collider[] overlappingCollders = Physics.OverlapBox(_placeTarget, _boxHalfExtend, Quaternion.LookRotation(transform.forward, Vector3.up), LayerMask.GetMask("Environment", "Victims", "Destructible"));
        if(overlappingCollders.Length == 0)
        {
            return true;
        }
        else
        {
            foreach (Collider collider in overlappingCollders)
            {
                if (!collider.gameObject.CompareTag("Floor"))
                {
                    Debug.LogFormat("Colliding object: {0}", collider.gameObject.name);
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsPlaceSafe()
    {
        Collider[] safeOverlap = Physics.OverlapBox(_placeTarget, _boxHalfExtend, Quaternion.LookRotation(transform.forward, Vector3.up), LayerMask.GetMask("NPCDropZone"));
        return safeOverlap.Length > 0;
    }
}
