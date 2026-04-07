using UnityEngine.InputSystem;

public interface ICursorGameMode
{
    void Enter(CursorController controller);

    void Exit();

    void OnClickInput(InputAction.CallbackContext context);

    void OnDragInputStarted(InputAction.CallbackContext context);

    void OnDragInputCanceled(InputAction.CallbackContext context);

    void Tick();
}