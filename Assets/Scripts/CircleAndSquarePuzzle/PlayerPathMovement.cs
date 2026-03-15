using System.ComponentModel;
using UnityEngine;

public class PlayerPathMovement : MonoBehaviour
{
    public float speed = 4f;
    public PathPoint currentPoint;

    PathPoint targetPoint;
    bool moving;


    void Update()
    {
        if (moving)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPoint.transform.position,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, targetPoint.transform.position) < 0.01f)
            {
                transform.position = targetPoint.transform.position;
                currentPoint = targetPoint;
                targetPoint = null;
                moving = false;
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
            TryMove(Vector3.up);

        if (Input.GetKeyDown(KeyCode.S))
            TryMove(Vector3.down);

        if (Input.GetKeyDown(KeyCode.A))
            TryMove(Vector3.back);

        if (Input.GetKeyDown(KeyCode.D))
            TryMove(Vector3.forward);
    }



    void TryMove(Vector3 dir)
    {
        PathPoint next = FindNeighbor(currentPoint, dir);
        if (!next) return;

        if (!CanMove(currentPoint, dir)) return;

        targetPoint = next;
        moving = true;
    }

    bool CanMove(PathPoint from, Vector3 dir)
    {
        PathPoint to = FindNeighbor(from, dir);
        if (!to) return false;

        if (dir == Vector3.up)
            return from.piece.WorldUp() && to.piece.WorldDown();

        if (dir == Vector3.down)
            return from.piece.WorldDown() && to.piece.WorldUp();

        if (dir == Vector3.back)
            return from.piece.WorldLeft() && to.piece.WorldRight();

        if (dir == Vector3.forward)
            return from.piece.WorldRight() && to.piece.WorldLeft();

        return false;
    }

    public float raycastLenght = 0.3f;

    [SerializeField] LayerMask pointLayer;

    PathPoint FindNeighbor(PathPoint from, Vector3 dir)
    {
        Vector3 origin = from.transform.position + dir * centerOffset;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, raycastLenght, pointLayer))
            return hit.collider.GetComponent<PathPoint>();

        return null;
    }
    void OnDrawGizmosSelected()
    {
        if (currentPoint == null) return;

        DrawRay(Vector3.up);
        DrawRay(Vector3.down);
        DrawRay(Vector3.back);
        DrawRay(Vector3.forward);
    }

    public float centerOffset;

    void DrawRay(Vector3 dir)
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = currentPoint.transform.position + dir * centerOffset;
        Gizmos.DrawLine(origin, origin + dir * raycastLenght);
    }
}