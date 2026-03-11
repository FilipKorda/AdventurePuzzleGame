using UnityEngine;

public class FurniturePuzzle : MonoBehaviour
{
    [SerializeField] private MovableBlock[] movableBolck;
    [SerializeField] private Animator left;
    [SerializeField] private Animator right;


    public void DisableAllMovableBlocks()
    {
        foreach(var block in movableBolck)
        {
            block.DisabelThisMovableBlock();
        }
    }


    public void ActiveOpenShelf()
    {
        left.SetTrigger("Interact");
        right.SetTrigger("Interact");
    }

}
