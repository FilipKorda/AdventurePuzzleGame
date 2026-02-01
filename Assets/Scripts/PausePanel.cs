using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private InputActionReference pauseAction;

    private bool isPaused;

    private void OnEnable()
    {
        pauseAction.action.performed += OnPause;
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPause;
        pauseAction.action.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        pausePanel.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        isPaused = false;
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
