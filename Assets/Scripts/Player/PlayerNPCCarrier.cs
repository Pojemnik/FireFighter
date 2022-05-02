using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCCarrier : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private Vector3 _npcDropOffset;
    [SerializeField]
    private Vector3 _boxHalfExtend;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent<bool> TargetStateChanged;

    [HideInInspector]
    public bool IsCarrying { get => _isCarrying; }

    private bool _isCarrying = false;
    private NPCController _npc;
    private bool _testCollision = false;
    private Vector3 _placeTarget;

    public void PickUpNPC(NPCController npc)
    {
        if (!_isCarrying)
        {
            _npc = npc;
            _npc.gameObject.SetActive(false);
            //_npc.OnPickup();
            //_npc.transform.parent = transform;
            _isCarrying = true;
            TargetStateChanged.Invoke(true);
        }
    }

    public void DropNPC()
    {
        if (_isCarrying && _npc.CanDrop)
        {
            _npc.gameObject.SetActive(true);
            //_npc.OnDrop();
            //_npc.transform.parent = null;
            _npc = null;
            _isCarrying = false;
            TargetStateChanged.Invoke(false);
        }
    }

    private void Update()
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
                Debug.Log("Doesn't collide with anyhing");
            }
            else
            {
                Debug.Log("Collides with something");
            }
        }
        else
        {
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
        return false;
    }

    public void SetOnFloor(bool state, Vector3 floorHit)
    {
        _testCollision = state;
        if (state)
        {
            _placeTarget = floorHit + _npcDropOffset.z * transform.forward + _npcDropOffset.y * Vector3.up;
        }
        //else
        //{
        //    //_npc.Hide();
        //}
    }
}
