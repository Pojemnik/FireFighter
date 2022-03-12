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

    private bool _pickedUp;
    private bool _safeToDrop;
    private bool _npcAvailable;

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

    private void UpdateLabel()
    {
        if(_pickedUp)
        {
            _label.text = _safeToDrop ? _safeDropText : _unsafeDropText;
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
