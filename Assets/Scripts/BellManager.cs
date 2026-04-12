using System.Collections;
using UnityEngine;

public class BellManager : MonoBehaviour
{
    private int currentStep = 0;

    [SerializeField] private GameObject chestObject;
    [SerializeField] private ChestManager chestManager;
    [SerializeField] private BoxCollider[] bellColliders;

    public void RegisterBell(int bellIndex)
    {
        if (bellIndex == currentStep)
        {
            currentStep++;

            if (currentStep == 4)
            {
                WinPuzzle();
            }
        }
        else
        {
            ResetSequence();
        }
    }

    private void WinPuzzle()
    {
        Debug.Log("WinPuzzle");
        StartCoroutine(MoveChestCoroutine());

        foreach (var bell in bellColliders)
        {
           bell.enabled = false;
        }
    }

    private void ResetSequence()
    {
        currentStep = 0;
        Debug.Log("Reset Sequence");
    }

    private IEnumerator MoveChestCoroutine()
    {
        float duration = 4f;
        float time = 0f;
        Vector3 startPosition = chestObject.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0.27f, 0f);

        while (time < duration)
        {
            time += Time.deltaTime;
            chestObject.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            yield return null;
        }

        chestObject.transform.position = targetPosition;
        chestManager.OpenChest();
    }
}