using System.Collections;
using UnityEngine;

public class TrapDoorVerticalManager : MonoBehaviour
{
    [SerializeField] private GameObject upPanel;
    [SerializeField] private GameObject downPanel;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float moveDuration = 3f;

    public void ActiveAnimation()
    {
        StartCoroutine(MakeObjectMoveUpAndDown());
    }

    private IEnumerator MakeObjectMoveUpAndDown()
    {
        Services.Audio.PlaySFX("OpenTrapdoorRoomTen");
        Services.Audio.PlaySFX("ShorterMovingStoneKryptex");
        Vector3 upStartPosition = upPanel.transform.position;
        Vector3 downStartPosition = downPanel.transform.position;

        Vector3 upTargetPosition = upStartPosition + Vector3.up * moveDistance;
        Vector3 downTargetPosition = downStartPosition + Vector3.down * moveDistance;

        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);

            upPanel.transform.position = Vector3.Lerp(upStartPosition, upTargetPosition, t);
            downPanel.transform.position = Vector3.Lerp(downStartPosition, downTargetPosition, t);

            yield return null;
        }

        upPanel.transform.position = upTargetPosition;
        downPanel.transform.position = downTargetPosition;
    }
}
