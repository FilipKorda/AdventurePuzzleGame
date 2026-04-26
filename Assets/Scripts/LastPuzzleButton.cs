using UnityEngine;

public class LastPuzzleButton : MonoBehaviour
{
    public bool isButtonActive;

    public Vector2Int index;
    private LastPuzzleToSolveManager manager;

    public Color activeColor;
    public Color deactiveColor;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        UpdateColor();
    }

    public void Init(Vector2Int idx, LastPuzzleToSolveManager mgr)
    {
        index = idx;
        manager = mgr;
        UpdateColor();
    }

    public void Toggle()
    {
        isButtonActive = !isButtonActive;
        UpdateColor();
    }

    public void SetState(bool state)
    {
        isButtonActive = state;
        UpdateColor();
    }

    public void ChangeNeiboursStatusButtons()
    {
        Services.Audio.PlaySFX("KeyboardMechanical");

        manager.ToggleAt(index);
        manager.ToggleAt(index + Vector2Int.left);
        manager.ToggleAt(index + Vector2Int.right);
        manager.ToggleAt(index + Vector2Int.up);
        manager.ToggleAt(index + Vector2Int.down);

        manager.CheckSolved();
    }

    private void UpdateColor()
    {
        rend.material.color = isButtonActive ? activeColor : deactiveColor;
    }
}
