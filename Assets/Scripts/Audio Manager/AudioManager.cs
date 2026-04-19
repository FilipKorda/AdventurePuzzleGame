using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IAudioService
{
    static AudioManager instance;

    AudioSource musicSource;
    AudioSource sfxSource;

    [SerializeField] SoundData[] sounds;
    [SerializeField] GameObject sfxSourcePrefab;

    Dictionary<string, AudioClip> soundMap;
    Dictionary<string, ProximityAudio> activeLoopSources = new();

    List<AudioClip> footstepClips;

    const string MusicVolumeKey = "MusicVolume";
    const string SFXVolumeKey = "SFXVolume";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureSources();
        EnsureSoundMap();

        musicSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        sfxSource.volume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

        Services.RegisterAudio(this);
    }

    private void Start()
    {
        PlayMusic("Music_1");
    }

    void EnsureSources()
    {
        if (musicSource != null && sfxSource != null) return;

        var sources = GetComponents<AudioSource>();

        if (sources.Length >= 2)
        {
            musicSource = sources[0];
            sfxSource = sources[1];
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
    }

    void EnsureSoundMap()
    {
        if (soundMap != null) return;

        soundMap = new Dictionary<string, AudioClip>();
        footstepClips = new List<AudioClip>();
        foreach (var s in sounds)
        {
            soundMap[s.Id] = s.Clip;

            if (s.Category == SoundCategory.Footstep)
                footstepClips.Add(s.Clip);
        }
    }

    public void PlayMusic(string id)
    {
        EnsureSources();
        EnsureSoundMap();

        if (!soundMap.TryGetValue(id, out var clip)) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        EnsureSources();
        musicSource.Stop();
    }

    public void PlaySFX(string id)
    {
        EnsureSources();
        EnsureSoundMap();

        if (soundMap.TryGetValue(id, out var clip))
            sfxSource.PlayOneShot(clip);
    }

    public void PlayOnLoopSFX(string id)
    {
        EnsureSoundMap();
        EnsureSources();

        if (!soundMap.TryGetValue(id, out var clip)) return;
        if (sfxSourcePrefab == null) return;

        if (activeLoopSources.TryGetValue(id, out var existing) && existing != null)
            return;

        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;

        var go = Instantiate(sfxSourcePrefab, player.position, Quaternion.identity);
        var prox = go.GetComponent<ProximityAudio>();
        var src = go.GetComponent<AudioSource>();

        if (prox == null || src == null)
        {
            Destroy(go);
            return;
        }

        src.clip = clip;
        src.loop = true;
        src.volume = sfxSource.volume;
        src.Play();

        prox.Initialize(player, 2f, 15f, 5f, 0f);
        activeLoopSources[id] = prox;
    }

    public void SetMusicVolume(float value)
    {
        EnsureSources();
        musicSource.volume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
    }

    public void SetSFXVolume(float value)
    {
        EnsureSources();
        sfxSource.volume = value;
        PlayerPrefs.SetFloat(SFXVolumeKey, value);

        foreach (var loop in activeLoopSources.Values)
            if (loop != null)
                loop.AudioSource.volume = value;
    }


    public void StopLoopSFX(string id)
    {
        if (!activeLoopSources.TryGetValue(id, out var prox)) return;
        if (prox == null) return;

        Destroy(prox.gameObject);
        activeLoopSources.Remove(id);
    }


    public float GetMusicVolume()
    {
        EnsureSources();
        return musicSource.volume;
    }

    public float GetSFXVolume()
    {
        EnsureSources();
        return sfxSource.volume;
    }

    public void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Count == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Count)];

        sfxSource.pitch = Random.Range(0.97f, 1.02f);
        sfxSource.volume = GetSFXVolume();
        sfxSource.PlayOneShot(clip);
    }
}