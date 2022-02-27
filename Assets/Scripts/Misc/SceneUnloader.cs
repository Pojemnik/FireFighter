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
            loading.LoadScene(nextScene);
        };
    }
}
