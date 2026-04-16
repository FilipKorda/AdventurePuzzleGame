using System.Collections;
using UnityEngine;

public class OpenTrapDoorsManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHandle;
    [SerializeField] private GameObject leftHandle;
    [SerializeField] private float rotateDuration = 1f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            OpenAnimation();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CloseAnimation();
        }
    }

    public void OpenAnimation()
    {
        StartCoroutine(OpenTrapDoorCoroutine());
    }

    public void CloseAnimation()
    {
        StartCoroutine(CloseTrapDoorCoroutine());
    }

    private IEnumerator OpenTrapDoorCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        Quaternion rightStartRotation = rightHandle.transform.localRotation;
        Quaternion leftStartRotation = leftHandle.transform.localRotation;

        Quaternion rightTargetRotation = Quaternion.Euler(
            rightHandle.transform.localEulerAngles.x,
            rightHandle.transform.localEulerAngles.y,
            0f
        );

        Quaternion leftTargetRotation = Quaternion.Euler(
            leftHandle.transform.localEulerAngles.x,
            leftHandle.transform.localEulerAngles.y,
            0f
        );

        float time = 0f;

        while (time < rotateDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / rotateDuration);

            rightHandle.transform.localRotation = Quaternion.Lerp(rightStartRotation, rightTargetRotation, t);
            leftHandle.transform.localRotation = Quaternion.Lerp(leftStartRotation, leftTargetRotation, t);

            yield return null;
        }

        rightHandle.transform.localRotation = rightTargetRotation;
        leftHandle.transform.localRotation = leftTargetRotation;
    }

    private IEnumerator CloseTrapDoorCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        Quaternion rightStartRotation = rightHandle.transform.localRotation;
        Quaternion leftStartRotation = leftHandle.transform.localRotation;

        Quaternion rightTargetRotation = Quaternion.Euler(
            rightHandle.transform.localEulerAngles.x,
            rightHandle.transform.localEulerAngles.y,
            90f
        );

        Quaternion leftTargetRotation = Quaternion.Euler(
            leftHandle.transform.localEulerAngles.x,
            leftHandle.transform.localEulerAngles.y,
            90f
        );

        float time = 0f;

        while (time < rotateDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / rotateDuration);

            rightHandle.transform.localRotation = Quaternion.Lerp(rightStartRotation, rightTargetRotation, t);
            leftHandle.transform.localRotation = Quaternion.Lerp(leftStartRotation, leftTargetRotation, t);

            yield return null;
        }

        rightHandle.transform.localRotation = rightTargetRotation;
        leftHandle.transform.localRotation = leftTargetRotation;
    }
}
