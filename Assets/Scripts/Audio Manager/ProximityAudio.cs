using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximityAudio : MonoBehaviour
{
    public AudioSource AudioSource { get; private set; }

    private Transform player;
    private float innerRadius = 2f;
    private float outerRadius = 15f;
    private float fadeSpeed = 5f;
    private float minVolume = 0f;
    private float baseVolume = 1f;
    private float currentVolume = 0f;

    private bool initialized = false;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        baseVolume = AudioSource != null ? AudioSource.volume : 1f;
        currentVolume = baseVolume;
    }

    public void Initialize(Transform playerTransform, float inner, float outer, float fadeS, float minVol)
    {
        player = playerTransform;
        innerRadius = Mathf.Max(0f, inner);
        outerRadius = Mathf.Max(innerRadius + 0.01f, outer);
        fadeSpeed = Mathf.Max(0.01f, fadeS);
        minVolume = Mathf.Clamp01(minVol);
        baseVolume = AudioSource != null ? AudioSource.volume : 1f;
        currentVolume = baseVolume;
        initialized = true;
    }

    private void Update()
    {
        if (!initialized) return;
        if (AudioSource == null) return;

        float target = 1f;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= innerRadius) target = 1f;
        else if (dist >= outerRadius) target = minVolume;
        else
        {
            float t = (dist - innerRadius) / (outerRadius - innerRadius);
            target = Mathf.Lerp(1f, minVolume, t);
        }

        float targetVolume = baseVolume * target;
        currentVolume = Mathf.MoveTowards(currentVolume, targetVolume, fadeSpeed * Time.deltaTime);
        AudioSource.volume = currentVolume;
    }

    public void StopAndDestroy()
    {
        if (AudioSource != null)
            AudioSource.Stop();
        Destroy(gameObject);
    }
}
