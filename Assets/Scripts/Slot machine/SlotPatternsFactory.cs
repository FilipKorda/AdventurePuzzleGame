using System.Collections.Generic;
using UnityEngine;

public static class SlotPatternsFactory
{
    public static List<SlotPatternData> CreateDefaultPatterns()
    {
        List<SlotPatternData> patterns = new();

        AddHorizontalPatterns(patterns);
        AddHorizontalLongPatterns(patterns);
        AddHorizontalXLPatterns(patterns);
        AddVerticalPatterns(patterns);
        AddDiagonalPatterns(patterns);

        patterns.Add(new SlotPatternData
        {
            patternName = "Zig",
            patternMultiplier = 4f,
            cells = new[]
            {
                new Vector2Int(0, 2),
                new Vector2Int(1, 1),
                new Vector2Int(2, 0),
                new Vector2Int(3, 1),
                new Vector2Int(4, 2)
            }
        });

        patterns.Add(new SlotPatternData
        {
            patternName = "Zag",
            patternMultiplier = 4f,
            cells = new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 1),
                new Vector2Int(2, 2),
                new Vector2Int(3, 1),
                new Vector2Int(4, 0)
            }
        });

        patterns.Add(new SlotPatternData
        {
            patternName = "Above",
            patternMultiplier = 7f,
            cells = new[]
            {
                new Vector2Int(2, 0),
                new Vector2Int(1, 1),
                new Vector2Int(3, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2),
                new Vector2Int(4, 2)
            }
        });

        patterns.Add(new SlotPatternData
        {
            patternName = "Below",
            patternMultiplier = 7f,
            cells = new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(3, 0),
                new Vector2Int(4, 0),
                new Vector2Int(1, 1),
                new Vector2Int(3, 1),
                new Vector2Int(2, 2)
            }
        });

        patterns.Add(new SlotPatternData
        {
            patternName = "Eye",
            patternMultiplier = 8f,
            cells = new[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(2, 0),
                new Vector2Int(3, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(3, 1),
                new Vector2Int(4, 1),
                new Vector2Int(1, 2),
                new Vector2Int(2, 2),
                new Vector2Int(3, 2)
            }
        });

        patterns.Add(new SlotPatternData
        {
            patternName = "Jackpot",
            patternMultiplier = 10f,
            cells = new[]
            {
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(4, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(4, 1),
                new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(3, 2), new Vector2Int(4, 2)
            }
        });

        return patterns;
    }

    private static void AddHorizontalPatterns(List<SlotPatternData> patterns)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int startX = 0; startX <= 2; startX++)
            {
                patterns.Add(new SlotPatternData
                {
                    patternName = "Horizontal",
                    patternMultiplier = 1f,
                    cells = new[]
                    {
                        new Vector2Int(startX + 0, row),
                        new Vector2Int(startX + 1, row),
                        new Vector2Int(startX + 2, row)
                    }
                });
            }
        }
    }

    private static void AddHorizontalLongPatterns(List<SlotPatternData> patterns)
    {
        for (int row = 0; row < 3; row++)
        {
            for (int startX = 0; startX <= 1; startX++)
            {
                patterns.Add(new SlotPatternData
                {
                    patternName = "Horizontal-L",
                    patternMultiplier = 2f,
                    cells = new[]
                    {
                        new Vector2Int(startX + 0, row),
                        new Vector2Int(startX + 1, row),
                        new Vector2Int(startX + 2, row),
                        new Vector2Int(startX + 3, row)
                    }
                });
            }
        }
    }

    private static void AddHorizontalXLPatterns(List<SlotPatternData> patterns)
    {
        for (int row = 0; row < 3; row++)
        {
            patterns.Add(new SlotPatternData
            {
                patternName = "Horizontal-XL",
                patternMultiplier = 3f,
                cells = new[]
                {
                    new Vector2Int(0, row),
                    new Vector2Int(1, row),
                    new Vector2Int(2, row),
                    new Vector2Int(3, row),
                    new Vector2Int(4, row)
                }
            });
        }
    }

    private static void AddVerticalPatterns(List<SlotPatternData> patterns)
    {
        for (int column = 0; column < 5; column++)
        {
            patterns.Add(new SlotPatternData
            {
                patternName = "Vertical",
                patternMultiplier = 1f,
                cells = new[]
                {
                    new Vector2Int(column, 0),
                    new Vector2Int(column, 1),
                    new Vector2Int(column, 2)
                }
            });
        }
    }

    private static void AddDiagonalPatterns(List<SlotPatternData> patterns)
    {
        // down-right
        for (int startX = 0; startX <= 2; startX++)
        {
            patterns.Add(new SlotPatternData
            {
                patternName = "Diagonal",
                patternMultiplier = 1f,
                cells = new[]
                {
                    new Vector2Int(startX + 0, 0),
                    new Vector2Int(startX + 1, 1),
                    new Vector2Int(startX + 2, 2)
                }
            });
        }

        // up-right
        for (int startX = 0; startX <= 2; startX++)
        {
            patterns.Add(new SlotPatternData
            {
                patternName = "Diagonal",
                patternMultiplier = 1f,
                cells = new[]
                {
                    new Vector2Int(startX + 0, 2),
                    new Vector2Int(startX + 1, 1),
                    new Vector2Int(startX + 2, 0)
                }
            });
        }
    }
}
