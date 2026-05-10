using System.Collections;
using UnityEngine;

public class PressArrowButton : MonoBehaviour
{
    [SerializeField] private Transform buttonVisual;
    [SerializeField] private float pressedOffsetY = -0.05f;
    [SerializeField] private float pressDuration = 0.12f;

    private Vector3 baseLocalPosition;
    private Coroutine buttonAnimRoutine;

    [SerializeField] private BoxCollider buttonBoxCollider;

    private void Awake()
    {
        if (buttonVisual == null)
            buttonVisual = transform;

        baseLocalPosition = buttonVisual.localPosition;
    }

    public void ForceResetButton()
    {
        if (buttonAnimRoutine != null)
        {
            StopCoroutine(buttonAnimRoutine);
            buttonAnimRoutine = null;
        }
        Services.Audio.PlaySFX("WallButtonPress");
        buttonVisual.localPosition = baseLocalPosition;
        buttonBoxCollider.enabled = true;
    }


    public void PlayPressAnimation()
    {
        if (buttonAnimRoutine != null)
            StopCoroutine(buttonAnimRoutine);
        Services.Audio.PlaySFX("WallButtonPress");
        buttonAnimRoutine = StartCoroutine(AnimPressButton());
    }

    public void PlayReleaseAnimation()
    {
        if (buttonAnimRoutine != null)
            StopCoroutine(buttonAnimRoutine);
        Services.Audio.PlaySFX("WallButtonPress");
        buttonAnimRoutine = StartCoroutine(AnimReleaseButton());
    }

    private IEnumerator AnimPressButton()
    {
        buttonBoxCollider.enabled = false;

        Vector3 startPos = buttonVisual.localPosition;
        Vector3 targetPos = new Vector3(
            baseLocalPosition.x,
            baseLocalPosition.y + pressedOffsetY,
            baseLocalPosition.z
        );

        float time = 0f;

        while (time < pressDuration)
        {
            time += Time.deltaTime;
            float t = time / pressDuration;

            buttonVisual.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        buttonVisual.localPosition = targetPos;
        buttonAnimRoutine = null;
    }

    private IEnumerator AnimReleaseButton()
    {
        Vector3 startPos = buttonVisual.localPosition;
        Vector3 targetPos = baseLocalPosition;

        float time = 0f;

        while (time < pressDuration)
        {
            time += Time.deltaTime;
            float t = time / pressDuration;

            buttonVisual.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        buttonVisual.localPosition = targetPos;
        buttonBoxCollider.enabled = true;
        buttonAnimRoutine = null;
    }
}
