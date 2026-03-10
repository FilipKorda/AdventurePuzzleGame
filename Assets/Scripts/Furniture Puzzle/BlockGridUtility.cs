using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BlockGridUtility
{
    public static List<Vector2> GetOccupiedCells(Vector2 start, BlockType type, BlockDirection direction)
    {
        int length = (int)type;
        List<Vector2> cells = new List<Vector2>();

        if (direction == BlockDirection.X)
        {
            if (type == BlockType.Short)
            {
                for (int i = 0; i < length; i++)
                {
                    cells.Add(start + new Vector2(0, i));
                }
            }
            else if (type == BlockType.Long)
            {
                cells.Add(start + new Vector2(0, -1));
                cells.Add(start + new Vector2(0, 0));
                cells.Add(start + new Vector2(0, 1));
            }
        }
        else if (direction == BlockDirection.Z)
        {
            if (type == BlockType.Short)
            {
                for (int i = 0; i < length; i++)
                {
                    cells.Add(start + new Vector2(i, 0));
                }
            }
            else if (type == BlockType.Long)
            {
                cells.Add(start + new Vector2(-1, 0));
                cells.Add(start + new Vector2(0, 0));
                cells.Add(start + new Vector2(1, 0));
            }
        }

        return cells;
    }
}