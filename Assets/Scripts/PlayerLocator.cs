using UnityEngine;

public static class PlayerLocator
{
    public static Transform PlayerTransform { get; private set; }

    public static void Register(Transform player)
    {
        PlayerTransform = player;
    }

    public static void Unregister(Transform player)
    {
        if (PlayerTransform == player)
            PlayerTransform = null;
    }
}
