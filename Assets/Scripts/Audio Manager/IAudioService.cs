public interface IAudioService
{
    void PlaySFX(string id);
    void PlayOnLoopSFX(string id);
    void PlayMusic(string id);
    void StopMusic();
}
