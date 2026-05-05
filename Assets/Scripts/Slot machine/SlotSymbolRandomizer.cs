using UnityEngine;

public static class SlotSymbolRandomizer
{
    public static SlotSymbolData GetRandomSymbol(SlotSymbolData[] symbols)
    {
        if (symbols == null || symbols.Length == 0)
            return null;

        float totalWeight = 0f;

        for (int i = 0; i < symbols.Length; i++)
            totalWeight += Mathf.Max(0f, symbols[i].spawnChancePercent);

        if (totalWeight <= 0f)
            return symbols[Random.Range(0, symbols.Length)];

        float roll = Random.Range(0f, totalWeight);
        float current = 0f;

        for (int i = 0; i < symbols.Length; i++)
        {
            current += Mathf.Max(0f, symbols[i].spawnChancePercent);

            if (roll <= current)
                return symbols[i];
        }

        return symbols[symbols.Length - 1];
    }
}
