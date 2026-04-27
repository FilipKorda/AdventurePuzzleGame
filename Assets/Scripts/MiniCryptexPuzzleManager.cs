using UnityEngine;

public class MiniCryptexPuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject cryptex;
    [SerializeField] private GameObject cryptex1;
    [SerializeField] private GameObject cryptex2;
    [SerializeField] private GameObject cryptex3;

    [SerializeField] private GameObject button;
    [SerializeField] private Animator animator;
    [SerializeField] private VerticalFollowPath verticalFollowPath;
    [SerializeField] private BoxCollider boxCollider;

    public void SolvePuzzle()
    {
        if (
            cryptex.GetComponent<InteractableItem>().CurrentCryptexIndex == 2 &&
            cryptex1.GetComponent<InteractableItem>().CurrentCryptexIndex == 4 &&
            cryptex2.GetComponent<InteractableItem>().CurrentCryptexIndex == 6 &&
            cryptex3.GetComponent<InteractableItem>().CurrentCryptexIndex == 3
           )
        {
            DisableSegments();
            WinPuzzle();
        }
    }

    private void DisableSegments()
    {
        cryptex.GetComponent<BoxCollider>().enabled = false;
        cryptex1.GetComponent<BoxCollider>().enabled = false;
        cryptex2.GetComponent<BoxCollider>().enabled = false;
        cryptex3.GetComponent<BoxCollider>().enabled = false;
        button.GetComponent<BoxCollider>().enabled = false;
    }

    private void WinPuzzle()
    {
        verticalFollowPath.StopSequence();
        boxCollider.enabled = false;
        animator.SetTrigger("Interact");
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }
}
