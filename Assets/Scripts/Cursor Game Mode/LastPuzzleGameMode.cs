using UnityEngine;
using UnityEngine.InputSystem;

public class LastPuzzleGameMode : ICursorGameMode
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
        if (!context.started) return;
        if (!controller.HasHit) return;

        Transform hitTransform = controller.CurrentHit.transform;

        if (hitTransform.TryGetComponent(out LastPuzzleButton lastPuzzleButton))
        {
            lastPuzzleButton.ChangeNeiboursStatusButtons();
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
