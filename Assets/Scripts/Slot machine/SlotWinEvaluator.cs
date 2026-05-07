using System.Collections.Generic;
using UnityEngine;

public static class SlotWinEvaluator
{
    public static int CalculateTotalWin(SlotSymbolData[,] grid, List<SlotPatternData> patterns, int currentBet)
    {
        float totalWin = 0f;

        if (grid == null || patterns == null || currentBet <= 0)
            return 0;

        foreach (SlotPatternData pattern in patterns)
        {
            if (TryMatchPattern(grid, pattern, out SlotSymbolData matchedSymbol))
            {
                float singlePatternWin = currentBet * matchedSymbol.symbolMultiplier * pattern.patternMultiplier;
                totalWin += singlePatternWin;

                Debug.Log(
                    $"Pattern win: {pattern.patternName} | Symbol: {matchedSymbol.id} | " +
                    $"Bet: {currentBet} | Symbol x{matchedSymbol.symbolMultiplier} | " +
                    $"Pattern x{pattern.patternMultiplier} | Win: {singlePatternWin}");
            }
        }

        return Mathf.RoundToInt(totalWin);
    }

    public static int CalculatePatternWin(SlotSymbolData[,] grid, SlotPatternData pattern, int currentBet)
    {
        if (grid == null || pattern == null || currentBet <= 0)
            return 0;

        if (!TryMatchPattern(grid, pattern, out SlotSymbolData matchedSymbol))
            return 0;

        float win = currentBet * matchedSymbol.symbolMultiplier * pattern.patternMultiplier;
        return Mathf.RoundToInt(win);
    }


    private static bool TryMatchPattern(SlotSymbolData[,] grid, SlotPatternData pattern, out SlotSymbolData matchedSymbol)
    {
        matchedSymbol = null;

        if (pattern == null || pattern.cells == null || pattern.cells.Length == 0)
            return false;

        Vector2Int firstCell = pattern.cells[0];
        SlotSymbolData firstSymbol = grid[firstCell.x, firstCell.y];

        if (firstSymbol == null)
            return false;

        for (int i = 1; i < pattern.cells.Length; i++)
        {
            Vector2Int cell = pattern.cells[i];
            SlotSymbolData symbol = grid[cell.x, cell.y];

            if (symbol == null || symbol.id != firstSymbol.id)
                return false;
        }

        matchedSymbol = firstSymbol;
        return true;
    }

    public static List<SlotPatternData> GetWinningPatterns(SlotSymbolData[,] grid, List<SlotPatternData> patterns)
    {
        List<SlotPatternData> winningPatterns = new();

        if (grid == null || patterns == null)
            return winningPatterns;

        foreach (SlotPatternData pattern in patterns)
        {
            if (TryMatchPattern(grid, pattern, out _))
                winningPatterns.Add(pattern);
        }

        return winningPatterns;
    }

}
