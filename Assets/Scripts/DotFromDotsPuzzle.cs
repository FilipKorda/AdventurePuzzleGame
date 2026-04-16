using UnityEngine;

public class DotFromDotsPuzzle : MonoBehaviour
{
    [SerializeField] private DotFromDotsPuzzle[] availableConnections;

    [Header("Visual")]
    [SerializeField] private Renderer childRenderer;
    [SerializeField] private Color selectedColor = Color.green;
    private Transform visualToScale;

    [Header("Pulse")]
    [SerializeField] private float pulseSpeed = 4f;
    private float pulseScaleAmount = 0.09f;

    private bool isConnected = false;
    private bool isPulsing = false;
    private Color baseColor;
    private Vector3 baseScale;

    public DotFromDotsPuzzle[] AvailableConnections => availableConnections;
    public bool IsConnected => isConnected;

    public int DotIndex;

    private void Awake()
    {
        if (childRenderer != null)
        {
            baseColor = childRenderer.material.color;
        }

        if (visualToScale == null)
        {
            visualToScale = transform;
        }

        baseScale = visualToScale.localScale;
    }

    private void Update()
    {
        if (!isPulsing) return;

        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseScaleAmount;
        visualToScale.localScale = baseScale * pulse;
    }

    public void SelectFirstDot()
    {
        isConnected = true;
        SetSelectedColor();
        Debug.Log("Dot selected: " + gameObject.name);
    }

    public void SelectNextDot()
    {
        isConnected = true;
        SetSelectedColor();
    }

    public void DeselectDot()
    {
        RestoreBaseColor();
    }

    public void ResetDot()
    {
        isConnected = false;
        StopPulse();
        RestoreBaseColor();
    }


    public bool CanConnectTo(DotFromDotsPuzzle otherDot)
    {
        if (otherDot == null) return false;

        for (int i = 0; i < availableConnections.Length; i++)
        {
            if (availableConnections[i] == otherDot)
            {
                return true;
            }
        }

        return false;
    }

    public void StartPulse()
    {
        isPulsing = true;
    }

    public void StopPulse()
    {
        isPulsing = false;

        if (visualToScale != null)
        {
            visualToScale.localScale = baseScale;
        }
    }

    private void SetSelectedColor()
    {
        if (childRenderer == null) return;
        childRenderer.material.color = selectedColor;
    }

    private void RestoreBaseColor()
    {
        if (childRenderer == null) return;
        childRenderer.material.color = baseColor;
    }
}
