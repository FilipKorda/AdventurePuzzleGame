public interface IAudioService
{
    void PlaySFX(string id);
    void PlayMusic(string id);
    void StopMusic();

    void PlayOnLoopSFX(string id);
    void SetMusicVolume(float value);
    void SetSFXVolume(float value);

    float GetMusicVolume();
    float GetSFXVolume();
}
