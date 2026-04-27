using UnityEngine;
using UnityEngine.InputSystem;

public class TwoCrystalsPuzzleGameMode : ICursorGameMode
{
    private CursorController controller;
    private MovingCrystal selectedCrystal;
    private bool isDragging;
    private Vector2 lastCursorPosition;

    public void Enter(CursorController controller)
    {
        this.controller = controller;
        selectedCrystal = null;
        isDragging = false;
    }

    public void Exit()
    {
        ReleaseSelectedCrystal();
        StopAudio();
    }

    public void OnClickInput(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!controller.HasHit) return;

        Transform hitTransform = controller.CurrentHit.transform;

        if (hitTransform.TryGetComponent(out MovingCrystal movingCrystal))
        {
            if (selectedCrystal != null && selectedCrystal != movingCrystal)
            {
                selectedCrystal.ReleaseControl();
            }

            selectedCrystal = movingCrystal;
            selectedCrystal.TakeControlOfThisCrystal();

            isDragging = true;
            lastCursorPosition = controller.GetCursorScreenPosition();
        }
    }

    public void OnDragInputStarted(InputAction.CallbackContext context)
    {
        if (selectedCrystal == null) return;

        isDragging = true;
        lastCursorPosition = controller.GetCursorScreenPosition();
    }

    public void OnDragInputCanceled(InputAction.CallbackContext context)
    {
        isDragging = false;
        ReleaseSelectedCrystal();
    }

    public void Tick()
    {
        if (selectedCrystal == null) return;
        if (!isDragging) return;

        Vector2 currentCursorPosition = controller.GetCursorScreenPosition();
        Vector2 cursorDelta = currentCursorPosition - lastCursorPosition;
        lastCursorPosition = currentCursorPosition;

        if (cursorDelta.sqrMagnitude < 0.001f)
        {
            selectedCrystal.UnlockNextMove();
            return;
        }

        if (Mathf.Abs(cursorDelta.x) > Mathf.Abs(cursorDelta.y))
        {
            selectedCrystal.MoveHorizontal(cursorDelta.x);
        }
        else
        {
            selectedCrystal.MoveVertical(cursorDelta.y);
        }
    }

    private void ReleaseSelectedCrystal()
    {
        if (selectedCrystal == null) return;

        selectedCrystal.ReleaseControl();
        selectedCrystal = null;
    }

    private void StopAudio()
    {
        // Services.Audio.StopLoopSFX("MovingStoneBraiser");
    }
}
