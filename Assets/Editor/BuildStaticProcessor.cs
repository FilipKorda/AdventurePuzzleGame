#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildStaticProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("<color=orange>BuildProcessor: Rozpoczynanie buildu. Ustawianie Static na FALSE...</color>");
        SetStaticStatus(false);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("<color=green>BuildProcessor: Build zakończony. Przywracanie Static na TRUE...</color>");
        SetStaticStatus(true);
    }

    private void SetStaticStatus(bool isStatic)
    {
        DevStaticManager[] managers = Object.FindObjectsByType<DevStaticManager>(FindObjectsSortMode.None);

        if (managers.Length == 0)
        {
            Debug.LogWarning("BuildProcessor: Nie znaleziono żadnego obiektu DevStaticManager na scenie.");
            return;
        }

        foreach (var manager in managers)
        {
            if (manager.objectsToModify == null) continue;

            int count = 0;
            foreach (GameObject obj in manager.objectsToModify)
            {
                if (obj != null)
                {
                    obj.isStatic = isStatic;
                    count++;
                }
            }

            Debug.Log($"BuildProcessor: Zmieniono status Static na {isStatic.ToString().ToUpper()} dla {count} obiektów w managerze: {manager.name}");
        }

        if (!Application.isPlaying)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
#endif