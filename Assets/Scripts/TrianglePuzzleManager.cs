using UnityEngine;

public class TrianglePuzzleManager : MonoBehaviour
{
    [Header("Triangle Puzzle Sequence")]
    [SerializeField]
    private TriangleEnum[] correctSequence = new TriangleEnum[]
    {
        TriangleEnum.TriangleDown,
        TriangleEnum.TriangleUp,
        TriangleEnum.TriangleDown,
        TriangleEnum.TriangleUp,
        TriangleEnum.TriangleUp,
        TriangleEnum.TriangleDown,
        TriangleEnum.TriangleDown,
        TriangleEnum.TriangleUp,
        TriangleEnum.TriangleDown,
        TriangleEnum.TriangleUp
    };
    [SerializeField] private BoxCollider[] buttonsBoxCollider;
    private int currentStep = 0;
    [SerializeField] private Animator animator;
    public void PressedTriangle(TriangleEnum pressed)
    {
        if (pressed == correctSequence[currentStep])
        {
            currentStep++;
            if (currentStep >= correctSequence.Length)
            {
                TrianglePuzzleWon();

            }
        }
        else
        {
            TriangleLost();

        }
    }

    private void TrianglePuzzleWon()
    {
        animator.SetTrigger("Open");
        currentStep = 0;
        DisableBoxCollider();
        Debug.Log("Kod poprawny – zagadka rozwiązana!");
    }

    private void TriangleLost()
    {
        currentStep = 0;
        Debug.Log("Błędna sekwencja");
    }

    private void DisableBoxCollider()
    {
        foreach (var boxCollider in buttonsBoxCollider)
        {
            boxCollider.enabled = false;
        }
    }
}