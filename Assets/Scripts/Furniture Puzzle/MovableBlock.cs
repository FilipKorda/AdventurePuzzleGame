using UnityEngine;

public enum BlockType
{
    Short = 2, // zajmuje 2 pola
    Long = 3   // zajmuje 3 pola
}
public enum BlockDirection { X, Z }

public class MovableBlock : MonoBehaviour
{
    public BlockType blockType;
    public BlockDirection direction;

    public float blockWidth = 0.8f;

    void Start()
    {
        SetScale();
    }

    public void SetScale()
    {
        Vector3 scale = Vector3.one;

        switch (blockType)
        {
            case BlockType.Short:
                scale = direction == BlockDirection.X ? new Vector3(blockWidth, 0.8f, 1.6f) : new Vector3(1.6f, 0.8f, blockWidth);
                break;
            case BlockType.Long:
                scale = direction == BlockDirection.X ? new Vector3(blockWidth, 0.8f, 2.4f) : new Vector3(2.4f, 0.8f, blockWidth);
                break;
        }

        transform.localScale = scale;
    }
}