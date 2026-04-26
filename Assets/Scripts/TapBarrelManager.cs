using System.Collections;
using UnityEngine;

public class TapBarrelManager : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour player;

    [SerializeField] private ParticleSystem waterParticle;

    [SerializeField] private Transform gear;
    [SerializeField] private GameObject bucket;

    [SerializeField] private float gearRotateDuration = 0.5f;

    private void Awake()
    {
        waterParticle.Stop();
        bucket.SetActive(false);
    }

    public void FillBucketActivator()
    {
        StartCoroutine(CorutinaFillBucket());
    }

    private IEnumerator CorutinaFillBucket()
    {
        player.disablePlayer = true;
        bucket.SetActive(true);
        yield return RotateGear(180f, gearRotateDuration); 

        waterParticle.Play();
        Services.Audio.PlaySFX("PourWater");

        yield return new WaitForSeconds(1f);
        waterParticle.Stop();
        bucket.SetActive(false);

        yield return RotateGear(-180f, gearRotateDuration);
        yield return new WaitForSeconds(0.2f);
        player.disablePlayer = false;

    }

    private IEnumerator RotateGear(float yAngle, float duration)
    {
        float time = 0f;
        float rotatedAngle = 0f;

        while (time < duration)
        {
            float deltaTime = Time.deltaTime;
            time += deltaTime;

            float targetRotatedAngle = Mathf.Lerp(0f, yAngle, Mathf.Clamp01(time / duration));
            float deltaAngle = targetRotatedAngle - rotatedAngle;
            rotatedAngle = targetRotatedAngle;

            gear.Rotate(0f, deltaAngle, 0f, Space.Self);

            yield return null;
        }

        float finalDelta = yAngle - rotatedAngle;
        gear.Rotate(0f, finalDelta, 0f, Space.Self);
    }

}
