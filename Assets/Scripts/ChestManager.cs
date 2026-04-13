using UnityEngine;
using System.Collections;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float chestOpenAngle = -90f;

    public void OpenChest()
    {
        StartCoroutine(OpenLidCoroutine());
    }

    private IEnumerator OpenLidCoroutine()
    {
        float duration = 2f;
        float time = 0f;

        Vector3 startRotation = lidTransform.localEulerAngles;
        Vector3 targetRotation = startRotation + new Vector3(0f, 0f, chestOpenAngle);

        while (time < duration)
        {
            time += Time.deltaTime;
            lidTransform.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, time / duration);
            yield return null;
        }

        lidTransform.localEulerAngles = targetRotation;
    }

    public void CloseChest()
    {
        StartCoroutine(CloseLidCoroutine());
    }

    private IEnumerator CloseLidCoroutine()
    {
        float duration = 2f;
        float time = 0f;

        Vector3 startRotation = lidTransform.localEulerAngles;
        Vector3 targetRotation = startRotation - new Vector3(0f, 0f, chestOpenAngle);

        while (time < duration)
        {
            time += Time.deltaTime;
            lidTransform.localEulerAngles = Vector3.Lerp(startRotation, targetRotation, time / duration);
            yield return null;
        }

        lidTransform.localEulerAngles = targetRotation;
    }

}