using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOptionsManager : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Toggle _muteToggle;

    private void Awake()
    {
        _muteToggle.onValueChanged.AddListener(OnMuteToggleChange);
    }

    private void OnMuteToggleChange(bool value)
    {
        AudioListener.volume = value ? 0 : 1;
    }
}
