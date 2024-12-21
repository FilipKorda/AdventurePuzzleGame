using TMPro;
using UnityEngine;

public class ReadablePanel : MonoBehaviour
{
    public TextMeshProUGUI headerTextUI;
    public TextMeshProUGUI mainTextUI;
    public TextMeshProUGUI signatureTextUI;

    public void ShowReadablePanel()
    {
        gameObject.SetActive(true);
    }

    public void HideReadablePanel()
    {
        gameObject.SetActive(false);
    }
}
