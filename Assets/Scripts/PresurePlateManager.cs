using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresurePlateManager : MonoBehaviour
{
    [SerializeField] private PressurePlate pressurePlate0;
    [SerializeField] private PressurePlate pressurePlate1;
    [SerializeField] private PressurePlate pressurePlate2;

    private List<PressurePlate> correctSequence = new List<PressurePlate>();
    private List<PressurePlate> pressedSequence = new List<PressurePlate>();

    [SerializeField] private BoxCollider pressurePlateBoxCollider;
    [SerializeField] private BoxCollider pressurePlate1BoxCollider;
    [SerializeField] private BoxCollider pressurePlate2BoxCollider;

    [SerializeField] private GameObject movingWallWithPainting;
    [SerializeField] private float targetY = 9.5f;
    [SerializeField] private float moveDuration = 2f;

    [SerializeField] private GameObject movingPillar;
    [SerializeField] private float targetYPillar = 7.8f;
    [SerializeField] private float moveDurationPillar = 2f;

    [SerializeField] private GameObject movingPillarOne;
    [SerializeField] private float targetYPillarOne = 7.8f;
    [SerializeField] private float moveDurationPillarOne = 2f;

    [SerializeField] private GameObject movingPillarTwo;
    [SerializeField] private float targetYPillarTwo = 7.8f;
    [SerializeField] private float moveDurationPillarTwo = 2f;


    public bool puzzleIsActivated = false;

    void Awake()
    {
        pressurePlate0.manager = this;
        pressurePlate1.manager = this;
        pressurePlate2.manager = this;

        correctSequence.Add(pressurePlate0);
        correctSequence.Add(pressurePlate1);
        correctSequence.Add(pressurePlate2);
    }

    public void PlatePressed(PressurePlate plate)
    {
        if(!puzzleIsActivated) return;

        if (!pressedSequence.Contains(plate))
            pressedSequence.Add(plate);

        if (pressedSequence.Count >= correctSequence.Count)
        {
            bool win = true;
            for (int i = 0; i < correctSequence.Count; i++)
            {
                if (pressedSequence[i] != correctSequence[i])
                {
                    win = false;
                    break;
                }
            }

            if (win)
            {
                WinPuzzle();
            }
            else
            {
                Debug.Log("PRZEGRANA!");
                ResetSequence();
            }
        }
    }

    private void ResetSequence()
    {
        foreach (var plate in pressedSequence)
            plate.ResetPlate();

        pressedSequence.Clear();
    }

    private void WinPuzzle()
    {
        StartMoveUp();
        ResetSequenceAfterWin();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void ResetSequenceAfterWin()
    {
        foreach (var plate in pressedSequence)
            plate.ResetPlate();

        pressedSequence.Clear();

        pressurePlateBoxCollider.enabled = false;
        pressurePlate1BoxCollider.enabled = false;
        pressurePlate2BoxCollider.enabled = false;
    }

    public void StartMoveUp()
    {
        StartCoroutine(MoveUpCoroutine());
        StartCoroutine(MovePillarRoutine());
        StartCoroutine(MovePillarOneRoutine());
        StartCoroutine(MovePillarTwoRoutine());
    }

    private IEnumerator MoveUpCoroutine()
    {
        float elapsed = 0f;
        Vector3 startPos = movingWallWithPainting.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            movingWallWithPainting.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        movingWallWithPainting.transform.position = targetPos;
    }

    private IEnumerator MovePillarRoutine()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = movingPillar.transform.position;

        Vector3 targetPosition = new Vector3(startingPosition.x, targetYPillar, startingPosition.z);

        while (elapsedTime < moveDurationPillar)
        {
            float t = elapsedTime / moveDurationPillar;


            movingPillar.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPillar.transform.position = targetPosition;
    }

    private IEnumerator MovePillarOneRoutine()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = movingPillarOne.transform.position;

        Vector3 targetPosition = new Vector3(startingPosition.x, targetYPillarOne, startingPosition.z);

        while (elapsedTime < moveDurationPillarOne)
        {
            float t = elapsedTime / moveDurationPillarOne;

            movingPillarOne.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPillarOne.transform.position = targetPosition;
    }

    private IEnumerator MovePillarTwoRoutine()
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = movingPillarTwo.transform.position;

        Vector3 targetPosition = new Vector3(startingPosition.x, targetYPillarTwo, startingPosition.z);

        while (elapsedTime < moveDurationPillarOne)
        {
            float t = elapsedTime / moveDurationPillarTwo;

            movingPillarTwo.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movingPillarTwo.transform.position = targetPosition;
    }
}
