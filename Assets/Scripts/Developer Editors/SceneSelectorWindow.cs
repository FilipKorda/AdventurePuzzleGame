using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelectorWindow : EditorWindow
{
    [MenuItem("Dev Tools/Scene Selector")]
    static void Open()
    {
        GetWindow<SceneSelectorWindow>("Scene Selector");
    }

    void OnGUI()
    {
        GUILayout.Space(10);

        if (GUILayout.Button("Main Menu", GUILayout.Height(40)))
        {
            OpenScene("MainMenu");
        }

        if (GUILayout.Button("Gameplay", GUILayout.Height(40)))
        {
            OpenScene("Gameplay");
        }
    }

    void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}.unity");
        }
    }
}