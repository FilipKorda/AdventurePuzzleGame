using UnityEngine;
using UnityEngine.InputSystem;

public class MovingBlockClickDragGameMode : ICursorGameMode
{
    private CursorController controller;

    private Transform selectedObject;
    private MovableBlockRotatingPillar movable;

    private Vector3 offset;
    private bool isDragging;
    private Vector2 lastCursorPosition;
    private float maxMovePerFrame = 0.01f;

    private float mouseMoveSpeed = 0.001f;
    private float gamepadMoveSpeed = 0.0005f;


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
        lastCursorPosition = controller.GetCursorScreenPosition();

        Ray ray = controller.GetRayFromScreenPoint(controller.GetCursorScreenPosition());
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
     
    }

    public void Tick()
    {
        if (selectedObject == null)
            return;

        if (!controller.IsClickHeld())
        {
            isDragging = false;
            selectedObject = null;
            movable = null;
            return;
        }

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 cursorPos = controller.GetCursorScreenPosition();
        Vector2 delta = cursorPos - lastCursorPosition;
        lastCursorPosition = cursorPos;

        float speed = controller.IsUsingGamepadCursor ? gamepadMoveSpeed : mouseMoveSpeed;

        Vector3 move;

        if (movable.axis == MoveAxis.X)
            move = new Vector3(delta.x * speed, 0f, 0f);
        else
            move = new Vector3(0f, delta.y * speed, 0f);

        move = Vector3.ClampMagnitude(move, maxMovePerFrame);

        movable.TryMove(move);
    }

}
