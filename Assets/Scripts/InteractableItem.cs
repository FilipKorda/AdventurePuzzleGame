using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using static Unity.Collections.AllocatorManager;

public class InteractableItem : MonoBehaviour, IPickupable, IBookThrowable, IOpenable, IReadable, IPressable,
    IPlaceable, ILockPick, IFillable, IPickupARenewableItem, IAlchemyStation, IReadableAndInteractable, IRecipePlaceable,
    IGetObject, ICrafting, IPinNumber, IRotate, ICryptex, IMirror, IGearLock, IGearRotate, IGear90, IPipeGearPuzzle,
    IFurniture, IWoodenBlockPuzzle, IWoodenBlock
{
    public enum InteractableType
    {
        Pickupable,
        Throwable,
        Openable,
        Readable,
        Pressable,
        Placeable,
        LockPick,
        Fillable,
        PickupARenewableItem,
        AlchemyStation,
        ReadableAndInteractableItem,
        PlaceRecipe,
        GetObject,
        Crafting,
        PinNumber,
        RotateStatue,
        Cryptex,
        MirrorMode,
        GearLockMode,
        RotateGear,
        RotateGear90,
        PipeGearPuzzle,
        Furniture,
        WoodenBlockPuzzle,
        WoodenBlock
    }

    public InteractableType interactableType;
    [SerializeField] private string itemName = "Przedmiot";
    [SerializeField] private LocalizedString localizeItemName;
    [SerializeField] private Sprite itemSprite;

    [Header("ID (if Pickupable)")]
    [Tooltip("ID tego przedmiotu, jeœli mo¿na go podnieœæ i umieœciæ w ekwipunku.")]
    [SerializeField] private int itemId;
    [SerializeField] private ItemID itemIdbyItemId;
    [SerializeField] private PinNumber pinNumber;

    [Header("Requirements (if Openable, Placeable, etc.)")]
    [Tooltip("Lista ID przedmiotów z ekwipunku, które s¹ wymagane do tej interakcji.")]
    [SerializeField] private int[] requiredItemIds;

    [Header("Components & Events")]
    [SerializeField] private Animator animator;
    [SerializeField] public ReadableTextData readableTextData;
    [SerializeField] public ReadableAndInteractableTextData readableAndInteractableTextData;
    [SerializeField] private UnityEvent onAllWallButtonPressed;
    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private BoxCollider boxCollider;
    public bool canOpenWithNoSelectedItem = false;

    [Header("Drop Settings")]
    [Tooltip("Prefab do stworzenia obiektu po wyrzuceniu go z ekwipunku. Przeci¹gnij tutaj prefab tego przedmiotu.")]
    [SerializeField] private GameObject itemPrefab;

    public bool isActualReading;
    private bool isActualOpen;
    private bool isActualPreesed;
    private bool isLockPicking;
    public bool isAlchemyRecipe = false;
    [SerializeField] private int requiredUses = 3;

    [Header("Fillable Source Settings (if Fillable)")]
    [Tooltip("Typ p³ynu, który dostarcza to Ÿród³o.")]
    [SerializeField] private LiquidType providedLiquidType = LiquidType.None;

    [Tooltip("Lista mapowañ, które definiuj¹, jaki pusty pojemnik zamienia siê w jaki nape³niony.")]
    [SerializeField] private FillMapping fillMapping;

    [Header("Alchemy Settings (if AlchemyStation)")]
    [Tooltip("Referencja do komponentu Cauldron na tym obiekcie.")]
    [SerializeField] private Cauldron cauldron;

    [Header("Recipes")]
    [SerializeField] private GameObject scroll_BigAcidPotion;
    [SerializeField] private GameObject scroll_SmallAngryTimePotion;
    [SerializeField] private GameObject scroll_ElderBadMoodPotion;
    [SerializeField] private GameObject scroll_OpenBloodPotion;
    [SerializeField] private GameObject scroll_BigGoodSoup;
    [SerializeField] private GameObject scroll_SmallHolyCowPotion;
    [SerializeField] private GameObject scroll_ElderLeafGoods;
    [SerializeField] private GameObject scroll_OpenNiceWater;
    [SerializeField] private GameObject scroll_BigWaterPotion;
    [SerializeField] private GameObject scroll_SmallWinePotion;

    [SerializeField] private RecipesCounter recipesCounter;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private bool isEndPanel;

    [SerializeField] private LOcalizeString localizationString;
    [SerializeField] private LOcalizeString localizationTwoString;

    [SerializeField] private GameObject metalCrabs;
    [SerializeField] private GameObject springs;
    [SerializeField] private GameObject metalscaffolds;
    [SerializeField] private GameObject collectibleGears;
    [SerializeField] private GameObject cables;
    [SerializeField] private Workbench workbench;
    private bool metalCrabsUsed;
    private bool springsUsed;
    private bool metalscaffoldsUsed;
    private bool gearsUsed;
    private bool cablesUsed;

    [Header("Pin Pad Highlight")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Color highlightColor;
    private Color baseColor;

    [Header("Statue Rotation Option")]
    [SerializeField] private StatueCompasPuzzle statueCompasPuzzle;
    [SerializeField] private float rotationDuration = 0.5f;
    bool isRotating = false;
    public WorldDirection CurrentDirection
    {
        get
        {
            float y = Mathf.Round(transform.eulerAngles.y) % 360f;

            if (y == 0f) return WorldDirection.North;
            if (y == 90f) return WorldDirection.West;
            if (y == 180f) return WorldDirection.South;
            return WorldDirection.East;
        }
    }
    public event Action<InteractableItem> OnRotationFinished;
    [SerializeField] private InteractableItem linkedReadable;
    public InteractableItem LinkedReadable => linkedReadable;
    [SerializeField] private DirectionTextSet directionTextSet;
    private float bookRotationDuration = 0.15f;

    [Header("Cryptex Rotation Option")]
    [SerializeField] private float cryptexRotateDuration = 0.25f;
    bool cryptexIsRotating = false;
    private int currentCryptexIndex = 0;
    public int CurrentCryptexIndex => currentCryptexIndex;

    [Header("Mirror")]
    [SerializeField] private Mirror mirror;

    [Header("Gear Lock Mode")]
    [SerializeField] private GearLockMode gearLockMode;
    [SerializeField] private float gearRotateDuration = 0.25f;
    bool gearIsRotating = false;
    private int currentGearIndex = 0;
    public int CurrentGearIndex => currentGearIndex;

    [Header("Puzzle Pipes Gears")]
    [SerializeField] private PipeGearPuzzle pipeGearPuzzle;
    [SerializeField] private float gear90RotateDuration = 0.25f;
    bool gear90IsRotating = false;
    public int currentGear90Index = 0;
    public int CurrentGear90Index => currentGear90Index;

    [Header("Furniture Puzzle")]
    [SerializeField] private MovableBlock movableBlock;

    [Header("Wooden Puzzle")]
    [SerializeField] private WoodenBlockPuzzle woodenBlockPuzzle;
    [SerializeField] float zOffset = 0.1f;
    [SerializeField] float duration = 0.5f;
    Vector3 startPosition;
    bool moved;
    bool isMovingWoodenBlock;
    bool highlighted;
    Coroutine currentRoutine;

    private void Awake()
    {
        if (rend != null)
        {
            baseColor = rend.material.color;
        }

        if(interactableType == InteractableType.WoodenBlock)
        {
            startPosition = transform.position;
        }
       
    }

    public void OnClick()
    {
        if (isMovingWoodenBlock)
            return;

        highlighted = !highlighted;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        Vector3 target = moved
            ? startPosition
            : startPosition + new Vector3(0f, 0f, zOffset);

        currentRoutine = StartCoroutine(MoveTo(target));
        moved = !moved;
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMovingWoodenBlock = true;

        Vector3 fromPos = transform.position;
        Color fromColor = rend.material.color;
        Color targetColor = highlighted ? highlightColor : baseColor;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            transform.position = Vector3.Lerp(fromPos, target, t);
            rend.material.color = Color.Lerp(fromColor, targetColor, t);

            yield return null;
        }

        transform.position = target;
        rend.material.color = targetColor;

        PuzzleBlockManager.Instance.BlockClicked(itemId);

        isMovingWoodenBlock = false;
    }

    public void ResetblockInstant(Color baseColor)
    {
        transform.position = startPosition;
        rend.material.color = baseColor;
        moved = false;
        highlighted = false;
        isMovingWoodenBlock = false;

    }

    public void EnterWoddenBlockPuzzle()
    {
        woodenBlockPuzzle.EnterWoodenPuzzleMode();
        Debug.Log("Wchodzis do wodden block puzzle!");
    }

    public void PushFurniture()
    {
        movableBlock.TakeControlOfTheThiBlock();

        Debug.Log("pchnij mebel");
    }

    public void EnterPipeGearPuzzleMode()
    {
        pipeGearPuzzle.EnterGearLockMode();
        Debug.Log("Wszed³eœ w tryb Pipe Gear Mode!");
    }

    public void RotateGear90()
    {
        if (gear90IsRotating)
            return;

        Services.Audio.PlaySFX("GearTick");
        currentGear90Index = (currentGear90Index + 1) % 4;

        Debug.Log(gameObject.name + "Current Gear Index: " + currentGear90Index);

        StartCoroutine(RotateGear90Smoothly());
    }

    IEnumerator RotateGear90Smoothly()
    {
        gear90IsRotating = true;
        boxCollider.enabled = false;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 90f, 0f);

        float time = 0f;

        while (time < gear90RotateDuration)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / gear90RotateDuration);
            yield return null;
        }

        transform.localRotation = targetRotation;

        boxCollider.enabled = true;
        gear90IsRotating = false;

        pipeGearPuzzle.CheckIfPuzzleSolved();

    }

    public void ResetCurrentGearIndex()
    {
        currentGearIndex = 0;
    }

    public void ResetCurrentGear90Index()
    {
        currentGear90Index = 0;
    }

    public void RotateGear()
    {
        if (gearIsRotating)
            return;

        Services.Audio.PlaySFX("GearTick");
        currentGearIndex = (currentGearIndex + 1) % 12;

        StartCoroutine(RotateGearSmoothly());
      
    }

    IEnumerator RotateGearSmoothly()
    {
        gearIsRotating = true;
        boxCollider.enabled = false;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 30f, 0f);

        float time = 0f;

        while (time < gearRotateDuration)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / gearRotateDuration);
            yield return null;
        }

        transform.localRotation = targetRotation;

        boxCollider.enabled = true;
        gearIsRotating = false;

        gearLockMode.CheckIfPuzzleSolved();
    }

    public void EnterGearLock()
    {
        gearLockMode.EnterGearLockMode();
        Debug.Log("Wszed³eœ w tryb Gear Mode!");
    }

    public void EnterTheMirrorMode()
    {
        mirror.EnableControl();

        Debug.Log("Wszed³eœ w tryb lustra!");
    }

    public void RotateCryptex()
    {
        if (cryptexIsRotating)
            return;

        Services.Audio.PlaySFX("MovingStoneKryptex");

        currentCryptexIndex = (currentCryptexIndex + 1) % 8;

        StartCoroutine(RotateCryptexSmoothly());

    }

    IEnumerator RotateCryptexSmoothly()
    {
        cryptexIsRotating = true;
        boxCollider.enabled = false;

        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(45f, 0f, 0f);

        float time = 0f;

        while (time < cryptexRotateDuration)
        {
            time += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time / cryptexRotateDuration);
            yield return null;
        }

        transform.localRotation = targetRotation;

        boxCollider.enabled = true;
        cryptexIsRotating = false;
    }


    public void RotateStatue()
    {
        if (isRotating)
            return;

        Services.Audio.PlaySFX("RotateStatue_StoneMove");
        Services.Audio.PlaySFX("GrabRecipe"); //przewraca strone w ksi¹¿ce

        StartCoroutine(RotateSmoothly());
        StartCoroutine(BookRotate());
    }

    IEnumerator RotateSmoothly()
    {
        isRotating = true;

        linkedReadable.boxCollider.enabled = false;
        boxCollider.enabled = false;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 90f, 0f);

        float time = 0f;

        while (time < rotationDuration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / rotationDuration);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;

        OnRotationFinished?.Invoke(this);

        ApplyText();

        linkedReadable.boxCollider.enabled = true;
        boxCollider.enabled = true;

        statueCompasPuzzle.CheckPuzzle();
    }

    private IEnumerator BookRotate()
    {
        Quaternion start = Quaternion.Euler(0f, 0f, 0f);
        Quaternion target = Quaternion.Euler(0f, 2.5f, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bookRotationDuration;
            linkedReadable.transform.localRotation = Quaternion.Lerp(start, target, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / bookRotationDuration;
            linkedReadable.transform.localRotation = Quaternion.Lerp(target, start, t);
            yield return null;
        }
    }

    public void ApplyText()
    {
        if (linkedReadable == null || directionTextSet == null)
            return;

        linkedReadable.readableTextData =
            directionTextSet.Get(CurrentDirection);
    }

    public void EnterPinNumber()
    {
        if (pinNumber == PinNumber.None)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.None);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.One)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.One);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Two)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Two);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Three)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Three);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Four)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Four);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Five)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Five);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Six)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Six);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Seven)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Seven);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Eight)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Eight);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Nine)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Nine);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Clear)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Clear);
            WallKeypad.Instance.EnterPinNumber();

        }
        else if (pinNumber == PinNumber.Accept)
        {
            WallKeypad.Instance.SetPinNumber(PinNumber.Accept);
            WallKeypad.Instance.EnterPinNumber();

        }
    }

    public void HighlightButton()
    {
        if (rend != null)
        {
            rend.material.color = highlightColor;
        }
    }

    public void ResetHighlightButton()
    {
        if (rend != null)
        {
            rend.material.color = baseColor;
        }
    }

    public void PlaceObjectToCraft()
    {
        bool craftedSomething = false;

        if (!metalCrabsUsed && Inventory.Instance.CollectedItems.Contains(ItemID.MetalCrabs))
        {
            metalCrabs.SetActive(true);
            Inventory.Instance.CollectedItems.Remove(ItemID.MetalCrabs);
            UIManager.Instance.RemoveItemFromUI(ItemID.MetalCrabs);
            metalCrabsUsed = true;
            craftedSomething = true;
        }

        if (!springsUsed && Inventory.Instance.CollectedItems.Contains(ItemID.Springs))
        {
            springs.SetActive(true);
            Inventory.Instance.CollectedItems.Remove(ItemID.Springs);
            UIManager.Instance.RemoveItemFromUI(ItemID.Springs);
            springsUsed = true;
            craftedSomething = true;
        }

        if (!metalscaffoldsUsed && Inventory.Instance.CollectedItems.Contains(ItemID.Metalscaffolds))
        {
            metalscaffolds.SetActive(true);
            Inventory.Instance.CollectedItems.Remove(ItemID.Metalscaffolds);
            UIManager.Instance.RemoveItemFromUI(ItemID.Metalscaffolds);
            metalscaffoldsUsed = true;
            craftedSomething = true;
        }

        if (!gearsUsed && Inventory.Instance.CollectedItems.Contains(ItemID.Collectible_Gears))
        {
            collectibleGears.SetActive(true);
            Inventory.Instance.CollectedItems.Remove(ItemID.Collectible_Gears);
            UIManager.Instance.RemoveItemFromUI(ItemID.Collectible_Gears);
            gearsUsed = true;
            craftedSomething = true;
        }

        if (!cablesUsed && Inventory.Instance.CollectedItems.Contains(ItemID.Cables))
        {
            cables.SetActive(true);
            Inventory.Instance.CollectedItems.Remove(ItemID.Cables);
            UIManager.Instance.RemoveItemFromUI(ItemID.Cables);
            cablesUsed = true;
            craftedSomething = true;
        }

        if (metalCrabsUsed && springsUsed && metalscaffoldsUsed && gearsUsed && cablesUsed)
        {
            boxCollider.enabled = false;

            UIManager.Instance.HideBackgroud();

            ActiveCrafting();
        }

        if (craftedSomething)
            Debug.Log("umieszczono obiekt do kraftowania");
        else
            Debug.Log("nie sie nie dzieje");
    }

    private void ActiveCrafting()
    {
        workbench.PlayCraftingAnimation();
    }

    public void GetObject()
    {
        if (interactableType != InteractableType.GetObject)
            return;

        Inventory.Instance.CollectedItems.Add(itemIdbyItemId);

        if (itemIdbyItemId == ItemID.lamp)
            UIManager.Instance.StartCoroutine(UIManager.Instance.ActiveLampNotification());
        else if (itemIdbyItemId == ItemID.MetalCrabs)
            UIManager.Instance.ShowMetalCrabImage();
        else if (itemIdbyItemId == ItemID.Springs)
            UIManager.Instance.ShowSpringImage();
        else if (itemIdbyItemId == ItemID.Metalscaffolds)
            UIManager.Instance.ShowMetalscaffoldsImage();
        else if (itemIdbyItemId == ItemID.Collectible_Gears)
            UIManager.Instance.ShowCollectibleGearsImage();
        else if (itemIdbyItemId == ItemID.Cables)
            UIManager.Instance.ShowCablesImage();

        Services.Audio.PlaySFX("PickUpItem");
        DisableThisGameObject();
    }

    public GameObject GetItemPrefab()
    {
        if (itemPrefab == null)
        {
            Debug.LogError($"B³¹d krytyczny: Przedmiot '{GetItemName()}' (ID: {itemId}) nie ma przypisanego prefabu w polu 'Item Prefab'! Nie mo¿na go wyrzuciæ.");
            return null;
        }
        return itemPrefab;
    }

    public void AddIngredient()
    {
        if (interactableType != InteractableType.AlchemyStation || cauldron == null) return;

        int selectedItemIdAsInt = UIManager.Instance.GetSelectedItemId();
        ItemID selectedItemId = (ItemID)selectedItemIdAsInt;

        if (selectedItemId == ItemID.PlantRoot || selectedItemId == ItemID.Leafs ||
            selectedItemId == ItemID.RawMeat || selectedItemId == ItemID.WineBucket ||
                selectedItemId == ItemID.AcidBucket || selectedItemId == ItemID.WaterBucket ||
              selectedItemId == ItemID.BloodBucket || selectedItemId == ItemID.EmptyBucket)
        {
            if (cauldron != null && cauldron.HasReadySolution() && selectedItemId == ItemID.EmptyBucket)
            {
                ItemID requiredEmptyContainerId = ItemID.EmptyBucket;

                if (selectedItemId == requiredEmptyContainerId)
                {
                    Services.Audio.PlaySFX("FillBucketWithPotion");

                    Inventory.Instance.RemoveItemFromInventoryByID(selectedItemIdAsInt);
                    UIManager.Instance.RemoveItemFromUIByID(selectedItemIdAsInt);

                    InteractableItem newPotion = cauldron.TakeSolution();
                    Inventory.Instance.AddItemToInventory(newPotion);

                    Debug.Log($"Nape³niono pojemnik. Otrzymano: {newPotion.GetItemName()}");
                }
                else
                {
                    if (localizationString.localizeString != null)
                        NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
                    Debug.Log("Wybierz pusty pojemnik, aby nabraæ roztwór.");
                }
            }
            else if (selectedItemId != ItemID.EmptyBucket)
            {
                cauldron.AddIngredient(selectedItemIdAsInt);

                if (selectedItemId == ItemID.WineBucket || selectedItemId == ItemID.AcidBucket ||
                      selectedItemId == ItemID.WaterBucket || selectedItemId == ItemID.BloodBucket)
                {
                    Services.Audio.PlaySFX("PourWater");
                }
                else if (selectedItemId == ItemID.PlantRoot)
                {
                    Services.Audio.PlaySFX("AddRoot");
                }
                else if (selectedItemId == ItemID.Leafs)
                {
                    Services.Audio.PlaySFX("AddLeafs");
                }
                else
                {
                    Services.Audio.PlaySFX("AddRawMeat");
                }
            }
            else
            {
                if (cauldron.isBrewing)
                {
                    NotificationSystem.Instance.ShowNotification(cauldron.localizeThreeString, 3);
                }
                else
                {
                    if (localizationTwoString.localizeString != null)
                        NotificationSystem.Instance.ShowNotification(localizationTwoString.localizeString, 3);
                }

            }
        }
        else
        {
            if (cauldron.HasReadySolution())
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
                Debug.Log("Wybierz pusty pojemnik, aby nabraæ roztwór.");
            }
            else
            {
                if (localizationTwoString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationTwoString.localizeString, 3);

                Debug.Log("Wybierz sk³adnik z ekwipunku, aby go dodaæ do kot³a.");
            }
        }
    }

    public void OnPickupARenewableItem()
    {
        if (interactableType == InteractableType.PickupARenewableItem)
        {
            if (UIManager.Instance != null && !UIManager.Instance.CanAddItemToUI())
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
            }
            else
            {
                Inventory.Instance.AddItemToInventory(this);
            }

        }

    }

    public void OnPickUp()
    {
        if (!isAlchemyRecipe && interactableType == InteractableType.Pickupable)
        {
            bool added = Inventory.Instance.AddItemToInventory(this);
            if (added)
            {
                Services.Audio.PlaySFX("PickUpItem");
                DestroyInteractable();
            }
            else
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);

            }
        }
        else if (isAlchemyRecipe && interactableType == InteractableType.Pickupable)
        {
            Services.Audio.PlaySFX("GrabRecipe");
            DestroyInteractable();
            Inventory.Instance.AddToInventoryAlchemyRecipe(this);
        }
    }

    public int GetItemId() => itemId;

    public void DestroyInteractable() => Destroy(gameObject);
    public void DisableThisGameObject()
    {
        gameObject.SetActive(false);
    }

    public string GetItemName()
    {
        try
        {
            if (localizeItemName != null)
            {
                var localized = localizeItemName.GetLocalizedString();
                if (!string.IsNullOrEmpty(localized))
                    return localized;
            }
        }
        catch
        {
        }

        return itemName;
    }

    public Sprite GetItemSprite() => itemSprite;

    public void OnFill()
    {
        if (interactableType != InteractableType.Fillable) return;

        int selectedId = UIManager.Instance.GetSelectedItemId();
        if (selectedId == -1)
        {
            Debug.Log("Musisz wybraæ pusty wiadro, aby go nape³niæ.");
            return;
        }

        if (fillMapping == null || fillMapping.requiredEmptyItem == null)
        {
            Debug.LogError($"Brak zdefiniowanego mapowania 'fillMapping' na obiekcie {gameObject.name}");
            return;
        }

        if (fillMapping.requiredEmptyItem.GetItemId() == selectedId)
        {
            if (fillMapping.resultingFilledItem == null)
            {
                Debug.LogError($"Brak przypisanego przedmiotu 'resultingFilledItem' na obiekcie {gameObject.name}");
                return;
            }

            Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
            UIManager.Instance.RemoveItemFromUIByID(selectedId);
            Inventory.Instance.AddItemToInventory(fillMapping.resultingFilledItem);

            Debug.Log($"Nape³niono '{fillMapping.requiredEmptyItem.GetItemName()}' p³ynem typu '{providedLiquidType}'. Otrzymano: {fillMapping.resultingFilledItem.GetItemName()}");
        }
    }

    public void OpenObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (canOpenWithNoSelectedItem)
            {
                if (animator != null)
                {
                    if (isEndPanel)
                    {
                        animator.SetTrigger("Interact");
                        StartEndPanel();
                    }
                    else
                    {
                        animator.SetTrigger("Interact");
                    }

                }
                else
                {

                    if (localizationString.localizeString != null)
                        NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
                    Debug.Log("Nie masz animatora");
                }

                isActualOpen = true;
            }
            else
            {
                if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
                {

                    int selectedId = UIManager.Instance.GetSelectedItemId();

                    if (selectedId == 4 || selectedId == 7 || selectedId == 11)
                    {
                        Services.Audio.PlaySFX("UseKeyToOpenDoor");
                    }
                    else if (selectedId == 2 || selectedId == 8 || selectedId == 9 || selectedId == 10)
                    {
                        Services.Audio.PlaySFX("KnifeCut");
                    }

                    Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
                    UIManager.Instance.RemoveItemFromUIByID(selectedId);

                    if (animator != null)
                    {
                        animator.SetTrigger("Interact");
                    }
                    else
                    {
                        Debug.Log("Nie masz animatora");
                    }

                    isActualOpen = true;
                }
                else
                {
                    if (localizationString.localizeString != null)
                        NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
                }
            }


        }
    }

    private void StartEndPanel()
    {
        StartCoroutine(CouritineEndPanel());
    }

    private IEnumerator CouritineEndPanel()
    {
        yield return new WaitForSeconds(1.3f);
        endPanel.SetActive(true);
    }

    public void PlaceObject()
    {
        if (interactableType == InteractableType.Placeable)
        {
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                boxCollider.enabled = false;
                objectToPlace.SetActive(true);
                int selectedId = UIManager.Instance.GetSelectedItemId();

                if (selectedId == 16)
                {
                    Services.Audio.PlaySFX("StartFire");
                    Services.Audio.PlayOnLoopSFX("FirePlayOnLoop");

                }
                else
                {
                    Services.Audio.PlaySFX("PlaceObject");
                }

                Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
                UIManager.Instance.RemoveItemFromUIByID(selectedId);
            }
            else
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
            }
        }
    }

    public void PlaceRecipeObject()
    {
        if (interactableType != InteractableType.PlaceRecipe)
            return;

        foreach (int requiredId in requiredItemIds)
        {
            if (!Inventory.Instance.HasItemWithId(requiredId))
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);

                continue;
            }


            if (requiredId == 26)
            {
                scroll_BigAcidPotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 27)
            {
                scroll_SmallAngryTimePotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 28)
            {
                scroll_ElderBadMoodPotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 29)
            {
                scroll_OpenBloodPotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 30)
            {
                scroll_BigGoodSoup.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 31)
            {
                scroll_SmallHolyCowPotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 32)
            {
                scroll_ElderLeafGoods.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 33)
            {
                scroll_OpenNiceWater.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 34)
            {
                scroll_BigWaterPotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }
            else if (requiredId == 35)
            {
                scroll_SmallWinePotion.SetActive(true);
                readableAndInteractableTextData.UnlockPageForRecipeId(requiredId);
            }

            Inventory.Instance.RemoveFromInventoryAlchemyRecipe(requiredId);
            recipesCounter.UpdateRecipeCount();
            Debug.LogWarning($"Znaleziono i usuniêto item o ID: {requiredId}");
        }


        if (
            scroll_BigAcidPotion.activeSelf &&
            scroll_SmallAngryTimePotion.activeSelf &&
            scroll_ElderBadMoodPotion.activeSelf &&
            scroll_OpenBloodPotion.activeSelf &&
            scroll_BigGoodSoup.activeSelf &&
            scroll_SmallHolyCowPotion.activeSelf &&
            scroll_ElderLeafGoods.activeSelf &&
            scroll_OpenNiceWater.activeSelf &&
            scroll_BigWaterPotion.activeSelf &&
            scroll_SmallWinePotion.activeSelf
        )
        {
            boxCollider.enabled = false;
            recipesCounter.DisableThisGameObject();
        }
    }

    public void OnBookThrow()
    {
        if (interactableType == InteractableType.Throwable)
        {
            if (TryGetComponent<Rigidbody>(out var rb))
            {
                Services.Audio.PlaySFX("ThrowBook");
                Vector3 throwDirection = transform.forward;
                float throwForce = 5f;
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning($"{gameObject.name} does not have a Rigidbody. Cannot apply force.");
            }
        }
    }

    public bool IsOpen() => isActualOpen;

    public void CloseObject()
    {
        if (interactableType == InteractableType.Openable)
        {
            if (isActualOpen)
            {
                isActualOpen = false;
                if (animator != null)
                {
                    animator.SetTrigger("Close");
                }
                else
                {
                    Debug.Log("Nie masz animatora");
                }
            }
        }
    }

    public void OnRead()
    {
        if (interactableType == InteractableType.Readable)
        {
            Services.Audio.PlaySFX("ReadBook");

            UIManager.Instance.readablePanel.ShowReadablePanel();
            UIManager.Instance.readablePanel.headerTextUI.text =
           readableTextData.localizeHeader.GetLocalizedString();

            UIManager.Instance.readablePanel.mainTextUI.text =
                readableTextData.localizeMainText.GetLocalizedString();

            UIManager.Instance.readablePanel.signatureTextUI.text =
               readableTextData.localizeSignature.GetLocalizedString();

            isActualReading = true;
        }

    }

    public void OnReadInteractable()
    {
        if (interactableType == InteractableType.ReadableAndInteractableItem)
        {
            UIManager.Instance.readableAndInteractablePanel.ShowReadablePanel();
            UIManager.Instance.readableAndInteractablePanel.headerTextUI.text = readableAndInteractableTextData.headerText.GetLocalizedString();

            int safeIndex = 0;
            if (readableAndInteractableTextData != null && readableAndInteractableTextData.bookPages != null)
            {
                readableAndInteractableTextData.currentPageIndex = readableAndInteractableTextData.GetFirstUnlockedPageIndex();

                safeIndex = Mathf.Clamp(readableAndInteractableTextData.currentPageIndex, 0, readableAndInteractableTextData.bookPages.Length - 1);
                UIManager.Instance.readableAndInteractablePanel.mainTextUI.text = readableAndInteractableTextData.bookPages[safeIndex].GetLocalizedString();
                UIManager.Instance.readableAndInteractablePanel.pressEorQTextUI.text = readableAndInteractableTextData.pressEorQText.GetLocalizedString();
            }

            isActualReading = true;

            UIManager.Instance.nextPageAction.action.Enable();
            UIManager.Instance.previousPageAction.action.Enable();
        }
    }


    public void OnStopReadInteractable()
    {
        if (interactableType == InteractableType.ReadableAndInteractableItem)
        {
            UIManager.Instance.readableAndInteractablePanel.HideReadablePanel();
            isActualReading = false;
        }
    }
    public bool IsReadingInteractable() => isActualReading;


    public bool IsReading() => isActualReading;

    public void OnStopRead()
    {
        if (interactableType == InteractableType.Readable)
        {
            Services.Audio.PlaySFX("ReadBook");
            UIManager.Instance.readablePanel.HideReadablePanel();
            isActualReading = false;
        }
    }

    public void OnPress()
    {
        if (interactableType == InteractableType.Pressable)
        {
            isActualPreesed = true;
            if (animator != null)
            {
                animator.SetTrigger("Interact");
            }
            else
            {
                Debug.Log("Nie masz animatora");
            }
            Services.Audio.PlaySFX("WallButtonPress");
            onAllWallButtonPressed?.Invoke();
        }
    }

    public bool IsPressed() => isActualPreesed;

    public void StartLockPick()
    {
        if (interactableType == InteractableType.LockPick)
        {
            if (UIManager.Instance != null && requiredItemIds.Contains(UIManager.Instance.GetSelectedItemId()))
            {
                Services.Audio.PlaySFX("EnterLockPicking");
                UIManager.Instance.lockPickPanel.ShowLockPickPanel();
                isLockPicking = true;
            }
            else
            {
                if (localizationString.localizeString != null)
                    NotificationSystem.Instance.ShowNotification(localizationString.localizeString, 3);
            }
        }
    }

    public bool IsLockPicking() => isLockPicking;

    public void StopLockPicking()
    {
        if (interactableType == InteractableType.LockPick)
        {
            Services.Audio.PlaySFX("StopLockPicking");
            UIManager.Instance.lockPickPanel.HideLockPickPanel();
            isLockPicking = false;
        }
    }

    [System.Serializable]
    public class FillMapping
    {
        [Tooltip("Pusty przedmiot, który gracz musi trzymaæ (np. prefab Pustego Wiadra).")]
        public InteractableItem requiredEmptyItem;

        [Tooltip("Przedmiot, który gracz otrzyma po nape³nieniu (np. prefab Wiadra z Wod¹).")]
        public InteractableItem resultingFilledItem;
    }
}

