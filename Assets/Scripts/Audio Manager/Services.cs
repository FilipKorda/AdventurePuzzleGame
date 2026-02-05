public static class Services
{
    public static IAudioService Audio { get; private set; }

    public static void RegisterAudio(IAudioService audio)
    {
        Audio = audio;
    }
}
