public interface IAudioService
{
    void PlaySFX(string id);
    void PlayMusic(string id);
    void StopMusic();

    void PlayOnLoopSFX(string id);
    void SetMusicVolume(float value);
    void SetSFXVolume(float value);

    void StopLoopSFX(string id);

    float GetMusicVolume();
    float GetSFXVolume();
}
