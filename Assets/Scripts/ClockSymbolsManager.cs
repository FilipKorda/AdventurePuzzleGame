using UnityEngine;

public class ClockSymbolsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clockSymbols;
    [SerializeField] private GameObject[] symbolsPlaceCollider;

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
        foreach(var clockSymbol in clockSymbols)
        {
            clockSymbol.GetComponent<BoxCollider>().enabled = false;
        }

        foreach(var col in symbolsPlaceCollider)
        {
            col.GetComponent<BoxCollider>().enabled = false;
        }

        Debug.Log("You win the puzzle!");
    }
}
