using UnityEngine;

public class PlayMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip Clip;

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        sfxSource.clip = Clip;
        sfxSource.Play();
    }

}
