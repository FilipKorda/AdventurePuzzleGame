using UnityEngine.InputSystem;

public interface ICursorGameMode
{
    void Enter(CursorController controller);
    void Exit();
    void OnInputStarted(InputAction.CallbackContext context);
    void OnInputCanceled(InputAction.CallbackContext context);
    void Tick();
}