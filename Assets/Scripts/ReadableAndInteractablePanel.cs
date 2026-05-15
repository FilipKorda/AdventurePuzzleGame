using TMPro;
using UnityEngine;

public class ReadableAndInteractablePanel : MonoBehaviour
{
    public TextMeshProUGUI headerTextUI;
    public TextMeshProUGUI mainTextUI;
    public TextMeshProUGUI pressEorQTextUI;

    public void ShowReadablePanel()
    {
        UIManager.Instance.EnableLeaveBookPanel();
        Services.Audio.PlaySFX("GrabRecipe");
        gameObject.SetActive(true);
    }

    public void HideReadablePanel()
    {
        UIManager.Instance.DisableSharedPanelText();
        Services.Audio.PlaySFX("GrabRecipe");
        gameObject.SetActive(false);
    }
}
