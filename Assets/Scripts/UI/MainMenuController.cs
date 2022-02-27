using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    UnityEngine.UI.Button _quitButton;

    private void Awake()
    {
        _quitButton.onClick.AddListener(() => Application.Quit());
    }
}
