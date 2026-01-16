using TMPro;
using UnityEngine;

public class ReadableAndInteractablePanel : MonoBehaviour
{
    public TextMeshProUGUI headerTextUI;
    public TextMeshProUGUI mainTextUI;
    public TextMeshProUGUI pressEorQTextUI;

    public void ShowReadablePanel()
    {
        gameObject.SetActive(true);
    }

    public void HideReadablePanel()
    {
        gameObject.SetActive(false);
    }
}
