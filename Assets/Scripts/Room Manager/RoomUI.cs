using System.Collections;
using UnityEngine;
using TMPro;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float visibleTime = 2f;
    [SerializeField] private float fadeDuration = 0.35f;

    private Coroutine showRoutine;

    private void Awake()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        if (roomNameText != null)
        {
            roomNameText.text = string.Empty;
        }
    }

    public void ShowRoomName(string roomName)
    {
        if (showRoutine != null)
        {
            StopCoroutine(showRoutine);
        }

        showRoutine = StartCoroutine(ShowRoomNameRoutine(roomName));
    }

    private IEnumerator ShowRoomNameRoutine(string roomName)
    {
        roomNameText.text = roomName;

        yield return FadeCanvas(0f, 1f);

        yield return new WaitForSeconds(visibleTime);

        yield return FadeCanvas(1f, 0f);

        showRoutine = null;
    }

    private IEnumerator FadeCanvas(float from, float to)
    {
        if (canvasGroup == null)
            yield break;

        float time = 0f;
        canvasGroup.alpha = from;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
