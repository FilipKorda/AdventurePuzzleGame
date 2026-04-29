using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<IPickupable> inventory = new();

    [Header("Drop Settings")]
    [SerializeField] private Transform dropPoint;
    [Header("Recpie Counter")]
    [SerializeField] private RecipesCounter recipesCounter;


    public HashSet<ItemID> CollectedItems = new HashSet<ItemID>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (dropPoint == null)
        {
            Debug.LogError("Drop Point nie jest przypisany w komponencie Inventory! Przypisz pusty obiekt-dziecko gracza.");

            dropPoint = transform;
        }
    }

    public bool HasItemWithId(int itemId)
    {
        return inventory.Any(item => item.GetItemId() == itemId);
    }

    public int CountItemsWithIds(IEnumerable<int> ids)
    {
        if (ids == null) return 0;
        var set = ids is HashSet<int> hs ? hs : new HashSet<int>(ids);
        return inventory.Count(item => set.Contains(item.GetItemId()));
    }

    public bool AddItemToInventory(IPickupable iPickupable)
    {
        if (UIManager.Instance != null && !UIManager.Instance.CanAddItemToUI())
        {
            //Debug.Log("Nie mo¿na podnieœæ przedmiotu — masz ju¿ maksymaln¹ liczbê przedmiotów, które mo¿esz nosiæ.");
            return false;
        }

        inventory.Add(iPickupable);
        UIManager.Instance?.AddItemToUI(iPickupable);
        return true;
    }

    public void AddToInventoryAlchemyRecipe(IPickupable iPickupable)
    {
       // Debug.LogWarning($"Dodano przedmiot do alchemicznego przepisu: {iPickupable.GetItemId()}");
        inventory.Add(iPickupable);
        recipesCounter.UpdateRecipeCount();
    }

    public void RemoveFromInventoryAlchemyRecipe(int itemId)
    {
        IPickupable itemToRemove = inventory.Find(item => item.GetItemId() == itemId);
        if (itemToRemove != null)
        {
            inventory.Remove(itemToRemove);
           // Debug.LogWarning($"Usuniêto przedmiot: {itemToRemove.GetItemId()}");
        }
    }

    public void RemoveItemFromInventoryByID(int itemId)
    {
        IPickupable itemToRemove = inventory.Find(item => item.GetItemId() == itemId);
        if (itemToRemove != null)
        {
            inventory.Remove(itemToRemove);
        }
    }

    public void DropItemByID(int itemId)
    {
        IPickupable itemToDrop = inventory.Find(item => item.GetItemId() == itemId);
        if (itemToDrop == null)
        {
            return;
        }

        GameObject itemPrefab = itemToDrop.GetItemPrefab();
        if (itemPrefab == null)
        {
            return;
        }

        Instantiate(itemPrefab, dropPoint.position, dropPoint.rotation);
       

        RemoveItemFromInventoryByID(itemId);
        UIManager.Instance.RemoveItemFromUIByID(itemId);
    }
}