using UnityEngine;

public class GameplaySessionMarker : MonoBehaviour
{
    private void Start()
    {
        BonusUnlockProgress.MarkGameAsPlayed();
    }
}
