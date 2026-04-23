using UnityEngine;

public class MainMenuCameraOrbit : MonoBehaviour
{
    [Header("Camera Path")]
    [SerializeField] private Transform[] cameraPathPoints;

    [Header("Look Target Path")]
    [SerializeField] private Transform lookTargetObject;
    [SerializeField] private Transform[] lookTargetPathPoints;

    [Header("Movement")]
    [SerializeField] private float pathSpeed = 0.2f;
    [SerializeField] private bool loop = true;

    private float cameraT;
    private float lookTargetT;

    private void OnEnable()
    {
        Time.timeScale = 1f;
        ResetCameraState();
    }

    private void Start()
    {
        ResetCameraState();
    }

    private void ResetCameraState()
    {
        cameraT = 0f;
        lookTargetT = 0f;

        if (cameraPathPoints != null && cameraPathPoints.Length > 0)
        {
            transform.position = cameraPathPoints[0].position;
        }

        if (lookTargetObject != null && lookTargetPathPoints != null && lookTargetPathPoints.Length > 0)
        {
            lookTargetObject.position = lookTargetPathPoints[0].position;
            transform.LookAt(lookTargetObject.position);
        }
    }

    private void Update()
    {
        if (cameraPathPoints != null && cameraPathPoints.Length >= 4)
        {
            cameraT += Time.unscaledDeltaTime * pathSpeed;
            transform.position = GetCatmullRomPosition(cameraPathPoints, cameraT, loop);
        }

        if (lookTargetObject != null && lookTargetPathPoints != null && lookTargetPathPoints.Length >= 4)
        {
            lookTargetT += Time.unscaledDeltaTime * pathSpeed;
            lookTargetObject.position = GetCatmullRomPosition(lookTargetPathPoints, lookTargetT, loop);
        }

        if (lookTargetObject != null)
        {
            transform.LookAt(lookTargetObject.position);
        }
    }

    private Vector3 GetCatmullRomPosition(Transform[] points, float t, bool shouldLoop)
    {
        int numSections = shouldLoop ? points.Length : points.Length - 3;
        if (numSections <= 0) return points[0].position;

        float u = t % numSections;
        if (!shouldLoop)
        {
            u = Mathf.Clamp(u, 0f, numSections - 0.001f);
        }

        int section = Mathf.FloorToInt(u);
        float localT = u - section;

        int p0, p1, p2, p3;

        if (shouldLoop)
        {
            p0 = (section - 1 + points.Length) % points.Length;
            p1 = section % points.Length;
            p2 = (section + 1) % points.Length;
            p3 = (section + 2) % points.Length;
        }
        else
        {
            p0 = section;
            p1 = section + 1;
            p2 = section + 2;
            p3 = section + 3;
        }

        return 0.5f * (
            (2f * points[p1].position) +
            (-points[p0].position + points[p2].position) * localT +
            (2f * points[p0].position - 5f * points[p1].position + 4f * points[p2].position - points[p3].position) * localT * localT +
            (-points[p0].position + 3f * points[p1].position - 3f * points[p2].position + points[p3].position) * localT * localT * localT
        );
    }
}
