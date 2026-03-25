using UnityEngine;
using UnityEngine.InputSystem;

public class ClickDragGameMode : ICursorGameMode
{
    private CursorController controller;

    private Transform selectedObject;
    private Vector3 offset;
    private Vector2 lastMousePosition;
    private bool isMovingLoopPlaying;

    private BraiserPuzzleMoveArea moveArea;
    private LayerMask interactableLayer;

    public void Enter(CursorController controller)
    {
        this.controller = controller;
        interactableLayer = controller.InteractableLayer;
        selectedObject = null;
        lastMousePosition = Vector2.zero;
        moveArea = null;
        isMovingLoopPlaying = false;
    }

    public void Exit()
    {
        StopAudio();
        selectedObject = null;
        moveArea = null;
        lastMousePosition = Vector2.zero;
    }

    public void OnInputStarted(InputAction.CallbackContext context)
    {
        if (!context.action.activeControl.path.Contains("leftButton")) return;

        if (selectedObject == null && controller.hasHit)
        {
            selectedObject = controller.currentHit.transform;
            offset = selectedObject.position - controller.currentHit.point;
            lastMousePosition = Mouse.current.position.ReadValue();

            if (controller.currentHit.transform.parent != null && controller.currentHit.transform.parent.childCount > 0)
            {
                moveArea = controller.currentHit.transform.parent .GetChild(0).GetComponent<BraiserPuzzleMoveArea>();
            }
        }
        else
        {
            Exit();
        }
    }

    public void OnInputCanceled(InputAction.CallbackContext context)
    {
        if (!context.action.activeControl.path.Contains("leftButton")) return;
        Exit();
    }

    public void Tick()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (selectedObject == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 delta = mousePos - lastMousePosition;

        if (delta.magnitude > 1f)
        {
            if (!isMovingLoopPlaying)
            {
                Services.Audio.PlayOnLoopSFX("MovingStoneBraiser");
                isMovingLoopPlaying = true;
            }
        }
        else
        {
            StopAudio();
        }

        Plane plane = new Plane(Vector3.up, new Vector3(0, selectedObject.position.y, 0));
        Ray ray = controller.CurrentCamera.ScreenPointToRay(mousePos);

        if (plane.Raycast(ray, out float dist))
        {
            Vector3 hitPoint = ray.GetPoint(dist);
            Vector3 target = hitPoint + offset;

            if (moveArea != null)
            {
                Vector2 min = moveArea.WorldMin;
                Vector2 max = moveArea.WorldMax;

                target.x = Mathf.Clamp(target.x, min.x, max.x);
                target.z = Mathf.Clamp(target.z, min.y, max.y);
            }

            selectedObject.position = new Vector3(
                target.x,
                selectedObject.position.y,
                target.z
            );
        }

        lastMousePosition = mousePos;
    }

    private void StopAudio()
    {
        if (!isMovingLoopPlaying) return;
        Services.Audio.StopLoopSFX("MovingStoneBraiser");
        isMovingLoopPlaying = false;
    }
}