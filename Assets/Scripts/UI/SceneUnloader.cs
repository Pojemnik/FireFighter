using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField]
    private string _menuScene;
    [SerializeField]
    private string _levelScene;

    private string _currentScene;

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;
    }

    public void GoToLevel()
    {
        ChangeScene(_levelScene);
    }

    public void GoToMenu()
    {
        ChangeScene(_menuScene);
    }

    private void ChangeScene(string nextScene)
    {
        SceneManager.LoadSceneAsync("LoadingScene").completed += (_) =>
        {
            Debug.Log("Change scene");
            LoadingScreenManager loading = FindObjectOfType<LoadingScreenManager>();
            loading.LoadAndUnloadScenes(_currentScene, nextScene);
        };
    }
}
