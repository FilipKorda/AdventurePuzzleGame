using UnityEngine;
using System.Collections.Generic;

public class GridOccupancy
{
    public HashSet<Vector2> OccupiedCells = new HashSet<Vector2>();

    public bool IsOccupied(Vector2 cell)
    {
        return OccupiedCells.Contains(cell);
    }

    public void SetOccupied(Vector2 cell)
    {
        OccupiedCells.Add(cell);
    }
}