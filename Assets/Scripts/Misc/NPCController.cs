using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOxygenManager))]
public class NPCController : MonoBehaviour
{
    private NPCManager _manager;
    private CharacterOxygenManager _oxygen;
    private bool _safeToDrop = false;

    public UnityEngine.Events.UnityEvent NPCSaved;

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
        NPCSaved.Invoke();
    }

    public void OnDrop()
    {
        if(_safeToDrop)
        {
            SaveNPC();
            gameObject.tag = "Untagged";
            Destroy(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("NPCDropZone"))
        {
            _safeToDrop = true;
            Debug.Log("Safe to drop");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NPCDropZone"))
        {
            _safeToDrop = false;
            Debug.Log("Not safe to drop");
        }
    }
}
