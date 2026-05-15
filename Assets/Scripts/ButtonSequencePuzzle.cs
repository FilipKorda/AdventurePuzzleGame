using System.Collections;
using UnityEngine;

public class ButtonSequencePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject pillar;

    [SerializeField] private GameObject leftHandle;
    [SerializeField] private GameObject rightHandle;

    [SerializeField] private int[] sequence;

    private int[] pressedSequence;
    private int pressedCount;

    [SerializeField] private PictureTerrainMovingObject[] pictureTerrainMovingObjects;
    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager;
    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager1;

    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;

    [SerializeField] private PressArrowButton[] pressArrowButtons;

    private void Awake()
    {
        pressedSequence = new int[sequence.Length];
    }

    public void PressButton(int buttonId)
    {
        if (pressedCount >= sequence.Length)
            return;

        pressedSequence[pressedCount] = buttonId;
        pressedCount++;

        if (pressedCount == sequence.Length)
        {
            CheckSequence();
        }
    }

    private void CheckSequence()
    {
        for (int i = 0; i < sequence.Length; i++)
        {
            if (pressedSequence[i] != sequence[i])
            {
                ResetPuzzle();
                return;
            }
        }

        WinPuzzle();
    }

    private void WinPuzzle()
    {
        MovePilalrDown();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void ResetPuzzle()
    {
        pressedCount = 0;
        StartCoroutine(ResetPuzzleNextFrame());
    }

    private IEnumerator ResetPuzzleNextFrame()
    {
        yield return new WaitForSeconds(0.5f);
        ResetButtons();
    }


    private void ResetButtons()
    {
        foreach (var item in pressArrowButtons)
        {
            item.ForceResetButton();
        }
    }


    private void MovePilalrDown()
    {
        StartCoroutine(MovePillarDownRoutine());
    }

    private IEnumerator MovePillarDownRoutine()
    {
        rotateAllGearsRoomTen.RotateAllGears();

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