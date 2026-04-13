using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableItem))]
public class InteractableItemEditor : Editor
{
    private SerializedProperty interactableTypeProp;
    private SerializedProperty itemNameProp;
    private SerializedProperty itemSpriteProp;
    private SerializedProperty itemIdProp;
    private SerializedProperty requiredItemIdsProp;
    private SerializedProperty animatorProp;
    private SerializedProperty readableTextDataProp;
    private SerializedProperty onAllWallButtonPressedProp;
    private SerializedProperty objectToPlaceProp;
    private SerializedProperty boxColliderProp;
    private SerializedProperty canOpenWithNoSelectedItem;
    private SerializedProperty providedLiquidTypeProp;
    private SerializedProperty fillMappingProp;
    private SerializedProperty cauldronStation;
    private SerializedProperty itemPrefab;
    private SerializedProperty readableAndInteractableTextData;
    private SerializedProperty isAlchemyRecipe;
    private SerializedProperty scroll_BigAcidPotion;
    private SerializedProperty scroll_SmallAngryTimePotion;
    private SerializedProperty scroll_ElderBadMoodPotion;
    private SerializedProperty scroll_OpenBloodPotion;
    private SerializedProperty scroll_BigGoodSoup;
    private SerializedProperty scroll_SmallHolyCowPotion;
    private SerializedProperty scroll_ElderLeafGoods;
    private SerializedProperty scroll_OpenNiceWater;
    private SerializedProperty scroll_BigWaterPotion;
    private SerializedProperty scroll_SmallWinePotion;
    private SerializedProperty recipesCounter;
    private SerializedProperty endPanel;
    private SerializedProperty isEndPanel;
    private SerializedProperty localizeItemName;
    private SerializedProperty localizationString;
    private SerializedProperty localizationTwoString;
    private SerializedProperty itemIdbyItemId;

    private SerializedProperty metalCrabs;
    private SerializedProperty springs;
    private SerializedProperty metalscaffolds;
    private SerializedProperty collectibleGears;
    private SerializedProperty cables;
    private SerializedProperty workbench;

    private SerializedProperty pinNumber;
    private SerializedProperty rend;
    private SerializedProperty highlightColor;

    private SerializedProperty rotationDuration;
    private SerializedProperty statueCompasPuzzle;
    private SerializedProperty linkedReadable;
    private SerializedProperty directionTextSet;

    private SerializedProperty cryptexRotateDuration;

    private SerializedProperty mirror;
    private SerializedProperty gearLockMode;
    private SerializedProperty gearRotateDuration;
    private SerializedProperty gear90RotateDuration;
    private SerializedProperty pipeGearPuzzle;
    private SerializedProperty currentGear90Index;

    private SerializedProperty movableBlock;
    private SerializedProperty woodenBlockPuzzle;
    private SerializedProperty zOffset;
    private SerializedProperty duration;

    private SerializedProperty swordPuzzle;
    private SerializedProperty triangleEnum;
    private SerializedProperty puzzleManager;

    private SerializedProperty shrine;
    private SerializedProperty pillar;
    private SerializedProperty grave;
    private SerializedProperty brokenPillar;
    private SerializedProperty woodenSword;
    private SerializedProperty isSymbolPlace;
    private SerializedProperty clockSymbolsManager;

    private SerializedProperty objectA;
    private SerializedProperty objectB;
    private SerializedProperty moveDistanceMovingBlockPuzzle;
    private SerializedProperty moveSpeedMovingBlockPuzzle;
    private SerializedProperty obstacleMask;
    private SerializedProperty rayLengthMovingBlockPuzzle;
    private SerializedProperty movingBlockPuzzleManager;

    private SerializedProperty blockPanel;
    private SerializedProperty ninePadPanelManager;

    private SerializedProperty circleAndSquarePuzzle;
    private SerializedProperty playerPathMovement;

    private SerializedProperty libraryButtonsManager;
    private SerializedProperty libraryButtonInt;

    private SerializedProperty safePuzzle;

    private SerializedProperty braiserPuzzle;
    private SerializedProperty braiserInt;

