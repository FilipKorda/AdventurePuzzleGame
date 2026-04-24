using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockPickGameMode : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Obiekty do poruszenia")]
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject lowerLeft;
    [SerializeField] private GameObject middle;
    [SerializeField] private GameObject lowerRight;
    [SerializeField] private GameObject right;

    [Header("G³ówny Lock Pick")]
    [SerializeField] private GameObject lockPick;
    [SerializeField] private float lockPickRotationDuration = 1.0f;
    [SerializeField] private RectTransform lockPickRectTransform;
    private Quaternion originalRotation;
    private Coroutine rotationCoroutine;
    private Vector3[] lockPickPositions;
    private int currentLockPickIndex = 4;
    private Coroutine moveCoroutineLockPick;
    private Vector3 lockPickLeftPos = new Vector3(23.2f, -75, 0);
    private Vector3 lockPickLowerLeftPos = new Vector3(78.5f, -75, 0);
    private Vector3 lockPickMiddlePos = new Vector3(144.5f, -75, 0);
    private Vector3 lockPickLowerRightPos = new Vector3(193.5f, -75, 0);
    private Vector3 lockPickRightPos = new Vector3(240, -75, 0);
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

    private bool blockMoveAndInteract = true;

    private InputSystem_Actions input;

    private void Awake()
    {
        InitOriginalPosition();
    }

    private void OnEnable()
    {
        InitLockPick();
        InitGameSequence();

        input = new InputSystem_Actions();

        input.LockPick.Enable();

        input.LockPick.MoveLeft.performed += OnMoveLeft;
        input.LockPick.MoveRight.performed += OnMoveRight;
        input.LockPick.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (left) left.transform.position = originalPositionLeft;
        if (lowerLeft) lowerLeft.transform.position = originalPositionLowerLeft;
        if (middle) middle.transform.position = originalPositionMiddle;
        if (lowerRight) lowerRight.transform.position = originalPositionLowerRight;
        if (right) right.transform.position = originalPositionRight;

        currentLockPickIndex = 4;
        if (lockPick) lockPick.transform.localPosition = lockPickPositions[4];

        currentSequenceStep = 0;
        blockMoveAndInteract = true;

        input.LockPick.MoveLeft.performed -= OnMoveLeft;
        input.LockPick.MoveRight.performed -= OnMoveRight;
        input.LockPick.Interact.performed -= OnInteract;

        input.LockPick.Disable();
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

    private void OnMoveLeft(InputAction.CallbackContext ctx)
    {
        if (blockMoveAndInteract) return;
        if (currentSequenceStep >= correctSequenceOrder.Length) return;

        MoveLockPickLeft();
    }

    private void OnMoveRight(InputAction.CallbackContext ctx)
    {
        if (blockMoveAndInteract) return;
        if (currentSequenceStep >= correctSequenceOrder.Length) return;

        MoveLockPickRight();
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (blockMoveAndInteract) return;
        if (currentSequenceStep >= correctSequenceOrder.Length) return;

        TryActivateLockPart();
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
        blockMoveAndInteract = false;
    }

    private void TryActivateLockPart()
    {
        int expectedIndex = correctSequenceOrder[currentSequenceStep];

        if (currentLockPickIndex == expectedIndex)
        {
            RotateLockPickObject();
            Debug.Log($"<color=green>Dobry ruch! Krok {currentSequenceStep + 1} zaliczony.</color>");

            Services.Audio.PlaySFX("LockPickCorrect");

            if (isLeft) MoveLeftUp();
            if (isLowerLeft) MoveLowerLeftUp();
            if (isMiddle) MoveMiddleUp();
            if (isLowerRight) MoveLowerRightUp();
            if (isRight) MoveRightUp();

            currentSequenceStep++;

            if (currentSequenceStep >= correctSequenceOrder.Length)
            {
                StartCoroutine(WinGame());
            }
        }
        else
        {
            Debug.LogError($"<color=red>Z³y ruch! Oczekiwano pozycji '{GetCurrentLockPickPositionName(expectedIndex)}', a jesteœ na '{GetCurrentLockPickPositionName()}'.</color>");
            Services.Audio.PlaySFX("LockPickFail");
            StartCoroutine(PlayWrongMoveAndReset());
        }
    }

    public void RotateLockPickObject()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(RotateObject());
    }

    private IEnumerator RotateObject()
    {
        float elapsedTime = 0f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -30);

        while (elapsedTime < lockPickRotationDuration)
        {
            if (lockPickRectTransform.localScale != Vector3.zero)
            {
                lockPickRectTransform.rotation = Quaternion.Slerp(
                    originalRotation,
                    targetRotation,
                    elapsedTime / lockPickRotationDuration
                );
            }
            else
            {
                Debug.LogWarning("Skala obiektu jest zerowa! Nie mo¿na interpolowaæ rotacji.");
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lockPickRectTransform.rotation = targetRotation;

        yield return null;

        while (elapsedTime < lockPickRotationDuration)
        {
            lockPickRectTransform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / lockPickRotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lockPickRectTransform.rotation = originalRotation;
        rotationCoroutine = null;
    }

    private IEnumerator PlayWrongMoveAndReset()
    {
        blockMoveAndInteract = true;

        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        rotationCoroutine = StartCoroutine(RotateObjectFaul());

        yield return rotationCoroutine;

        ResetGame();

        blockMoveAndInteract = false;
    }

    private IEnumerator RotateObjectFaul()
    {
        float elapsedTime = 0f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, -7);

        while (elapsedTime < lockPickRotationDuration)
        {
            if (lockPickRectTransform.localScale != Vector3.zero)
            {
                lockPickRectTransform.rotation = Quaternion.Slerp(
                    originalRotation,
                    targetRotation,
                    elapsedTime / lockPickRotationDuration
                );
            }
            else
            {
                Debug.LogWarning("Skala obiektu jest zerowa! Nie mo¿na interpolowaæ rotacji.");
                yield break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lockPickRectTransform.rotation = targetRotation;

        yield return null;

        while (elapsedTime < lockPickRotationDuration)
        {
            lockPickRectTransform.rotation = Quaternion.Slerp(targetRotation, originalRotation, elapsedTime / lockPickRotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lockPickRectTransform.rotation = originalRotation;
        rotationCoroutine = null;
    }

    private IEnumerator WinGame()
    {
        blockMoveAndInteract = true;
        yield return new WaitForSeconds(0.5f);

        Inventory.Instance.RemoveItemFromInventoryByID(15);
        UIManager.Instance.RemoveItemFromUIByID(15);
        UIManager.Instance.lockPickPanel.HideLockPickPanel();

        if (animator != null)
        {
            animator.SetTrigger("Open");
        }
        else
        {
            Debug.Log("Nie masz animatora");
        }

        Debug.Log("<color=yellow>GRATULACJE! ZAMEK OTWARTY!</color>");
    }

    private void ResetGame()
    {
        Debug.LogError("Resetowanie gry...");

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