using UnityEngine;

public class BraiserPuzzleMoveArea : MonoBehaviour
{
    public Vector2 areaMin = new(-5, -5);
    public Vector2 areaMax = new(5, 5);

    public Vector2 WorldMin =>
        new Vector2(transform.position.x + areaMin.x, transform.position.z + areaMin.y);

    public Vector2 WorldMax =>
        new Vector2(transform.position.x + areaMax.x, transform.position.z + areaMax.y);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = transform.position + new Vector3(
            (areaMin.x + areaMax.x) * 0.5f,
            0f,
            (areaMin.y + areaMax.y) * 0.5f
        );

        Vector3 size = new Vector3(
            areaMax.x - areaMin.x,
            0.1f,
            areaMax.y - areaMin.y
        );

        Gizmos.DrawWireCube(center, size);
    }
}