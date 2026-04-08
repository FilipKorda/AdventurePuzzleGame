using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBlockClickDragGameMode : ICursorGameMode
{
    private CursorController controller;

    private Transform selectedObject;
    private MovableBlockRotatingPillar movable;

    private Vector3 offset;
    private bool isDragging;
    private Vector2 lastMousePosition;
    private float maxMovePerFrame = 0.01f;

    public void Enter(CursorController controller)
    {
        this.controller = controller;
        selectedObject = null;
        movable = null;
        isDragging = false;
    }

    public void Exit()
    {
        selectedObject = null;
        movable = null;
        isDragging = false;
    }

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (!controller.HasHit)
        {
            selectedObject = null;
            movable = null;
            return;
        }

        MovableBlockRotatingPillar pillar =
            controller.CurrentHit.transform.GetComponent<MovableBlockRotatingPillar>();


        if (pillar == null)
        {
            selectedObject = null;
            movable = null;
            return;
        }

        selectedObject = pillar.transform;
        movable = pillar;

        offset = selectedObject.position - controller.CurrentHit.point;

    }

    public void OnDragInputStarted(InputAction.CallbackContext context)
    {
        if (selectedObject == null || movable == null)
            return;

        isDragging = true;
        lastMousePosition = Mouse.current.position.ReadValue();

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

    public void OnDragInputCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
    }

    public void Tick()
    {
        if (selectedObject == null || movable == null || !isDragging)
            return;

        HandleMovement();
    }


    private void HandleMovement()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 delta = mousePos - lastMousePosition;
        lastMousePosition = mousePos;

        Vector3 move;

        if (movable.axis == MoveAxis.X)
            move = new Vector3(delta.x * 0.01f, 0f, 0f);
        else
            move = new Vector3(0f, delta.y * 0.01f, 0f);

        move = Vector3.ClampMagnitude(move, maxMovePerFrame);

        movable.TryMove(move);
    }
}