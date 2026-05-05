using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField] private SlotMachineReel[] reels = new SlotMachineReel[5];

    [Header("Spin Settings")]
    [SerializeField] private float delayBetweenReels = 0.12f;
    [SerializeField] private int extraStepsPerNextReel = 4;

    [Header("Optional")]
    [SerializeField] private Animator slotLeverAnimator;
    [SerializeField] private string leverTriggerName = "Pull";

    [Header("Credits")]
    [SerializeField] private int currentCredits;
    [SerializeField] private TextMeshProUGUI creditsText;

    [Header("Bet")]
    [SerializeField] private int currentBet = 1;
    [SerializeField] private TextMeshProUGUI betText;

    [Header("Win UI")]
    [SerializeField] private TextMeshProUGUI winText;

    [Header("Patterns")]
    [SerializeField] private List<SlotPatternData> patterns = new();

    private readonly SlotSymbolData[,] currentGrid = new SlotSymbolData[5, 3];

    private Coroutine spinRoutine;

    public bool IsSpinning => spinRoutine != null;

    [SerializeField] private Color winningSymbolColor = Color.green;


    private void Awake()
    {
        if (patterns == null || patterns.Count == 0)
            patterns = SlotPatternsFactory.CreateDefaultPatterns();
    }

    private void Start()
    {
        currentBet = Mathf.Max(1, currentBet);
        RefreshCreditsText();
        RefreshBetText();
        RefreshWinText(0);
    }


    private void ResetReelColors()
    {
        if (reels == null)
            return;

        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] != null)
                reels[i].ResetRowColors();
        }
    }

    private void HighlightWinningPatterns(List<SlotPatternData> winningPatterns)
    {
        if (winningPatterns == null)
            return;

        for (int i = 0; i < winningPatterns.Count; i++)
        {
            SlotPatternData pattern = winningPatterns[i];

            if (pattern == null || pattern.cells == null)
                continue;

            for (int j = 0; j < pattern.cells.Length; j++)
            {
                Vector2Int cell = pattern.cells[j];

                if (cell.x < 0 || cell.x >= reels.Length)
                    continue;

                if (reels[cell.x] == null)
                    continue;

                reels[cell.x].SetRowColor(cell.y, winningSymbolColor);
            }
        }
    }


    public void PullLever()
    {
        if (spinRoutine != null)
            return;

        if (!HasEnoughCreditsForBet())
        {
            Debug.Log("Wrzuć kredyty, żeby zagrać.");
            return;
        }

        SpendCredits(currentBet);
        RefreshWinText(0);

        if (slotLeverAnimator != null && !string.IsNullOrWhiteSpace(leverTriggerName))
            slotLeverAnimator.SetTrigger(leverTriggerName);

        ResetReelColors();

        spinRoutine = StartCoroutine(SpinAllReelsRoutine());
    }

    private IEnumerator SpinAllReelsRoutine()
    {
        if (reels == null || reels.Length == 0)
        {
            spinRoutine = null;
            yield break;
        }

        int baseSteps = Random.Range(22, 30);

        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] != null)
                reels[i].SpinWithSteps(baseSteps + (i * extraStepsPerNextReel));

            yield return new WaitForSeconds(delayBetweenReels);
        }

        bool anyReelStillSpinning = true;

        while (anyReelStillSpinning)
        {
            anyReelStillSpinning = false;

            for (int i = 0; i < reels.Length; i++)
            {
                if (reels[i] != null && reels[i].IsSpinning)
                {
                    anyReelStillSpinning = true;
                    break;
                }
            }

            yield return null;
        }

        BuildCurrentGrid();

        List<SlotPatternData> winningPatterns = SlotWinEvaluator.GetWinningPatterns(currentGrid, patterns);
        HighlightWinningPatterns(winningPatterns);


        int totalWin = SlotWinEvaluator.CalculateTotalWin(currentGrid, patterns, currentBet);

        if (totalWin > 0)
        {
            currentCredits += totalWin;
            RefreshCreditsText();
        }

        RefreshWinText(totalWin);
        Debug.Log("Slot total win: " + totalWin);

        spinRoutine = null;
    }

    public void AddCredits(int amount)
    {
        if (IsSpinning)
        {
            Debug.Log("Nie można dodawać kredytów podczas kręcenia bębnów.");
            return;
        }

        if (amount <= 0)
            return;

        currentCredits += amount;

        if (currentBet < 1)
            currentBet = 1;

        if (currentBet > currentCredits)
            currentBet = currentCredits;

        RefreshCreditsText();
        RefreshBetText();
    }

    public void InsertCredit()
    {
        AddCredits(1);
    }

    public void IncreaseBet(int amount)
    {
        if (IsSpinning)
            return;

        if (currentCredits <= 0)
            return;

        if (amount <= 0)
            return;

        currentBet = Mathf.Min(currentBet + amount, currentCredits);
        currentBet = Mathf.Max(1, currentBet);

        RefreshBetText();
    }

    public void DecreaseBet(int amount)
    {
        if (IsSpinning)
            return;

        if (amount <= 0)
            return;

        currentBet = Mathf.Max(currentBet - amount, 1);
        RefreshBetText();
    }

    public bool HasEnoughCreditsForBet()
    {
        return currentCredits >= currentBet;
    }

    private void SpendCredits(int amount)
    {
        currentCredits = Mathf.Max(0, currentCredits - amount);

        if (currentCredits <= 0)
            currentBet = 1;
        else if (currentBet > currentCredits)
            currentBet = currentCredits;

        RefreshCreditsText();
        RefreshBetText();
    }

    private void BuildCurrentGrid()
    {
        for (int column = 0; column < reels.Length && column < currentGrid.GetLength(0); column++)
        {
            if (reels[column] == null)
                continue;

            SlotSymbolData[] visibleSymbols = reels[column].GetVisibleSymbols();

            for (int row = 0; row < visibleSymbols.Length && row < currentGrid.GetLength(1); row++)
            {
                currentGrid[column, row] = visibleSymbols[row];
            }
        }
    }

    private void RefreshCreditsText()
    {
        if (creditsText != null)
            creditsText.text = currentCredits.ToString();
    }

    private void RefreshBetText()
    {
        if (betText != null)
            betText.text = currentBet.ToString();
    }

    private void RefreshWinText(int winAmount)
    {
        if (winText != null)
            winText.text = winAmount.ToString();
    }
}
