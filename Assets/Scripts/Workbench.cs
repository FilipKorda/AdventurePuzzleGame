using UnityEngine;

public class Workbench : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject metalCrabs;
    [SerializeField] private GameObject springs;
    [SerializeField] private GameObject metalscaffolds;
    [SerializeField] private GameObject collectibleGears;
    [SerializeField] private GameObject cables;

    [SerializeField] private GameObject craftedItem;

    public void PlayCraftingAnimation()
    {
        animator.SetTrigger("PlayAnim");
    }

    public void DisableAllGameObjectsAndActveOne()
    {
        metalCrabs.SetActive(false);
        springs.SetActive(false);
        metalscaffolds.SetActive(false);
        collectibleGears.SetActive(false);
        cables.SetActive(false);

        craftedItem.SetActive(true);
    }
}
