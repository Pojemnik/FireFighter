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

    public void PickUpNPC(NPCController npc)
    {
        if (!_isCarrying)
        {
            _npc = npc;
            _isCarrying = true;
            _npc.transform.parent = transform;
            TargetStateChanged.Invoke(true);
        }
    }

    public void DropNPC()
    {
        if(_isCarrying)
        {
            _npc.transform.parent = null;
            _npc = null;
            _isCarrying = false;
            TargetStateChanged.Invoke(false);
        }
    }
}
