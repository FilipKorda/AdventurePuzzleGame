#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

public class DevStaticManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerStartPosition;

    [Header("Obiekty do zmiany statusu Static na false podczas buildu")]
    public List<GameObject> objectsToModify;


    public OpenDoorDeveloper[] openDoorDevelopers;

    [ContextMenu("Ustaw wszystko na Static = TRUE")]
    public void SetStaticTrue()
    {
        SetAllStatic(true);
    }

    [ContextMenu("Ustaw wszystko na Static = FALSE")]
    public void SetStaticFalse()
    {
        SetAllStatic(false);
    }

    [ContextMenu("Open Door = FALSE")]
    public void OpenDoorFalse()
    {
        foreach (var door in openDoorDevelopers)
        {
            door.openDoor = false;
        }
    }

    [ContextMenu("Open Door = TRUE")]
    public void OpenDoorTrue()
    {
        foreach (var door in openDoorDevelopers)
        {
            door.openDoor = true;
        }
    }

    [ContextMenu("Set Player Start Position")]
    public void SetPlayerStartPosition()
    {
        playerTransform.SetPositionAndRotation(playerStartPosition.position, playerStartPosition.rotation);
    }

    public void SetAllStatic(bool value)
    {
        int count = 0;
        foreach (var obj in objectsToModify)
        {
            if (obj != null)
            {
                obj.isStatic = value;
                count++;
            }
        }

        Debug.Log($"[DevStaticManager] Zmieniono {count} obiektów na Static = {value}");
    }

}
#endif