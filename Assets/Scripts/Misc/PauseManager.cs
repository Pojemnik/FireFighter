using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private CursorManager _cursorManager;
    [SerializeField]
    private GameObject _gameUI;
    [SerializeField]
    private GameObject _pauseUI;
    [SerializeField]
    private PlayerInputAdapter _playerInputAdapter;
    [SerializeField]
    private UnityEngine.UI.Button _unpauseButton;

    private bool _pauseState = false;

    public event System.EventHandler LockPause;

    private void Start()
    {
        _gameUI.SetActive(true);
        _pauseUI.SetActive(false);
        _unpauseButton.onClick.AddListener(Unpause);
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void TogglePause()
    {
        if (_pauseState)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void LockWithoutPause()
    {
        _cursorManager.UnlockCursor();
        AudioListener.pause = true;
        _playerInputAdapter.EnableMovement = false;
        LockPause?.Invoke(this, null);
    }

    public void LockAll()
    {
        _pauseState = true;
        _cursorManager.UnlockCursor();
        _gameUI.SetActive(false);
        Time.timeScale = 0;
        _playerInputAdapter.EnableMovement = false;
        AudioListener.pause = true;
        LockPause?.Invoke(this, null);
    }

    private void Pause()
    {
        _pauseState = true;
        _cursorManager.UnlockCursor();
        _gameUI.SetActive(false);
        _pauseUI.SetActive(true);
        Time.timeScale = 0;
        _playerInputAdapter.EnableMovement = false;
        AudioListener.pause = true;
    }

    private void Unpause()
    {
        _pauseState = false;
        _cursorManager.LockCursor();
        _gameUI.SetActive(true);
        _pauseUI.SetActive(false);
        Time.timeScale = 1;
        _playerInputAdapter.EnableMovement = true;
        AudioListener.pause = false;
    }
}
