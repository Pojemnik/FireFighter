using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputAdapter : MonoBehaviour
{
    [SerializeField]
    private PauseManager _pauseManager;

    public void TogglePause(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _pauseManager.TogglePause();
        }
    }
}
