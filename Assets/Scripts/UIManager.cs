using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    public ReadablePanel readablePanel;
    public LockPickPanel lockPickPanel;
    private List<ItemSlot> itemSlots = new();
    private int selectedItemId = -1;

    [Header("Input Settings")]
    [SerializeField] private InputActionReference[] selectItemActions;

    private void OnEnable()
    {
        RegisterSelectItemActions();
    }

    private void OnDisable()
    {
        UnregisterSelectItemActions();
    }

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
    }

    /// Dodaje przedmiot do UI (ekranu).
    public void AddItemToUI(IPickupable ipickupable)
    {
        GameObject itemSlot = Instantiate(itemSlotPrefab, inventoryPanel);

        if (itemSlot.TryGetComponent<ItemSlot>(out var itemSlotScript))
        {
            itemSlotScript.SetItem(ipickupable.GetItemName(), ipickupable.GetItemSprite(), ipickupable.GetItemId());

            StartCoroutine(HideItemNameAfterDelay(itemSlotScript, 2f));

            itemSlots.Add(itemSlotScript);
        }

    }

    /// Usuwa przedmiot z UI.
    public void RemoveItemFromUIByID(int itemId)
    {
        foreach (var itemSlot in itemSlots)
        {
            if (itemSlot.GetItemId() == itemId)
            {
                Destroy(itemSlot.gameObject);
                itemSlots.Remove(itemSlot);
                break;
            }
        }
    }


    private IEnumerator HideItemNameAfterDelay(ItemSlot itemSlot, float delay)
    {
        yield return new WaitForSeconds(delay);

        itemSlot.ClearItemName();
    }

    private void RegisterSelectItemActions()
    {
        for (int i = 0; i < selectItemActions.Length; i++)
        {
            int index = i;
            selectItemActions[i].action.performed += context => OnSelectItemPerformed(index);
        }
    }

    private void UnregisterSelectItemActions()
    {
        for (int i = 0; i < selectItemActions.Length; i++)
        {
            selectItemActions[i].action.performed -= context => OnSelectItemPerformed(i);
        }
    }

    private void OnSelectItemPerformed(int index)
    {
        if (index >= 0 && index < itemSlots.Count)
        {
            SelectItem(index);
        }
    }

    private void SelectItem(int index)
    {
        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            itemSlots[selectedItemId].SetHighlighted(false);
        }

        selectedItemId = index;

        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            itemSlots[selectedItemId].SetHighlighted(true);
            Debug.Log($"Zaznaczono element: {itemSlots[selectedItemId].GetItemName()} (ItemID: {itemSlots[selectedItemId].GetItemId()})");
        }
        else
        {
            Debug.LogWarning($"Próbowano zaznaczyæ nieistniej¹cy element o indeksie {selectedItemId}.");
        }
    }

    public int GetSelectedItemId()
    {
        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            return itemSlots[selectedItemId].GetItemId();
        }
        return 0; // Wartoœæ oznaczaj¹ca brak zaznaczonego przedmiotu czyli uzywanie  przedmiotów krych nie zbierasz, dŸwignie, skrzynie itd
    }
 
}
