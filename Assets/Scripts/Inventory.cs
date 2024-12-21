using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    private List<IPickupable> inventory = new(); 

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Dodaje przedmiot do ekwipunku i UI.
    /// </summary>
    public void AddItemToInventory(IPickupable iPickupable)
    {
        inventory.Add(iPickupable);
        UIManager.Instance.AddItemToUI(iPickupable);
    }

    /// <summary>
    /// Usuwa przedmiot z ekwipunku i UI.
    /// </summary>
    public void RemoveItemFromInventoryByID(int itemId)
    {
        IPickupable itemToRemove = inventory.Find(item => item.GetItemId() == itemId);
        if (itemToRemove != null)
        {
            inventory.Remove(itemToRemove);       
        }
    }

}
