using UnityEngine;

public class SquareInAllSquarePuzzle : MonoBehaviour
{
    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;
    [SerializeField] private GameObject upArrow;
    [SerializeField] private GameObject downArrow;

    [SerializeField] private BoxCollider squareBoxCollider;
    [SerializeField] private bool isBlocker = false;

    public int row;
    public int column;
    public bool squareIsSelected = false;

    public bool IsBlocker => isBlocker;

    private Renderer squareRenderer;
    private Color baseColor;

    [SerializeField] private Color selectAsFirst;
    [SerializeField] private Color selectAsDirection;

    private void Awake()
    {
        squareRenderer = GetComponentInChildren<Renderer>();
        baseColor = squareRenderer.material.color;

        HideAllArrows();

        if (isBlocker)
        {
            squareRenderer.material.color = Color.red;
            squareBoxCollider.enabled = false;
        }
    }

    public void SetBaseColor()
    {
        if (isBlocker)
        {
            squareRenderer.material.color = Color.red;
            return;
        }

        squareRenderer.material.color = baseColor;
    }

    public void SelectAsFirst()
    {
        if (isBlocker) return;

        squareIsSelected = true;
        squareRenderer.material.color = selectAsFirst;
        squareBoxCollider.enabled = false;
    }

    public void SelectAsDirection()
    {
        if (isBlocker) return;

        squareIsSelected = true;
        squareRenderer.material.color = selectAsDirection;
        squareBoxCollider.enabled = false;
    }

    public void ShowAvailableDirections(bool canGoLeft, bool canGoRight, bool canGoUp, bool canGoDown)
    {
        if (isBlocker) return;

        if (leftArrow != null) leftArrow.SetActive(canGoLeft);
        if (rightArrow != null) rightArrow.SetActive(canGoRight);
        if (upArrow != null) upArrow.SetActive(canGoUp);
        if (downArrow != null) downArrow.SetActive(canGoDown);
    }

    public void HideAllArrows()
    {
        if (leftArrow != null) leftArrow.SetActive(false);
        if (rightArrow != null) rightArrow.SetActive(false);
        if (upArrow != null) upArrow.SetActive(false);
        if (downArrow != null) downArrow.SetActive(false);
    }

    public void ResetSquare()
    {
        squareIsSelected = false;
        HideAllArrows();
        SetBaseColor();

        if (squareBoxCollider != null)
        {
            squareBoxCollider.enabled = !isBlocker;
        }
    }
}
