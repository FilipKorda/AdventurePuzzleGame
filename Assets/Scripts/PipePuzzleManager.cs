using UnityEngine;

public class PipePuzzleManager : MonoBehaviour
{
    [SerializeField] private CircleAndSquarePuzzle circleAndSquarePuzzle;
    [SerializeField] private PipeGearPuzzle pipeGearPuzzle;
    [SerializeField] private MovingBlockPuzzleManager movingBlockPuzzleManager;
    [SerializeField] private Animator animator;

    public void CheckWInBothPipePuzzle()
    {
        if(circleAndSquarePuzzle.winPuzzle && pipeGearPuzzle.puzzleWin && movingBlockPuzzleManager.puzzleWin)
        {
            animator.SetTrigger("Open");
        }
    }
}
