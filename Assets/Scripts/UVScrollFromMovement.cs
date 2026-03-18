using UnityEngine;

public class UVScrollFromMovement : MonoBehaviour
{
    public Renderer targetRenderer;
    public float scrollMultiplier = 0.1f;

    Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        Vector3 delta = transform.position - lastPos;
        lastPos = transform.position;

        Vector2 uvDelta = new Vector2(delta.x, delta.z) * scrollMultiplier;

        Material mat = targetRenderer.material;
        Vector4 offset = mat.GetVector("_UVOffset");
        offset.x += uvDelta.x;
        offset.y += uvDelta.y;
        mat.SetVector("_UVOffset", offset);
    }
}