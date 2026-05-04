using UnityEngine;
using System.Linq;

public class SwitchBaseManager : MonoBehaviour
{
    [SerializeField] private BoolVariable[] boolVariables;
    private bool allSwitchesActivated = false;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider[] boxColliders;
    [SerializeField] private ChunkDisabler chunkDisabler;

    private void OnEnable()
    {
        foreach (var variable in boolVariables)
        {
            variable.OnValueChanged += CheckVariables;
        }
    }

    private void OnDisable()
    {
        foreach (var variable in boolVariables)
        {
            variable.OnValueChanged -= CheckVariables;
        }
    }

    private void CheckVariables()
    {
        if (allSwitchesActivated) return;

        if (boolVariables.All(v => v.Value))
        {
            //Debug.Log("Sukces! Wszystkie prze³¹czniki s¹ ON!");
            chunkDisabler.Run();
            DisableBoxColliders();
            animator.SetTrigger("Interact");
            allSwitchesActivated = true;
        }
    }

    private void DisableBoxColliders()
    {
        foreach (BoxCollider collider in boxColliders)
        {
            collider.enabled = false;
        }
    }
}