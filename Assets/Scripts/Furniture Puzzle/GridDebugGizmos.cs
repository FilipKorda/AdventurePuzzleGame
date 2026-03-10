using UnityEngine;

public class GridDebugGizmos : MonoBehaviour
{
    public GridGenerator gridGenerator;
    public BlockSpawner blockSpawner;

    void OnDrawGizmos()
    {
        if (gridGenerator == null || blockSpawner.Grid.OccupiedCells == null) return;

        Gizmos.color = Color.red;

        foreach (var cell in blockSpawner.Grid.OccupiedCells)
        {
            int x = (int)cell.x;
            int z = (int)cell.y;

            Vector3 worldPos = gridGenerator.transform.position +
                               new Vector3(x * gridGenerator.blockSize,
                                           1f,
                                           z * gridGenerator.blockSize);

            Gizmos.DrawWireCube(worldPos, new Vector3(gridGenerator.blockSize, 0.1f, gridGenerator.blockSize));
        }
    }
}