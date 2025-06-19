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

    [SerializeField] private InputActionReference navigateNextAction;
    [SerializeField] private InputActionReference navigatePreviousAction;

    private void OnEnable()
    {
        RegisterSelectItemActions();

        // NOWA REJESTRACJA AKCJI PADA
        if (navigateNextAction != null)
        {
            navigateNextAction.action.Enable();
            navigateNextAction.action.performed += OnNavigateNext;
        }
        if (navigatePreviousAction != null)
        {
            navigatePreviousAction.action.Enable();
            navigatePreviousAction.action.performed += OnNavigatePrevious;
        }
    }

    private void OnDisable()
    {
        UnregisterSelectItemActions();

        if (navigateNextAction != null)
        {
            navigateNextAction.action.performed -= OnNavigateNext;
            navigateNextAction.action.Disable();
        }
        if (navigatePreviousAction != null)
        {
            navigatePreviousAction.action.performed -= OnNavigatePrevious;
            navigatePreviousAction.action.Disable();
        }
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

    public void AddItemToUI(IPickupable ipickupable)
    {
        GameObject itemSlot = Instantiate(itemSlotPrefab, inventoryPanel);

        if (itemSlot.TryGetComponent<ItemSlot>(out var itemSlotScript))
        {
            itemSlotScript.SetItem(ipickupable.GetItemName(), ipickupable.GetItemSprite(), ipickupable.GetItemId());
            StartCoroutine(HideItemNameAfterDelay(itemSlotScript, 2f));
            itemSlots.Add(itemSlotScript);

            if (selectedItemId == -1 && itemSlots.Count > 0)
            {
                SelectItem(0);
            }
        }
    }

    public void RemoveItemFromUIByID(int itemId)
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].GetItemId() == itemId)
            {
                if (i == selectedItemId)
                {
                    itemSlots[i].SetHighlighted(false);
                    Destroy(itemSlots[i].gameObject);
                    itemSlots.RemoveAt(i);
                    selectedItemId = -1; 

                    if (itemSlots.Count > 0)
                    {
                        SelectItem(0);
                    }
                }
                else
                {
                    Destroy(itemSlots[i].gameObject);
                    itemSlots.RemoveAt(i);
                }
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
            selectItemActions[i].action.Enable(); 
            selectItemActions[i].action.performed += context => OnSelectItemPerformed(index);
        }
    }

    private void UnregisterSelectItemActions()
    {
        foreach (var actionRef in selectItemActions)
        {
            if (actionRef != null && actionRef.action != null)
            {
                actionRef.action.performed -= context => OnSelectItemPerformed(System.Array.IndexOf(selectItemActions, actionRef));
                actionRef.action.Disable();
            }
        }
    }

    private void OnNavigateNext(InputAction.CallbackContext context)
    {
        SelectNextItem();
    }

    private void OnNavigatePrevious(InputAction.CallbackContext context)
    {
        SelectPreviousItem();
    }

    public void SelectNextItem()
    {
        if (itemSlots.Count == 0) return;

        int nextIndex = (selectedItemId + 1) % itemSlots.Count;
        SelectItem(nextIndex);
    }

    public void SelectPreviousItem()
    {
        if (itemSlots.Count == 0) return;

        int prevIndex = selectedItemId - 1;
        if (prevIndex < 0)
        {
            prevIndex = itemSlots.Count - 1; 
        }
        SelectItem(prevIndex);
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
        if (itemSlots.Count == 0 || index < 0 || index >= itemSlots.Count)
        {
            if (selectedItemId != -1 && selectedItemId < itemSlots.Count)
            {
                itemSlots[selectedItemId].SetHighlighted(false);
            }
            selectedItemId = -1;
            return;
        }

        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            itemSlots[selectedItemId].SetHighlighted(false);
        }

        selectedItemId = index;

        itemSlots[selectedItemId].SetHighlighted(true);
        Debug.Log($"Zaznaczono element: {itemSlots[selectedItemId].GetItemName()} (ItemID: {itemSlots[selectedItemId].GetItemId()})");
    }

    public int GetSelectedItemId()
    {
        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            return itemSlots[selectedItemId].GetItemId();
        }
        return 0;
    }
}