using UnityEngine;

public class PuzzleBoardHandle : MonoBehaviour
{
    [SerializeField] private float centerZRotation = 72f;
    [SerializeField] private float amplitude = 3f;
    [SerializeField] private float speed = 1f;

    private bool isPlaying = false;
    private Vector3 startLocalRotation;

    private void Awake()
    {
        startLocalRotation = transform.localEulerAngles;
    }

    private void Update()
    {
        if (!isPlaying) return;

        Vector3 newRotation = startLocalRotation;
        newRotation.z = centerZRotation + Mathf.Sin(Time.time * speed) * amplitude;
        transform.localEulerAngles = newRotation;
    }

    public void PlayAnimation()
    {
        isPlaying = true;
    }

    public void StopAnimation()
    {
        isPlaying = false;
    }
}
