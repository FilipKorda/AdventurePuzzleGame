using System.Collections.Generic;
using UnityEngine;

public static class BlockGridUtility
{
   /* public static List<Vector2Int> GetOccupiedCells(
        Vector2Int start,
        BlockType type,
        BlockDirection direction)
    {
        int length = (int)type;
        List<Vector2Int> cells = new List<Vector2Int>();

        if (direction == BlockDirection.X)
        {
            if (type == BlockType.Short)
            {
                for (int i = 0; i < length; i++)
                    cells.Add(start + new Vector2Int(0, i));
            }
            else
            {
                cells.Add(start + new Vector2Int(0, -1));
                cells.Add(start);
                cells.Add(start + new Vector2Int(0, 1));
            }
        }
        else
        {
            if (type == BlockType.Short)
            {
                for (int i = 0; i < length; i++)
                    cells.Add(start + new Vector2Int(i, 0));
            }
            else
            {
                cells.Add(start + new Vector2Int(-1, 0));
                cells.Add(start);
                cells.Add(start + new Vector2Int(1, 0));
            }
        }

        return cells;
    }*/
}