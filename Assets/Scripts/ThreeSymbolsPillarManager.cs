using UnityEngine;
using UnityEngine.UI;

public class ThreeSymbolsPillarManager : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;

    [SerializeField] private WallTrapdoorsManager wallTrapdoorsManager;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private MovingPillarManager movingPillarManager;
    public void CheckWinPuzzle()
    {
        if (image.sprite.name == "Symbol For Pillar Puzzle"
            && image1.sprite.name == "Symbol For Pillar Puzzle1"
            && image2.sprite.name == "Symbol For Pillar Puzzle")
        {
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
            wallTrapdoorsManager.OpenTrapDoor();
            DisalePuzzle();
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
