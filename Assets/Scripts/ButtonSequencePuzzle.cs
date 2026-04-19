using System.Collections;
using UnityEngine;

public class ButtonSequencePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject pillar;

    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;

    [SerializeField] private int[] sequence;
    private int index;

    [SerializeField] private PictureTerrainMovingObject[] pictureTerrainMovingObjects;
    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager;
    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager1;


    private void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.L))
        {
            WinPuzzle();
        }*/
    }

    public void PressButton(int buttonId)
    {
        if (sequence[index] == buttonId)
        {
            index++;

            if (index == sequence.Length)
            {
                WinPuzzle();
            }
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void WinPuzzle()
    {
        MovePilalrDown();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void ResetPuzzle()
    {
        index = 0;
    }

    private void MovePilalrDown()
    {
        StartCoroutine(MovePillarDownRoutine());
    }

    private IEnumerator MovePillarDownRoutine()
    {
        Vector3 startPos = pillar.transform.position;
        Vector3 targetPos = startPos + Vector3.down * 1.19f;

        float time = 0f;
        float duration = 3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            pillar.transform.position = Vector3.Lerp(startPos, targetPos, time / duration);
            yield return null;
        }

        pillar.transform.position = targetPos;

        StartCoroutine(OpenLeftAndRightDoor());
        wallTrapdoorsManager.OpenTrapDoor();
        wallTrapdoorsManager1.OpenTrapDoor();
    }


    private IEnumerator OpenLeftAndRightDoor()
    {
        float duration = 3f;
        float time = 0f;

        Quaternion leftStart = Quaternion.Euler(0f, 0f, 0f);
        Quaternion leftEnd = Quaternion.Euler(0f, 96f, 0f);

        Quaternion rightStart = Quaternion.Euler(-270f, 0f, 0f);
        Quaternion rightEnd = Quaternion.Euler(-270f, 0f, 96f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            leftHandle.transform.localRotation = Quaternion.Lerp(leftStart, leftEnd, t);
            rightHandle.transform.localRotation = Quaternion.Lerp(rightStart, rightEnd, t);

            yield return null;
        }

        leftHandle.transform.localRotation = leftEnd;
        rightHandle.transform.localRotation = rightEnd;

        ActiveAnotherPuzzle();
    }

    private void ActiveAnotherPuzzle()
    {
        foreach (var item in pictureTerrainMovingObjects)
        {
            item.ActivePuzzle = true;
        }
    }

}