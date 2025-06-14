using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider progressBar;

    public void LoadSceneAsyncWithProgressBar(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        canvas.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            progressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Loading complete! Activating scene...");
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
