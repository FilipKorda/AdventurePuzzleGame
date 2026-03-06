using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FinalLaserHitReactable : MonoBehaviour, ILaserReactable
{
    public LaserHitType hitType;

    private bool isActive = false;
    private Color baseEmission;

    public float intensity = 5f;

    public Animator animator;
    public Mirror[] mirror;
    public BoxCollider[] boxColliders;
    public LaserBeam laserBeam;

    private void Awake()
    {
        var renderer = GetComponent<MeshRenderer>();
        baseEmission = renderer.material.GetColor("_EmissionColor");
    }

    private IEnumerator DisableLaserAfterSecond()
    {
        yield return new WaitForSeconds(2f);
        laserBeam.activeLaser = false;

    }

    private void DisableAllMirrorsAndColliders()
    {
        StartCoroutine(DisableLaserAfterSecond());

        foreach (var mirror in mirror)
        {
            mirror.DisableControl();
        }
        foreach (var collider in boxColliders)
        {
            collider.enabled = false;
        }

    }

    public void OnLaserEnter()
    {
        if (!isActive)
        {
            isActive = true;
            var renderer = GetComponent<MeshRenderer>();
            renderer.material.SetColor("_EmissionColor", baseEmission * intensity);

            PlayHitSound();


            if (animator != null)
            {
                animator.SetTrigger("Open");
                DisableAllMirrorsAndColliders();
            }
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
