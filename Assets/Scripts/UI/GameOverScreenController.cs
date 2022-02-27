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

    [Header("Config")]
    [SerializeField]
    private float _blendInTime;

    private UnityEngine.UI.Image _image;

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
        _image = GetComponent<UnityEngine.UI.Image>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _backToMenuButton.gameObject.SetActive(false);
        _resetLevelButton.gameObject.SetActive(false);
        StartCoroutine(ShowScreenCoroutine());
    }

    private IEnumerator ShowScreenCoroutine()
    {
        _image.CrossFadeAlpha(0, 0, true);
        _image.CrossFadeAlpha(1, 2, true);
        _pauseManager.LockWithoutPause();
        yield return new WaitForSecondsRealtime(_blendInTime);
        _backToMenuButton.gameObject.SetActive(true);
        _resetLevelButton.gameObject.SetActive(true);
    }
}
