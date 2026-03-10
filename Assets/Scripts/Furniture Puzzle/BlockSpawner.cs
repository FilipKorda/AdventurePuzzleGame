using UnityEngine;
[System.Serializable]
public class BlockConfig
{
    public GameObject prefab;
    public BlockType type;
    public BlockDirection direction;
    public Vector2 gridPosition;
}

public class BlockSpawner : MonoBehaviour
{
    public int gridSizeX = 6;
    public int gridSizeZ = 6;
    private float blockSize = 0.8f;

    public BlockConfig[] blocksToSpawn;

    private GridOccupancy grid;
    public GridOccupancy Grid => grid; 

    void Start()
    {
        grid = new GridOccupancy();
        SpawnBlocks();
    }

    void SpawnBlocks()
    {
        foreach (var config in blocksToSpawn)
        {
            Vector2 startCell = config.gridPosition;

            var occupiedCells = BlockGridUtility.GetOccupiedCells(
                startCell,
                config.type,
                config.direction
            );

            foreach (var cell in occupiedCells)
            {
                grid.SetOccupied(cell);
            }

            Vector3 localPos = new Vector3(
                startCell.x * blockSize,
                0.35f,
                startCell.y * blockSize
            );

            GameObject block = Instantiate(config.prefab, transform);
            block.transform.SetLocalPositionAndRotation(localPos, Quaternion.identity);
            MovableBlock mb = block.GetComponent<MovableBlock>();
            mb.blockType = config.type;
            mb.direction = config.direction;
            mb.SetScale();
        }
    }
}