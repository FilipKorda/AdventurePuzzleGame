using UnityEngine;

public class CircleAndSquareCollisionDetected : MonoBehaviour
{
    [SerializeField] private PathPiece pathPiece;
    [SerializeField] private LayerMask targetLayer;

    enum Direction { Up, Down, Left, Right }
    [SerializeField] private Direction direction;

    private bool collisionDetected;

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionDetected) return;
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        SetBool(true);
        collisionDetected = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collisionDetected) return;
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        SetBool(true);
        collisionDetected = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collisionDetected) return;
        if ((targetLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        SetBool(false);
        collisionDetected = false;
    }

    void SetBool(bool value)
    {
        Vector3 worldDir = GetWorldDirection();

        Vector3 localDir = pathPiece.transform.InverseTransformDirection(worldDir);

        if (Vector3.Dot(localDir, Vector3.up) > 0.9f)
            pathPiece.localUp = value;
        else if (Vector3.Dot(localDir, Vector3.down) > 0.9f)
            pathPiece.localDown = value;
        else if (Vector3.Dot(localDir, Vector3.back) > 0.9f)
            pathPiece.localLeft = value;
        else if (Vector3.Dot(localDir, Vector3.forward) > 0.9f)
            pathPiece.localRight = value;
    }

    Vector3 GetWorldDirection()
    {
        switch (direction)
        {
            case Direction.Up: return Vector3.up;       // Y+
            case Direction.Down: return Vector3.down;   // Y-
            case Direction.Left: return Vector3.back;   // Z-
            case Direction.Right: return Vector3.forward; // Z+
        }
        return Vector3.zero;
    }
}