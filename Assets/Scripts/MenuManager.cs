using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject englishButton;
    [SerializeField] private GameObject settingsPanel;

    private bool usingGamepad;

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed) return;

        var action = obj as InputAction;
        if (action == null || action.activeControl == null) return;

        var device = action.activeControl.device;

        if (device is Gamepad || device is Keyboard)
        {
            if (!usingGamepad)
            {
                usingGamepad = true;
                if (settingsPanel.activeInHierarchy)
                {
                    SelectEnglishButton();
                }
                else
                {
                    SelectPlayButton();
                }

            }
        }
        else if (device is Mouse)
        {
            if (usingGamepad)
            {
                usingGamepad = false;
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    public void SelectPlayButton()
    {
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void SelectEnglishButton()
    {
        EventSystem.current.SetSelectedGameObject(englishButton);
    }
}