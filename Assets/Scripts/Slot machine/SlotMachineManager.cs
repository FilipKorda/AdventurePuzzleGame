using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField] private SlotMachineReel[] reels = new SlotMachineReel[5];

    [Header("Spin Settings")]
    [SerializeField] private float delayBetweenReels = 0.12f;
    [SerializeField] private int extraStepsPerNextReel = 4;

    [Header("Optional")]
    [SerializeField] private Animator slotLeverAnimator;
    [SerializeField] private string leverTriggerName = "Pull";
    [SerializeField] private Animator coinInsertAnim;

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

    private int spinBet;

    [SerializeField] private LocalizedString inserCreditsNotification;
    [SerializeField] private LocalizedString udontHaveMoneyNotification;


    [Header("Demo Mode")]
    [SerializeField] private bool demoMode = true;
    [SerializeField][Range(0f, 1f)] private float demoBonusWinChance = 0.35f;


    [SerializeField] private float highlightPatternDuration = 0.5f;
    [SerializeField] private float delayBetweenPatterns = 0.15f;

    private Coroutine highlightRoutine;

    [SerializeField] private float winCountAnimationDuration = 0.35f;

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

    public void PullLever()
    {
        if (spinRoutine != null)
            return;

        if (!HasEnoughCreditsForBet())
        {
            if (inserCreditsNotification != null)
                NotificationSystem.Instance.ShowNotification(inserCreditsNotification, 2);
            return;
        }

        spinBet = currentBet;
        SpendCredits(spinBet);
        RefreshWinText(0);

        if (slotLeverAnimator != null)
            slotLeverAnimator.SetTrigger(leverTriggerName);

        if (highlightRoutine != null)
        {
            StopCoroutine(highlightRoutine);
            highlightRoutine = null;
        }

        ResetAllReelColors();


        spinRoutine = StartCoroutine(SpinAllReelsRoutine());
        Services.Audio.PlaySFX("PULLLEVERSLOTMACHINE");
    }

    private void ResetAllReelColors()
    {
        if (reels == null)
            return;

        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] != null)
                reels[i].ResetRowColors();
        }
    }
    private void HighlightSinglePattern(SlotPatternData pattern)
    {
        if (pattern == null || pattern.cells == null)
            return;

        for (int i = 0; i < pattern.cells.Length; i++)
        {
            Vector2Int cell = pattern.cells[i];

            if (cell.x < 0 || cell.x >= reels.Length)
                continue;

            if (reels[cell.x] == null)
                continue;

            reels[cell.x].SetRowColor(cell.y, winningSymbolColor);
        }
    }

    private IEnumerator HighlightWinningPatternsSequence(List<SlotPatternData> winningPatterns)
    {
        if (winningPatterns == null || winningPatterns.Count == 0)
            yield break;

        int displayedWin = 0;
        RefreshWinText(0);

        for (int i = 0; i < winningPatterns.Count; i++)
        {
            SlotPatternData pattern = winningPatterns[i];

            ResetAllReelColors();
            HighlightSinglePattern(pattern);

            int patternWin = SlotWinEvaluator.CalculatePatternWin(currentGrid, pattern, spinBet);
            int targetWin = displayedWin + patternWin;

            Services.Audio.PlaySFX("WINSLOTMACHINE");

            yield return StartCoroutine(AnimateWinText(displayedWin, targetWin, winCountAnimationDuration));

            displayedWin = targetWin;

            yield return new WaitForSeconds(highlightPatternDuration);

            ResetAllReelColors();
            yield return new WaitForSeconds(delayBetweenPatterns);
        }

        for (int i = 0; i < winningPatterns.Count; i++)
        {
            HighlightSinglePattern(winningPatterns[i]);
        }
    }



    private IEnumerator SpinAllReelsRoutine()
    {
        if (reels == null || reels.Length == 0)
        {
            spinRoutine = null;
            yield break;
        }

        int baseSteps = 22;

        for (int i = 0; i < reels.Length; i++)
        {
            if (reels[i] != null)
                reels[i].SpinWithSteps(baseSteps + (i * extraStepsPerNextReel));
            Services.Audio.PlaySFX("SPINNINGREEL");
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

        if (demoMode)
            TryApplyDemoBonusWin();


        List<SlotPatternData> winningPatterns = SlotWinEvaluator.GetWinningPatterns(currentGrid, patterns);
        int totalWin = SlotWinEvaluator.CalculateTotalWin(currentGrid, patterns, spinBet);

        if (totalWin > 0)
        {
            currentCredits += totalWin;
            RefreshCreditsText();
        }
        else
        {
            RefreshWinText(0);
        }

        if (winningPatterns.Count > 0)
        {
            if (highlightRoutine != null)
                StopCoroutine(highlightRoutine);

            highlightRoutine = StartCoroutine(HighlightWinningPatternsSequence(winningPatterns));
        }


        spinRoutine = null;
    }

    public void AddCreditsFromInventory(int amount)
    {
        if (IsSpinning)
        {
            return;
        }

        if (amount <= 0)
            return;

        if (Inventory.Instance == null)
        {
            Debug.Log("Brak Inventory.Instance.");
            return;
        }

        if (!Inventory.Instance.SpendCoins(amount))
        {
            if (inserCreditsNotification != null)
                NotificationSystem.Instance.ShowNotification(udontHaveMoneyNotification, 2);
            return;
        }

        coinInsertAnim.SetTrigger("Insert");
        Services.Audio.PlaySFX("SlotMachineInsert");

        currentCredits += amount;

        if (currentBet < 1)
            currentBet = 1;

        if (currentBet > currentCredits)
            currentBet = currentCredits;

        RefreshCreditsText();
        RefreshBetText();
    }


    public void InsertTenCredits()
    {
        AddCreditsFromInventory(10);
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
        Services.Audio.PlaySFX("PRESSSLOTMACHINEBUTTON");

    }

    public void DecreaseBet(int amount)
    {
        if (IsSpinning)
            return;

        if (amount <= 0)
            return;

        currentBet = Mathf.Max(currentBet - amount, 1);
        RefreshBetText();

        Services.Audio.PlaySFX("PRESSSLOTMACHINEBUTTON");
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

    private IEnumerator AnimateWinText(int fromValue, int toValue, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            int currentValue = Mathf.RoundToInt(Mathf.Lerp(fromValue, toValue, t));
            RefreshWinText(currentValue);

            yield return null;
        }

        RefreshWinText(toValue);
    }


    private void TryApplyDemoBonusWin()
    {
        List<SlotPatternData> currentWinningPatterns = SlotWinEvaluator.GetWinningPatterns(currentGrid, patterns);
        if (currentWinningPatterns.Count > 0)
            return;

        if (Random.value > demoBonusWinChance)
            return;

        SlotPatternData targetPattern = GetRandomSmallWinPattern();
        if (targetPattern == null || targetPattern.cells == null || targetPattern.cells.Length == 0)
            return;

        SlotSymbolData targetSymbol = GetLowestMultiplierSymbol();
        if (targetSymbol == null)
            return;

        for (int i = 0; i < targetPattern.cells.Length; i++)
        {
            Vector2Int cell = targetPattern.cells[i];
            currentGrid[cell.x, cell.y] = targetSymbol;
        }

        ApplyGridToReels();
        Debug.Log("Demo mode applied a bonus small win.");
    }

    private SlotPatternData GetRandomSmallWinPattern()
    {
        List<SlotPatternData> smallPatterns = new();

        for (int i = 0; i < patterns.Count; i++)
        {
            SlotPatternData pattern = patterns[i];
            if (pattern == null)
                continue;

            if (pattern.patternMultiplier <= 2f)
                smallPatterns.Add(pattern);
        }

        if (smallPatterns.Count == 0)
            return null;

        return smallPatterns[Random.Range(0, smallPatterns.Count)];
    }

    private SlotSymbolData GetLowestMultiplierSymbol()
    {
        SlotSymbolData lowestSymbol = null;

        for (int x = 0; x < currentGrid.GetLength(0); x++)
        {
            for (int y = 0; y < currentGrid.GetLength(1); y++)
            {
                SlotSymbolData symbol = currentGrid[x, y];
                if (symbol == null)
                    continue;

                if (lowestSymbol == null || symbol.symbolMultiplier < lowestSymbol.symbolMultiplier)
                    lowestSymbol = symbol;
            }
        }

        return lowestSymbol;
    }

    private void ApplyGridToReels()
    {
        for (int column = 0; column < reels.Length && column < currentGrid.GetLength(0); column++)
        {
            if (reels[column] == null)
                continue;

            reels[column].SetVisibleSymbols(
                currentGrid[column, 0],
                currentGrid[column, 1],
                currentGrid[column, 2]);
        }
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
