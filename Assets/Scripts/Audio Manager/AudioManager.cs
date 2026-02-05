using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private SoundData[] sounds;

    [Header("Looped SFX Prefab (must contain AudioSource + ProximityAudio)")]
    [SerializeField] private GameObject sfxSourcePrefab;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private float loopInnerRadius = 2f;
    [SerializeField] private float loopOuterRadius = 15f;
    [SerializeField] private float loopFadeSpeed = 5f;
    [SerializeField] private float loopMinVolume = 0f;

    private Dictionary<string, AudioClip> soundMap;
    private Dictionary<string, ProximityAudio> activeLoopSources = new();

    void Awake()
    {
        soundMap = new Dictionary<string, AudioClip>();

        foreach (var sound in sounds)
            soundMap[sound.Id] = sound.Clip;
    }

    public void PlaySFX(string id)
    {
        if (soundMap.TryGetValue(id, out var clip))
            sfxSource.PlayOneShot(clip);
    }

    public void PlayOnLoopSFX(string id)
    {
        if (!soundMap.TryGetValue(id, out var clip)) return;
        if (sfxSourcePrefab == null)
        {
            Debug.LogWarning("AudioManager: sfxSourcePrefab nie jest przypisany. Nie mo¿na odtworzyæ loopuj¹cego SFX.");
            return;
        }

        if (activeLoopSources.TryGetValue(id, out var existing) && existing != null)
        {
            if (existing.AudioSource.clip == clip && !existing.AudioSource.isPlaying)
                existing.AudioSource.Play();
            return;
        }

        var go = Instantiate(sfxSourcePrefab, playerTransform.position, Quaternion.identity);
        var prox = go.GetComponent<ProximityAudio>();
        var src = go.GetComponent<AudioSource>();

        src.clip = clip;
        src.loop = true;
        src.playOnAwake = false;
        src.volume = sfxSource != null ? sfxSource.volume : 1f;
        src.Play();

        prox.Initialize(playerTransform, loopInnerRadius, loopOuterRadius, loopFadeSpeed, loopMinVolume);

        activeLoopSources[id] = prox;
    }

    public void PlayMusic(string id)
    {
        if (!soundMap.TryGetValue(id, out var clip)) return;

        musicSource.clip = clip;
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
}
