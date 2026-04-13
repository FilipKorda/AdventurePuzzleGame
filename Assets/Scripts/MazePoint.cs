using System.Collections.Generic;
using UnityEngine;

public class MazePoint : MonoBehaviour
{
    [Header("Static Connections (Set in Editor)")]
    public List<MazePoint> neighbors;
    [Header("Dynamic Connections (Runtime)")]
    public List<MazePoint> dynamicNeighbors = new List<MazePoint>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
       
        foreach (var n in neighbors)
        {
            if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
        }

        Gizmos.color = Color.green;
        foreach (var n in dynamicNeighbors)
        {
            if (n != null) Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }

    public List<MazePoint> AllNeighbors
    {
        get
        {
            List<MazePoint> all = new List<MazePoint>(neighbors);
            all.AddRange(dynamicNeighbors);
            return all;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MazePoint otherPoint = other.GetComponent<MazePoint>();
        if (otherPoint != null && otherPoint != this)
        {
            if (!dynamicNeighbors.Contains(otherPoint))
            {
                dynamicNeighbors.Add(otherPoint);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MazePoint otherPoint = other.GetComponent<MazePoint>();
        if (otherPoint != null)
        {
            if (dynamicNeighbors.Contains(otherPoint))
            {
                dynamicNeighbors.Remove(otherPoint);
            }
        }
    }
}