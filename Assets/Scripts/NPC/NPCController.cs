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
    private Rigidbody _rb;

    public UnityEngine.Events.UnityEvent NPCSaved;
    public bool CanDrop { get => _status == NpcStatus.SafeToDrop || _status == NpcStatus.CanDrop; }

    public enum NpcStatus
    {
        SafeToDrop,
        CanDrop,
        CantDrop,
        Hidden,
        Dropped,
        Dead,
        Saved
    }

    public NpcStatus Status { get => _status; }
    private NpcStatus _status = NpcStatus.Hidden;

    private void Start()
    {
        CheckComponentsAndReferences();
        _materialManager.FadeOutEnd += (_, _) => Destroy(gameObject);
        _oxygen = GetComponent<CharacterOxygenManager>();
        _rb = GetComponent<Rigidbody>();
        _manager.AddLivingNPC();
        _oxygen.m_EventDeath.AddListener(OnDeath);
    }

    private void CheckComponentsAndReferences()
    {
        _manager = FindObjectOfType<NPCManager>();
        if (_manager == null)
        {
            Debug.LogError("No NPC manager found in current scene. NPCs would not work correctly");
        }
        _labelController = FindObjectOfType<NPCInteractionLabelController>();
        if (_labelController == null)
        {
            Debug.LogError("No NPC label controller found in current scene. UI would not work correctly");
        }
        if (_materialManager == null)
        {
            Debug.LogErrorFormat("No NPC material manager found in NPC {0}", gameObject.name);
        }
    }

    private void OnDeath()
    {
        _manager.OnNPCDeath();
        gameObject.tag = "Untagged";
        _status = NpcStatus.Dead;
        _materialManager.SetStatus(_status);
        Debug.LogFormat("NPC {0} died", gameObject.name);
    }

    public void OnPickup()
    {
        gameObject.layer = LayerMask.NameToLayer("CarriedVictim");
        _rb.isKinematic = true;
        _rb.useGravity = false;
        _labelController.OnSafeZoneContainmentChange(false);
        _status = NpcStatus.Hidden;
        _materialManager.SetStatus(_status);
    }

    public void SaveNPC()
    {
        _manager.OnNPCSaved();
        _status = NpcStatus.Saved;
        _materialManager.SetStatus(_status);
        NPCSaved.Invoke();
    }

    public void OnDrop()
    {
        gameObject.layer = LayerMask.NameToLayer("Victims");
        _rb.isKinematic = false;
        _rb.useGravity = true;
        if (_status == NpcStatus.SafeToDrop)
        {
            SaveNPC();
        }
        else
        {
            _status = NpcStatus.Dropped;
            _materialManager.SetStatus(_status);
        }
    }

    public void SetStatusSafe()
    {
        _status = NpcStatus.SafeToDrop;
        _labelController.OnCanDropStateChange(true);
        _labelController.OnSafeZoneContainmentChange(true);
        _materialManager.SetStatus(_status);
    }

    public void SetStatusUnsafe()
    {
        _status = NpcStatus.CanDrop;
        _labelController.OnCanDropStateChange(true);
        _labelController.OnSafeZoneContainmentChange(false);
        _materialManager.SetStatus(_status);
    }

    public void SetStatusHidden()
    {
        _status = NpcStatus.Hidden;
        _labelController.OnSafeZoneContainmentChange(false);
        _labelController.OnCanDropStateChange(false);
        _materialManager.SetStatus(_status);
    }

    public void SetStatusCantDrop()
    {
        _status = NpcStatus.CantDrop;
        _labelController.OnSafeZoneContainmentChange(false);
        _labelController.OnCanDropStateChange(false);
        _materialManager.SetStatus(_status);
    }
}
