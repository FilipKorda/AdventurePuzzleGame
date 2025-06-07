using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableItem))]
public class InteractableItemEditor : Editor
{
    // Zmienne do przechowywania referencji do serializowanych w³aœciwoœci
    private SerializedProperty interactableTypeProp;
    private SerializedProperty itemNameProp;
    private SerializedProperty itemSpriteProp;
    private SerializedProperty itemIdProp; // Nowa w³aœciwoœæ
    private SerializedProperty requiredItemIdsProp; // Nowa w³aœciwoœæ (tablica)
    private SerializedProperty animatorProp;
    private SerializedProperty readableTextDataProp;
    private SerializedProperty onAllWallButtonPressedProp;
    private SerializedProperty objectToPlaceProp;
    private SerializedProperty boxColliderProp;

    private void OnEnable()
    {
        // ZnajdŸ wszystkie w³aœciwoœci raz, przy w³¹czeniu inspektora
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
    }

    public override void OnInspectorGUI()
    {
        // Zawsze zaczynaj od tej linii
        serializedObject.Update();

        // Wyœwietl pole wyboru typu interakcji
        EditorGUILayout.PropertyField(interactableTypeProp, new GUIContent("Interactable Type"));

        // Pobierz aktualnie wybrany typ, ¿eby u¿yæ go w switch
        InteractableItem.InteractableType currentType = (InteractableItem.InteractableType)interactableTypeProp.enumValueIndex;

        // Zawsze pokazuj nazwê i sprite - przydatne do wyœwietlania podpowiedzi w grze (np. "Otwórz Drzwi")
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("General Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(itemNameProp, new GUIContent("Item Name"));
        EditorGUILayout.PropertyField(itemSpriteProp, new GUIContent("UI Sprite (optional)"));
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Type-Specific Properties", EditorStyles.boldLabel);

        // Wyœwietl zmienne w zale¿noœci od wybranego typu
        switch (currentType)
        {
            case InteractableItem.InteractableType.Pickupable:
                EditorGUILayout.PropertyField(itemIdProp, new GUIContent("Item ID"));
                EditorGUILayout.HelpBox("To jest ID tego przedmiotu, gdy znajdzie siê w ekwipunku.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Openable:
                EditorGUILayout.PropertyField(requiredItemIdsProp, new GUIContent("Required Item IDs"));
                EditorGUILayout.PropertyField(animatorProp, new GUIContent("Animator"));
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
        }

        // Zawsze koñcz t¹ lini¹, aby zapisaæ zmiany
        serializedObject.ApplyModifiedProperties();
    }
}