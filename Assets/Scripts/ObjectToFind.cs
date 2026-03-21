using UnityEngine;

public class ObjectToFind : MonoBehaviour
{
    public Color baseColor = Color.white;
    public Color freeColor = Color.green;

    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = freeColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        rend.material.color = baseColor;
    }

    private void OnTriggerExit(Collider other)
    {
        rend.material.color = freeColor;
    }
}