using System.Collections.Generic;
using UnityEngine;

public class GridOccupancy
{
    public HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();

    public bool IsOccupied(Vector2Int cell)
    {
        return occupied.Contains(cell);
    }

    public void SetOccupied(Vector2Int cell)
    {
        occupied.Add(cell);
    }

    public void ClearOccupied(Vector2Int cell)
    {
        occupied.Remove(cell);
    }

    public void SetMultiple(IEnumerable<Vector2Int> cells)
    {
        foreach (var c in cells)
            occupied.Add(c);
    }

    public void ClearMultiple(IEnumerable<Vector2Int> cells)
    {
        foreach (var c in cells)
            occupied.Remove(c);
    }
}