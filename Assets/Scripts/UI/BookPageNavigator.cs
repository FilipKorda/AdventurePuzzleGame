using TMPro;
using UnityEngine;

public class BookPageNavigator : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    [SerializeField] private TextMeshProUGUI pageIndicatorText;
    [SerializeField] private int currentPageIndex;

    private void Start()
    {
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, GetLastPageIndex());
        RefreshPages();
    }

    public void NextPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        if (currentPageIndex >= GetLastPageIndex())
            return;

        currentPageIndex++;
        RefreshPages();
    }

    public void PreviousPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        if (currentPageIndex <= 0)
            return;

        currentPageIndex--;
        RefreshPages();
    }

    public void GoToPage(int pageIndex)
    {
        if (pages == null || pages.Length == 0)
            return;

        currentPageIndex = Mathf.Clamp(pageIndex, 0, GetLastPageIndex());
        RefreshPages();
    }

    public void RefreshPages()
    {
        if (pages == null || pages.Length == 0)
        {
            RefreshIndicator(0, 0);
            return;
        }

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == currentPageIndex);
        }

        RefreshIndicator(currentPageIndex + 1, pages.Length);
    }

    private void RefreshIndicator(int currentPageNumber, int totalPages)
    {
        if (pageIndicatorText == null)
            return;

        pageIndicatorText.text = totalPages <= 0
            ? "0 / 0"
            : currentPageNumber + " / " + totalPages;
    }

    private int GetLastPageIndex()
    {
        return pages == null ? 0 : Mathf.Max(0, pages.Length - 1);
    }
}
