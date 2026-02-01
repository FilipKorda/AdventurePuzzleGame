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
    private SerializedProperty requiredItemIds;
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

    private void OnEnable()
    {
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
        requiredItemIds = serializedObject.FindProperty("requiredItemIds");
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
    }

    public override void OnInspectorGUI()
    {
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
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Openable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item IDs"));
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(canOpenWithNoSelectedItem, new GUIContent("Bool Can Open With seleted Item"));
                EditorGUILayout.PropertyField(endPanel, new GUIContent("End Panel"));
                EditorGUILayout.PropertyField(isEndPanel, new GUIContent("Is End Panel"));
                EditorGUILayout.HelpBox("Wymaga przedmiotu (lub przedmiotów) o podanych ID, aby mo¿na by³o go otworzyæ.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Throwable:
                EditorGUILayout.HelpBox("Ten obiekt wymaga komponentu Rigidbody, aby mo¿na by³o nim rzuciæ.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Readable:
                EditorGUILayout.PropertyField(readableTextDataProp, new GUIContent("Readable Text Data"));
                break;

            case InteractableItem.InteractableType.Pressable:
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(onAllWallButtonPressedProp, new GUIContent("On Pressed Event"));
                break;

            case InteractableItem.InteractableType.Placeable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item ID"));
                EditorGUILayout.PropertyField(objectToPlaceProp, new GUIContent("Object To Place"));
                EditorGUILayout.PropertyField(boxColliderProp, new GUIContent("Interaction Collider"));
                EditorGUILayout.HelpBox("Wymaga przedmiotu o podanym ID, aby umieœciæ obiekt w œwiecie.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.LockPick:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.HelpBox("Potrzebujesz tego itemu o tym ID ¿eby otowrzyæ k³ótke", MessageType.Info);
                break;
            case InteractableItem.InteractableType.Fillable:
                EditorGUILayout.PropertyField(providedLiquidTypeProp, new GUIContent("Liquid Type"));
                EditorGUILayout.PropertyField(fillMappingProp, new GUIContent("Fill Mapping"), true);
                EditorGUILayout.HelpBox("Okreœl typ p³ynu i zmapuj pusty pojemnik na jego nape³nion¹ wersjê.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.PickupARenewableItem:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.PropertyField(itemPrefab, new GUIContent("Item to Drop From UI"));
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;
            case InteractableItem.InteractableType.AlchemyStation:
                EditorGUILayout.PropertyField(cauldronStation, new GUIContent("Cauldron"));
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
                EditorGUILayout.PropertyField(requiredItemIds, new GUIContent("Required Items ID"));
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
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}