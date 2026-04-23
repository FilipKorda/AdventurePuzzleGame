using System.Collections;
using UnityEngine;

public class CorrectSixteenPanelsManager : MonoBehaviour
{
    [SerializeField] private CorrectSixteenPanel[] panels = new CorrectSixteenPanel[16];

    [Header("Poprawny uklad indeksow obrazow dla 16 paneli")]
    [SerializeField] private int[] correctSequence = new int[16];

    [SerializeField] private PuzzleBoardHandle puzzleBoardHandle;

    [SerializeField] private GameObject leftChest;
    [SerializeField] private ChestManager leftChestManager;
    [SerializeField] private GameObject rightChest;
    [SerializeField] private ChestManager rightChestManager;

    [SerializeField] private float chestMoveDistance = 2f;
    [SerializeField] private float chestMoveDuration = 1f;
    [SerializeField] private CorrectSixteenSymbolsPillar correctSixteenSymbolsPillar;

    [SerializeField] private ChestManager rewardChestManager;

    [SerializeField] private GameObject leftPaper;
    private Coroutine waitForPaperCoroutine;
    private bool isClosingChest = false;

    [SerializeField] private GameObject pillar;
    [SerializeField] private float pillarMoveDownDistance = 1f;
    [SerializeField] private RotateAllGearsRoomTen rotateAllGearsRoomTen;

    public void CheckPuzzle()
    {
        if (!IsSequenceCorrect()) return;

        WinPuzzle();
    }

    private bool IsSequenceCorrect()
    {
        if (panels == null || panels.Length != 16)
        {
            Debug.LogWarning("Panels array must contain exactly 16 elements.");
            return false;
        }

        if (correctSequence == null || correctSequence.Length != 16)
        {
            Debug.LogWarning("Correct sequence array must contain exactly 16 elements.");
            return false;
        }

        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] == null)
            {
                Debug.LogWarning($"Panel at index {i} is null.");
                return false;
            }

            if (panels[i].CurrentVisibleImageIndex != correctSequence[i])
            {
                return false;
            }
        }

        return true;
    }

    private void WinPuzzle()
    {
        DisablePanels();
        correctSixteenSymbolsPillar.ExitAfterWin();
        puzzleBoardHandle.StopAnimation();
        rewardChestManager.OpenChest();
        CloseAndHideChest();
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private void CloseAndHideChest()
    {
        if (isClosingChest) return;

        if (!leftPaper.activeInHierarchy)
        {
            isClosingChest = true;
            StartCoroutine(CourutineCloseAndHideChest());
        }
        else
        {
            if (waitForPaperCoroutine == null)
            {
                waitForPaperCoroutine = StartCoroutine(WaitUntilLeftPaperIsHidden());
            }
        }
    }

    private IEnumerator WaitUntilLeftPaperIsHidden()
    {
        while (leftPaper.activeInHierarchy)
        {
            yield return new WaitForSeconds(1f);
        }

        waitForPaperCoroutine = null;

        if (!isClosingChest)
        {
            isClosingChest = true;
            StartCoroutine(CourutineCloseAndHideChest());
        }
    }

    private IEnumerator CourutineCloseAndHideChest()
    {
        leftChestManager.CloseChest();
        rightChestManager.CloseChest();

        yield return new WaitForSeconds(chestMoveDuration);

        Vector3 leftStartPosition = leftChest.transform.position;
        Vector3 rightStartPosition = rightChest.transform.position;

        Vector3 leftTargetPosition = leftStartPosition + Vector3.right * chestMoveDistance;
        Vector3 rightTargetPosition = rightStartPosition + Vector3.left * chestMoveDistance;

        float time = 0f;

        while (time < chestMoveDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / chestMoveDuration);

            leftChest.transform.position = Vector3.Lerp(leftStartPosition, leftTargetPosition, t);
            rightChest.transform.position = Vector3.Lerp(rightStartPosition, rightTargetPosition, t);

            yield return null;
        }

        leftChest.transform.position = leftTargetPosition;
        rightChest.transform.position = rightTargetPosition;

 
        isClosingChest = false;
        MovePilalrDown();
    }


    private void MovePilalrDown()
    {
        StartCoroutine(CourtineMovePillarDown());
    }

    private IEnumerator CourtineMovePillarDown()
    {

        rotateAllGearsRoomTen.RotateAllGears();
        yield return new WaitForSeconds(1f);

        Vector3 startPosition = pillar.transform.position;
        Vector3 targetPosition = startPosition + Vector3.down * pillarMoveDownDistance;

        float duration = 3f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            pillar.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        pillar.transform.position = targetPosition;
    }



    private void DisablePanels()
    {
        foreach (var panel in panels)
        {
            panel.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
