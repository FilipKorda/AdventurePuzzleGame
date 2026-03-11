using UnityEngine;

public class PuzzleBlockManager : MonoBehaviour
{
    public static PuzzleBlockManager Instance;
    [SerializeField] private WoodenBlockPuzzle woodenBlockPuzzle;

    bool[] clickedBlocks = new bool[12];
    int[] winningSet = { 0, 2, 3, 5, 7,9,10 };

    void Awake()
    {
        Instance = this;
    }

    public void BlockClicked(int id)
    {
        clickedBlocks[id] = !clickedBlocks[id];
        CheckWin();
    }

    void CheckWin()
    {
        for (int i = 0; i < clickedBlocks.Length; i++)
        {
            bool shouldBeActive = System.Array.IndexOf(winningSet, i) != -1;

            if (clickedBlocks[i] != shouldBeActive)
                return;
        }

        woodenBlockPuzzle.PuzzleWon();
        Debug.Log("WYGRANA");
    }

    public void ResetClickedBlock()
    {
        for (int i = 0; i < clickedBlocks.Length; i++)
        {
            clickedBlocks[i] = false;
        }
        
    }

}
