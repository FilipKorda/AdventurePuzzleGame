using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IAudioService
{
    static AudioManager instance;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private SoundData[] sounds;

    [Header("Looped SFX Prefab")]
    [SerializeField] private GameObject sfxSourcePrefab;

    [SerializeField] private float loopInnerRadius = 2f;
    [SerializeField] private float loopOuterRadius = 15f;
    [SerializeField] private float loopFadeSpeed = 5f;
    [SerializeField] private float loopMinVolume = 0f;

    Dictionary<string, AudioClip> soundMap;
    Dictionary<string, ProximityAudio> activeLoopSources = new();

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

        Services.RegisterAudio(this);

        soundMap = new Dictionary<string, AudioClip>();
        foreach (var sound in sounds)
            soundMap[sound.Id] = sound.Clip;

        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        PlayMusic("Music_1");
    }

    public void PlaySFX(string id)
    {
        if (soundMap.TryGetValue(id, out var clip))
            sfxSource.PlayOneShot(clip);
    }

    public void PlayOnLoopSFX(string id)
    {
        if (!soundMap.TryGetValue(id, out var clip)) return;
        if (sfxSourcePrefab == null) return;

        if (activeLoopSources.TryGetValue(id, out var existing) && existing != null)
        {
            if (!existing.AudioSource.isPlaying)
                existing.AudioSource.Play();
            return;
        }

        var player = PlayerLocator.PlayerTransform;
        if (player == null) return;
        var go = Instantiate(sfxSourcePrefab, player.position, Quaternion.identity);
        var prox = go.GetComponent<ProximityAudio>();
        var src = go.GetComponent<AudioSource>();

        src.clip = clip;
        src.loop = true;
        src.playOnAwake = false;
        src.volume = sfxSource.volume;
        src.Play();

        prox.Initialize(player, loopInnerRadius, loopOuterRadius, loopFadeSpeed, loopMinVolume);


        activeLoopSources[id] = prox;
    }

    public void PlayMusic(string id)
    {
        if (!soundMap.TryGetValue(id, out var clip)) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopLoopSFX(string id)
    {
        if (activeLoopSources.TryGetValue(id, out var prox) && prox != null)
        {
            prox.StopAndDestroy();
            activeLoopSources.Remove(id);
        }
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
        PlayerPrefs.SetFloat(SFXVolumeKey, value);

        foreach (var loop in activeLoopSources.Values)
            if (loop != null)
                loop.AudioSource.volume = value;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }
}
