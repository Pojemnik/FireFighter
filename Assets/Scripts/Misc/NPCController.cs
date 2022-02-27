using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOxygenManager))]
public class NPCController : MonoBehaviour
{
    private NPCManager _manager;
    private CharacterOxygenManager _oxygen;

    private void Start()
    {
        _manager = FindObjectOfType<NPCManager>();
        _oxygen = GetComponent<CharacterOxygenManager>();
        _manager.AddLivingNPC();
        _oxygen.m_EventDeath.AddListener(_manager.OnNPCDeath);
    }

    public void SaveNPC()
    {
        _manager.OnNPCSaved();
    }
}
