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
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Openable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item IDs"));
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
                EditorGUILayout.PropertyField(canOpenWithNoSelectedItem, new GUIContent("Bool Can Open With seleted Item"));
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
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}