using UnityEngine;
public enum MoveAxis
{
    X,
    Z
}
public class MovableBlockRotatingPillar : MonoBehaviour
{
    public MoveAxis axis;
    BoxCollider[] colliders;

    void Awake()
    {
        colliders = GetComponentsInChildren<BoxCollider>();
    }

    void OnEnable()
    {
        foreach (var c in colliders)
            PuzzleCollisionRegistry.AllColliders.Add(c);
    }

    void OnDisable()
    {
        foreach (var c in colliders)
            PuzzleCollisionRegistry.AllColliders.Remove(c);
    }

    public void TryMove(Vector3 delta)
    {
        if (IsBlocked(delta))
            return;

        transform.position += delta;
    }

    bool IsBlocked(Vector3 delta)
    {
        foreach (var myCol in colliders)
        {
            var myBounds = myCol.bounds;
            myBounds.center += delta;

            foreach (var other in PuzzleCollisionRegistry.AllColliders)
            {
                if (System.Array.IndexOf(colliders, other) >= 0)
                    continue;

                if (other.transform.root == transform)
                    continue;

                if (myBounds.Intersects(other.bounds))
                {
                    Debug.Log($"BLOCKED by {other.name}", this);
                    return true;
                }
            }
        }

        return false;
    }

}
