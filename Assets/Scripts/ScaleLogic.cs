using UnityEngine;
using System.Collections;

public class ScaleLogic : MonoBehaviour
{
    [SerializeField] private GameObject jointHinge;
    [SerializeField] private GameObject[] helperHinges;
    [SerializeField] private float maxAngle = 10f;
    [SerializeField] private int maxDifference = 10;

    private Coroutine rotateRoutine;
    private float currentAngle;

    public void SetBalance(int difference)
    {
        difference = Mathf.Clamp(difference, -maxDifference, maxDifference);
        float targetAngle = (difference / (float)maxDifference) * maxAngle;

        if (rotateRoutine != null)
            StopCoroutine(rotateRoutine);

        rotateRoutine = StartCoroutine(RotateTo(targetAngle));
    }

    private IEnumerator RotateTo(float targetAngle)
    {
        float startAngle = currentAngle;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            currentAngle = Mathf.Lerp(startAngle, targetAngle, t);

            jointHinge.transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);

            for (int i = 0; i < helperHinges.Length; i++)
                helperHinges[i].transform.localRotation = Quaternion.Euler(0f, -currentAngle, 0f);

            yield return null;
        }

        currentAngle = targetAngle;
        jointHinge.transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);

        for (int i = 0; i < helperHinges.Length; i++)
            helperHinges[i].transform.localRotation = Quaternion.Euler(0f, -currentAngle, 0f);
    }
}