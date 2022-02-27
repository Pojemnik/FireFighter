using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputAdapter : MonoBehaviour
{
    [SerializeField]
    private PauseManager _pauseManager;

    private bool _canPause;

    private void Start()
    {
        _pauseManager.LockPause += (_, _) => _canPause = false;
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.started && _canPause)
        {
            _pauseManager.TogglePause();
        }
    }
}
