using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<IPickupable> inventory = new();

    [Header("Drop Settings")]
    [SerializeField] private Transform dropPoint;

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

    public void AddItemToInventory(IPickupable iPickupable)
    {
        inventory.Add(iPickupable);
        UIManager.Instance.AddItemToUI(iPickupable);
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
            Debug.LogWarning($"Próbowano wyrzuciæ przedmiot o ID {itemId}, ale nie ma go w ekwipunku.");
            return;
        }

        GameObject itemPrefab = itemToDrop.GetItemPrefab();
        if (itemPrefab == null)
        {
            return;
        }

        Instantiate(itemPrefab, dropPoint.position, dropPoint.rotation);
        Debug.Log($"Wyrzucono przedmiot: {itemToDrop.GetItemName()}");

        RemoveItemFromInventoryByID(itemId);
        UIManager.Instance.RemoveItemFromUIByID(itemId);
    }
}