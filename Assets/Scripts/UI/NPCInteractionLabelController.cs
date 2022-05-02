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
    [SerializeField]
    private string _tooFarText;
    [SerializeField]
    private string _notOnFloorText;


    private bool _pickedUp = false;
    private bool _safeToDrop = false;
    private bool _npcAvailable = false;
    private bool _canDrop = true;
    private bool _tooFar = false;

    public enum NpcLabelEnum
    {
        SafeToDrop,
        CantDrop,
        CanDrop,
        CanPickUp,
        TooFar,
        NotOnFloor,
        NotAvailable
    }

    private NpcLabelEnum _status = NpcLabelEnum.NotAvailable;
    private Dictionary<NpcLabelEnum, string> _texts;
    private TMPro.TextMeshProUGUI _label;

    private void Start()
    {
        _label = GetComponent<TMPro.TextMeshProUGUI>();
        _texts = new Dictionary<NpcLabelEnum, string>()
        {
            { NpcLabelEnum.SafeToDrop, _safeDropText },
            { NpcLabelEnum.CantDrop, _cantDropText },
            { NpcLabelEnum.CanDrop, _unsafeDropText },
            { NpcLabelEnum.CanPickUp, _pickupText },
            { NpcLabelEnum.TooFar, _tooFarText },
            { NpcLabelEnum.NotOnFloor, _notOnFloorText },
        };
    }

    public void SetLabelStatus(NpcLabelEnum status)
    {
        _status = status;
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if(_status == NpcLabelEnum.NotAvailable)
        {
            _label.enabled = false;
        }
        else
        {
            _label.enabled = true;
            _label.text = _texts[_status];
        }
    }
}
