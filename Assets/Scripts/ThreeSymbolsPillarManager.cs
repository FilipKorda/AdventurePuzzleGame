using UnityEngine;
using UnityEngine.UI;

public class ThreeSymbolsPillarManager : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;

    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private MovingPillarManager movingPillarManager;
    [SerializeField] private Animator animator;

    public void CheckWinPuzzle()
    {
        if (image.sprite.name == "Symbol For Pillar Puzzle"
            && image1.sprite.name == "Symbol For Pillar Puzzle1"
            && image2.sprite.name == "Symbol For Pillar Puzzle")
        {

            animator.SetTrigger("Interact");
            DisalePuzzle();
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
        }
    }

    private void DisalePuzzle()
    {
        sphereCollider.enabled = false;

        foreach (var boxCollider in boxColliders)
        {
            boxCollider.enabled = false;
        }

        movingPillarManager.DisableInput();
    }
}
