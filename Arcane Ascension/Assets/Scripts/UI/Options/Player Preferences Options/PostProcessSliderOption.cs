using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Updates a slider option.
/// </summary>
public class PostProcessSliderOption : PlayerPreferencesOption
{
    [SerializeField] private PPrefsOptions optionToUpdate;
    [SerializeField] private Slider slider;
    [SerializeField] private VolumeProfile postProcess;

    // Post process
    private ColorAdjustments colorAdjustments;
    private Bloom bloom;

    private void Awake()
    {
        postProcess.TryGet(out colorAdjustments);
        postProcess.TryGet(out bloom);
    }

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        if (colorAdjustments == null || bloom == null)
        {
            postProcess.TryGet(out colorAdjustments);
            postProcess.TryGet(out bloom);
        }

        if (optionToUpdate == PPrefsOptions.Bloom)
            slider.value = PlayerPrefs.GetFloat(optionToUpdate.ToString(), 3f);
        else
            slider.value = PlayerPrefs.GetFloat(optionToUpdate.ToString(), 0);

        switch(optionToUpdate)
        {
            case PPrefsOptions.Bloom:
                bloom.intensity.value = slider.value;
                break;
            case PPrefsOptions.Brightness:
                colorAdjustments.postExposure.value = slider.value;
                break;
            case PPrefsOptions.Contrast:
                colorAdjustments.contrast.value = slider.value;
                break;
            case PPrefsOptions.Saturation:
                colorAdjustments.saturation.value = slider.value;
                break;
        }
    }

    public override void UpdateValue(float value)
    {
        if (colorAdjustments == null || bloom == null)
        {
            postProcess.TryGet(out colorAdjustments);
            postProcess.TryGet(out bloom);
        }

        PlayerPrefs.SetFloat(optionToUpdate.ToString(), value);

        switch (optionToUpdate)
        {
            case PPrefsOptions.Bloom:
                bloom.intensity.value = value;
                break;
            case PPrefsOptions.Brightness:
                colorAdjustments.postExposure.value = value;
                break;
            case PPrefsOptions.Contrast:
                colorAdjustments.contrast.value = value;
                break;
            case PPrefsOptions.Saturation:
                colorAdjustments.saturation.value = value;
                break;
        }
    }
}
