using UnityEngine;

public class FramePuzzlemanager : MonoBehaviour
{
    [SerializeField] private GameObject[] puzzles;
    [SerializeField] private GameObject[] puzzlesPlaceCollider;
    [SerializeField] private Animator animator;

    public void CheckAllPuzzlesBlocks()
    {
        foreach (var puzzle in puzzles)
        {
            if (!puzzle.activeInHierarchy)
            {
                return;
            }
        }

        WinPuzzle();
    }

    private void WinPuzzle()
    {
        foreach (var puzzle in puzzles)
        {
            puzzle.GetComponent<BoxCollider>().enabled = false;
        }

        foreach (var col in puzzlesPlaceCollider)
        {
            col.GetComponent<BoxCollider>().enabled = false;
        }

        animator.SetTrigger("Open");
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }
}
