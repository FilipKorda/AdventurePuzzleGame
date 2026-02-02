using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Cauldron : MonoBehaviour
{
    [Tooltip("Lista wszystkich przepisów, które ten kocio³ mo¿e uwarzyæ.")]
    [SerializeField] private List<AlchemyRecipe> availableRecipes;
    [SerializeField] private MultiStageMover multiStageMover;

    private List<int> currentIngredients = new List<int>();
    private int maxIngredientsInRecipe = 5;

    private InteractableItem readySolution = null;

    public InteractableItem emptyBucket;

    public InteractableItem badRecipiesSolution;

    public Renderer cylinder;
    public TextMeshProUGUI brewingTimeText;

    [Tooltip("Materia³, który zostanie ustawiony gdy przepis siê nie zgadza (brudna woda).")]
    [SerializeField] private Material badRecipeMaterial;
    private Material initialCylinderMaterial;

    public bool isBrewing = false;
    private AlchemyRecipe currentRecipe = null;
    private float remainingBrewingTime = 0f;
    private Material targetMaterial = null;

    [Tooltip("Czas gotowania dla nieudanych przepisów (jeœli przepis nie istnieje).")]
    [SerializeField] private float badRecipeBrewingTime = 3f;
    [SerializeField] private ParticleSystem ps;

    public GameObject fireObject;

    public LocalizedString localizeString;
    public LocalizedString localizeTwoString;
    public LocalizedString localizeThreeString;
    public LocalizedString localizeStringGetMixture;


    private void Awake()
    {
        if (cylinder != null)
        {
            initialCylinderMaterial = cylinder.sharedMaterial;
        }
    }

    private void Update()
    {
        if (!isBrewing)
        {
            if (brewingTimeText != null)
            {
                brewingTimeText.text = string.Empty;
            }
            return;
        }

        remainingBrewingTime -= Time.deltaTime;

        if (brewingTimeText != null)
        {
            brewingTimeText.text = $"{Mathf.Max(0f, remainingBrewingTime):F1}s";
        }

        if (remainingBrewingTime <= 0f)
        {
            CompleteBrewing();
        }
    }

    public void AddIngredient(int itemId)
    {
        if(multiStageMover.fireObject.activeSelf == false)
        {
            NotificationSystem.Instance.ShowNotification(localizeString, 3);
          //  Debug.Log("Podpal ogieñ zanim  bedziesz wk³adaæ sk³adniki  do kocio³a.");
            return;
        }

        if (HasReadySolution())
        {
            NotificationSystem.Instance.ShowNotification(localizeTwoString, 3);
          //  Debug.Log("Kocio³ zawiera ju¿ gotowy roztwór. Opró¿nij go najpierw.");
            return;
        }

        multiStageMover.MoveWaterUp();
        Debug.Log($"Dodano sk³adnik o ID: {itemId} do kot³a.");
        currentIngredients.Add(itemId);

        if (itemId == (int)ItemID.AcidBucket || itemId == (int)ItemID.WaterBucket ||
              itemId == (int)ItemID.BloodBucket || itemId == (int)ItemID.EmptyBucket)
        {
            Inventory.Instance.RemoveItemFromInventoryByID(itemId);
            UIManager.Instance.RemoveItemFromUIByID(itemId);

            Inventory.Instance.AddItemToInventory(emptyBucket);
        }
        else
        {
            Inventory.Instance.RemoveItemFromInventoryByID(itemId);
            UIManager.Instance.RemoveItemFromUIByID(itemId);
        }

        CheckForMatchingRecipe();
    }

    private void CheckForMatchingRecipe()
    {
        currentIngredients.Sort();

        foreach (var recipe in availableRecipes)
        {
            if (currentIngredients.Count == recipe.ingredientItemIds.Count)
            {
                var sortedRecipeIngredients = recipe.ingredientItemIds.OrderBy(id => id).ToList();

                if (currentIngredients.SequenceEqual(sortedRecipeIngredients))
                {
                    if (!IsFireActive())
                    {
                        NotificationSystem.Instance.ShowNotification(localizeString, 3);
                        //Debug.Log("Nie mo¿na rozpocz¹æ gotowania — ogieñ jest zgaszony. W³¹cz ogieñ, aby zacz¹æ gotowaæ.");
                        return;
                    }

                    Debug.Log($"Rozpoczynanie gotowania: {recipe.resultingPotion.GetItemName()} (czas: {recipe.brewingTime}s)");

                    StartBrewing(recipe);
                    currentIngredients.Clear();
                    return;
                }
            }
        }

        if (currentIngredients.Count >= maxIngredientsInRecipe)
        {
            if (!IsFireActive())
            {
                NotificationSystem.Instance.ShowNotification(localizeString, 3);
                //Debug.Log("Nie mo¿na rozpocz¹æ gotowania — ogieñ jest zgaszony. W³¹cz ogieñ, aby zacz¹æ gotowaæ.");
                return;
            }

            StartBrewingBadRecipe();
            currentIngredients.Clear();
        }
    }

    private bool IsFireActive()
    {
        if (fireObject == null)
        {
            Debug.LogWarning("Brak przypisanego 'fireObject' w komponencie Cauldron.");
            return false;
        }

        return fireObject.activeInHierarchy;
    }

    private void StartBrewing(AlchemyRecipe recipe)
    {
        isBrewing = true;
        currentRecipe = recipe;
        remainingBrewingTime = Mathf.Max(0.01f, recipe.brewingTime);
        targetMaterial = recipe.recipeMat;

        if (cylinder == null)
        {
            Debug.LogWarning("Brak przypisanego renderera 'cylinder' w komponencie Cauldron.");
        }
        else if (targetMaterial == null)
        {
            Debug.LogWarning($"Brak przypisanego 'recipeMat' w przepisie {recipe.name}.");
        }

        if (brewingTimeText == null)
        {
            Debug.LogWarning("Brak przypisanego 'brewingTimeText' w komponencie Cauldron.");
        }

        if (ps != null && !ps.isPlaying)
        {
            ps.Play();
        }
    }

    private void StartBrewingBadRecipe()
    {
        isBrewing = true;
        currentRecipe = null;
        remainingBrewingTime = Mathf.Max(0.01f, badRecipeBrewingTime);
        targetMaterial = badRecipeMaterial;

        if (cylinder == null)
        {
            Debug.LogWarning("Brak przypisanego renderera 'cylinder' w komponencie Cauldron.");
        }
        else if (badRecipeMaterial == null)
        {
            Debug.LogWarning("Brak przypisanego 'badRecipeMaterial' w komponencie Cauldron.");
        }

        if (brewingTimeText == null)
        {
            Debug.LogWarning("Brak przypisanego 'brewingTimeText' w komponencie Cauldron.");
        }

        if (ps != null && !ps.isPlaying)
        {
            ps.Play();
        }
    }

    private void CompleteBrewing()
    {
        isBrewing = false;

        if (currentRecipe != null)
        {
            readySolution = currentRecipe.resultingPotion;
            Debug.Log($"Ukoñczono gotowanie: {readySolution.GetItemName()}");
        }
        else
        {
            readySolution = badRecipiesSolution;
            Debug.Log("Ukoñczono gotowanie: nieudany przepis (brudna woda).");
        }

        if (cylinder != null && targetMaterial != null)
        {
            cylinder.material = targetMaterial;
        }

        if (brewingTimeText != null)
        {
            brewingTimeText.text = "";
        }

        if (ps != null && ps.isPlaying)
        {
            ps.Stop();
        }

        currentRecipe = null;
        targetMaterial = null;
        remainingBrewingTime = 0f;
    }

    public bool HasReadySolution()
    {
        return readySolution != null;
    }

    public InteractableItem TakeSolution()
    {
        if (HasReadySolution())
        {
            multiStageMover.ResetObjectState();
            InteractableItem solutionToReturn = readySolution;
            readySolution = null;

            if (cylinder != null && initialCylinderMaterial != null)
            {
                cylinder.material = initialCylinderMaterial;
            }

            NotificationSystem.Instance.ShowNotification(localizeStringGetMixture, 3);
            Debug.Log("Pobrano roztwór z kot³a.");
            return solutionToReturn;
        }
        return null;
    }
}