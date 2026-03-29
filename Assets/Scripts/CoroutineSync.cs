using UnityEngine;
using System.Collections;

public class CoroutineSync : MonoBehaviour
{
    public IEnumerator RunParallel(params IEnumerator[] routines)
    {
        int finished = 0;

        foreach (var routine in routines)
        {
            StartCoroutine(Run(routine, () => finished++));
        }

        while (finished < routines.Length)
            yield return null;
    }

    IEnumerator Run(IEnumerator routine, System.Action onFinish)
    {
        yield return StartCoroutine(routine);
        onFinish?.Invoke();
    }
}