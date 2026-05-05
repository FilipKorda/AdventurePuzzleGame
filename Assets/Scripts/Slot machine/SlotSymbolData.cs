using UnityEngine;

[System.Serializable]
public class SlotSymbolData
{
    public string id;
    public Sprite sprite;
    public int symbolMultiplier = 2;
    [Range(0f, 100f)] public float spawnChancePercent = 10f;
}
