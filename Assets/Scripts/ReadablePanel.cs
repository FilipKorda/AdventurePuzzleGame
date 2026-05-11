using TMPro;
using UnityEngine;

public class ReadablePanel : MonoBehaviour
{
    public TextMeshProUGUI headerTextUI;
    public TextMeshProUGUI mainTextUI;
    public TextMeshProUGUI signatureTextUI;

    public void ShowReadablePanel()
    {
        UIManager.Instance.EnableLeaveBookPanel();
        gameObject.SetActive(true);
    }

    public void HideReadablePanel()
    {
        UIManager.Instance.DisableSharedPanelText();
        gameObject.SetActive(false);
    }
}
