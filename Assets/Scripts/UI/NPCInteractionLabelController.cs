using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractionLabelController : MonoBehaviour
{
    [SerializeField]
    private string _pickupText;
    [SerializeField]
    private string _unsafeDropText;
    [SerializeField]
    private string _safeDropText;
    [SerializeField]
    private string _cantDropText;

    private bool _pickedUp = false;
    private bool _safeToDrop = false;
    private bool _npcAvailable = false;
    private bool _canDrop = true;

    private TMPro.TextMeshProUGUI _label;

    private void Start()
    {
        _label = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void OnNPCAvailabilityChange(bool state)
    {
        _npcAvailable = state;
        UpdateLabel();
    }

    public void OnNPCPickupStateChange(bool state)
    {
        _pickedUp = state;
        UpdateLabel();
    }

    public void OnSafeZoneContainmentChange(bool state)
    {
        _safeToDrop = state;
        UpdateLabel();
    }

    public void OnCanDropStateChange(bool state)
    {
        _canDrop = state;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if(_pickedUp)
        {
            if (_canDrop)
            {
                _label.text = _safeToDrop ? _safeDropText : _unsafeDropText;
            }
            else
            {
                _label.text = _cantDropText;
            }
            _label.enabled = true;
        }
        else
        {
            if(_npcAvailable)
            {
                _label.text = _pickupText;
                _label.enabled = true;
            }
            else
            {
                _label.enabled = false;
            }
        }
    }
}
