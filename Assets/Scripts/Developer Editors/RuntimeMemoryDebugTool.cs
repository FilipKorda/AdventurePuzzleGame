using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

public class RuntimeMemoryDebugTool : MonoBehaviour
{
    [Header("Toggle")]
    [SerializeField] private InputActionReference toggleInput;
    [SerializeField] private bool visibleOnStart;

    [Header("Refresh")]
    [SerializeField] private float refreshInterval = 0.25f;

    [Header("Window")]
    [SerializeField] private Rect windowRect = new Rect(20f, 20f, 420f, 220f);

    private bool isVisible;
    private float nextRefreshTime;
    private string cachedText = string.Empty;
    private readonly StringBuilder sb = new StringBuilder(512);

    private void Awake()
    {
        isVisible = visibleOnStart;
        RefreshStats();
    }

    private void OnEnable()
    {
        if (toggleInput != null)
        {
            toggleInput.action.Enable();
            toggleInput.action.performed += OnTogglePerformed;
        }
    }

    private void OnDisable()
    {
        if (toggleInput != null)
        {
            toggleInput.action.performed -= OnTogglePerformed;
            toggleInput.action.Disable();
        }
    }

    private void Update()
    {
        if (!isVisible)
            return;

        if (Time.unscaledTime >= nextRefreshTime)
        {
            RefreshStats();
            nextRefreshTime = Time.unscaledTime + refreshInterval;
        }
    }

    private void OnGUI()
    {
        if (!isVisible)
            return;

        GUI.Box(windowRect, "Runtime Memory Debug");
        GUI.Label(
            new Rect(windowRect.x + 12f, windowRect.y + 28f, windowRect.width - 24f, windowRect.height - 40f),
            cachedText);
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        isVisible = !isVisible;

        if (isVisible)
        {
            RefreshStats();
            nextRefreshTime = Time.unscaledTime + refreshInterval;
        }
    }

    private void RefreshStats()
    {
        long unityAllocated = Profiler.GetTotalAllocatedMemoryLong();
        long unityReserved = Profiler.GetTotalReservedMemoryLong();
        long unityUnusedReserved = Profiler.GetTotalUnusedReservedMemoryLong();
        long gcHeap = System.GC.GetTotalMemory(false);

        long processWorkingSet = 0;
        long privateMemory = 0;

        try
        {
            using Process currentProcess = Process.GetCurrentProcess();
            processWorkingSet = currentProcess.WorkingSet64;
            privateMemory = currentProcess.PrivateMemorySize64;
        }
        catch (System.Exception exception)
        {
            Debug.LogWarning("RuntimeMemoryDebugTool: could not read process memory. " + exception.Message);
        }

        sb.Clear();
        sb.AppendLine("Unity Allocated: " + FormatMb(unityAllocated));
        sb.AppendLine("Unity Reserved: " + FormatMb(unityReserved));
        cachedText = sb.ToString();
    }

    private static string FormatMb(long bytes)
    {
        if (bytes <= 0)
            return "0 MB";

        float mb = bytes / (1024f * 1024f);
        return mb.ToString("F1") + " MB";
    }
}
