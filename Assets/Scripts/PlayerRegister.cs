using UnityEngine;

public class PlayerRegister : MonoBehaviour
{
    void Awake()
    {
        PlayerLocator.Register(transform);
    }

    void OnDestroy()
    {
        PlayerLocator.Unregister(transform);
    }
}
