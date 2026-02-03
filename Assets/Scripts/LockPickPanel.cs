using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LockPickPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lockPickText;
    [SerializeField] private LocalizedString localizeString;


    public void ShowLockPickPanel()
    {
        StartLockPickMode();
    }

    public void HideLockPickPanel()
    {
        StopLockPickMode();
    }

    private void StartLockPickMode()
    {
        gameObject.SetActive(true);
        lockPickText.text = localizeString.GetLocalizedString();
        lockPickText.gameObject.SetActive(true);
    }

    private void StopLockPickMode()
    {
        gameObject.SetActive(false);
        lockPickText.text = "";
        lockPickText.gameObject.SetActive(false);
    }
}
