using System.Collections;
using UnityEngine;

public class ClockSymbolsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clockSymbols;
    [SerializeField] private GameObject[] symbolsPlaceCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private LaserBeam laserBeam;

    public void CheckAllClockSymbols()
    {
        foreach (var clockSymbol in clockSymbols)
        {
            if (!clockSymbol.activeInHierarchy)
            {
                return;
            }
        }
        WinPuzzle();
    }


    private void WinPuzzle()
    {
        foreach (var clockSymbol in clockSymbols)
        {
            clockSymbol.GetComponent<BoxCollider>().enabled = false;
        }

        foreach (var col in symbolsPlaceCollider)
        {
            col.GetComponent<BoxCollider>().enabled = false;
        }

        animator.SetTrigger("Open");
        StartCoroutine(StartLaser());
    }

    private IEnumerator StartLaser()
    {
        yield return new WaitForSeconds(1);
        laserBeam.ToggleLaser(true);
    }
}
