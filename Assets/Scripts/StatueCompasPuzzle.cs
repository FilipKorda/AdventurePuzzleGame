using UnityEngine;

public class StatueCompasPuzzle : MonoBehaviour
{
    [SerializeField] private InteractableItem[] statues;
    [SerializeField] private WorldDirection[] requiredDirections;
    [SerializeField] private BoxCollider[] statuesBoxColliders;

    bool puzzleSolved;

    void Awake()
    {
        for (int i = 0; i < statues.Length; i++)
        {
            statues[i].OnRotationFinished += OnStatueRotated;
        }
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

        Debug.Log("Puzzle solved");
    }
}