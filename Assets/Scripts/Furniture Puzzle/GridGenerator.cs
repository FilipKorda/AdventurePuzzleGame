using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject blockPrefab;
    public int gridSizeX = 6;
    public int gridSizeZ = 6;
    public float blockSize = 1f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 localPos = new(x * blockSize, 0, z * blockSize);
                GameObject block = Instantiate(blockPrefab, transform);
                block.transform.SetLocalPositionAndRotation(localPos, Quaternion.identity);
            }
        }
    }
}