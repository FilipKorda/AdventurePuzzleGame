using UnityEngine;
using UnityEngine.UI;

public class UISliderSound : MonoBehaviour
{
    private Slider slider;
    private string changeSoundId = "UISlider";
    private float minDeltaToPlay = 0.05f;

    private float lastValue;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        if (slider != null)
        {
            lastValue = slider.value;
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        if (Mathf.Abs(newValue - lastValue) < minDeltaToPlay)
            return;

        lastValue = newValue;
        Services.Audio.PlaySFX(changeSoundId);
    }
}
