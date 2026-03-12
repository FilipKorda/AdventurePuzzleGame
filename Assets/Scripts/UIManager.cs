using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private InteractableItem bookWithAlchemiaRecepisItem;
    public ReadablePanel readablePanel;
    public ReadableAndInteractablePanel readableAndInteractablePanel;
    [SerializeField] private GameObject gameModeLockPickPanel;
    public LockPickPanel lockPickPanel;
    private List<ItemSlot> itemSlots = new();
    private int selectedItemId = -1;

    [Header("Inventory Settings")]
    [SerializeField] private int maxVisibleSlots = 5;

    [Header("Input Settings")]
    [SerializeField] private InputActionReference[] selectItemActions;

    [SerializeField] private InputActionReference navigateNextAction;
    [SerializeField] private InputActionReference navigatePreviousAction;

    [SerializeField] private InputActionReference dropItemAction;
    [SerializeField] private InputActionReference drinkOrEatAction;

    public InputActionReference nextPageAction;
    public InputActionReference previousPageAction;

    [SerializeField] private TextMeshProUGUI itemNameText;
    private System.Action<InputAction.CallbackContext>[] selectItemPerformedCallbacks;

    [Header("Toggle Lamp")]
    [SerializeField] private TextMeshProUGUI pressFToToggleLamp;

    [Header("Crafting Objects")]
    [SerializeField] private ItemstoCraftHolderUI itemstoCraftHolderUI;
    [SerializeField] private Image imageMetalCrabs;
    [SerializeField] private Image imageSprings;
    [SerializeField] private Image imageMetalscaffolds;
    [SerializeField] private Image imageCollectible_Gears;
    [SerializeField] private Image imageCables;

    [Header("Morse And Glifs Book")]
    [SerializeField] private InputActionReference readBookAction;
    [SerializeField] private InputActionReference nextCodeMordeAndGlifsPageAction;
    [SerializeField] private InputActionReference previousCodeMordeAndGlifsPageAction;
    [SerializeField] private GameObject morseAndGlifsBook;
    [SerializeField] private GameObject bookPage1;
    [SerializeField] private GameObject bookPage2;
    [SerializeField] private GameObject bookPage3;
    [SerializeField] private GameObject bookPage4;
    [SerializeField] private GameObject bookPage5;
    [SerializeField] private GameObject bookPage6;
    private int currentPage = 0;
    private GameObject[] pages;

    [Header("Mirror UI")]
    [SerializeField] private GameObject mirrorInputPanel;

    [Header("Drink Or Eat UI")]
    [SerializeField] private GameObject drinkOrEatPanel;

    [Header("Gear Mode Panel")]
    [SerializeField] private GameObject gearModePanel;

    [Header("Furniture  Mode Panel")]
    [SerializeField] private GameObject furnitureVertivalModePanel;
    [SerializeField] private GameObject furnitureHorizontalModePanel;

    [Header("Wooden Puzzle panel")]
    [SerializeField] private GameObject woodenPuzzlePanel;
    [SerializeField] private GameObject papytusPuzzleWoodenPuzzleSolve;

    [Header("Triangle Puzzle")]
    [SerializeField] private GameObject papyrusTrianglePuzzle;

    public void EnableWoodenPuzzlePanel()
    {
        woodenPuzzlePanel.SetActive(true);
    }

    public void DisableWoodenPuzzlePanel()
    {
        woodenPuzzlePanel.SetActive(false);
    }

    public void EnableVerticalFurnitureModePanel()
    {
        furnitureVertivalModePanel.SetActive(true);
    }

    public void DisableVerticalFurnitureModePanel()
    {
        furnitureVertivalModePanel.SetActive(false);
    }

    public void EnableHorizontalFurnitureModePanel()
    {
        furnitureHorizontalModePanel.SetActive(true);
    }

    public void DisableHorizontalFurnitureModePanel()
    {
        furnitureHorizontalModePanel.SetActive(false);
    }

    public void EnableGearModePanel()
    {
        gearModePanel.SetActive(true);
    }

    public void DisableGearModePanel()
    {
        gearModePanel.SetActive(false);
    }

    public void EnableDrinkOrEatPanel()
    {
        drinkOrEatPanel.SetActive(true);
    }

    public void DisableDrinkOrEatPanel()
    {
        drinkOrEatPanel.SetActive(false);
    }

    public void EnableMirrorInputPanel()
    {
        mirrorInputPanel.SetActive(true);
    }

    public void DisableMirrorInputPanel()
    {
        mirrorInputPanel.SetActive(false);
    }

    public void ShowMetalCrabImage()
    {
        itemstoCraftHolderUI.OnItemCollected(imageMetalCrabs);
    }

    public void ShowSpringImage()
    {
        itemstoCraftHolderUI.OnItemCollected(imageSprings);
    }

    public void ShowMetalscaffoldsImage()
    {
        itemstoCraftHolderUI.OnItemCollected(imageMetalscaffolds);
    }

    public void ShowCollectibleGearsImage()
    {
        itemstoCraftHolderUI.OnItemCollected(imageCollectible_Gears);
    }

    public void ShowCablesImage()
    {
        itemstoCraftHolderUI.OnItemCollected(imageCables);
    }

    public void HideBackgroud()
    {
        itemstoCraftHolderUI.HideBackgroud();
    }

    public void RemoveItemFromUI(ItemID itemID)
    {
        if (itemID == ItemID.MetalCrabs)
            imageMetalCrabs.gameObject.SetActive(false);
        else if (itemID == ItemID.Springs)
            imageSprings.gameObject.SetActive(false);
        else if (itemID == ItemID.Metalscaffolds)
            imageMetalscaffolds.gameObject.SetActive(false);
        else if (itemID == ItemID.Collectible_Gears)
            imageCollectible_Gears.gameObject.SetActive(false);
        else if (itemID == ItemID.Cables)
            imageCables.gameObject.SetActive(false);
    }

    public IEnumerator ActiveLampNotification()
    {
        if (pressFToToggleLamp != null)
        {
            pressFToToggleLamp.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(7);

        if (pressFToToggleLamp != null)
        {
            pressFToToggleLamp.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        RegisterSelectItemActions();

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
        if (dropItemAction != null)
        {
            dropItemAction.action.Enable();
            dropItemAction.action.performed += OnDropItemPerformed;
        }
        if (drinkOrEatAction != null)
        {
            drinkOrEatAction.action.Enable();
            drinkOrEatAction.action.performed += OnDrinkOrEatPerformed;
        }

        if (readBookAction != null)
        {
            readBookAction.action.Enable();
            readBookAction.action.performed += OnReadBookPerformed;
        }

        if (nextPageAction != null)
            nextPageAction.action.performed += OnNextPage;

        if (previousPageAction != null)
            previousPageAction.action.performed += OnPreviousPage;

        if (nextCodeMordeAndGlifsPageAction != null)
            nextCodeMordeAndGlifsPageAction.action.performed += OnNextCodeMordeAndGlifsPage;

        if (previousCodeMordeAndGlifsPageAction != null)
            previousCodeMordeAndGlifsPageAction.action.performed += OnPreviousCodeMordeAndGlifsPage;
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
        if (dropItemAction != null)
        {
            dropItemAction.action.performed -= OnDropItemPerformed;
            dropItemAction.action.Disable();
        }
        if (drinkOrEatAction != null)
        {
            drinkOrEatAction.action.performed -= OnDrinkOrEatPerformed;
            drinkOrEatAction.action.Disable();
        }

        if (readBookAction != null)
        {
            readBookAction.action.performed -= OnReadBookPerformed;
            readBookAction.action.Disable();
        }

        if (nextPageAction != null)
            nextPageAction.action.performed -= OnNextPage;

        if (previousPageAction != null)
            previousPageAction.action.performed -= OnPreviousPage;

        if (nextCodeMordeAndGlifsPageAction != null)
            nextCodeMordeAndGlifsPageAction.action.performed -= OnNextCodeMordeAndGlifsPage;

        if (previousCodeMordeAndGlifsPageAction != null)
            previousCodeMordeAndGlifsPageAction.action.performed -= OnPreviousCodeMordeAndGlifsPage;
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

        pages = new[]
    {
        bookPage1,
        bookPage2,
        bookPage3,
        bookPage4,
        bookPage5,
        bookPage6
    };
    }


    private void Start()
    {
        morseAndGlifsBook.SetActive(false);
        drinkOrEatPanel.SetActive(false);
        papytusPuzzleWoodenPuzzleSolve.SetActive(false);
    }

    private void OnReadBookPerformed(InputAction.CallbackContext context)
    {
        int currentSelectedId = GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.MorseAndGlifsBook:
                ReadBookCodeMordeAndGlifs();
                break;
            case ItemID.Papyrus:
                ReadBookPapyrus();
                break;
            case ItemID.PapyrusTrianglePuzzle:
                ReadBookPapyrusTrianglePuzzle();
                break;
        }

        Services.Audio.PlaySFX("ReadBook");
    }


    private void ReadBookPapyrusTrianglePuzzle()
    {
        bool isOpen = papyrusTrianglePuzzle.activeSelf;
        papyrusTrianglePuzzle.SetActive(!isOpen);
        if (!isOpen)
        {
            papyrusTrianglePuzzle.SetActive(true);
        }
        else
        {
            papyrusTrianglePuzzle.SetActive(false);
        }
    }

    private void ReadBookPapyrus()
    {
        bool isOpen = papytusPuzzleWoodenPuzzleSolve.activeSelf;
        papytusPuzzleWoodenPuzzleSolve.SetActive(!isOpen);
        if (!isOpen)
        {
            papytusPuzzleWoodenPuzzleSolve.SetActive(true);
        }
        else
        {
            papytusPuzzleWoodenPuzzleSolve.SetActive(false);
        }
    }

    private void ReadBookCodeMordeAndGlifs()
    {
        bool isOpen = morseAndGlifsBook.activeSelf;

        morseAndGlifsBook.SetActive(!isOpen);

        if (!isOpen)
        {
            nextCodeMordeAndGlifsPageAction.action.Enable();
            previousCodeMordeAndGlifsPageAction.action.Enable();
        }
        else
        {
            nextCodeMordeAndGlifsPageAction.action.Disable();
            previousCodeMordeAndGlifsPageAction.action.Disable();
        }
    }


    private void OnNextCodeMordeAndGlifsPage(InputAction.CallbackContext context)
    {
        if (!morseAndGlifsBook.activeSelf) return;

        Services.Audio.PlaySFX("ReadBook");

        pages[currentPage].SetActive(false);
        currentPage = Mathf.Min(currentPage + 1, pages.Length - 1);
        pages[currentPage].SetActive(true);
    }

    private void OnPreviousCodeMordeAndGlifsPage(InputAction.CallbackContext context)
    {
        if (!morseAndGlifsBook.activeSelf) return;

        Services.Audio.PlaySFX("ReadBook");

        pages[currentPage].SetActive(false);
        currentPage = Mathf.Max(currentPage - 1, 0);
        pages[currentPage].SetActive(true);
    }

    private void OnNextPage(InputAction.CallbackContext context)
    {
        if (!bookWithAlchemiaRecepisItem.isActualReading) return;
        var data = bookWithAlchemiaRecepisItem.readableAndInteractableTextData;
        if (data == null || data.bookPages == null) return;

        int start = Mathf.Clamp(data.currentPageIndex, 0, data.bookPages.Length - 1);
        int nextIndex = start;
        for (int i = start + 1; i < data.bookPages.Length; i++)
        {
            if (data.IsPageUnlocked(i))
            {
                nextIndex = i;
                break;
            }
        }

        if (nextIndex != start)
        {
            data.currentPageIndex = nextIndex;
            readableAndInteractablePanel.mainTextUI.text =
                data.bookPages[data.currentPageIndex].GetLocalizedString();
        }
    }

    private void OnPreviousPage(InputAction.CallbackContext context)
    {
        if (!bookWithAlchemiaRecepisItem.isActualReading) return;
        var data = bookWithAlchemiaRecepisItem.readableAndInteractableTextData;
        if (data == null || data.bookPages == null) return;

        int start = Mathf.Clamp(data.currentPageIndex, 0, data.bookPages.Length - 1);
        int prevIndex = start;
        for (int i = start - 1; i >= 0; i--)
        {
            if (data.IsPageUnlocked(i))
            {
                prevIndex = i;
                break;
            }
        }

        if (prevIndex != start)
        {
            data.currentPageIndex = prevIndex;
            readableAndInteractablePanel.mainTextUI.text =
                data.bookPages[data.currentPageIndex].GetLocalizedString();
        }
    }


    private void OnDrinkOrEatPerformed(InputAction.CallbackContext context)
    {
        int currentSelectedId = GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        if (Ailments.Instance == null)
        {
            Debug.LogError("Brak instancji Ailments na scenie!");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.WaterBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (WaterBucket).");
                Ailments.Instance.ApplyWaterBucketEffect();
                break;

            case ItemID.AcidBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (AcidBucket).");
                Ailments.Instance.ApplyAcidBucketEffect();
                break;

            case ItemID.BloodBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (BloodBucket).");
                Ailments.Instance.ApplyBloodBucketEffect();
                break;

            case ItemID.WineBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (WineBucket).");
                Ailments.Instance.ApplyWineBucketEffect();
                break;

            case ItemID.RawMeat:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (RawMeat).");
                Ailments.Instance.ApplyRawMeatEffect();
                break;

            case ItemID.NiceWater:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (NiceWater).");
                Ailments.Instance.ApplyNiceWaterEffect();
                break;

            case ItemID.MudWater:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Wypito (MudWater).");
                Ailments.Instance.ApplyMudWaterEffect();
                break;

            case ItemID.LeafGoodBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (LeafGoods).");
                Ailments.Instance.ApplyLeafGoodsEffect();
                break;

            case ItemID.AngryTimeBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (AngryTime).");
                Ailments.Instance.ApplyAngryTimeEffect();
                break;

            case ItemID.BadMoodBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (BadMood).");
                Ailments.Instance.ApplyBadMoodEffect();
                break;

            case ItemID.GoodSoupBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (GoodSoup).");
                Ailments.Instance.ApplyGoodSoupEffect();
                break;

            case ItemID.HolyCowBucket:
                UseConsumableItem(currentSelectedId);
                Debug.Log("Zjedzono (HolyCow).");
                Ailments.Instance.ApplyHolyCowEffect();
                break;

            default:

                Debug.Log("Tego przedmiotu nie mo¿na zjeœæ ani wypiæ.");
                break;
        }
    }

    private void UseConsumableItem(int itemId)
    {
        DisableDrinkOrEatPanel();
        Services.Audio.PlaySFX("Drink");
        Inventory.Instance.RemoveItemFromInventoryByID(itemId);
        RemoveItemFromUIByID(itemId);
    }

    private void OnDropItemPerformed(InputAction.CallbackContext context)
    {
        if (readableAndInteractablePanel != null && readableAndInteractablePanel.gameObject.activeInHierarchy
            || gameModeLockPickPanel.activeInHierarchy || morseAndGlifsBook.activeInHierarchy || gearModePanel.activeInHierarchy || papytusPuzzleWoodenPuzzleSolve.activeInHierarchy)
        {
            Debug.Log("Nie mo¿na wyrzuciæ przedmiotu podczas przegl¹dania czytanej strony.");
            return;
        }

        DisableDrinkOrEatPanel();

        int currentSelectedId = GetSelectedItemId();
        if (currentSelectedId != 0)
        {
            Services.Audio.PlaySFX("DropItem");
            Inventory.Instance.DropItemByID(currentSelectedId);
        }
        else
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do wyrzucenia.");
        }
    }

    public bool CanAddItemToUI()
    {
        return itemSlots.Count < maxVisibleSlots;
    }

    public void AddItemToUI(IPickupable ipickupable)
    {
        if (!CanAddItemToUI())
        {
            Debug.Log("Osi¹gniêto maksymaln¹ liczbê przedmiotów, które mo¿esz nosiæ.");
            return;
        }

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
                    DisableDrinkOrEatPanel();

                    itemSlots[i].SetHighlighted(false);
                    Destroy(itemSlots[i].gameObject);
                    itemSlots.RemoveAt(i);
                    selectedItemId = -1;

                    if (itemSlots.Count > 0)
                    {
                        SelectItem(0);
                    }
                    else
                    {
                        if (itemNameText != null)
                        {
                            itemNameText.text = string.Empty;
                        }
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
        if (selectItemActions == null) return;

        selectItemPerformedCallbacks = new System.Action<InputAction.CallbackContext>[selectItemActions.Length];

        for (int i = 0; i < selectItemActions.Length; i++)
        {
            int index = i;
            var actionRef = selectItemActions[i];
            if (actionRef == null || actionRef.action == null) continue;

            // zapamiêtujemy callback, ¿eby potem poprawnie go odpi¹æ
            selectItemPerformedCallbacks[i] = context => OnSelectItemPerformed(index);
            actionRef.action.Enable();
            actionRef.action.performed += selectItemPerformedCallbacks[i];
        }
    }

    private void UnregisterSelectItemActions()
    {
        if (selectItemActions == null) return;

        for (int i = 0; i < selectItemActions.Length; i++)
        {
            var actionRef = selectItemActions[i];
            if (actionRef == null || actionRef.action == null) continue;

            if (selectItemPerformedCallbacks != null && i < selectItemPerformedCallbacks.Length && selectItemPerformedCallbacks[i] != null)
            {
                actionRef.action.performed -= selectItemPerformedCallbacks[i];
                selectItemPerformedCallbacks[i] = null;
            }
            actionRef.action.Disable();
        }

        selectItemPerformedCallbacks = null;
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

            if (itemNameText != null)
                itemNameText.text = string.Empty;

            DisableDrinkOrEatPanel();
            return;
        }

        if (selectedItemId >= 0 && selectedItemId < itemSlots.Count)
        {
            itemSlots[selectedItemId].SetHighlighted(false);
        }

        selectedItemId = index;

        itemSlots[selectedItemId].SetHighlighted(true);

        if (itemNameText != null)
            itemNameText.text = itemSlots[selectedItemId].GetItemName();

        ItemID id = (ItemID)itemSlots[selectedItemId].GetItemId();

        if (IsConsumable(id))
            EnableDrinkOrEatPanel();
        else
            DisableDrinkOrEatPanel();

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

    private bool IsConsumable(ItemID id)
    {
        switch (id)
        {
            case ItemID.WaterBucket:
            case ItemID.AcidBucket:
            case ItemID.BloodBucket:
            case ItemID.WineBucket:
            case ItemID.RawMeat:
            case ItemID.NiceWater:
            case ItemID.MudWater:
            case ItemID.LeafGoodBucket:
            case ItemID.AngryTimeBucket:
            case ItemID.BadMoodBucket:
            case ItemID.GoodSoupBucket:
            case ItemID.HolyCowBucket:
                return true;

            default:
                return false;
        }
    }
}