    private SerializedProperty swordPuzzlePiece;
    private SerializedProperty clubPuzzlePiece;
    private SerializedProperty framePuzzlemanager;
    private SerializedProperty skeletonSwordPiece;
    private SerializedProperty skeletonWarAxePiece;
    private SerializedProperty skeletonHelmetPiece;
    private SerializedProperty skeletonFullHelmetPiece;

    private SerializedProperty wallSwitchOnOffManager;
    private SerializedProperty switchType;

    private SerializedProperty weight;
    private SerializedProperty mainBottle;
    private SerializedProperty mainMug;
    private SerializedProperty nail;
    private SerializedProperty hatch;
    private SerializedProperty isWeightObject;
    private SerializedProperty scaleWeightSystem;

    private SerializedProperty isShorterCryptexSound;

    private SerializedProperty briefcaseManager;
    private SerializedProperty movingBlockBriefcase;
    private SerializedProperty imageSlider;

    private SerializedProperty symbol0;
    private SerializedProperty symbol1;
    private SerializedProperty symbol2;

    private SerializedProperty movingPillarManager;
    private SerializedProperty rotatingPillarMovingBlock;

    private SerializedProperty moveSphereOnePillarPuzzle;
    private SerializedProperty pictureTerrainMovingObject;
    private SerializedProperty coverAllSquarePuzzlePillar;
    private SerializedProperty correctSixteenSymbolsPillar;


