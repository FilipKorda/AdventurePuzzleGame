using UnityEngine;
using UnityEngine.InputSystem;

public class ClickDragGameMode : ICursorGameMode
{
    private CursorController controller;

    private Transform selectedObject;
    private Vector3 offset;
    private bool isMovingLoopPlaying;
    private bool isDragging;

    private BraiserPuzzleMoveArea moveArea;

    private float lastMoveTime;
    private float movementStopDelay = 0.1f;
    private float movementThreshold = 0.000001f;
    public void Enter(CursorController controller)
    {
        this.controller = controller;
        selectedObject = null;
        moveArea = null;
        isMovingLoopPlaying = false;
    }

    public void Exit()
    {
        StopAudio();
        selectedObject = null;
        moveArea = null;
        isMovingLoopPlaying = false;
    }

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (controller.HasHit)
        {
            selectedObject = controller.CurrentHit.transform;
            offset = selectedObject.position - controller.CurrentHit.point;

            if (selectedObject.parent != null && selectedObject.parent.childCount > 0)
            {
                moveArea = selectedObject.parent.GetChild(0)
                    .GetComponent<BraiserPuzzleMoveArea>();
            }
        }
        else
        {
            selectedObject = null;
        }
    }

    public void OnDragInputStarted(InputAction.CallbackContext context)
    {
        if (!controller.HasHit)
        {
            selectedObject = null;
            StopAudio();
        }

        if (selectedObject != null)
        {
            isDragging = true;

            Ray ray = controller.GetRayFromScreenPoint(Mouse.current.position.ReadValue());
            Plane plane = new Plane(Vector3.up, new Vector3(0, selectedObject.position.y, 0));

            if (plane.Raycast(ray, out float dist))
            {
                Vector3 hitPoint = ray.GetPoint(dist);
                offset = new Vector3(
                    selectedObject.position.x - hitPoint.x,
                    0f,
                    selectedObject.position.z - hitPoint.z
                );
            }
        }
    }

    public void OnDragInputCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;

    }

    public void Tick()
    {
        if (selectedObject == null) return;

        bool isCurrentlyMoving = isDragging && HandleMovement();

        if (isCurrentlyMoving)
        {
            lastMoveTime = Time.time;

            if (!isMovingLoopPlaying)
            {
                Services.Audio.PlayOnLoopSFX("MovingStoneBraiser");
                isMovingLoopPlaying = true;
            }
        }
        else
        {
            if (isMovingLoopPlaying && Time.time - lastMoveTime > movementStopDelay)
            {
                StopAudio();
            }
        }
    }

    private bool HandleMovement()
    {
        Ray ray = controller.GetRayFromScreenPoint(Mouse.current.position.ReadValue());
        Plane plane = new Plane(Vector3.up, new Vector3(0, selectedObject.position.y, 0));

        if (!plane.Raycast(ray, out float dist)) return false;

        Vector3 hitPoint = ray.GetPoint(dist);

        Vector3 target = new Vector3(
            hitPoint.x + offset.x,
            selectedObject.position.y,
            hitPoint.z + offset.z
        );

        if (moveArea != null)
        {
            Vector2 min = moveArea.WorldMin;
            Vector2 max = moveArea.WorldMax;

            target.x = Mathf.Clamp(target.x, min.x, max.x);
            target.z = Mathf.Clamp(target.z, min.y, max.y);
        }

        if ((selectedObject.position - target).sqrMagnitude > movementThreshold)
        {
            selectedObject.position = target;
            return true;
        }

        return false;
    }

    private void StopAudio()
    {
        Services.Audio.StopLoopSFX("MovingStoneBraiser");
        isMovingLoopPlaying = false;
    }
}