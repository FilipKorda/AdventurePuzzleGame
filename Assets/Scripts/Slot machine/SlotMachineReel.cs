using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineReel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image[] visibleRows = new Image[3];

    [Header("Symbols")]
    [SerializeField] private SlotSymbolData[] symbols;

    [Header("Reel Visual")]
    [SerializeField] private Transform reelVisual;
    [SerializeField] private float spinRotationPerStep = 45f;

    [Header("Spin Settings")]
    [SerializeField] private float startStepInterval = 0.04f;
    [SerializeField] private float endStepInterval = 0.14f;
    [SerializeField] private int minSpinSteps = 22;
    [SerializeField] private int maxSpinSteps = 36;

    private readonly SlotSymbolData[] currentVisibleSymbols = new SlotSymbolData[3];

    private Coroutine spinRoutine;
    private float baseXRotation;
    private float baseYRotation;
    private float currentVisualZRotation;

    public bool IsSpinning => spinRoutine != null;

    [Header("Colors")]
    [SerializeField] private Color defaultSymbolColor = Color.white;


    private void Awake()
    {
        if (reelVisual == null)
            reelVisual = transform;

        Vector3 startEuler = reelVisual.localEulerAngles;
        baseXRotation = startEuler.x;
        baseYRotation = startEuler.y;
        currentVisualZRotation = startEuler.z;

        InitializeVisibleSymbols();
        RefreshVisibleSymbols();
        ResetRowColors();
    }


    public void ResetRowColors()
    {
        if (visibleRows == null)
            return;

        for (int i = 0; i < visibleRows.Length; i++)
        {
            if (visibleRows[i] != null)
                visibleRows[i].color = defaultSymbolColor;
        }
    }

    public void SetRowColor(int rowIndex, Color color)
    {
        if (visibleRows == null)
            return;

        if (rowIndex < 0 || rowIndex >= visibleRows.Length)
            return;

        if (visibleRows[rowIndex] != null)
            visibleRows[rowIndex].color = color;
    }



    public void SpinRandom()
    {
        if (symbols == null || symbols.Length == 0)
            return;

        int steps = Random.Range(minSpinSteps, maxSpinSteps + 1);
        SpinWithSteps(steps);
    }

    public void SpinWithSteps(int steps)
    {
        if (symbols == null || symbols.Length == 0)
            return;

        if (spinRoutine != null)
            StopCoroutine(spinRoutine);

        spinRoutine = StartCoroutine(SpinRoutine(steps));
    }

    public SlotSymbolData[] GetVisibleSymbols()
    {
        return new[]
        {
            currentVisibleSymbols[0],
            currentVisibleSymbols[1],
            currentVisibleSymbols[2]
        };
    }

    private IEnumerator SpinRoutine(int totalSteps)
    {
        totalSteps = Mathf.Max(1, totalSteps);

        for (int step = 0; step < totalSteps; step++)
        {
            ShiftSymbols();
            RefreshVisibleSymbols();

            if (reelVisual != null)
            {
                currentVisualZRotation += spinRotationPerStep;
                reelVisual.localRotation = Quaternion.Euler(baseXRotation, baseYRotation, currentVisualZRotation);
            }

            float progress = totalSteps <= 1 ? 1f : step / (float)(totalSteps - 1);
            float waitTime = Mathf.Lerp(startStepInterval, endStepInterval, progress);

            yield return new WaitForSeconds(waitTime);
        }

        spinRoutine = null;
    }

    private void InitializeVisibleSymbols()
    {
        for (int i = 0; i < currentVisibleSymbols.Length; i++)
        {
            currentVisibleSymbols[i] = SlotSymbolRandomizer.GetRandomSymbol(symbols);
        }
    }

    private void ShiftSymbols()
    {
        currentVisibleSymbols[0] = currentVisibleSymbols[1];
        currentVisibleSymbols[1] = currentVisibleSymbols[2];
        currentVisibleSymbols[2] = SlotSymbolRandomizer.GetRandomSymbol(symbols);
    }

    private void RefreshVisibleSymbols()
    {
        if (visibleRows == null || visibleRows.Length < 3)
            return;

        for (int i = 0; i < visibleRows.Length; i++)
        {
            if (visibleRows[i] == null)
                continue;

            visibleRows[i].sprite = currentVisibleSymbols[i] != null ? currentVisibleSymbols[i].sprite : null;
        }
    }
}
