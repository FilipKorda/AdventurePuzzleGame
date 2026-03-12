
using UnityEngine;

public class SwordPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject[] swords;
    [SerializeField] private Animator animator;


    public void CheckSwordStatus()
    {
        bool allSwordsInPlace = true;
        foreach (GameObject sword in swords)
        {
            if (!sword.activeInHierarchy)
            {
                allSwordsInPlace = false;
                break;
            }
        }
        if (allSwordsInPlace)
        {
            WinPuzzle();
        }
    }


    private void WinPuzzle()
    {
        animator.SetTrigger("Open");

        Debug.Log("All swords are in place! Puzzle solved!");
    }

}
