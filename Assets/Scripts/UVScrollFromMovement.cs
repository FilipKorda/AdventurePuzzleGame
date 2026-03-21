using System.Collections;
using UnityEngine;

public class UVScrollFromMovement : MonoBehaviour
{
    public Renderer targetRenderer;
    public float scrollMultiplier = 0.1f;

    public LayerMask detectableLayer;
    public float rayDistance = 2f;
    public Vector3 rayOffset = new(0f, 0f, 0.5f);
    Vector3 lastPos;

    public bool isActive = false;
    public bool goodSymbolSelected = false;

    [SerializeField] private BraiserPuzzle braiserPuzzle;

    void Start()
    {
        isActive = false;
        lastPos = transform.position;
    }

    public void PerformScrollUV()
    {
        StartCoroutine(ScrollUV());
    }

    IEnumerator ScrollUV()
    {
        while (true)
        {
            Vector3 delta = transform.position - lastPos;
            lastPos = transform.position;

            Vector2 uvDelta = new Vector2(delta.x, delta.z) * scrollMultiplier;

            Material mat = targetRenderer.material;
            Vector4 offset = mat.GetVector("_UVOffset");
            offset.x += uvDelta.x;
            offset.y += uvDelta.y;
            mat.SetVector("_UVOffset", offset);

            yield return null;
        }
    }

    public void ActivateRaycast()
    {
        StartCoroutine(RaycastDown());
    }

    IEnumerator RaycastDown()
    {
        while (true)
        {
            Vector3 rayOrigin = transform.position + rayOffset;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, detectableLayer))
            {
                goodSymbolSelected = true;
                braiserPuzzle.PuzzleWin();
            }
            else
            {
                goodSymbolSelected = false;
            }

            yield return new WaitForSeconds(1f);
        }
    }


    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (isActive)
        {
            Vector3 rayOrigin = transform.position + rayOffset;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * rayDistance);

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, rayDistance, detectableLayer))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.1f);
            }

        }

#endif
    }
}