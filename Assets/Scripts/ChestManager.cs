using UnityEngine;
using System.Collections;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float chestOpenAngle = -90f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

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
}