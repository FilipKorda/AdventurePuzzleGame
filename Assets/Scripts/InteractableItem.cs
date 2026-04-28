using System;
using System.Collections;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class InteractableItem : MonoBehaviour, IPickupable, IBookThrowable, IOpenable, IReadable, IPressable,
    IPlaceable, ILockPick, IFillable, IPickupARenewableItem, IAlchemyStation, IReadableAndInteractable, IRecipePlaceable,
    IGetObject, ICrafting, IPinNumber, IRotate, ICryptex, IMirror, IGearLock, IGearRotate, IGear90, IPipeGearPuzzle,
    IFurniture, IWoodenBlockPuzzle, IWoodenBlock, ITrianglePuzzle, ISymbolPlaceable, IArrowDirection, IPuzzlePipePart,
    IBlockButton, INinePadPanel, ICircleAndSquarePuzzle, IRotateCircleAndSquarePuzzle, IPlayerSphereMovement, ILibraryButton,
    ISafe, IBraiser, IFramePuzzle, IWallSwitchOnOff, IPlaceOnScale, IBriefcase, IMovingBlockBriefcase, IPaintingMove,
    IPlacePillarSymbol, IPillarMoveSphere, IRotatingPillar, IRotateOnePillar, IMoveSphereOnePillarPuzzle, IPictureTerrainObject,
    ICoverAllSquarePuzzle, ICorrectSixteenSymbols, ITwoCrystalsPuzzle, IRotatingCirclePuzzle, ITwelveDotsPuzzle, ILastPuzzle
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
        WoodenBlock,
        TrianglePuzzle,
        SymbolPlaceable,
        ArrowUp,
        ArrowDown,
        ArrowLeft,
        ArrowRight,
        PuzzlePart,
        PressWoodenButton,
        NinePadPanelPuzzle,
        CircleAndSquarePuzzle,
        RotateCircleAndSquarePuzzle,
        PlayerSphereMovement,
        LibraryButton,
        Safe,
        Braiser,
        PuzzleFrame,
        WallSwitchOnOff,
        PlaceOnScale,
        Briefcase,
        MovingBlockBriefcase,
        MovePainting,
        PlacePillarSymbol,
        ClickMovePillarSphere,
        RotatingPillar,
        RotateOnePillar,
        MoveSphereOnePillarPuzzle,
        PictureTerrainObject,
        CoverAllSquarePuzzle,
        CorrectSixteenSymbols,
        TwoCrystalsPuzzle,
        RotatingCirclePuzzle,
        TwelveDotsPuzzle,
        LastPuzzle
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
    public bool isWeightObject = false;
    [SerializeField] private int requiredUses = 3;

    [Header("Fillable Source Settings (if Fillable)")]
    [Tooltip("Typ p³ynu, który dostarcza to Ÿród³o.")]
    [SerializeField] private LiquidType providedLiquidType = LiquidType.None;

    [Tooltip("Lista mapowañ, które definiuj¹, jaki pusty pojemnik zamienia siê w jaki nape³niony.")]
    [SerializeField] private FillMapping fillMapping;
    [SerializeField] private TapBarrelManager tapBarrelManager;

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

    [Header("Moving Block Puzzle Arrow Direction")]
    [SerializeField] private GameObject objectA;
    [SerializeField] private GameObject objectB;
    [SerializeField] private float moveDistanceMovingBlockPuzzle = 1f;
    [SerializeField] private float moveSpeedMovingBlockPuzzle = 5f;
    [SerializeField] private float rayLengthMovingBlockPuzzle = 0.45f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private MovingBlockPuzzleManager movingBlockPuzzleManager;
    private bool movingBlockIsMoving = false;

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
    [SerializeField] private bool isShorterCryptexSound = false;

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

    [Header("Sword Puzzle")]
    [SerializeField] private SwordPuzzle swordPuzzle;

    [Header("Triangle Puzzle")]
    [SerializeField] private TriangleEnum triangleEnum;
    [SerializeField] private TrianglePuzzleManager puzzleManager;

    [Header("Symbol Place Objects")]
    [SerializeField] private GameObject shrine;
    [SerializeField] private GameObject pillar;
    [SerializeField] private GameObject grave;
    [SerializeField] private GameObject brokenPillar;
    [SerializeField] private GameObject woodenSword;
    [SerializeField] private bool isSymbolPlace = false;
    [SerializeField] private ClockSymbolsManager clockSymbolsManager;

    [Header("Wooden Block Button")]
    [SerializeField] private BlockPanel blockPanel;
    [SerializeField] private NinePadPanelManager ninePadPanelManager;
    private bool buttonWasPressed = false;
    public bool buttonIsPressed = false;


    [SerializeField] private CircleAndSquarePuzzle circleAndSquarePuzzle;

    [Header("Rotate Circle And Square Puzzle")]
    private bool rotateCircleAndSquarePuzzle = false;
    [SerializeField] private PlayerPathMovement playerPathMovement;

    [Header("Library puzzle")]
    [SerializeField] private LibraryButtonsManager libraryButtonsManager;
    [SerializeField] private int libraryButtonInt = 0;

    [Header("Safe Puzzle")]
    [SerializeField] private SafePuzzle safePuzzle;

    [Header("Braiser Puzzle")]
    [SerializeField] private BraiserPuzzle braiserPuzzle;
    [SerializeField] private int braiserInt = 0;

    [Header("Puzzle Frame")]
    [SerializeField] private GameObject swordPuzzlePiece;
    [SerializeField] private GameObject clubPuzzlePiece;

    [SerializeField] private GameObject skeletonSwordPiece;
    [SerializeField] private GameObject skeletonWarAxePiece;
    [SerializeField] private GameObject skeletonHelmetPiece;
    [SerializeField] private GameObject skeletonFullHelmetPiece;

    [SerializeField] private FramePuzzlemanager framePuzzlemanager;


    [Header("Wall Switch On Off Puzzle")]
    [SerializeField] private WallSwitchOnOffManager wallSwitchOnOffManager;
    [SerializeField] private WallSwitchType switchType;

    public bool IsOn => isOn;
    private bool isOn = false;
    private bool isAnimating = false;

    [Header("Scale Puzzle")]
    [SerializeField] private ScaleWeightSystem scaleWeightSystem;
    [SerializeField] private WeightItem weight;
    [SerializeField] private WeightItem mainBottle;
    [SerializeField] private WeightItem mainMug;
    [SerializeField] private WeightItem nail;
    [SerializeField] private WeightItem hatch;

    [Header("Briefcase Manager")]
    [SerializeField] private BriefcaseManager briefcaseManager;
    [Header("Moving Block Briefcase")]
    [SerializeField] private MovingBlockBriefcase movingBlockBriefcase;
    [Header("Move Painting")]
    [SerializeField] private ImageSlider imageSlider;
    [Header("Pillar Puzzle")]
    [SerializeField] private GameObject symbol0;
    [SerializeField] private GameObject symbol1;
    [SerializeField] private GameObject symbol2;
    [Header("Click Move Pillar Sphere")]
    [SerializeField] private MovingPillarManager movingPillarManager;
    [Header("Rotating Pillar Moving Block")]
    [SerializeField] private RotatingPillarMovingBlock rotatingPillarMovingBlock;
    [Header("Move Sphere One Pillar Puzzle")]
    [SerializeField] private MoveSphereOnePillarPuzzle moveSphereOnePillarPuzzle;
    public bool canRotateMoveSphereOnePillarPuzzle = true;
    [Header("Picture Tarrain Moving Object")]
    [SerializeField] private PictureTerrainMovingObject pictureTerrainMovingObject;
    [Header("Cover All Square Puzzle")]
    [SerializeField] private CoverAllSquarePuzzlePillar coverAllSquarePuzzlePillar;
    [Header("Correct Sixteen Symbols")]
    [SerializeField] private CorrectSixteenSymbolsPillar correctSixteenSymbolsPillar;
    [Header("Two Crystals Puzzle")]
    [SerializeField] private TwoCrystalsPuzzle twoCrystalsPuzzle;
    [Header("Two Crystals Puzzle]")]
    [SerializeField] private RotatingCirclePuzzle rotatingCirclePuzzle;
    [Header("Twelve Dots Puzzle]")]
    [SerializeField] private TwelveDotPuzzle twelveDotPuzzle;
    [Header("Last Puzzle]")]
    [SerializeField] private LastPuzzle lastPuzzle;

    private void Awake()
    {
        if (rend != null)
        {
            baseColor = rend.material.color;
        }

        if (interactableType == InteractableType.WoodenBlock || interactableType == InteractableType.PressWoodenButton)
        {
            startPosition = transform.position;
        }

    }

    public void EnterLastPuzzle()
    {
        CursorController.Instance.SetGameMode(new LastPuzzleGameMode());
        lastPuzzle.EnterPuzzle();
    }

    public void EnterTwelveDotsPuzzle()
    {
        CursorController.Instance.SetGameMode(new TwelveDotsPuzzleGameMode());
        twelveDotPuzzle.EnterPuzzle();

    }

    public void EnterRotatingCirclePuzzle()
    {
        CursorController.Instance.SetGameMode(new RotatingCirclePuzzleGameMode());
        rotatingCirclePuzzle.EnterPuzzle();
    }

    public void EnterTwoCrystalsPuzzle()
    {
        CursorController.Instance.SetGameMode(new TwoCrystalsPuzzleGameMode());
        twoCrystalsPuzzle.EnterPuzzle();
    }

    public void EnterCorrectSixteenSymbolsPuzzle()
    {
        CursorController.Instance.SetGameMode(new CorrectSixteenSymbolsGameMode());
        correctSixteenSymbolsPillar.EnterPuzzle();
    }

    public void EnterCoverAllSquarePuzzle()
    {
        CursorController.Instance.SetGameMode(new AllSquareClickGameMode());
        coverAllSquarePuzzlePillar.EnterPuzzle();
    }

    public void ClickPicturetarrainMovingSphere()
    {
        pictureTerrainMovingObject.SetClickAndDrag(true);
    }

    public void ClickMovingSphere()
    {
        moveSphereOnePillarPuzzle.SetClickAndDrag(true);
    }

    public void RotatePillar()
    {
        if (canRotateMoveSphereOnePillarPuzzle)
        {
            StartCoroutine(RotatePillarCoroutine(90f, 0.5f));
            Services.Audio.PlaySFX("MoveStoneInTheFloor");
        }
    }

    private IEnumerator RotatePillarCoroutine(float angle, float duration)
    {
        canRotateMoveSphereOnePillarPuzzle = false;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, angle);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRotation;

        canRotateMoveSphereOnePillarPuzzle = true;
    }

    public void EnterRotatingPillarPuzzle()
    {
        CursorController.Instance.SetGameMode(new MovingBlockClickDragGameMode());
        rotatingPillarMovingBlock.EnterPuzzle();
    }

    public void ClickMovePillarSphere()
    {
        movingPillarManager.SetClickAndDrag(true);
    }

    public void PlacePillarSymbol()
    {
        int currentSelectedId = UIManager.Instance.GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.PillarPuzzle0:
                symbol0.SetActive(true);
                break;
            case ItemID.PillarPuzzle1:
                symbol1.SetActive(true);
                break;
            case ItemID.PillarPuzzle2:
                symbol2.SetActive(true);
                break;
        }

        Services.Audio.PlaySFX("PuzzlePiece");
        Inventory.Instance.RemoveItemFromInventoryByID(currentSelectedId);
        UIManager.Instance.RemoveItemFromUIByID(currentSelectedId);
    }

    public void MovePainting()
    {
        imageSlider.Next();

    }
    public void ClickMovingBlockBriefcase()
    {
        movingBlockBriefcase.SetClickAndDrag(true);
    }

    public void EnterBriefcasePuzzle()
    {
        briefcaseManager.EnterPuzzle();
    }

    public void PlaceOnScale()
    {
        int currentSelectedId = UIManager.Instance.GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.Weight:
                weight.gameObject.SetActive(true);
                scaleWeightSystem.AddItem(weight);
                break;
            case ItemID.MainBottle:
                mainBottle.gameObject.SetActive(true);
                scaleWeightSystem.AddItem(mainBottle);
                break;
            case ItemID.MainMug:
                mainMug.gameObject.SetActive(true);
                scaleWeightSystem.AddItem(mainMug);
                break;
            case ItemID.Nail:
                nail.gameObject.SetActive(true);
                scaleWeightSystem.AddItem(nail);
                break;
            case ItemID.Hatch:
                hatch.gameObject.SetActive(true);
                scaleWeightSystem.AddItem(hatch);
                break;
        }

        Services.Audio.PlaySFX("PuzzlePiece");
        Inventory.Instance.RemoveItemFromInventoryByID(currentSelectedId);
        UIManager.Instance.RemoveItemFromUIByID(currentSelectedId);
    }

    public void ClickWallSwitch()
    {
        if (isAnimating)
            return;

        Services.Audio.PlaySFX("WallButtonPress");
        wallSwitchOnOffManager.OnSwitchPressed(switchType);
    }

    public void Toggle()
    {
        isOn = !isOn;
        StartCoroutine(RotateSwitch(isOn ? 8f : -8f));
    }

    private IEnumerator RotateSwitch(float targetZ)
    {
        isAnimating = true;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = Quaternion.Euler(targetZ, 0f, 0f);

        float time = 0f;
        float duration = 0.1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localRotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        transform.localRotation = endRot;
        isAnimating = false;
    }

    public void PlacePuzzleIntoFrame()
    {
        int currentSelectedId = UIManager.Instance.GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.KnightSwordPiece:
                swordPuzzlePiece.SetActive(true);
                break;
            case ItemID.KnightClubPiece:
                clubPuzzlePiece.SetActive(true);
                break;
            case ItemID.SkeletonSwordPiece:
                skeletonSwordPiece.SetActive(true);
                break;
            case ItemID.SkeletonWarAxePiece:
                skeletonWarAxePiece.SetActive(true);
                break;
            case ItemID.SkeletonHelmetPiece:
                skeletonHelmetPiece.SetActive(true);
                break;
            case ItemID.SkeletonFullHelmetPiece:
                skeletonFullHelmetPiece.SetActive(true);
                break;
        }

        framePuzzlemanager.CheckAllPuzzlesBlocks();
        Services.Audio.PlaySFX("PuzzlePiece");
        Inventory.Instance.RemoveItemFromInventoryByID(currentSelectedId);
        UIManager.Instance.RemoveItemFromUIByID(currentSelectedId);

    }

    public void EnterBraiserPuzzle()
    {
        CursorController.Instance.SetGameMode(new ClickDragGameMode());

        if (braiserInt == 0)
        {
            braiserPuzzle.EnterPuzzle0();
        }
        else if (braiserInt == 1)
        {
            braiserPuzzle.EnterPuzzle1();
        }
        else if (braiserInt == 2)
        {
            braiserPuzzle.EnterPuzzle2();
        }
        else
        {
            braiserPuzzle.EnterPuzzle3();
        }
    }

    public void EnterSafe()
    {
        safePuzzle.EnterPuzzleMode();
    }

    public void PressLibraryButton()
    {
        Services.Audio.PlaySFX("WallButtonPress");
        libraryButtonsManager.PressButton(libraryButtonInt);
        animator.SetTrigger("Interact");

    }

    public void ClickAndDrag()
    {
        playerPathMovement.SetClickAndDrag(true);
    }

    public void RotateCircleAndSquarePuzzle()
    {
        if (rotateCircleAndSquarePuzzle) return;
        StartCoroutine(CourutineRotateCircleAndSquarePuzzle());
    }

    public IEnumerator CourutineRotateCircleAndSquarePuzzle()
    {
        Services.Audio.PlaySFX("RotateWheelv2");
        rotateCircleAndSquarePuzzle = true;
        transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
        Quaternion targetRotation = startRotation * Quaternion.Euler(-90f, 0f, 0f);

        float time = 0f;
        float duration = 0.3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            yield return null;
        }

        transform.rotation = targetRotation;
        yield return null;

        transform.position = startPosition;
        rotateCircleAndSquarePuzzle = false;
    }

    public void EnterCircleAndSquarePuzzle()
    {
        circleAndSquarePuzzle.EnterPuzzle();
    }

    public void EnterNinePadPuzzle()
    {
        ninePadPanelManager.EnterPuzzle();
    }

    public void PressButton()
    {
        if (buttonWasPressed) return;

        if (buttonIsPressed)
        {
            buttonIsPressed = false;
            highlighted = false;
            StartCoroutine(MoveBlockCoroutine(gameObject, Vector3.left));
        }
        else
        {
            buttonIsPressed = true;
            highlighted = true;
            StartCoroutine(MoveBlockCoroutine(gameObject, Vector3.right));
        }


    }

    private IEnumerator MoveBlockCoroutine(GameObject block, Vector3 direction)
    {
        Services.Audio.PlaySFX("WallButtonPress");
        buttonWasPressed = true;

        blockPanel.CheckIfAllButtonsArePreesed();

        Vector3 start = block.transform.position;
        Vector3 target = start + direction * 0.015f;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(MoveToColorChange());

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 10f;
            block.transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        block.transform.position = target;
        buttonWasPressed = false;
        ninePadPanelManager.CheckIfAllButtonsArePressed();
    }

    public void ResetMoveBlockPosition()
    {
        StopAllCoroutines();
        transform.position = startPosition;
        buttonIsPressed = false;
        buttonWasPressed = false;
    }

    public void EnterPuzzlePipePart()
    {
        movingBlockPuzzleManager.EnterMovingBlockPuzzle();

    }

    public void ArrowUp()
    {
        TryMove(Vector3.forward);
    }

    public void ArrowDown()
    {
        TryMove(Vector3.back);
    }

    public void ArrowLeft()
    {
        TryMove(Vector3.left);
    }

    public void ArrowRight()
    {
        TryMove(Vector3.right);
    }

    private void TryMove(Vector3 direction)
    {
        if (movingBlockIsMoving) return;

        if (CanMove(objectA.transform.position, direction))
            StartCoroutine(MoveSingle(objectA, direction));

        if (CanMove(objectB.transform.position, direction))
            StartCoroutine(MoveSingle(objectB, direction));
    }

    private bool CanMove(Vector3 origin, Vector3 direction)
    {
        if (direction == Vector3.left || direction == Vector3.right)
        {
            return !Physics.Raycast(origin, direction, rayLengthMovingBlockPuzzle, obstacleMask);
        }

        if (direction == Vector3.forward)
        {
            return !Physics.Raycast(origin, Vector3.up, rayLengthMovingBlockPuzzle, obstacleMask);
        }

        if (direction == Vector3.back)
        {
            return !Physics.Raycast(origin, Vector3.down, rayLengthMovingBlockPuzzle, obstacleMask);
        }

        return true;
    }

    private IEnumerator MoveSingle(GameObject obj, Vector3 direction)
    {
        movingBlockIsMoving = true;
        Services.Audio.PlaySFX("FastMove");
        Vector3 start = obj.transform.localPosition;
        Vector3 target = start + direction * moveDistanceMovingBlockPuzzle;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeedMovingBlockPuzzle;
            obj.transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
        movingBlockPuzzleManager.CheckCorrectPositionOfAnObjects();
        movingBlockIsMoving = false;


    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (objectA != null)
            DrawRays(objectA.transform.position);

        if (objectB != null)
            DrawRays(objectB.transform.position);
#endif
    }

    private void DrawRays(Vector3 origin)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector3.up * rayLengthMovingBlockPuzzle);
        Gizmos.DrawLine(origin, origin + Vector3.down * rayLengthMovingBlockPuzzle);
        Gizmos.DrawLine(origin, origin + Vector3.left * rayLengthMovingBlockPuzzle);
        Gizmos.DrawLine(origin, origin + Vector3.right * rayLengthMovingBlockPuzzle);
    }

    public void PlaceSymbol()
    {
        int currentSelectedId = UIManager.Instance.GetSelectedItemId();
        if (currentSelectedId == 0)
        {
            Debug.Log("Nie wybrano ¿adnego przedmiotu do u¿ycia.");
            return;
        }

        switch ((ItemID)currentSelectedId)
        {
            case ItemID.Shrine:
                shrine.SetActive(true);
                break;
            case ItemID.SymbolPillar:
                pillar.SetActive(true);
                break;
            case ItemID.Grave:
                grave.SetActive(true);
                break;
            case ItemID.BrokenPillar:
                brokenPillar.SetActive(true);
                break;
            case ItemID.SymbolSword:
                woodenSword.SetActive(true);
                break;
        }

        Services.Audio.PlaySFX("PlaceObject");
        clockSymbolsManager.CheckAllClockSymbols();
        Inventory.Instance.RemoveItemFromInventoryByID(currentSelectedId);
        UIManager.Instance.RemoveItemFromUIByID(currentSelectedId);
    }

    public void ClickTriangleButton()
    {
        puzzleManager.PressedTriangle(triangleEnum);
        animator.SetTrigger("Interact");
        Services.Audio.PlaySFX("WallButtonPress");
    }

    public void OnClick()
    {
        if (isMovingWoodenBlock)
            return;

        Services.Audio.PlaySFX("WallButtonPress");

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

    IEnumerator MoveToColorChange()
    {
        Color fromColor = rend.material.color;
        Color targetColor = highlighted ? highlightColor : baseColor;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.2f;

            rend.material.color = Color.Lerp(fromColor, targetColor, t);

            yield return null;
        }

        rend.material.color = targetColor;
    }

    public void ResetblockInstant(Color baseColor)
    {
        transform.position = startPosition;
        rend.material.color = baseColor;
        moved = false;
        highlighted = false;
        isMovingWoodenBlock = false;
    }

    public void ResetColorblockInstant(Color baseColor)
    {
        rend.material.color = baseColor;
        highlighted = false;
    }

    public void EnterWoddenBlockPuzzle()
    {
        woodenBlockPuzzle.EnterWoodenPuzzleMode();
    }

    public void PushFurniture()
    {
        movableBlock.TakeControlOfTheThiBlock();

    }

    public void EnterPipeGearPuzzleMode()
    {
        pipeGearPuzzle.EnterGearLockMode();
    }

    public void RotateGear90()
    {
        if (gear90IsRotating)
            return;

        Services.Audio.PlaySFX("GearTick");
        currentGear90Index = (currentGear90Index + 1) % 4;


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
    }

    public void EnterTheMirrorMode()
    {
        mirror.EnableControl();

    }

    public void RotateCryptex()
    {
        if (cryptexIsRotating)
            return;

        if (isShorterCryptexSound)
        {
            Services.Audio.PlaySFX("ShorterMovingStoneKryptex");
        }
        else
        {
            Services.Audio.PlaySFX("MovingStoneKryptex");
        }


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
        Services.Audio.PlaySFX("ClickPinNumber");
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


        Services.Audio.PlaySFX("PlaceObject");

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
                if (cauldron.fireObject.activeInHierarchy)
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
                Services.Audio.PlaySFX("PickUpItem");
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

                if (isSymbolPlace)
                {
                    if (isWeightObject)
                    {
                        var weightGameOject = gameObject.GetComponent<WeightItem>();
                        scaleWeightSystem.RemoveItem(weightGameOject);
                    }

                    DisableThisGameObject();
                }
                else
                {
                    DestroyInteractable();
                }

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

            if (tapBarrelManager != null)
            {
                tapBarrelManager.FillBucketActivator();
            }
            else
            {
                Inventory.Instance.AddItemToInventory(fillMapping.resultingFilledItem);
                Services.Audio.PlaySFX("PourWater");
            }

            Inventory.Instance.RemoveItemFromInventoryByID(selectedId);
            UIManager.Instance.RemoveItemFromUIByID(selectedId);

            if (tapBarrelManager != null)
            {
                StartCoroutine(CouritineFill());
            }

        }
    }

    private IEnumerator CouritineFill()
    {

        yield return new WaitForSeconds(1.5f);
        Inventory.Instance.AddItemToInventory(fillMapping.resultingFilledItem);
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

                    if (selectedId == 4 || selectedId == 7 || selectedId == 11 || selectedId == 79
                        || selectedId == 62 || selectedId == 67 || selectedId == 55)
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
                else if (selectedId == 52)
                {
                    swordPuzzle.CheckSwordStatus();
                    Services.Audio.PlaySFX("PlaceObject");
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

