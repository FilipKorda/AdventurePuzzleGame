using UnityEngine;
using System.Collections;

public class LockPickGameMode : MonoBehaviour
{
    [Header("Obiekty do poruszenia")]
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject lowerLeft;
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject lowerRight;
    [SerializeField] private GameObject right;

    [Header("G³ówny Lock Pick")]
    [SerializeField] private GameObject lockPick;
    private Vector3[] lockPickPositions;
    private int currentLockPickIndex = 4;
    private Coroutine moveCoroutineLockPick;
    private Vector3 lockPickLeftPos = new Vector3(-32.6f, -70, 0);
    private Vector3 lockPickLowerLeftPos = new Vector3(37.7f, -70, 0);
    private Vector3 lockPickMiddlePos = new Vector3(107, -70, 0);
    private Vector3 lockPickLowerRightPos = new Vector3(174, -70, 0);
    private Vector3 lockPickRightPos = new Vector3(244, -70, 0);
    private bool isLeft = false;
    private bool isLowerLeft = false;
    private bool isMiddle = false;
    private bool isLowerRight = false;
    private bool isRight = false;

    [Header("Ustawienia Ruchu")]
    [Tooltip("Jak szybko obiekty maj¹ siê poruszaæ. Wiêksza wartoœæ = szybszy ruch.")]
    [SerializeField] private float moveSpeed = 2f;
    [Tooltip("O ile jednostek w górê maj¹ siê przesun¹æ obiekty.")]
    [SerializeField] private float moveHeight = 10f;

    private Vector3 originalPositionLeft;
    private Vector3 originalPositionLowerLeft;
    private Vector3 originalPositionMiddle;
    private Vector3 originalPositionLowerRight;
    private Vector3 originalPositionRight;

    private Coroutine moveCoroutineLeft;
    private Coroutine moveCoroutineLowerLeft;
    private Coroutine moveCoroutineMiddle;
    private Coroutine moveCoroutineLowerRight;
    private Coroutine moveCoroutineRight;

    private int currentSequenceStep = 0; 
    private int[] correctSequenceOrder;

    private void OnEnable()
    {
        InitOriginalPosition();
        InitLockPick();
        InitGameSequence(); 
    }

    private void OnDisable()
    {
        InitOriginalPosition();
    }

    void InitOriginalPosition()
    {
        if (left != null) originalPositionLeft = left.transform.position;
        if (lowerLeft != null) originalPositionLowerLeft = lowerLeft.transform.position;
        if (middle != null) originalPositionMiddle = middle.transform.position;
        if (lowerRight != null) originalPositionLowerRight = lowerRight.transform.position;
        if (right != null) originalPositionRight = right.transform.position;
    }

    private void MoveLeftUp() { if (left != null) StartMove(ref moveCoroutineLeft, left, originalPositionLeft + Vector3.up * moveHeight); }
    private void MoveLowerLeftUp() { if (lowerLeft != null) StartMove(ref moveCoroutineLowerLeft, lowerLeft, originalPositionLowerLeft + Vector3.up * moveHeight); }
    private void MoveMiddleUp() { if (middle != null) StartMove(ref moveCoroutineMiddle, middle, originalPositionMiddle + Vector3.up * moveHeight); }
    private void MoveLowerRightUp() { if (lowerRight != null) StartMove(ref moveCoroutineLowerRight, lowerRight, originalPositionLowerRight + Vector3.up * moveHeight); }
    private void MoveRightUp() { if (right != null) StartMove(ref moveCoroutineRight, right, originalPositionRight + Vector3.up * moveHeight); }
    private void MoveLeftDown() { if (left != null) StartMove(ref moveCoroutineLeft, left, originalPositionLeft); }
    private void MoveLowerLeftDown() { if (lowerLeft != null) StartMove(ref moveCoroutineLowerLeft, lowerLeft, originalPositionLowerLeft); }
    private void MoveMiddleDown() { if (middle != null) StartMove(ref moveCoroutineMiddle, middle, originalPositionMiddle); }
    private void MoveLowerRightDown() { if (lowerRight != null) StartMove(ref moveCoroutineLowerRight, lowerRight, originalPositionLowerRight); }
    private void MoveRightDown() { if (right != null) StartMove(ref moveCoroutineRight, right, originalPositionRight); }


    private void StartMove(ref Coroutine coroutineRef, GameObject part, Vector3 targetPosition)
    {
        if (coroutineRef != null) { StopCoroutine(coroutineRef); }
        coroutineRef = StartCoroutine(MoveObject(part, targetPosition, moveSpeed));
    }

    private IEnumerator MoveObject(GameObject objectToMove, Vector3 endPos, float speed)
    {
        float journeyProgress = 0f;
        Vector3 startPos = objectToMove.transform.position;
        while (journeyProgress < 1.0f)
        {
            journeyProgress += Time.deltaTime * speed;
            objectToMove.transform.position = Vector3.Lerp(startPos, endPos, journeyProgress);
            yield return null;
        }
        objectToMove.transform.position = endPos;
    }

