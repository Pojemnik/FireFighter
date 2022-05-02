using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private PlayerNPCCarrier _carrier;
    [SerializeField]
    private NPCInteractionLabelController _labelController;

    [Header("Config")]
    [SerializeField]
    private float _interactionRange;

    private NPCController _targetNPC = null;

    //That's ugly
    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactionRange, LayerMask.GetMask("Victims")))
        {
            if (hit.transform.gameObject.CompareTag("NPC"))
            {
                _targetNPC = hit.transform.gameObject.GetComponent<NPCController>();
                if (_targetNPC == null)
                {
                    if (!_carrier.IsCarrying)
                    {
                        _labelController.SetLabelStatus(NPCInteractionLabelController.NpcLabelEnum.NotAvailable);
                    }
                    Debug.LogErrorFormat("NPC {0} is tagged correctly, but it doesn't have NPCController", hit.transform.gameObject.name);
                }
                else
                {
                    if (!_carrier.IsCarrying)
                    {
                        _labelController.SetLabelStatus(NPCInteractionLabelController.NpcLabelEnum.CanPickUp);
                    }
                }
            }
            else
            {
                _targetNPC = null;
                if (!_carrier.IsCarrying)
                {
                    _labelController.SetLabelStatus(NPCInteractionLabelController.NpcLabelEnum.NotAvailable);
                }
            }
        }
        else
        {
            _targetNPC = null;
            if (_carrier.IsCarrying)
            {
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit floorHit, _interactionRange, LayerMask.GetMask("Environment", "Destructible", "NPCDropZone", "Floor")))
                {
                    _carrier.SetOnFloor(true, floorHit.point);
                }
                else
                {
                    _carrier.SetOnFloor(false, Vector3.one);
                }
            }
            else
            {
                _labelController.SetLabelStatus(NPCInteractionLabelController.NpcLabelEnum.NotAvailable);
            }
        }
    }

    public void PickUp()
    {
        if (_targetNPC == null)
        {
            return;
        }
        _carrier.PickUpNPC(_targetNPC);
        _targetNPC = null;
    }
}
