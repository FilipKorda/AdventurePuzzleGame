using UnityEngine;
using System.Collections.Generic;

public class ScaleWeightSystem : MonoBehaviour
{
    [SerializeField] private int leftSideWeight = 10;
    [SerializeField] private ScaleLogic scaleLogic;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider boxCollider;

    private List<WeightItem> rightSideItems = new List<WeightItem>();

    [SerializeField] private BoxCollider[] allItems;

    private void Start()
    {
        UpdateScale();
    }

    public void AddItem(WeightItem item)
    {
        if (!rightSideItems.Contains(item))
            rightSideItems.Add(item);

        UpdateScale();
    }

    public void RemoveItem(WeightItem item)
    {
        if (rightSideItems.Contains(item))
            rightSideItems.Remove(item);

        UpdateScale();
    }

    private void UpdateScale()
    {
        int rightWeight = 0;

        foreach (var item in rightSideItems)
            rightWeight += item.Weight;


        int difference = rightWeight - leftSideWeight;
        scaleLogic.SetBalance(difference);

        if (rightWeight == leftSideWeight)
            WinPuzzle();
    }

    private void DisaleAllItemsColliders()
    {
        foreach(var item in rightSideItems)
        {
            item.enabled = false;
        }
    }

    public void WinPuzzle()
    {
        boxCollider.enabled = false;
        DisaleAllItemsColliders();
        animator.SetTrigger("Interact");
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }
}