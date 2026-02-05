using UnityEngine;

public class AnimationAudioEvent : MonoBehaviour
{
    public void PlaySFX(string id)
    {
        Services.Audio.PlaySFX(id);
    }

    public void PlayMusic(string id)
    {
        Services.Audio.PlayMusic(id);
    }

    public void StopMusic()
    {
        Services.Audio.StopMusic();
    }
}
