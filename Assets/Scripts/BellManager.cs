using System.Collections;
using UnityEngine;

public class BellManager : MonoBehaviour
{
    private int currentStep = 0;

    [SerializeField] private GameObject chestObject;
    [SerializeField] private ChestManager chestManager;
    [SerializeField] private BoxCollider[] bellColliders;

    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;

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
        StartCoroutine(MoveChestCoroutine());
       
        foreach (var bell in bellColliders)
        {
           bell.enabled = false;
        }
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void ResetSequence()
    {
        currentStep = 0;
    }

    private IEnumerator MoveChestCoroutine()
    {
        rotateAllGearsRoomTen.RotateTenGears();
        Services.Audio.PlaySFX("ChestMovingStone");
        float duration = 3f;
        float time = 0f;
        Vector3 startPosition = chestObject.transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0.29f, 0f);

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