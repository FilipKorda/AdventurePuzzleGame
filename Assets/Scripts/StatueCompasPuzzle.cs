using System.Collections;
using UnityEngine;

public class StatueCompasPuzzle : MonoBehaviour
{
    [SerializeField] private InteractableItem[] statues;
    [SerializeField] private WorldDirection[] requiredDirections;
    [SerializeField] private BoxCollider[] statuesBoxColliders;
    [SerializeField] private Transform point;
    [SerializeField] private Transform button;

    bool puzzleSolved;

    void Awake()
    {
        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].OnRotationFinished += OnStatueRotated;
        }

        button.localPosition = new Vector3(-8.2f, button.localPosition.y, button.localPosition.z);
    }

    void OnDestroy()
    {
        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].OnRotationFinished -= OnStatueRotated;
        }
    }

    void OnStatueRotated(InteractableItem statue)
    {
        statue.ApplyText();
        CheckPuzzle();
    }

    public void CheckPuzzle()
    {
        if (puzzleSolved)
            return;

        if (statues.Length != requiredDirections.Length)
            return;

        for (int i = 0; i < statues.Length; i++)
        {
            if (statues[i].CurrentDirection != requiredDirections[i])
                return;
        }

        PuzzleSolved();
    }



    void PuzzleSolved()
    {
        puzzleSolved = true;

        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].enabled = false;
        }

        foreach (var boxCollider in statuesBoxColliders)
        {
            boxCollider.enabled = false;
        }

       // Debug.Log("Puzzle solved");

        MoveButton();
        RotateAllStatuesToPoint();
    }


    private void RotateAllStatuesToPoint()
    {
        StartCoroutine(ActiveSFXDelay());

        foreach (var statue in statues)
            StartCoroutine(RotateToPoint(statue.transform));
    }

    private IEnumerator RotateToPoint(Transform target)
    {
        Quaternion startRotation = target.rotation;

        Vector3 dir = point.position - target.position;
        dir.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(dir);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 4.2f;
            target.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }
    }

    private void MoveButton()
    {
        StartCoroutine(MoveButtonX(-8.2f, -8.065f, 0.2f));
    }

    private IEnumerator MoveButtonX(float fromX, float toX, float duration)
    {
        Vector3 start = button.localPosition;
        Vector3 end = new Vector3(toX, start.y, start.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float x = Mathf.Lerp(fromX, toX, t);
            button.localPosition = new Vector3(x, start.y, start.z);
            yield return null;
        }
    }

    private IEnumerator ActiveSFXDelay()
    {
        Services.Audio.PlaySFX("RotateStatue_StoneMove");
        yield return new WaitForSeconds(0.1f);
        Services.Audio.PlaySFX("RotateStatue_StoneMove");
        yield return new WaitForSeconds(0.13f);
        Services.Audio.PlaySFX("RotateStatue_StoneMove");
        yield return new WaitForSeconds(0.16f);
        Services.Audio.PlaySFX("RotateStatue_StoneMove");
        yield return null;
    }
}