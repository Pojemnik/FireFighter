using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private NPCStatusController _statusDisplay;
    [SerializeField]
    private UIBarController _bar;
    [SerializeField]
    private GameObject _npc;

    private void Start()
    {
        if(!CheckReferences())
        {
            return;
        }
        CharacterOxygenManager oxygen = _npc.GetComponent<CharacterOxygenManager>();
        if(oxygen == null)
        {
            Debug.LogErrorFormat("No oxygen manager found in npc {0}", _npc.name);
            return;
        }
        _bar.MaxValue = oxygen.MaxOxygen;
        oxygen.m_EventChangeOxygenState.AddListener(OnOxygenChange);
        oxygen.m_EventDeath.AddListener(OnNPCDeath);
        NPCController npcController = _npc.GetComponent<NPCController>();
        if (npcController == null)
        {
            Debug.LogErrorFormat("No NPC controller found in npc {0}", _npc.name);
            return;
        }
        npcController.NPCSaved.AddListener(OnNPCSaved);

    }

    private bool CheckReferences()
    {
        if (_npc == null)
        {
            Debug.LogErrorFormat("No NPC reference in NPCUIManager {0}", gameObject.name);
            return false;
        }
        if (_bar == null)
        {
            Debug.LogErrorFormat("No oxygen bar reference in NPCUIManager {0}", gameObject.name);
            return false;
        }
        if (_statusDisplay == null)
        {
            Debug.LogErrorFormat("No NPC status display reference in NPCUIManager {0}", gameObject.name);
            return false;
        }
        return true;
    }

    private void OnOxygenChange(float value)
    {
        _bar.OnValueChange(value);
    }

    private void OnNPCDeath()
    {
        _bar.gameObject.SetActive(false);
        _statusDisplay.OnDeath();
    }

    private void OnNPCSaved()
    {
        _bar.gameObject.SetActive(false);
        _statusDisplay.OnSaved();
    }
}
