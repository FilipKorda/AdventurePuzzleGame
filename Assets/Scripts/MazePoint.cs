using System.Collections.Generic;
using UnityEngine;

public class MazePoint : MonoBehaviour
{
    public List<MazePoint> neighbors;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var n in neighbors)
        {
            if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }
}