using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNPCCarrier : MonoBehaviour
{
    [Header("Events")]
    public UnityEngine.Events.UnityEvent<bool> TargetStateChanged;

    [HideInInspector]
    public bool IsCarrying { get => _isCarrying; }

    private bool _isCarrying = false;
    private NPCController _npc;
    private Rigidbody _npcRB;

    public void PickUpNPC(NPCController npc)
    {
        if (!_isCarrying)
        {
            _npc = npc;
            _npc.OnPickup();
            _isCarrying = true;
            _npcRB = _npc.GetComponent<Rigidbody>();
            _npcRB.isKinematic = true;
            _npcRB.useGravity = false;
            _npc.transform.parent = transform;
            TargetStateChanged.Invoke(true);
        }
    }

    public void DropNPC()
    {
        if(_isCarrying)
        {
            _npc.OnDrop();
            _npc.transform.parent = null;
            _npcRB.isKinematic = false;
            _npcRB.useGravity = true;
            _npc = null;
            _isCarrying = false;
            TargetStateChanged.Invoke(false);
        }
    }
}
