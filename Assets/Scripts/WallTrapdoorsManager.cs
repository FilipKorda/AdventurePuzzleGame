using System.Collections;
using UnityEngine;

public class WallTrapdoorsManager : MonoBehaviour
{
    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;

    public void OpenTrapDoor()
    {
        StartCoroutine(OpenTrapDoorCoroutine());
    }

    private IEnumerator OpenTrapDoorCoroutine()
    {
        float time = 0f;
        float duration = 3f;

        float leftStartY = 90f;
        float leftTargetY = 200f;

        float rightStartY = 90f;
        float rightTargetY = -20f;

        while (time < duration)
        {
            float t = time / duration;

            float leftY = Mathf.Lerp(leftStartY, leftTargetY, t);
            float rightY = Mathf.Lerp(rightStartY, rightTargetY, t);

            leftHandle.transform.rotation = Quaternion.Euler(180f, leftY, 0f);
            rightHandle.transform.rotation = Quaternion.Euler(0f, rightY, 0f);

            time += Time.deltaTime;
            yield return null;
        }

        leftHandle.transform.rotation = Quaternion.Euler(180f, leftTargetY, 0f);
        rightHandle.transform.rotation = Quaternion.Euler(0f, rightTargetY, 0f);
    }
}