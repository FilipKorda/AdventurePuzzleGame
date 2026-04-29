using System.Collections.Generic;
using UnityEngine;

public class DevStaticManager : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform playerStartPosition;

    [Header("Obiekty do zmiany statusu Static na false podczas buildu")]
    public List<GameObject> objectsToModify;

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
#endif
}