    private void OnEnable()
    {
        if (serializedObject == null || serializedObject.targetObject == null)
            return;

        interactableTypeProp = serializedObject.FindProperty("interactableType");
        itemNameProp = serializedObject.FindProperty("itemName");
        itemSpriteProp = serializedObject.FindProperty("itemSprite");
        itemIdProp = serializedObject.FindProperty("itemId");
        requiredItemIdsProp = serializedObject.FindProperty("requiredItemIds");
        animatorProp = serializedObject.FindProperty("animator");
        readableTextDataProp = serializedObject.FindProperty("readableTextData");
        onAllWallButtonPressedProp = serializedObject.FindProperty("onAllWallButtonPressed");
        objectToPlaceProp = serializedObject.FindProperty("objectToPlace");
        boxColliderProp = serializedObject.FindProperty("boxCollider");
        canOpenWithNoSelectedItem = serializedObject.FindProperty("canOpenWithNoSelectedItem");
        providedLiquidTypeProp = serializedObject.FindProperty("providedLiquidType");
        fillMappingProp = serializedObject.FindProperty("fillMapping");
        cauldronStation = serializedObject.FindProperty("cauldron");
        itemPrefab = serializedObject.FindProperty("itemPrefab");
        readableAndInteractableTextData = serializedObject.FindProperty("readableAndInteractableTextData");
        isAlchemyRecipe = serializedObject.FindProperty("isAlchemyRecipe");
        scroll_BigAcidPotion = serializedObject.FindProperty("scroll_BigAcidPotion");
        scroll_SmallAngryTimePotion = serializedObject.FindProperty("scroll_SmallAngryTimePotion");
        scroll_ElderBadMoodPotion = serializedObject.FindProperty("scroll_ElderBadMoodPotion");
        scroll_OpenBloodPotion = serializedObject.FindProperty("scroll_OpenBloodPotion");
        scroll_BigGoodSoup = serializedObject.FindProperty("scroll_BigGoodSoup");
        scroll_SmallHolyCowPotion = serializedObject.FindProperty("scroll_SmallHolyCowPotion");
        scroll_ElderLeafGoods = serializedObject.FindProperty("scroll_ElderLeafGoods");
        scroll_OpenNiceWater = serializedObject.FindProperty("scroll_OpenNiceWater");
        scroll_BigWaterPotion = serializedObject.FindProperty("scroll_BigWaterPotion");
        scroll_SmallWinePotion = serializedObject.FindProperty("scroll_SmallWinePotion");
        recipesCounter = serializedObject.FindProperty("recipesCounter");
        endPanel = serializedObject.FindProperty("endPanel");
        isEndPanel = serializedObject.FindProperty("isEndPanel");
        localizeItemName = serializedObject.FindProperty("localizeItemName");
        localizationString = serializedObject.FindProperty("localizationString");
        localizationTwoString = serializedObject.FindProperty("localizationTwoString");
        itemIdbyItemId = serializedObject.FindProperty("itemIdbyItemId");

        metalCrabs = serializedObject.FindProperty("metalCrabs");
        springs = serializedObject.FindProperty("springs");
        metalscaffolds = serializedObject.FindProperty("metalscaffolds");
        collectibleGears = serializedObject.FindProperty("collectibleGears");
        cables = serializedObject.FindProperty("cables");
        workbench = serializedObject.FindProperty("workbench");

        pinNumber = serializedObject.FindProperty("pinNumber");
        rend = serializedObject.FindProperty("rend");
        highlightColor = serializedObject.FindProperty("highlightColor");

        rotationDuration = serializedObject.FindProperty("rotationDuration");
        statueCompasPuzzle = serializedObject.FindProperty("statueCompasPuzzle");
        linkedReadable = serializedObject.FindProperty("linkedReadable");
        directionTextSet = serializedObject.FindProperty("directionTextSet");

        cryptexRotateDuration = serializedObject.FindProperty("cryptexRotateDuration");

        mirror = serializedObject.FindProperty("mirror");
        gearLockMode = serializedObject.FindProperty("gearLockMode");
        gearRotateDuration = serializedObject.FindProperty("gearRotateDuration");
        gear90RotateDuration = serializedObject.FindProperty("gear90RotateDuration");
        pipeGearPuzzle = serializedObject.FindProperty("pipeGearPuzzle");
        currentGear90Index = serializedObject.FindProperty("currentGear90Index");

        movableBlock = serializedObject.FindProperty("movableBlock");
        woodenBlockPuzzle = serializedObject.FindProperty("woodenBlockPuzzle");
        zOffset = serializedObject.FindProperty("zOffset");
        duration = serializedObject.FindProperty("duration");

        swordPuzzle = serializedObject.FindProperty("swordPuzzle");
        triangleEnum = serializedObject.FindProperty("triangleEnum");
        puzzleManager = serializedObject.FindProperty("puzzleManager");

        shrine = serializedObject.FindProperty("shrine");
        pillar = serializedObject.FindProperty("pillar");
        grave = serializedObject.FindProperty("grave");
        brokenPillar = serializedObject.FindProperty("brokenPillar");
        woodenSword = serializedObject.FindProperty("woodenSword");
        isSymbolPlace = serializedObject.FindProperty("isSymbolPlace");
        clockSymbolsManager = serializedObject.FindProperty("clockSymbolsManager");

        objectA = serializedObject.FindProperty("objectA");
        objectB = serializedObject.FindProperty("objectB");
        moveDistanceMovingBlockPuzzle = serializedObject.FindProperty("moveDistanceMovingBlockPuzzle");
        moveSpeedMovingBlockPuzzle = serializedObject.FindProperty("moveSpeedMovingBlockPuzzle");
        obstacleMask = serializedObject.FindProperty("obstacleMask");
        rayLengthMovingBlockPuzzle = serializedObject.FindProperty("rayLengthMovingBlockPuzzle");
        movingBlockPuzzleManager = serializedObject.FindProperty("movingBlockPuzzleManager");

        blockPanel = serializedObject.FindProperty("blockPanel");
        ninePadPanelManager = serializedObject.FindProperty("ninePadPanelManager");
        circleAndSquarePuzzle = serializedObject.FindProperty("circleAndSquarePuzzle");
        playerPathMovement = serializedObject.FindProperty("playerPathMovement");

        libraryButtonsManager = serializedObject.FindProperty("libraryButtonsManager");
        libraryButtonInt = serializedObject.FindProperty("libraryButtonInt");

        safePuzzle = serializedObject.FindProperty("safePuzzle");

        braiserPuzzle = serializedObject.FindProperty("braiserPuzzle");
        braiserInt = serializedObject.FindProperty("braiserInt");

        swordPuzzlePiece = serializedObject.FindProperty("swordPuzzlePiece");
        clubPuzzlePiece = serializedObject.FindProperty("clubPuzzlePiece");
        framePuzzlemanager = serializedObject.FindProperty("framePuzzlemanager");

        skeletonSwordPiece = serializedObject.FindProperty("skeletonSwordPiece");
        skeletonWarAxePiece = serializedObject.FindProperty("skeletonWarAxePiece");
        skeletonHelmetPiece = serializedObject.FindProperty("skeletonHelmetPiece");
        skeletonFullHelmetPiece = serializedObject.FindProperty("skeletonFullHelmetPiece");

        wallSwitchOnOffManager = serializedObject.FindProperty("wallSwitchOnOffManager");
        switchType = serializedObject.FindProperty("switchType");

        weight = serializedObject.FindProperty("weight");
        mainBottle = serializedObject.FindProperty("mainBottle");
        mainMug = serializedObject.FindProperty("mainMug");
        nail = serializedObject.FindProperty("nail");
        hatch = serializedObject.FindProperty("hatch");
        isWeightObject = serializedObject.FindProperty("isWeightObject");
        scaleWeightSystem = serializedObject.FindProperty("scaleWeightSystem");

        isShorterCryptexSound = serializedObject.FindProperty("isShorterCryptexSound");

        briefcaseManager = serializedObject.FindProperty("briefcaseManager");
        movingBlockBriefcase = serializedObject.FindProperty("movingBlockBriefcase");

        imageSlider = serializedObject.FindProperty("imageSlider");

        symbol0 = serializedObject.FindProperty("symbol0");
        symbol1 = serializedObject.FindProperty("symbol1");
        symbol2 = serializedObject.FindProperty("symbol2");

        movingPillarManager = serializedObject.FindProperty("movingPillarManager");
        rotatingPillarMovingBlock = serializedObject.FindProperty("rotatingPillarMovingBlock");
        moveSphereOnePillarPuzzle = serializedObject.FindProperty("moveSphereOnePillarPuzzle");
        pictureTerrainMovingObject = serializedObject.FindProperty("pictureTerrainMovingObject");
        coverAllSquarePuzzlePillar = serializedObject.FindProperty("coverAllSquarePuzzlePillar");
        correctSixteenSymbolsPillar = serializedObject.FindProperty("correctSixteenSymbolsPillar");

    }

