using System.Collections;
using UnityEngine;

public class RotateAllGearsRoomTen : MonoBehaviour
{
    [SerializeField] private Transform[] gears;


  /*  private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            RotateAllGears();
        }
    }*/

    public void RotateAllGears()
    {
        StartCoroutine(RotateGears(gears.Length));
    }

    public void RotateTenGears()
    {
        StartCoroutine(RotateGears(10));
    }

    private IEnumerator RotateGears(int count)
    {
        Services.Audio.PlaySFX("MoveGearRoomTen");
        float duration = 6f;
        float elapsedTime = 0f;
        float rotationSpeed = 360f / duration;

        int limit = Mathf.Min(count, gears.Length);

        while (elapsedTime < duration)
        {
            for (int i = 0; i < limit; i++)
            {
                gears[i].Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.World);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}