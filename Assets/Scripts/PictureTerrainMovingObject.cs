using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PictureTerrainMovingObject : MonoBehaviour
{
    static PictureTerrainMovingObject activeObject;
    [SerializeField] private PictureTarrainMazePoint currentPoint;

    [SerializeField] private InputActionReference dragDeltaInput;
    [SerializeField] private InputActionReference dragHoldInput;
    [SerializeField] private float dragSpeed = 0.01f;
    [SerializeField] private float snapSmooth = 0.06f;
    [SerializeField] private SphereCollider sphereCollider;

    bool holdingPPM;
    Coroutine moveRoutine;

    public bool ActivePuzzle = false;

    bool isSelected;


    void OnEnable()
    {
        dragDeltaInput.action.Enable();
        dragDeltaInput.action.performed += OnDragDelta;

        dragHoldInput.action.Enable();
        dragHoldInput.action.started += OnDragHoldStarted;
        dragHoldInput.action.canceled += OnDragHoldCanceled;
    }


    void OnDisable()
    {
        dragDeltaInput.action.performed -= OnDragDelta;
        dragDeltaInput.action.Disable();

        dragHoldInput.action.started -= OnDragHoldStarted;
        dragHoldInput.action.canceled -= OnDragHoldCanceled;
        dragHoldInput.action.Disable();
    }

    void OnDragHoldStarted(InputAction.CallbackContext ctx)
    {
        SetClickAndDrag(true);
    }

    void OnDragHoldCanceled(InputAction.CallbackContext ctx)
    {
        SetClickAndDrag(false);
    }

    void OnDragDelta(InputAction.CallbackContext context)
    {
        if (!ActivePuzzle)
            return;

        if (!holdingPPM || !isSelected)
            return;

        Vector2 delta = context.ReadValue<Vector2>();
        if (moveRoutine != null)
            return;

        Transform target = GetNextPoint(delta);
        if (target == null)
            return;

        moveRoutine = StartCoroutine(SnapToPosition(target));
    }


    void Start()
    {
        if (currentPoint != null)
            currentPoint.TryOccupy(this);
    }

    public void SetClickAndDrag(bool state)
    {
        if (!ActivePuzzle)
            return;

        if (state)
        {
            if (activeObject != null && activeObject != this)
                return;

            activeObject = this;
            isSelected = true;
            holdingPPM = true;

            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }
        }
        else
        {
            if (activeObject == this)
                activeObject = null;

            isSelected = false;
            holdingPPM = false;
        }
    }

    Transform GetNextPoint(Vector2 delta)
    {
        if (currentPoint == null) return null;

        Vector3 camRight = Camera.main.transform.right;
        Vector3 camUp = Camera.main.transform.up;

        Vector3 mouseDir = (camRight * delta.x + camUp * delta.y).normalized;

        PictureTarrainMazePoint closest = null;
        float maxDot = 0.2f;

        foreach (var neighbor in currentPoint.AllNeighbors)
        {
            if (neighbor == null) continue;
            if (neighbor.IsOccupied) continue;

            Vector3 dirToNeighbor = (neighbor.transform.position - currentPoint.transform.position).normalized;

            float dot = Vector3.Dot(mouseDir, dirToNeighbor);

            if (dot > maxDot)
            {
                maxDot = dot;
                closest = neighbor;
            }
        }

        if (closest != null && maxDot > 0.5f)
        {
            return closest.transform;
        }
        return null;
    }

    IEnumerator SnapToPosition(Transform snapTarget)
    {
        var nextPoint = snapTarget.GetComponent<PictureTarrainMazePoint>();
        if (nextPoint == null) yield break;
        if (!nextPoint.TryOccupy(this)) yield break;

        if (currentPoint != null)
            currentPoint.Release(this);

        Vector3 targetPos = snapTarget.position;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetPos) > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, snapSmooth);
            yield return null;
        }

        transform.position = targetPos;
        currentPoint = nextPoint;
        moveRoutine = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WinTriggerTenRoomPuzzle"))
        {
            WinPuzzle();
        }
    }


    private void WinPuzzle()
    {
        ActivePuzzle = false;

        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");

    }
 
}
