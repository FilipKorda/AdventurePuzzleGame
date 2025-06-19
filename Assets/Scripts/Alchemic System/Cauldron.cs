using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Cauldron : MonoBehaviour
{
    [Tooltip("Lista wszystkich przepisów, które ten kocio³ mo¿e uwarzyæ.")]
    [SerializeField] private List<AlchemyRecipe> availableRecipes;

    private List<int> currentIngredients = new List<int>();

    private InteractableItem readySolution = null;

    public void AddIngredient(int itemId)
    {
        if (HasReadySolution())
        {
            Debug.Log("Kocio³ zawiera ju¿ gotowy roztwór. Opró¿nij go najpierw.");
            return;
        }

        Debug.Log($"Dodano sk³adnik o ID: {itemId} do kot³a.");
        currentIngredients.Add(itemId);

        Inventory.Instance.RemoveItemFromInventoryByID(itemId);
        UIManager.Instance.RemoveItemFromUIByID(itemId);

        CheckForMatchingRecipe();
    }

    private void CheckForMatchingRecipe()
    {
        currentIngredients.Sort();

        foreach (var recipe in availableRecipes)
        {
            var sortedRecipeIngredients = recipe.ingredientItemIds.OrderBy(id => id).ToList();

            if (currentIngredients.SequenceEqual(sortedRecipeIngredients))
            {
                Debug.Log($"Uda³o siê uwarzyæ: {recipe.resultingPotion.GetItemName()}!");
                readySolution = recipe.resultingPotion;
                currentIngredients.Clear(); 

      
                return;
            }
        }
    }

    public bool HasReadySolution()
    {
        return readySolution != null;
    }

    public InteractableItem TakeSolution()
    {
        if (HasReadySolution())
        {
            InteractableItem solutionToReturn = readySolution;
            readySolution = null; 
            Debug.Log("Pobrano roztwór z kot³a.");
            return solutionToReturn;
        }
        return null;
    }
}