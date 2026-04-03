using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public bool activeLaser = false;

    public LineRenderer line;
    public int maxReflections = 10;
    public float maxDistance = 100f;

    HashSet<ILaserReactable> currentHits = new();
    HashSet<ILaserReactable> lastHits = new();

    [SerializeField] LayerMask mirrorLayer;
    [SerializeField] LayerMask passLayer;
    [SerializeField] LayerMask reactableLayer;

    [SerializeField] float refreshRate = 0.05f;

    public void ToggleLaser(bool toogleLaser)
    {
        activeLaser = toogleLaser;
    }

    void Update()
    {
        DrawLaser();
    }

    bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

    void DrawLaser()
    {
        if (activeLaser)
        {
            currentHits.Clear();

            Ray ray = new(transform.position, transform.forward);
            line.positionCount = 1;
            line.SetPosition(0, ray.origin);

            int reflections = 0;

            while (reflections < maxReflections)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, hit.point);

                    if (IsInLayerMask(hit.collider.gameObject, reactableLayer))
                    {
                        if (hit.collider.TryGetComponent<ILaserReactable>(out var reactable))
                        {
                            currentHits.Add(reactable);

                            if (!lastHits.Contains(reactable))
                                reactable.OnLaserEnter();
                        }
                    }

                    if (IsInLayerMask(hit.collider.gameObject, mirrorLayer))
                    {
                        Vector3 reflectDir = Vector3.Reflect(ray.direction, hit.normal);
                        ray = new Ray(hit.point + reflectDir * 0.001f, reflectDir);
                        reflections++;
                    }
                    else if (IsInLayerMask(hit.collider.gameObject, passLayer))
                    {
                        ray = new Ray(hit.point + ray.direction * 0.001f, ray.direction);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, ray.origin + ray.direction * maxDistance);
                    break;
                }
            }

            foreach (var reactable in lastHits)
            {
                if (!currentHits.Contains(reactable))
                    reactable.OnLaserExit();
            }

            lastHits.Clear();

            foreach (var reactable in currentHits)
                lastHits.Add(reactable);
        }

    }
}