    public override void OnInspectorGUI()
    {
        if (serializedObject == null || serializedObject.targetObject == null)
            return;

        serializedObject.Update();

        EditorGUILayout.PropertyField(interactableTypeProp, new GUIContent("Interactable Type"));

        InteractableItem.InteractableType currentType = (InteractableItem.InteractableType)interactableTypeProp.enumValueIndex;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("General Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(itemNameProp, new GUIContent("Item Name"));
        EditorGUILayout.PropertyField(itemSpriteProp, new GUIContent("UI Sprite (optional)"));
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Type-Specific Properties", EditorStyles.boldLabel);

        switch (currentType)
        {
            case InteractableItem.InteractableType.Pickupable:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.PropertyField(itemPrefab, new GUIContent("Item to Drop From UI"));
                EditorGUILayout.PropertyField(isAlchemyRecipe, new GUIContent("Is Item Alchemy recipe?"));
                EditorGUILayout.PropertyField(localizeItemName, new GUIContent("Localize Item Name"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.PropertyField(itemIdbyItemId, new GUIContent("Item Id Enum"));
                EditorGUILayout.PropertyField(isSymbolPlace, new GUIContent("Is Symbol Place"));
                EditorGUILayout.PropertyField(isWeightObject, new GUIContent("Is Weight Object"));
                EditorGUILayout.PropertyField(scaleWeightSystem, new GUIContent("Scale Weight System"));
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Openable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item IDs"));
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(canOpenWithNoSelectedItem, new GUIContent("Bool Can Open With seleted Item"));
                EditorGUILayout.PropertyField(endPanel, new GUIContent("End Panel"));
                EditorGUILayout.PropertyField(isEndPanel, new GUIContent("Is End Panel"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.HelpBox("Wymaga przedmiotu (lub przedmiotów) o podanych ID, aby mo¿na by³o go otworzyæ.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Throwable:
                EditorGUILayout.HelpBox("Ten obiekt wymaga komponentu Rigidbody, aby mo¿na by³o nim rzuciæ.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Readable:
                EditorGUILayout.PropertyField(readableTextDataProp, new GUIContent("Readable Text Data"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Book Collider"));
                break;

            case InteractableItem.InteractableType.Pressable:
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(onAllWallButtonPressedProp, new GUIContent("On Pressed Event"));
                break;

            case InteractableItem.InteractableType.Placeable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item ID"));
                EditorGUILayout.PropertyField(objectToPlaceProp, new GUIContent("Object To Place"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Interaction Collider"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.PropertyField(swordPuzzle, new GUIContent("Sword Puzzle"));
                EditorGUILayout.HelpBox("Wymaga przedmiotu o podanym ID, aby umieœciæ obiekt w œwiecie.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.LockPick:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.HelpBox("Potrzebujesz tego itemu o tym ID ¿eby otowrzyæ k³ótke", MessageType.Info);
                break;
            case InteractableItem.InteractableType.Fillable:
                EditorGUILayout.PropertyField(providedLiquidTypeProp, new GUIContent("Liquid Type"));
                EditorGUILayout.PropertyField(fillMappingProp, new GUIContent("Fill Mapping"), true);
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.HelpBox("Okreœl typ p³ynu i zmapuj pusty pojemnik na jego nape³nion¹ wersjê.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.PickupARenewableItem:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.PropertyField(itemPrefab, new GUIContent("Item to Drop From UI"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.PropertyField(localizeItemName, new GUIContent("Localize Item Name"));
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.AlchemyStation:
                EditorGUILayout.PropertyField(cauldronStation, new GUIContent("Cauldron"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                EditorGUILayout.PropertyField(localizationTwoString, new GUIContent("localization Two String"));
                EditorGUILayout.HelpBox(".", MessageType.Info);
                break;
            case InteractableItem.InteractableType.ReadableAndInteractableItem:
                EditorGUILayout.PropertyField(readableAndInteractableTextData, new GUIContent("Readable Text Data"));
                EditorGUILayout.PropertyField(scroll_BigAcidPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallAngryTimePotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_ElderBadMoodPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_OpenBloodPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_BigGoodSoup, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallHolyCowPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_ElderLeafGoods, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_OpenNiceWater, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_BigWaterPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallWinePotion, new GUIContent(""));
                break;
            case InteractableItem.InteractableType.PlaceRecipe:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Items ID"));
                EditorGUILayout.PropertyField(readableAndInteractableTextData, new GUIContent("Readable And Interactable TextData"));
                EditorGUILayout.PropertyField(scroll_BigAcidPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallAngryTimePotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_ElderBadMoodPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_OpenBloodPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_BigGoodSoup, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallHolyCowPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_ElderLeafGoods, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_OpenNiceWater, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_BigWaterPotion, new GUIContent(""));
                EditorGUILayout.PropertyField(scroll_SmallWinePotion, new GUIContent(""));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Box Collider"));
                EditorGUILayout.PropertyField(recipesCounter, new GUIContent("Recipes Counter"));
                EditorGUILayout.PropertyField(localizationString, new GUIContent("Localization String"));
                break;
            case InteractableItem.InteractableType.GetObject:
                EditorGUILayout.PropertyField(itemIdbyItemId, new GUIContent("Required Items ID by Item Id"));
                break;
            case InteractableItem.InteractableType.Crafting:
                EditorGUILayout.PropertyField(metalCrabs, new GUIContent("Metal Crabs Game Object"));
                EditorGUILayout.PropertyField(springs, new GUIContent("Springs Game Object"));
                EditorGUILayout.PropertyField(metalscaffolds, new GUIContent("Metalscaffolds Game Object"));
                EditorGUILayout.PropertyField(collectibleGears, new GUIContent("Collectible Gears Game Object"));
                EditorGUILayout.PropertyField(cables, new GUIContent("Cables Game Object"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Box Collider"));
                EditorGUILayout.PropertyField(workbench, new GUIContent("workbench"));
                break;
            case InteractableItem.InteractableType.PinNumber:
                EditorGUILayout.PropertyField(pinNumber, new GUIContent("Pin Number"));
                EditorGUILayout.PropertyField(rend, new GUIContent("Render"));
                EditorGUILayout.PropertyField(highlightColor, new GUIContent("HighlightColor"));
                break;
            case InteractableItem.InteractableType.RotateStatue:
                EditorGUILayout.PropertyField(rotationDuration, new GUIContent("Rotation Duration"));
                EditorGUILayout.PropertyField(statueCompasPuzzle, new GUIContent("Statue Compas Puzzle"));
                EditorGUILayout.PropertyField(linkedReadable, new GUIContent("Linked Readable"));
                EditorGUILayout.PropertyField(directionTextSet, new GUIContent("Direction Text Set"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Book Collider"));
                break;
            case InteractableItem.InteractableType.Cryptex:
                EditorGUILayout.PropertyField(cryptexRotateDuration, new GUIContent("Cryptex Rotate Duration"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Book Collider"));
                EditorGUILayout.PropertyField(isShorterCryptexSound, new GUIContent("Is Shorter Cryptex Sound"));
                break;
            case InteractableItem.InteractableType.MirrorMode:
                EditorGUILayout.PropertyField(mirror, new GUIContent("Rirror"));
                break;
            case InteractableItem.InteractableType.GearLockMode:
                EditorGUILayout.PropertyField(gearLockMode, new GUIContent("Gear Lock Mode"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Box Collider"));
                break;
            case InteractableItem.InteractableType.RotateGear:
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Box Collider"));
                EditorGUILayout.PropertyField(gearRotateDuration, new GUIContent("Gear Rotate Duration"));
                EditorGUILayout.PropertyField(gearLockMode, new GUIContent("Gear Lock Mode"));
                break;
            case InteractableItem.InteractableType.RotateGear90:
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Box Collider"));
                EditorGUILayout.PropertyField(gear90RotateDuration, new GUIContent("Gear90 Rotate Duration"));
                EditorGUILayout.PropertyField(pipeGearPuzzle, new GUIContent("Pipe Gear Puzzle"));
                EditorGUILayout.PropertyField(currentGear90Index, new GUIContent("currentGear90Index"));
                break;
            case InteractableItem.InteractableType.PipeGearPuzzle:
                EditorGUILayout.PropertyField(pipeGearPuzzle, new GUIContent("Pipe Gear Puzzle"));
                break;

            case InteractableItem.InteractableType.Furniture:
                EditorGUILayout.PropertyField(movableBlock, new GUIContent("Movable Block"));
                break;

            case InteractableItem.InteractableType.WoodenBlockPuzzle:
                EditorGUILayout.PropertyField(woodenBlockPuzzle, new GUIContent("Wooden Block Puzzle"));
                break;

            case InteractableItem.InteractableType.WoodenBlock:
                EditorGUILayout.PropertyField(zOffset, new GUIContent("Z Offset"));
                EditorGUILayout.PropertyField(duration, new GUIContent("Duration"));
                EditorGUILayout.PropertyField(highlightColor, new GUIContent("Highlight Color"));
                EditorGUILayout.PropertyField(rend, new GUIContent("Renderer"));
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("id"));
                break;

            case InteractableItem.InteractableType.TrianglePuzzle:
                EditorGUILayout.PropertyField(triangleEnum, new GUIContent("Triangle Enum"));
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(puzzleManager, new GUIContent("Puzzle Manager"));
                break;

            case InteractableItem.InteractableType.SymbolPlaceable:
                EditorGUILayout.PropertyField(shrine, new GUIContent("Shrine"));
                EditorGUILayout.PropertyField(pillar, new GUIContent("Pillar"));
                EditorGUILayout.PropertyField(grave, new GUIContent("Grave"));
                EditorGUILayout.PropertyField(brokenPillar, new GUIContent("Broken Pillar"));
                EditorGUILayout.PropertyField(woodenSword, new GUIContent("Wooden Sword"));
                EditorGUILayout.PropertyField(clockSymbolsManager, new GUIContent("Clock Symbols Manager"));
                break;

            case InteractableItem.InteractableType.ArrowUp:
                EditorGUILayout.PropertyField(objectA, new GUIContent("Object A"));
                EditorGUILayout.PropertyField(objectB, new GUIContent("Object B"));
                EditorGUILayout.PropertyField(moveDistanceMovingBlockPuzzle, new GUIContent("moveDistanceMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(moveSpeedMovingBlockPuzzle, new GUIContent("moveSpeedMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(obstacleMask, new GUIContent("obstacleMask"));
                EditorGUILayout.PropertyField(rayLengthMovingBlockPuzzle, new GUIContent("rayLengthMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(movingBlockPuzzleManager, new GUIContent("movingBlockPuzzleManager"));
                break;
            case InteractableItem.InteractableType.ArrowDown:
                EditorGUILayout.PropertyField(objectA, new GUIContent("Object A"));
                EditorGUILayout.PropertyField(objectB, new GUIContent("Object B"));
                EditorGUILayout.PropertyField(moveDistanceMovingBlockPuzzle, new GUIContent("moveDistanceMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(moveSpeedMovingBlockPuzzle, new GUIContent("moveSpeedMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(obstacleMask, new GUIContent("obstacleMask"));
                EditorGUILayout.PropertyField(rayLengthMovingBlockPuzzle, new GUIContent("rayLengthMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(movingBlockPuzzleManager, new GUIContent("movingBlockPuzzleManager"));
                break;
            case InteractableItem.InteractableType.ArrowLeft:
                EditorGUILayout.PropertyField(objectA, new GUIContent("Object A"));
                EditorGUILayout.PropertyField(objectB, new GUIContent("Object B"));
                EditorGUILayout.PropertyField(moveDistanceMovingBlockPuzzle, new GUIContent("moveDistanceMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(moveSpeedMovingBlockPuzzle, new GUIContent("moveSpeedMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(obstacleMask, new GUIContent("obstacleMask"));
                EditorGUILayout.PropertyField(rayLengthMovingBlockPuzzle, new GUIContent("rayLengthMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(movingBlockPuzzleManager, new GUIContent("movingBlockPuzzleManager"));
                break;
            case InteractableItem.InteractableType.ArrowRight:
                EditorGUILayout.PropertyField(objectA, new GUIContent("Object A"));
                EditorGUILayout.PropertyField(objectB, new GUIContent("Object B"));
                EditorGUILayout.PropertyField(moveDistanceMovingBlockPuzzle, new GUIContent("moveDistanceMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(moveSpeedMovingBlockPuzzle, new GUIContent("moveSpeedMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(obstacleMask, new GUIContent("obstacleMask"));
                EditorGUILayout.PropertyField(rayLengthMovingBlockPuzzle, new GUIContent("rayLengthMovingBlockPuzzle"));
                EditorGUILayout.PropertyField(movingBlockPuzzleManager, new GUIContent("movingBlockPuzzleManager"));
                break;
            case InteractableItem.InteractableType.PuzzlePart:
                EditorGUILayout.PropertyField(movingBlockPuzzleManager, new GUIContent("movingBlockPuzzleManager"));
                break;
            case InteractableItem.InteractableType.PressWoodenButton:
                EditorGUILayout.PropertyField(blockPanel, new GUIContent("Block Panel"));
                EditorGUILayout.PropertyField(ninePadPanelManager, new GUIContent("NinePadPanelManager"));
                EditorGUILayout.PropertyField(highlightColor, new GUIContent("Highlight Color"));
                EditorGUILayout.PropertyField(rend, new GUIContent("Renderer"));
                break;
            case InteractableItem.InteractableType.NinePadPanelPuzzle:
                EditorGUILayout.PropertyField(ninePadPanelManager, new GUIContent("Nine Pad Panel Manager"));
                break;
            case InteractableItem.InteractableType.CircleAndSquarePuzzle:
                EditorGUILayout.PropertyField(circleAndSquarePuzzle, new GUIContent("Circle And Square Puzzle"));
                break;
            case InteractableItem.InteractableType.PlayerSphereMovement:
                EditorGUILayout.PropertyField(playerPathMovement, new GUIContent("Player Path Movement"));
                break;
            case InteractableItem.InteractableType.LibraryButton:
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("animator"));
                EditorGUILayout.PropertyField(libraryButtonsManager, new GUIContent("Library Buttons Manager"));
                EditorGUILayout.PropertyField(libraryButtonInt, new GUIContent("Library Button Int"));
                break;
            case InteractableItem.InteractableType.Safe:
                EditorGUILayout.PropertyField(safePuzzle, new GUIContent("SafePuzzle"));
                break;
            case InteractableItem.InteractableType.Braiser:
                EditorGUILayout.PropertyField(braiserPuzzle, new GUIContent("Braiser Puzzle"));
                EditorGUILayout.PropertyField(braiserInt, new GUIContent("Braiser Int"));
                break;
            case InteractableItem.InteractableType.PuzzleFrame:
                EditorGUILayout.PropertyField(swordPuzzlePiece, new GUIContent("Sword Puzzle Piece"));
                EditorGUILayout.PropertyField(clubPuzzlePiece, new GUIContent("Club Puzzle Piece"));
                EditorGUILayout.PropertyField(framePuzzlemanager, new GUIContent("Frame Puzzle Manager"));
                EditorGUILayout.PropertyField(skeletonSwordPiece, new GUIContent("Skeleton Sword Piece"));
                EditorGUILayout.PropertyField(skeletonWarAxePiece, new GUIContent("Skeleton War Axe Piece"));
                EditorGUILayout.PropertyField(skeletonHelmetPiece, new GUIContent("Skeleton Helmet Piece"));
                EditorGUILayout.PropertyField(skeletonFullHelmetPiece, new GUIContent("Skeleton Full Helmet Piece"));
                break;
            case InteractableItem.InteractableType.WallSwitchOnOff:
                EditorGUILayout.PropertyField(wallSwitchOnOffManager, new GUIContent("Wall Switch On Off Manager"));
                EditorGUILayout.PropertyField(switchType, new GUIContent("Switch Type"));
                break;
            case InteractableItem.InteractableType.PlaceOnScale:
                EditorGUILayout.PropertyField(weight, new GUIContent("Weight"));
                EditorGUILayout.PropertyField(mainBottle, new GUIContent("Main Bottle"));
                EditorGUILayout.PropertyField(mainMug, new GUIContent("Main Mug"));
                EditorGUILayout.PropertyField(nail, new GUIContent("Nail"));
                EditorGUILayout.PropertyField(hatch, new GUIContent("Hatch"));
                EditorGUILayout.PropertyField(scaleWeightSystem, new GUIContent("Scale Weight System"));
                break;
            case InteractableItem.InteractableType.Briefcase:
                EditorGUILayout.PropertyField(briefcaseManager, new GUIContent("Briefcase Manager"));
                break;
            case InteractableItem.InteractableType.MovingBlockBriefcase:
                EditorGUILayout.PropertyField(movingBlockBriefcase, new GUIContent("Moving Block Briefcase"));
                break;
            case InteractableItem.InteractableType.MovePainting:
                EditorGUILayout.PropertyField(imageSlider, new GUIContent("Image Slider"));
                break;

            case InteractableItem.InteractableType.PlacePillarSymbol:
                EditorGUILayout.PropertyField(symbol0, new GUIContent("symbol0"));
                EditorGUILayout.PropertyField(symbol1, new GUIContent("symbol1"));
                EditorGUILayout.PropertyField(symbol2, new GUIContent("symbol2"));
                break;

            case InteractableItem.InteractableType.ClickMovePillarSphere:
                EditorGUILayout.PropertyField(movingPillarManager, new GUIContent("Moving Pillar Manager"));
                break;

            case InteractableItem.InteractableType.RotatingPillar:
                EditorGUILayout.PropertyField(rotatingPillarMovingBlock, new GUIContent("Rotating Pillar Moving Block"));          
                break;

            case InteractableItem.InteractableType.MoveSphereOnePillarPuzzle:
                EditorGUILayout.PropertyField(moveSphereOnePillarPuzzle, new GUIContent("Move Sphere One Pillar Puzzle"));
                break;
            case InteractableItem.InteractableType.RotateOnePillar:
                 EditorGUILayout.PropertyField(moveSphereOnePillarPuzzle, new GUIContent("Move Sphere One Pillar Puzzle"));
                break;

            case InteractableItem.InteractableType.PictureTerrainObject:
                EditorGUILayout.PropertyField(pictureTerrainMovingObject, new GUIContent("Picture Terrain Moving Object"));
                break;

            case InteractableItem.InteractableType.CoverAllSquarePuzzle:
                EditorGUILayout.PropertyField(coverAllSquarePuzzlePillar, new GUIContent("Cover All Square Puzzle Pillar"));
                break;
            case InteractableItem.InteractableType.CorrectSixteenSymbols:
                EditorGUILayout.PropertyField(correctSixteenSymbolsPillar, new GUIContent("Correct Sixteen Symbols Pillar"));
                break;
                
        }

        serializedObject.ApplyModifiedProperties();
    }
}