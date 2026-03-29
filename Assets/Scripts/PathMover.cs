using UnityEngine;
using System.Collections;

public class PathMover : MonoBehaviour
{
    public IEnumerator MoveToPoint(
        Transform obj,
        Transform target,
        float speed,
        System.Action onReached
    )
    {
        while (Vector3.Distance(obj.position, target.position) > 0.001f)
        {
            obj.position = Vector3.MoveTowards(
                obj.position,
                target.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        onReached?.Invoke();
    } 
}