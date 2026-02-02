using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "ReadableAndInteractableTextData", menuName = "ScriptableObjects/ReadableAndInteractableTextData")]
public class ReadableAndInteractableTextData : ScriptableObject
{
    [Header("Text to Display")]
    public LocalizedString headerText;
    public LocalizedString[] bookPages;
    public LocalizedString pressEorQText;

    public int currentPageIndex;

    [Header("Unlocks")]
    public bool[] unlockedPages;

    private void OnValidate()
    {
        if (bookPages == null)
            return;

        if (unlockedPages == null || unlockedPages.Length != bookPages.Length)
        {
            var newArr = new bool[bookPages.Length];
            if (unlockedPages != null)
            {
                int copyLen = Mathf.Min(unlockedPages.Length, newArr.Length);
                for (int i = 0; i < copyLen; i++)
                    newArr[i] = unlockedPages[i];
            }
            unlockedPages = newArr;
        }

        if (unlockedPages.Length > 0)
            unlockedPages[0] = true;

        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, Mathf.Max(0, bookPages.Length - 1));
    }

    public bool IsPageUnlocked(int index)
    {
        if (bookPages == null) return false;
        if (index < 0 || index >= bookPages.Length) return false;
        if (index == 0) return true;
        if (unlockedPages == null) return false;
        return unlockedPages[index];
    }

    public void UnlockPage(int index)
    {
        if (bookPages == null) return;
        if (index == 0) return;

        if (unlockedPages == null || unlockedPages.Length != bookPages.Length)
            unlockedPages = new bool[bookPages.Length];

        if (index >= 0 && index < unlockedPages.Length)
            unlockedPages[index] = true;
    }

    public int GetFirstUnlockedPageIndex()
    {
        if (bookPages == null || bookPages.Length == 0) return 0;

        for (int i = 0; i < bookPages.Length; i++)
            if (IsPageUnlocked(i))
                return i;
        return 0;
    }

    public void UnlockPageForRecipeId(int recipeId)
    {
        const int recipeIdBase = 25;
        int index = recipeId - recipeIdBase;
        if (bookPages == null) return;
        if (index >= 1 && index < bookPages.Length)
            UnlockPage(index);
    }
}
