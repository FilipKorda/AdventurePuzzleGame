using UnityEngine;
using UnityEngine.InputSystem;

public class TwoCrystalsPuzzleGameMode : ICursorGameMode
{
    private CursorController controller;
    private MovingCrystal selectedCrystal;
    private bool isDragging;

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
        if (!controller.HasHit) return;

        Transform hitTransform = controller.CurrentHit.transform;

        if (hitTransform.TryGetComponent(out MovingCrystal movingCrystal))
        {
            selectedCrystal = movingCrystal;
            selectedCrystal.TakeControlOfThisCrystal();

            isDragging = true;
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
        if (selectedCrystal == null) return;

        if (!Mouse.current.leftButton.isPressed)
        {
            isDragging = false;
            ReleaseSelectedCrystal();
            return;
        }

        if (!isDragging) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (mouseDelta.sqrMagnitude < 0.001f)
        {
            selectedCrystal.UnlockNextMove();
            return;
        }

        if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
        {
            selectedCrystal.MoveHorizontal(mouseDelta.x);
        }
        else
        {
            selectedCrystal.MoveVertical(mouseDelta.y);
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
