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
        progressBar.value = 0f;

        Canvas.ForceUpdateCanvases();
        yield return null;
        yield return null;

        float timer = 0f;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressBar.value = progress;

            if (asyncOperation.progress >= 0.9f)
            {
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
