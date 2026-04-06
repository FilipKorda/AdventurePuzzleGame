using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBlockBriefcase : MonoBehaviour
{
    public Transform arrowObject;
    [SerializeField] private Transform[] arrowPoints;
    private int currentArrowIndex = 0;
    private Coroutine arrowRoutine;

    public Transform leftPoint;
    public Transform centerPoint;
    public Transform rightPoint;

    [SerializeField] private InputActionReference dragDeltaInput;
    [SerializeField] private InputActionReference dragHoldInput;

    [SerializeField] private float dragSpeed = 0.01f;
    [SerializeField] private float snapSmooth = 0.06f;
    [SerializeField] private float returnSmooth = 0.15f;
    [SerializeField] private float stayTime = 0.3f;

    bool holdingPPM;
    Vector3 velocity;
    Coroutine moveRoutine;

    [SerializeField] private BriefcaseManager briefcaseManager;
    [SerializeField] private BoxCollider[] boxCollidersToDisable;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator animatorPrefabBriefcase;
    [SerializeField] private Animator animatorBriefcase;

    private enum Direction
    {
        Left,
        Right
    }

    private Direction[] winningSequence = new Direction[]
    {
    Direction.Left,
    Direction.Right,
    Direction.Right,
    Direction.Left,
    Direction.Right
    };

    private List<Direction> currentSequence = new List<Direction>();

    void Start()
    {
        StartArrowAtFirstPoint();
        transform.position = centerPoint.position;
    }

    void OnEnable()
    {
        if (dragDeltaInput != null)
        {
            dragDeltaInput.action.Enable();
            dragDeltaInput.action.performed += OnDragDelta;
        }

        if (dragHoldInput != null)
        {
            dragHoldInput.action.Enable();
            dragHoldInput.action.performed += ctx => SetClickAndDrag(true);
            dragHoldInput.action.canceled += ctx => SetClickAndDrag(false);
        }
    }

    void OnDisable()
    {
        if (dragDeltaInput != null)
        {
            dragDeltaInput.action.performed -= OnDragDelta;
            dragDeltaInput.action.Disable();
        }

        if (dragHoldInput != null)
        {
            dragHoldInput.action.performed -= ctx => SetClickAndDrag(true);
            dragHoldInput.action.canceled -= ctx => SetClickAndDrag(false);
            dragHoldInput.action.Disable();
        }
    }

    void OnDragDelta(InputAction.CallbackContext context)
    {
        if (!holdingPPM) return;

        Vector2 delta = context.ReadValue<Vector2>();
        Vector3 offset = new(-delta.x * dragSpeed, 0f, 0f);
        transform.position += offset;
    }

    public void SetClickAndDrag(bool state)
    {
        holdingPPM = state;

        if (!state)
        {
            Transform target = GetClosestPoint();

            if (moveRoutine != null)
                StopCoroutine(moveRoutine);

            moveRoutine = StartCoroutine(SnapThenReturn(target));
        }
        else
        {
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
        }
    }

    Transform GetClosestPoint()
    {
        float dLeft = Vector3.Distance(transform.position, leftPoint.position);
        float dRight = Vector3.Distance(transform.position, rightPoint.position);
        float dCenter = Vector3.Distance(transform.position, centerPoint.position);

        if (dLeft < dRight && dLeft < dCenter)
        {
            MoveArrowOneStep();
            RecordDirection(Direction.Left);
            return leftPoint;
        }

        if (dRight < dLeft && dRight < dCenter)
        {
            MoveArrowOneStep();
            RecordDirection(Direction.Right);
            return rightPoint;
        }

        return centerPoint;
    }

    IEnumerator SnapThenReturn(Transform snapTarget)
    {
        yield return MoveSmooth(snapTarget.position, snapSmooth);
        yield return new WaitForSeconds(stayTime);
        yield return MoveSmooth(centerPoint.position, returnSmooth);
    }

    IEnumerator MoveSmooth(Vector3 target, float smooth)
    {
        velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target,
                ref velocity,
                smooth
            );

            yield return null;
        }

        transform.position = target;
    }

    #region Arrow Movement
    void StartArrowAtFirstPoint()
    {
        if (arrowPoints.Length == 0) return;

        arrowObject.position = arrowPoints[0].position;
        currentArrowIndex = 1;
    }

    public void MoveArrowOneStep()
    {
        if (arrowPoints.Length == 0 || currentArrowIndex >= arrowPoints.Length) return;

        Transform target = arrowPoints[currentArrowIndex];

        if (arrowRoutine != null)
            StopCoroutine(arrowRoutine);

        arrowRoutine = StartCoroutine(MoveArrowToPoint(target.position));

        currentArrowIndex++;
    }

    IEnumerator MoveArrowToPoint(Vector3 target)
    {
        Vector3 localVelocity = Vector3.zero;
        float smooth = 0.1f;

        while (Vector3.Distance(arrowObject.position, target) > 0.001f)
        {
            arrowObject.position = Vector3.SmoothDamp(
                arrowObject.position,
                target,
                ref localVelocity,
                smooth
            );

            yield return null;
        }

        arrowObject.position = target;
    }
    #endregion


    private void RecordDirection(Direction dir)
    {
        currentSequence.Add(dir);

        if (currentSequence.Count > winningSequence.Length)
            currentSequence.RemoveAt(0);

        CheckPuzzleStatus();
    }

    private void CheckPuzzleStatus()
    {
        if (currentSequence.Count < winningSequence.Length)
            return;

        for (int i = 0; i < winningSequence.Length; i++)
        {
            if (currentSequence[i] != winningSequence[i])
            {
                ResetPuzzle();
                return;
            }
        }

        Debug.Log("Wygrałeś!");

        StartCoroutine(WinPuzzle());
    }

    public void ResetPuzzle()
    {
        currentSequence.Clear();

        if (arrowRoutine != null)
        {
            StopCoroutine(arrowRoutine);
            arrowRoutine = null;
        }

        arrowObject.position = arrowPoints[0].position;
        currentArrowIndex = 1;

        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }
    }

    private IEnumerator WinPuzzle()
    {
        foreach (var boxCollider in boxCollidersToDisable)
        {
            boxCollider.enabled = false;
        }

        animator.SetTrigger("Open");
        animatorPrefabBriefcase.SetTrigger("Open");
        animatorBriefcase.SetTrigger("Open");

        yield return new WaitForSeconds(1f);

        briefcaseManager.ExitAfterWin();
      
    }
}