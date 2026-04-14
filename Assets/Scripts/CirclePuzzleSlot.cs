using UnityEngine;

public class CirclePuzzleSlot : MonoBehaviour
{
    [SerializeField] private CirclePuzzleSymbol initialSymbol;

    public CirclePuzzleSymbol CurrentSymbol { get; private set; }
    public bool HasSymbol => CurrentSymbol != null;

    private void Awake()
    {
        CurrentSymbol = initialSymbol;
        SnapCurrentSymbolToSlot();
    }

    public void SetSymbol(CirclePuzzleSymbol symbol)
    {
        CurrentSymbol = symbol;
    }

    public void ClearSymbol()
    {
        CurrentSymbol = null;
    }

    public void SnapCurrentSymbolToSlot()
    {
        if (CurrentSymbol == null) return;
        CurrentSymbol.transform.position = transform.position;
    }
}
