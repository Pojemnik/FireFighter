using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    public void LoadAndUnloadScenes(string sceneToUnload, string sceneToLoad)
    {
        StartCoroutine(LoadingCoroutine(sceneToUnload, sceneToLoad));
    }

    IEnumerator LoadingCoroutine(string sceneToUnload, string sceneToLoad)
    {
        if (sceneToLoad == sceneToUnload)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}
