using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    IEnumerator Start()
    {
        while (Services.Audio == null)
            yield return null;

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
