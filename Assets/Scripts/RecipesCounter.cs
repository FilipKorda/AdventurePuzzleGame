using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipesCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeAmountText;
    [SerializeField] private int recipeAmount;

    private readonly List<int> recipeIds = new() { 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };

    private void Start()
    {
        if (recipeAmountText == null)
        {
            Debug.LogError("RecipesCounter: recipeAmountText nie jest przypisany.");
            return;
        }

        UpdateRecipeCount();
    }

    public void UpdateRecipeCount()
    {
        if (recipeAmountText == null) return;

        if (Inventory.Instance == null)
        {
            recipeAmount = 0;
        }
        else
        {
            recipeAmount = Inventory.Instance.CountItemsWithIds(recipeIds);
        }

        recipeAmountText.text = recipeAmount.ToString();
    }

    public void DisableThisGameObject()
    {
        gameObject.SetActive(false);
    }
}