    private void StartMoveLockPickObject(ref Coroutine coroutineRef, GameObject part, Vector3 targetPosition)
    {
        if (coroutineRef != null) { StopCoroutine(coroutineRef); }
        coroutineRef = StartCoroutine(MoveLockPickObject(part, targetPosition, moveSpeed));
    }

    private IEnumerator MoveLockPickObject(GameObject objectToMove, Vector3 endPos, float speed)
    {
        float journeyProgress = 0f;
        Vector3 startPos = objectToMove.transform.localPosition;
        while (journeyProgress < 1.0f)
        {
            journeyProgress += Time.deltaTime * speed;
            objectToMove.transform.localPosition = Vector3.Lerp(startPos, endPos, journeyProgress);
            yield return null;
        }
        objectToMove.transform.localPosition = endPos;
    }

    private void Update()
    {
        if (currentSequenceStep < correctSequenceOrder.Length)
        {
            if (Input.GetKeyDown(KeyCode.A)) { MoveLockPickLeft(); }
            if (Input.GetKeyDown(KeyCode.D)) { MoveLockPickRight(); }

            if (Input.GetKeyDown(KeyCode.E))
            {
                TryActivateLockPart();
            }
        }
    }

    private void InitLockPick()
    {
        lockPickPositions = new Vector3[]
        {
            lockPickLeftPos, lockPickLowerLeftPos, lockPickMiddlePos, lockPickLowerRightPos, lockPickRightPos
        };

        if (lockPick != null)
        {
            lockPick.transform.localPosition = lockPickPositions[currentLockPickIndex];
            UpdateLockPickFlags();
            Debug.Log("LockPick startuje na pozycji: " + GetCurrentLockPickPositionName());
        }
    }

    public void MoveLockPickLeft()
    {
        if (currentLockPickIndex > 0)
        {
            currentLockPickIndex--;
            StartLockPickMove();
        }
    }

    public void MoveLockPickRight()
    {
        if (currentLockPickIndex < lockPickPositions.Length - 1)
        {
            currentLockPickIndex++;
            StartLockPickMove();
        }
    }

    private void StartLockPickMove()
    {
        if (lockPick == null) return;
        if (moveCoroutineLockPick != null) { StopCoroutine(moveCoroutineLockPick); }

        UpdateLockPickFlags();
        Vector3 targetPosition = lockPickPositions[currentLockPickIndex];

        Debug.Log("LockPick przesuwa siê na pozycjê: " + GetCurrentLockPickPositionName());
        moveCoroutineLockPick = StartCoroutine(MoveLockPickObject(lockPick, targetPosition, moveSpeed));
    }

    private void InitGameSequence()
    {     
        correctSequenceOrder = new int[] { 0, 1, 4, 2, 3 };
        currentSequenceStep = 0; 
    }

    private void TryActivateLockPart()
    {
        int expectedIndex = correctSequenceOrder[currentSequenceStep];

        if (currentLockPickIndex == expectedIndex)
        {
            Debug.Log($"<color=green>Dobry ruch! Krok {currentSequenceStep + 1} zaliczony.</color>");

            if (isLeft) MoveLeftUp();
            if (isLowerLeft) MoveLowerLeftUp();
            if (isMiddle) MoveMiddleUp();
            if (isLowerRight) MoveLowerRightUp();
            if (isRight) MoveRightUp();

            currentSequenceStep++;

            if (currentSequenceStep >= correctSequenceOrder.Length)
            {
                WinGame();
            }
        }
        else
        {
            Debug.Log($"<color=red>Z³y ruch! Oczekiwano pozycji '{GetCurrentLockPickPositionName(expectedIndex)}', a jesteœ na '{GetCurrentLockPickPositionName()}'.</color>");
            ResetGame();
        }
    }

    private void WinGame()
    {
        Debug.Log("<color=yellow>GRATULACJE! ZAMEK OTWARTY!</color>");
    }

    private void ResetGame()
    {
        Debug.Log("Resetowanie gry...");

        MoveLeftDown();
        MoveLowerLeftDown();
        MoveMiddleDown();
        MoveLowerRightDown();
        MoveRightDown();

        currentLockPickIndex = 4;
        StartLockPickMove();

        currentSequenceStep = 0;
    }

    private void UpdateLockPickFlags()
    {
        isLeft = false;
        isLowerLeft = false;
        isMiddle = false;
        isLowerRight = false;
        isRight = false;

        switch (currentLockPickIndex)
        {
            case 0: isLeft = true; break;
            case 1: isLowerLeft = true; break;
            case 2: isMiddle = true; break;
            case 3: isLowerRight = true; break;
            case 4: isRight = true; break;
        }
    }

    public string GetCurrentLockPickPositionName()
    {
        switch (currentLockPickIndex)
        {
            case 0: return "Left";
            case 1: return "Lower Left";
            case 2: return "Middle";
            case 3: return "Lower Right";
            case 4: return "Right";
            default: return "Nieznana pozycja";
        }
    }
    private string GetCurrentLockPickPositionName(int index)
    {
        switch (index)
        {
            case 0: return "Left";
            case 1: return "Lower Left";
            case 2: return "Middle";
            case 3: return "Lower Right";
            case 4: return "Right";
            default: return "Nieznana pozycja";
        }
    }
}