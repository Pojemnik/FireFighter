using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreenController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private UnityEngine.UI.Button _backToMenuButton;
    [SerializeField]
    private UnityEngine.UI.Button _resetLevelButton;
    [SerializeField]
    private SceneUnloader _unloader;
    [SerializeField]
    private PauseManager _pauseManager;

    private void Awake()
    {
        _backToMenuButton.onClick.AddListener(() =>
        {
            _unloader.GoToMenu();
        });
        _resetLevelButton.onClick.AddListener(() =>
        {
            _unloader.GoToLevel();
        });
        //gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _pauseManager.PauseWithoutShowingUI();
    }
}
