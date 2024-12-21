using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractableItem))]
public class InteractableItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteractableItem item = (InteractableItem)target;

        item.interactableType = (InteractableItem.InteractableType)EditorGUILayout.EnumPopup("Interactable Type", item.interactableType);

        // Wyœwietl zmienne w zale¿noœci od wybranego typu
        switch (item.interactableType)
        {
            case InteractableItem.InteractableType.Pickupable:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemName"), new GUIContent("Item Name"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("itemSprite"), new GUIContent("Item Sprite"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredItemId"), new GUIContent("Required Item ID"));
                break;

            case InteractableItem.InteractableType.Openable:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("requiredItemId"), new GUIContent("Required Item ID"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"), new GUIContent("Animator"));
                break;

            case InteractableItem.InteractableType.Throwable:
                EditorGUILayout.HelpBox("No properties to display for Throwable.", MessageType.Info);
                break;

            case InteractableItem.InteractableType.Readable:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("readableTextData"), new GUIContent("Readable Text Data"));
                break;
        }

        // Zapisanie zmian w obiekcie
        serializedObject.ApplyModifiedProperties();
    }
}
