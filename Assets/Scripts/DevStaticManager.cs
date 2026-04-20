using System.Collections.Generic;
using UnityEngine;

public class DevStaticManager : MonoBehaviour
{
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