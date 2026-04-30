using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPathMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference thisSphereInput;
    Vector2 mouseDelta;
    bool holdingPPM;

    public float speed = 4f;
    public PathPoint currentPoint;
    public PathPoint resetPosition;

    PathPoint targetPoint;
    bool moving;

    public float raycastLenght = 0.25f;

    Coroutine moveRoutine;

    [SerializeField] LayerMask pointLayer;

    [SerializeField] private Transform rotatinWheel;
    [SerializeField] private Transform staticGround;

    [SerializeField] private CircleAndSquarePuzzle circleAndSquarePuzzle;

    public void OnEnable()
    {
        if (thisSphereInput != null)
        {
            thisSphereInput.action.Enable();
            thisSphereInput.action.performed += OnInputPerformed;
            thisSphereInput.action.canceled += OnInputCanceled;
        }
    }

    private void OnDisable()
    {
        if (thisSphereInput != null)
        {
            thisSphereInput.action.performed -= OnInputPerformed;
            thisSphereInput.action.canceled -= OnInputCanceled;
            thisSphereInput.action.Disable();
        }
    }


    private void WinPuzzle()
    {
        if (currentPoint.isEnd)
        {
            StartCoroutine(CourutineWinPuzzle());
        }

        if (thisSphereInput != null)
        {
            thisSphereInput.action.performed -= OnInputPerformed;
            thisSphereInput.action.canceled -= OnInputCanceled;
            thisSphereInput.action.Disable();
        }

        SetClickAndDrag(false);
    }

    private IEnumerator CourutineWinPuzzle()
    {
        yield return null;

        circleAndSquarePuzzle.ExitAfterWin();
    }

    public void SetClickAndDrag(bool state)
    {
        holdingPPM = state;

        if (!state)
            mouseDelta = Vector2.zero;
    }

    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        SetClickAndDrag(false);
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        if (!holdingPPM) return;

        mouseDelta = context.ReadValue<Vector2>();
        SphereInput();
    }

    void SphereInput()
    {
        if (moving)
            return;

        if (!holdingPPM)
            return;

        if (Mathf.Abs(mouseDelta.y) > Mathf.Abs(mouseDelta.x))
        {
            if (mouseDelta.y > 0)
                TryMove(Vector3.up);
            else
                TryMove(Vector3.down);
        }
        else
        {
            if (mouseDelta.x < 0)
                TryMove(Vector3.back);
            else
                TryMove(Vector3.forward);
        }
    }

    IEnumerator MoveToTarget()
    {
        
        Vector3 startPos = transform.position;
        Vector3 endPos = targetPoint.transform.position;
        float t = 0f;
        Services.Audio.PlaySFX("FastMove");
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        
        transform.position = endPos;
        currentPoint = targetPoint;

        if (currentPoint.stickToWheel)
            transform.SetParent(rotatinWheel.transform);
        else
            transform.SetParent(staticGround.transform);

        raycastLenght = currentPoint.changeRaycastLenght ? 0.12f : 0.25f;

        targetPoint = null;
        moving = false;
        moveRoutine = null;
        WinPuzzle();
    }

    void TryMove(Vector3 dir)
    {
        PathPoint next = FindNeighbor(currentPoint, dir);
        if (!next) return;

        if (!CanMove(currentPoint, dir)) return;

        targetPoint = next;
        moving = true;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveToTarget());


    }

    bool CanMove(PathPoint from, Vector3 dir)
    {
        PathPoint to = FindNeighbor(from, dir);
        if (!to) return false;

        if (dir == Vector3.up)
            return from.piece.WorldUp() && to.piece.WorldDown();

        if (dir == Vector3.down)
            return from.piece.WorldDown() && to.piece.WorldUp();

        if (dir == Vector3.back)
            return from.piece.WorldLeft() && to.piece.WorldRight();

        if (dir == Vector3.forward)
            return from.piece.WorldRight() && to.piece.WorldLeft();

        return false;
    }

    PathPoint FindNeighbor(PathPoint from, Vector3 dir)
    {
        Vector3 origin = from.transform.position + dir * centerOffset;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, raycastLenght, pointLayer))
            return hit.collider.GetComponent<PathPoint>();

        return null;
    }


    public void ResetToCurrentPoint()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        moving = false;
        targetPoint = null;

        transform.position = resetPosition.transform.position;

        if (resetPosition.stickToWheel)
            transform.SetParent(rotatinWheel.transform);
        else
            transform.SetParent(staticGround.transform);

        raycastLenght = resetPosition.changeRaycastLenght ? 0.12f : 0.25f;
    }


    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (currentPoint == null) return;

        DrawRay(Vector3.up);
        DrawRay(Vector3.down);
        DrawRay(Vector3.back);
        DrawRay(Vector3.forward);
#endif
    }

    public float centerOffset;

    void DrawRay(Vector3 dir)
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = currentPoint.transform.position + dir * centerOffset;
        Gizmos.DrawLine(origin, origin + dir * raycastLenght);
    }
}