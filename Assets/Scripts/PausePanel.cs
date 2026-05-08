using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private GameObject[] buttonObjects;

    [SerializeField] private GameObject hintsPanel;

    private bool isPaused;
    private bool allowPause = true;

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
        if (!allowPause)
            return;

        if (!isPaused)
        {
            PauseGame();
            return;
        }

        if (settingsPanel.activeSelf)
        {
            CloseSettings();
            return;
        }

        ResumeGame();
    }

    private void SetButtons(bool toggle)
    {
        foreach (var button in buttonObjects)
        {
            button.SetActive(toggle);
        }
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
        SetButtons(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        SetButtons(true);
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void ActiveHintsPanel()
    {
        hintsPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void DeactiveHintsPanel()
    {
        hintsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void SetAllowPause(bool value)
    {
        allowPause = value;
    }
}
