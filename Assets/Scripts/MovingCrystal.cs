using System;
using System.Collections;
using UnityEngine;

public class MovingCrystal : MonoBehaviour
{
    public event Action<MovingCrystal, bool> OnActiveStateChanged;
    [SerializeField] private bool isActive;
    [SerializeField] private float inputThreshold = 0.2f;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private TwoCrystalsGridManager gridManager;

    public bool isPlayer = false;
    public bool IsActive => isActive;
    public BoxCollider boxCollider;

    private bool isControlled = false;
    private bool canMove = true;
    private bool isMoving = false;
    private Coroutine moveCoroutine;



    private void OnEnable()
    {
        SetActiveState(true);
    }


    public void SetActiveState(bool value)
    {
        if (isActive == value) return;

        isActive = value;
        OnActiveStateChanged?.Invoke(this, isActive);
    }

    public void TakeControlOfThisCrystal()
    {
        isControlled = true;
    }

    public void ReleaseControl()
    {
        isControlled = false;
        canMove = true;
    }

    public void MoveHorizontal(float mouseXDelta)
    {
        if (!isControlled) return;
        if (!canMove) return;
        if (isMoving) return;
        if (Mathf.Abs(mouseXDelta) < inputThreshold) return;
        if (gridManager == null) return;

        int deltaX = mouseXDelta > 0f ? 1 : -1;

        if (gridManager.TryMoveCrystal(this, 0, deltaX, out Vector3 targetWorldPosition))
        {
            canMove = false;
            StartMoveAnimation(targetWorldPosition);
        }
    }

    public void MoveVertical(float mouseYDelta)
    {
        if (!isControlled) return;
        if (!canMove) return;
        if (isMoving) return;
        if (Mathf.Abs(mouseYDelta) < inputThreshold) return;
        if (gridManager == null) return;

        int deltaY = mouseYDelta > 0f ? 1 : -1;

        if (gridManager.TryMoveCrystal(this, deltaY, 0, out Vector3 targetWorldPosition))
        {
            canMove = false;
            StartMoveAnimation(targetWorldPosition);
        }
    }

    public void UnlockNextMove()
    {
        if (isMoving) return;
        canMove = true;
    }

    private void StartMoveAnimation(Vector3 targetWorldPosition)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveToPositionCoroutine(targetWorldPosition));
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 targetWorldPosition)
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < moveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / moveDuration);

            transform.position = Vector3.Lerp(startPosition, targetWorldPosition, t);
            yield return null;
        }

        transform.position = targetWorldPosition;
      

        if (isPlayer)
        {
            gridManager.CheckWinPuzzle();
        }
        isMoving = false;
    }
}
