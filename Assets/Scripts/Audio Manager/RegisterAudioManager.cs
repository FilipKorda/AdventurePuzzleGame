using UnityEngine;

public class RegisterAudioManager : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    void Awake()
    {
        Services.RegisterAudio(audioManager);
    }
}
