
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSelectionManager : MonoBehaviour
{
    public static SquareSelectionManager Instance { get; private set; }

    [SerializeField] private SquareInAllSquarePuzzle[] allSquares;

    private SquareInAllSquarePuzzle firstSelectedSquare;
    private SquareInAllSquarePuzzle currentEndSquare;
    private bool directionWasChosen = false;

    public bool canSelect = false;

    [SerializeField] private float delayBetweenSquares = 0.08f;
    private bool isAnimatingSelection = false;

    [SerializeField] private GameObject leftChest;
    [SerializeField] private ChestManager leftChestManager;
    [SerializeField] private GameObject rightChest;
    [SerializeField] private ChestManager rightChestManager;

    [SerializeField] private float chestMoveDistance = 2f;
    [SerializeField] private float chestMoveDuration = 1f;

    [SerializeField] private CoverAllSquarePuzzlePillar coverAllSquarePuzzlePillar;
    //[SerializeField] private PresurePlateManager presurePlateManager;
    [SerializeField] private PuzzleBoardHandle puzzleBoardHandle;

    public bool puzzleIsCompleted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool TrySelectSquare(SquareInAllSquarePuzzle square)
    {
        if (!canSelect) return false;
        if (isAnimatingSelection) return false;
        if (square == null) return false;
        if (square.IsBlocker) return false;
        if (square.squareIsSelected) return false;

        Services.Audio.PlaySFX("SquareSelect");

        if (firstSelectedSquare == null)
        {
            firstSelectedSquare = square;
            currentEndSquare = square;

            firstSelectedSquare.SelectAsFirst();
            ShowDirectionsForCurrentEndSquare();

            return true;
        }

        if (!IsNeighbor(currentEndSquare, square))
        {
            Debug.Log("Nastepny kwadrat musi sasiadowac z ostatnim zaznaczonym kwadratem.");
            return false;
        }

        currentEndSquare.HideAllArrows();
        StartCoroutine(SelectSquaresInDirectionRoutine(square));
        return true;
    }

    private void ShowDirectionsForCurrentEndSquare()
    {
        if (currentEndSquare == null) return;

        bool canGoLeft = CanGoTo(currentEndSquare.row, currentEndSquare.column + 1);
        bool canGoRight = CanGoTo(currentEndSquare.row, currentEndSquare.column - 1);
        bool canGoUp = CanGoTo(currentEndSquare.row - 1, currentEndSquare.column);
        bool canGoDown = CanGoTo(currentEndSquare.row + 1, currentEndSquare.column);

        currentEndSquare.ShowAvailableDirections(canGoLeft, canGoRight, canGoUp, canGoDown);
    }

    private IEnumerator SelectSquaresInDirectionRoutine(SquareInAllSquarePuzzle secondSquare)
    {
        isAnimatingSelection = true;
        directionWasChosen = true;

        List<SquareInAllSquarePuzzle> squaresToSelect = GetSquaresInDirection(secondSquare);

        if (squaresToSelect.Count == 0)
        {
            ShowDirectionsForCurrentEndSquare();
            directionWasChosen = false;
            isAnimatingSelection = false;
            CheckWinOrLose();
            yield break;
        }

        for (int i = 0; i < squaresToSelect.Count; i++)
        {
            SquareInAllSquarePuzzle square = squaresToSelect[i];
            square.SelectAsDirection();
            currentEndSquare = square;

            yield return new WaitForSeconds(delayBetweenSquares);
            Services.Audio.PlaySFX("SquareSelect");
        }

        ShowDirectionsForCurrentEndSquare();
        directionWasChosen = false;
        isAnimatingSelection = false;

        CheckWinOrLose();
    }

    private List<SquareInAllSquarePuzzle> GetSquaresInDirection(SquareInAllSquarePuzzle secondSquare)
    {
        List<SquareInAllSquarePuzzle> result = new List<SquareInAllSquarePuzzle>();

        int directionRow = secondSquare.row - currentEndSquare.row;
        int directionColumn = secondSquare.column - currentEndSquare.column;

        int currentRow = currentEndSquare.row + directionRow;
        int currentColumn = currentEndSquare.column + directionColumn;

        while (true)
        {
            SquareInAllSquarePuzzle square = GetSquareAt(currentRow, currentColumn);

            if (square == null)
                break;

            if (square.squareIsSelected || square.IsBlocker)
                break;

            result.Add(square);

            currentRow += directionRow;
            currentColumn += directionColumn;
        }

        return result;
    }

    private bool HasAnyAvailableMove()
    {
        if (currentEndSquare == null) return false;

        bool canGoLeft = CanGoTo(currentEndSquare.row, currentEndSquare.column + 1);
        bool canGoRight = CanGoTo(currentEndSquare.row, currentEndSquare.column - 1);
        bool canGoUp = CanGoTo(currentEndSquare.row - 1, currentEndSquare.column);
        bool canGoDown = CanGoTo(currentEndSquare.row + 1, currentEndSquare.column);

        return canGoLeft || canGoRight || canGoUp || canGoDown;
    }

    private void CheckWinOrLose()
    {
        if (AreAllAvailableSquaresSelected())
        {
            WinPuzzle();
            return;
        }

        if (!HasAnyAvailableMove())
        {
            LosePuzzle();
        }
    }

    private bool AreAllAvailableSquaresSelected()
    {
        for (int i = 0; i < allSquares.Length; i++)
        {
            SquareInAllSquarePuzzle square = allSquares[i];

            if (square == null) continue;
            if (square.IsBlocker) continue;

            if (!square.squareIsSelected)
            {
                return false;
            }
        }

        return true;
    }


    private bool CanGoTo(int row, int column)
    {
        SquareInAllSquarePuzzle square = GetSquareAt(row, column);
        return square != null && !square.squareIsSelected && !square.IsBlocker;
    }

    private bool IsNeighbor(SquareInAllSquarePuzzle a, SquareInAllSquarePuzzle b)
    {
        int rowDifference = Mathf.Abs(a.row - b.row);
        int columnDifference = Mathf.Abs(a.column - b.column);

        return rowDifference + columnDifference == 1;
    }

   

    private SquareInAllSquarePuzzle GetSquareAt(int row, int column)
    {
        for (int i = 0; i < allSquares.Length; i++)
        {
            SquareInAllSquarePuzzle square = allSquares[i];

            if (square != null && square.row == row && square.column == column)
            {
                return square;
            }
        }

        return null;
    }

    private void WinPuzzle()
    {
        puzzleIsCompleted = true;
        canSelect = false;
        coverAllSquarePuzzlePillar.ExitAfterWin();

        
        puzzleBoardHandle.PlayAnimation();
        StartCoroutine(PlayWinSequence());
        Services.Audio.PlaySFX("SafeWinAkaPuzzleWin");
    }

    private IEnumerator PlayWinSequence()
    {
        yield return StartCoroutine(AnimateChests());

        if (leftChestManager != null)
        {
            leftChestManager.OpenChest();
        }

        if (rightChestManager != null)
        {
            rightChestManager.OpenChest();
        }
    }


/*    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(AnimateChests());
        }
    }*/

    private IEnumerator AnimateChests()
    {
        if (leftChest == null || rightChest == null)
        {
            yield break;
        }
        Services.Audio.PlaySFX("ChestMovingStone");
   

        Vector3 leftStartPosition = leftChest.transform.position;
        Vector3 rightStartPosition = rightChest.transform.position;

        Vector3 leftTargetPosition = leftStartPosition + Vector3.left * chestMoveDistance;
        Vector3 rightTargetPosition = rightStartPosition + Vector3.right * chestMoveDistance;

        float elapsedTime = 0f;

        while (elapsedTime < chestMoveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / chestMoveDuration);

            leftChest.transform.position = Vector3.Lerp(leftStartPosition, leftTargetPosition, t);
            rightChest.transform.position = Vector3.Lerp(rightStartPosition, rightTargetPosition, t);

            yield return null;
        }

        leftChest.transform.position = leftTargetPosition;
        rightChest.transform.position = rightTargetPosition;
    }

    private void LosePuzzle()
    {
        canSelect = false;
        Debug.Log("LOSE PUZZLE");

        ResetSelection();
        canSelect = true;
    }

    public void ResetSelection()
    {
        if (firstSelectedSquare != null)
        {
            firstSelectedSquare.HideAllArrows();
        }

        if (currentEndSquare != null)
        {
            currentEndSquare.HideAllArrows();
        }

        for (int i = 0; i < allSquares.Length; i++)
        {
            SquareInAllSquarePuzzle square = allSquares[i];

            if (square == null) continue;
            square.ResetSquare();
        }

        firstSelectedSquare = null;
        currentEndSquare = null;
        directionWasChosen = false;
    }
}
