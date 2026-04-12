using System.Collections;
using UnityEngine;

public class PictureTerrainPuzzleManager : MonoBehaviour
{
    [SerializeField] private PictureTerrainMovingObject interactableItemsClubs;
    [SerializeField] private PictureTerrainMovingObject interactableItemsDiamont;
    [SerializeField] private PictureTerrainMovingObject interactableItemsSpades;
    [SerializeField] private PictureTerrainMovingObject interactableItemsHeart;

    [SerializeField] private GameObject chestObject;
    [SerializeField] private ChestManager chestManager;

    private void DisableMovingObjects()
    {
        interactableItemsClubs.sphereCollider.enabled = false;
        interactableItemsDiamont.sphereCollider.enabled = false;
        interactableItemsSpades.sphereCollider.enabled = false;
        interactableItemsHeart.sphereCollider.enabled = false;

        interactableItemsClubs.ActivePuzzle = false;
        interactableItemsDiamont.ActivePuzzle = false;
        interactableItemsSpades.ActivePuzzle = false;
        interactableItemsHeart.ActivePuzzle = false;
    }

    public void CheckWinPuzzle()
    {
        if (interactableItemsClubs.IsInPlace && interactableItemsDiamont.IsInPlace && interactableItemsSpades.IsInPlace && interactableItemsHeart.IsInPlace)
        {
            DisableMovingObjects();
            StartCoroutine(MoveChestCoroutine());
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
            Debug.Log("Puzzle Win");
        }
    }

    private IEnumerator MoveChestCoroutine()
    {
        float duration = 4f;
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
