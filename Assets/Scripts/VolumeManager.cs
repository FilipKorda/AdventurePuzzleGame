using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        musicSlider.value = Services.Audio.GetMusicVolume();
        sfxSlider.value = Services.Audio.GetSFXVolume();

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        Services.Audio.SetMusicVolume(value);
    }

    void OnSFXVolumeChanged(float value)
    {
        Services.Audio.SetSFXVolume(value);
    }
}
