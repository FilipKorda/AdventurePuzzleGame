using UnityEngine;

public class PathPiece : MonoBehaviour
{
    public bool localUp;
    public bool localDown;
    public bool localLeft;   // wzdłuż Z-
    public bool localRight;  // wzdłuż Z+

    public bool WorldUp()
    {
        return IsOpen(Vector3.up);    // Y+
    }

    public bool WorldDown()
    {
        return IsOpen(Vector3.down);  // Y-
    }

    public bool WorldLeft()
    {
        return IsOpen(Vector3.back);  // Z-
    }

    public bool WorldRight()
    {
        return IsOpen(Vector3.forward); // Z+
    }

    bool IsOpen(Vector3 worldDir)
    {
        Vector3 localDir = transform.InverseTransformDirection(worldDir);

        if (Vector3.Dot(localDir, Vector3.up) > 0.9f) return localUp;
        if (Vector3.Dot(localDir, Vector3.down) > 0.9f) return localDown;
        if (Vector3.Dot(localDir, Vector3.back) > 0.9f) return localLeft;
        if (Vector3.Dot(localDir, Vector3.forward) > 0.9f) return localRight;

        return false;
    }
}