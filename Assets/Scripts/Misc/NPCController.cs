using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOxygenManager))]
public class NPCController : MonoBehaviour
{
    private NPCManager _manager;
    private CharacterOxygenManager _oxygen;
    private bool _safeToDrop = false;
    private bool _dead;

    public UnityEngine.Events.UnityEvent NPCSaved;

    private void Start()
    {
        _manager = FindObjectOfType<NPCManager>();
        _oxygen = GetComponent<CharacterOxygenManager>();
        _manager.AddLivingNPC();
        _oxygen.m_EventDeath.AddListener(OnDeath);
    }

    private void OnDeath()
    {
        _manager.OnNPCDeath();
        gameObject.tag = "Untagged";
        _dead = true;

    }

    public void OnPickup()
    {
        SetChildernActive(false);
    }

    private void SetChildernActive(bool active)
    {
        int i = 0;
        while (i < transform.childCount)
        {
            transform.GetChild(i).gameObject.SetActive(active);
            i++;
        }
    }


    public void SaveNPC()
    {
        _manager.OnNPCSaved();
        NPCSaved.Invoke();
    }

    public void OnDrop()
    {
        SetChildernActive(true);
        if (_safeToDrop && !_dead)
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
