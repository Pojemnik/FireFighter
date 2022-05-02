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

    [Header("Config")]
    [SerializeField]
    private float _interactionRange;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent<bool> TargetStateChanged;

    private NPCController _targetNPC = null;

    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactionRange, LayerMask.GetMask("Victims")))
        {
            if (hit.transform.gameObject.CompareTag("NPC"))
            {
                _targetNPC = hit.transform.gameObject.GetComponent<NPCController>();
                if (_targetNPC == null)
                {
                    TargetStateChanged.Invoke(false);
                    Debug.LogErrorFormat("NPC {0} is tagged correctly, but it doesn't have NPCController", hit.transform.gameObject.name);
                }
                else
                {
                    if (!_carrier.IsCarrying)
                    {
                        TargetStateChanged.Invoke(true);
                    }
                }
            }
            else
            {
                _targetNPC = null;
                TargetStateChanged.Invoke(false);
            }
        }
        else
        {
            _targetNPC = null;
            TargetStateChanged.Invoke(false);
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
        TargetStateChanged.Invoke(false);
    }
}
