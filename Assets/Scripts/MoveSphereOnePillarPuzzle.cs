using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSphereOnePillarPuzzle : MonoBehaviour
{
    [SerializeField] private MazePoint currentPoint;

    [SerializeField] private InputActionReference dragDeltaInput;
    [SerializeField] private InputActionReference dragHoldInput;
    [SerializeField] private float dragSpeed = 0.01f;
    [SerializeField] private float snapSmooth = 0.06f;
    [SerializeField] private SphereCollider sphereCollider;

    bool holdingPPM;
    Coroutine moveRoutine;

    public bool ActivePuzzle = false;

    [SerializeField] private InteractableItem[] rotatingPillars;
    [SerializeField] private BoxCollider[] rotatingPillarBoxColliders;
    [SerializeField] private ChestManager chestManager;
    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager;


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
        if (ActivePuzzle)
        {
            if (!holdingPPM) return;

            Vector2 delta = context.ReadValue<Vector2>();
            if (moveRoutine != null) return;

            Transform target = GetNextPoint(delta);
            if (target != null)
            {
                if (moveRoutine != null)
                    StopCoroutine(moveRoutine);

                moveRoutine = StartCoroutine(SnapToPosition(target));
            }
        }
    }

    public void SetClickAndDrag(bool state)
    {
        if (ActivePuzzle)
        {
            holdingPPM = state;

            if (!state)
            {
                return;
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
        sphereCollider.enabled = false;

        foreach (var rotatingPillar in rotatingPillars)
        {
            rotatingPillar.canRotateMoveSphereOnePillarPuzzle = false;
        }
        foreach (var rotatingPillarBoxCollider in rotatingPillarBoxColliders)
        {
            rotatingPillarBoxCollider.enabled = false;
        }

        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");

        OpenChest();
        wallTrapdoorsManager.OpenTrapDoor();

    }

    private void OpenChest()
    {
        chestManager.OpenChest();
    }

    Transform GetNextPoint(Vector2 delta)
    {
        if (currentPoint == null) return null;

        Vector3 camRight = Camera.main.transform.right;
        Vector3 camUp = Camera.main.transform.up;

        Vector3 mouseDir = (camRight * delta.x + camUp * delta.y).normalized;

        MazePoint closest = null;
        float maxDot = 0.2f;

        foreach (var neighbor in currentPoint.AllNeighbors)
        {
            if (neighbor == null) continue;

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
        if (snapTarget == null) yield break;

        Vector3 targetPos = snapTarget.position;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, targetPos) > 0.001f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, snapSmooth);
            yield return null;
        }

        transform.position = targetPos;
        currentPoint = snapTarget.GetComponent<MazePoint>();
        moveRoutine = null;

        transform.SetParent(snapTarget);

        moveRoutine = null;
    }
}
