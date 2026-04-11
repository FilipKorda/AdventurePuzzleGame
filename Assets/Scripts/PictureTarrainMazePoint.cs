using System.Collections.Generic;
using UnityEngine;

public class PictureTarrainMazePoint : MonoBehaviour
{
    public List<PictureTarrainMazePoint> neighbors;

    PictureTerrainMovingObject occupant;

    public bool IsOccupied => occupant != null;

    public bool TryOccupy(PictureTerrainMovingObject obj)
    {
        if (occupant != null) return false;
        occupant = obj;
        return true;
    }

    public void Release(PictureTerrainMovingObject obj)
    {
        if (occupant == obj)
            occupant = null;
    }

    public List<PictureTarrainMazePoint> AllNeighbors
    {
        get { return new List<PictureTarrainMazePoint>(neighbors); }
    }
}