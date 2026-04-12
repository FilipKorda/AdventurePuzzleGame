using UnityEngine;

public class Bell : MonoBehaviour
{
    [SerializeField] private int bellIndex;
    [SerializeField] private string sfxId;
    [SerializeField] private BellManager bellManager;

    public void PlayBell()
    {
        Services.Audio.PlaySFX(sfxId);
        bellManager.RegisterBell(bellIndex);
    }
}