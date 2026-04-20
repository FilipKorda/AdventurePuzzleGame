using System.Collections;
using UnityEngine;

public class MovingWallwithPaintingManager : MonoBehaviour
{
    [SerializeField] private GameObject gate;

    [SerializeField] private ChestManager chestManager;

    [SerializeField] private ImageSlider imageSliderAnimal;
    [SerializeField] private ImageSlider imageSliderFace;
    [SerializeField] private ImageSlider imageSliderWeapon;
    [SerializeField] private ImageSlider imageSliderFlower;
    [SerializeField] private ImageSlider imageSliderNeckles;

    [SerializeField] private int animalWinIndex;
    [SerializeField] private int faceWinIndex;
    [SerializeField] private int weaponWinIndex;
    [SerializeField] private int flowerWinIndex;
    [SerializeField] private int necklesWinIndex;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MoveGateUp();
        }
    }


    public void CheckWinPuzzle()
    {
        if (imageSliderAnimal.CurrentIndex == animalWinIndex &&
            imageSliderFace.CurrentIndex == faceWinIndex &&
            imageSliderWeapon.CurrentIndex == weaponWinIndex &&
            imageSliderFlower.CurrentIndex == flowerWinIndex &&
            imageSliderNeckles.CurrentIndex == necklesWinIndex)
        {
            chestManager.OpenChest();
            MoveGateUp();
            Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
        }
    }

    private void MoveGateUp()
    {
        StartCoroutine(CoututineMoveGateUp());
        Services.Audio.PlaySFX("CloseOpenGate"); 
    }

    private IEnumerator CoututineMoveGateUp()
    {
        Vector3 startPos = gate.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 3.8f, 0);
        float elapsed = 0f;
        float duration = 6f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            gate.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        gate.transform.position = targetPos;
    }
}