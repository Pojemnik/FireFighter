using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterOxygenManager))]
[RequireComponent(typeof(Rigidbody))]
public class NPCController : MonoBehaviour
{
    [SerializeField]
    private NPCMaterialManager _materialManager;

    private NPCManager _manager;
    private NPCInteractionLabelController _labelController;
    private CharacterOxygenManager _oxygen;
    private bool _safeToDrop = false;
    private bool _dead;
    private Rigidbody _rb;

    public UnityEngine.Events.UnityEvent NPCSaved;

    private void Start()
    {
        _manager = FindObjectOfType<NPCManager>();
        if(_manager == null)
        {
            Debug.LogError("No NPC manager found in current scene. NPCs would not work correctly");
        }
        _labelController = FindObjectOfType<NPCInteractionLabelController>();
        if (_labelController == null)
        {
            Debug.LogError("No NPC label controller found in current scene. UI would not work correctly");
        }
        if(_materialManager == null)
        {
            Debug.LogErrorFormat("No NPC material manager found in NPC {0}", gameObject.name);
        }
        _materialManager.FadeOutEnd += (_, _) => Destroy(gameObject);
        _oxygen = GetComponent<CharacterOxygenManager>();
        _rb = GetComponent<Rigidbody>();
        _manager.AddLivingNPC();
        _oxygen.m_EventDeath.AddListener(OnDeath);
    }

    private void OnDeath()
    {
        _manager.OnNPCDeath();
        _materialManager.OnDeath();
        gameObject.tag = "Untagged";
        _dead = true;
        Debug.LogFormat("NPC {0} died", gameObject.name);
    }

    public void OnPickup()
    {
        gameObject.layer = LayerMask.NameToLayer("CarriedVictim");
        _materialManager.OnPickupStateChange(true);
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _labelController.OnSafeZoneContainmentChange(_safeToDrop);
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
        _materialManager.OnSafe();
        NPCSaved.Invoke();
    }

    public void OnDrop()
    {
        gameObject.layer = LayerMask.NameToLayer("Victims");
        _materialManager.OnPickupStateChange(false);
        _rb.isKinematic = false;
        _rb.useGravity = true;
        if (_safeToDrop && !_dead)
        {
            SaveNPC();
            gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("NPCDropZone"))
        {
            _safeToDrop = true;
            _labelController.OnSafeZoneContainmentChange(true);
            _materialManager.OnSafeStateChange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NPCDropZone"))
        {
            _safeToDrop = false;
            _labelController.OnSafeZoneContainmentChange(false);
            _materialManager.OnSafeStateChange(false);
        }
    }
}
