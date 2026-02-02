using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI locationText;

    [Header("Settings")]
    [SerializeField] private float defaultDisplayTime = 3f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (locationText != null)
            locationText.text = "";
    }

    public void ShowNotification(LocalizedString localizeString, float duration = -1f)
    {
        if (locationText == null)
        {
            Debug.LogWarning("NotificationSystem: locationText nie jest przypisany w inspectorze.");
            return;
        }

        locationText.text = localizeString.GetLocalizedString();      

        float showTime = duration > 0f ? duration : defaultDisplayTime;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideAfterDelay(showTime));
    }

    public void SetLocationText(LocalizedString localizeString)
    {
        if (locationText == null) return;
        locationText.text = localizeString.GetLocalizedString();
    }

    public void HideLocation()
    {
        if (locationText == null) return;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }

        locationText.text = "";
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (locationText != null)
            locationText.text = "";

        hideCoroutine = null;
    }
}
