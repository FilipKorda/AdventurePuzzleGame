using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class LaserHitReactable : MonoBehaviour, ILaserReactable
{
    public LaserHitType hitType;

    private bool isActive = false;
    private Color baseEmission;

    public float intensity = 5f;

    private void Awake()
    {
        var renderer = GetComponent<MeshRenderer>();
        baseEmission = renderer.material.GetColor("_EmissionColor");
    }

    public void OnLaserEnter()
    {
        if (!isActive)
        {
            isActive = true;
            var renderer = GetComponent<MeshRenderer>();
            renderer.material.SetColor("_EmissionColor", baseEmission * intensity);

            PlayHitSound();        
        }
    }

    private void PlayHitSound()
    {
        switch (hitType)
        {
            case LaserHitType.cristal:
                Services.Audio.PlaySFX("CristalEnter");
                break;
            case LaserHitType.cristal2:
                Services.Audio.PlaySFX("Cristal2Enter");
                break;
            case LaserHitType.cristal3:
                Services.Audio.PlaySFX("Cristal3Enter");
                break;
        }
    }

    private void PlayReleaseSound()
    {
        switch (hitType)
        {
            case LaserHitType.cristal:
                Services.Audio.PlaySFX("CrystalRelease");
                break;
            case LaserHitType.cristal2:
                Services.Audio.PlaySFX("Crystal2Release");
                break;
            case LaserHitType.cristal3:
                Services.Audio.PlaySFX("Crystal3Release");
                break;
        }
    }

    public void OnLaserExit()
    {
        if (isActive)
        {
            isActive = false;
            var renderer = GetComponent<MeshRenderer>();
            renderer.material.SetColor("_EmissionColor", baseEmission);

            PlayReleaseSound();
        }
    }
}