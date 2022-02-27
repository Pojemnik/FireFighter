using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _camera;

    [Header("Config")]
    [SerializeField]
    private float _interactionRange;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent<bool> TargetStateChanged;

    private NPCController _targetNPC = null;

    private void Update()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactionRange, ~LayerMask.GetMask("AxeZone", "ExtinguisherZone", "Player")))
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
                    TargetStateChanged.Invoke(true);
                }
                Debug.LogFormat("Looking at NPC {0}", hit.transform.gameObject.name);
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
        }
    }

    public void Interact()
    {
        if (_targetNPC == null)
        {
            return;
        }
        _targetNPC.transform.parent = transform;
    }
}
