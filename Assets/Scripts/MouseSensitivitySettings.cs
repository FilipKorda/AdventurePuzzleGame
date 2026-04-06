using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivitySettings : MonoBehaviour
{
    public static float MouseSensitivity { get; private set; }

    [SerializeField] Slider sensitivitySlider;

    private const string PrefKey = "Mouse_Sensitivity";
    private const float DefaultValue = 1f;

    private void Awake()
    {
        MouseSensitivity = PlayerPrefs.GetFloat(PrefKey, DefaultValue);
    }

    private void Start()
    {
        sensitivitySlider.SetValueWithoutNotify(MouseSensitivity);
    }

    public static void SetSensitivity(float value)
    {
        MouseSensitivity = value;
    }

    public void OnSensitivityChanged(float value)
    {
        MouseSensitivity = value;
        PlayerPrefs.SetFloat(PrefKey, value);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        OnSensitivityChanged(MouseSensitivity);
    }
}