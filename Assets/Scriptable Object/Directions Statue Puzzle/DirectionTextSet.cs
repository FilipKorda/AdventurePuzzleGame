using UnityEngine;

[CreateAssetMenu(menuName = "Statue/Direction Text Set")]
public class DirectionTextSet : ScriptableObject
{
    public ReadableTextData north;
    public ReadableTextData east;
    public ReadableTextData south;
    public ReadableTextData west;

    public ReadableTextData Get(WorldDirection direction)
    {
        switch (direction)
        {
            case WorldDirection.North: return north;
            case WorldDirection.East: return east;
            case WorldDirection.South: return south;
            case WorldDirection.West: return west;
        }
        return null;
    }
}