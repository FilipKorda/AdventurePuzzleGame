using UnityEngine;
using UnityEngine.InputSystem;

public class AllSquareClickGameMode : ICursorGameMode
{
    private CursorController controller;

    public void Enter(CursorController controller)
    {
        this.controller = controller;
    }

    public void Exit()
    {
    }

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (!controller.HasHit) return;

        Transform hitTransform = controller.CurrentHit.transform;

        if (hitTransform.TryGetComponent(out SquareInAllSquarePuzzle square))
        {
            SquareSelectionManager.Instance.TrySelectSquare(square);
        }
    }

    public void OnDragInputStarted(InputAction.CallbackContext context)
    {
    }

    public void OnDragInputCanceled(InputAction.CallbackContext context)
    {
    }

    public void Tick()
    {
    }
}
