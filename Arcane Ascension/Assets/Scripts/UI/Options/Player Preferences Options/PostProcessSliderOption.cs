using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Updates a slider option.
/// </summary>
public class PostProcessOption : PlayerPreferencesOption
{
    [SerializeField] private PPrefsOptions optionToUpdate;
    [SerializeField] private Slider slider;
    [SerializeField] private VolumeProfile postProcess;

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        slider.value = PlayerPrefs.GetFloat(optionToUpdate.ToString(), 0);

        switch(optionToUpdate)
        {
            case PPrefsOptions.Bloom:

                break;
            case PPrefsOptions.Brightness:

                break;
            case PPrefsOptions.Contrast:

                break;
            case PPrefsOptions.Saturation:

                break;
        }
    }

    public override void UpdateValue(float value)
    {
        PlayerPrefs.SetFloat(optionToUpdate.ToString(), value);

        switch (optionToUpdate)
        {
            case PPrefsOptions.Bloom:

                break;
            case PPrefsOptions.Brightness:

                break;
            case PPrefsOptions.Contrast:

                break;
            case PPrefsOptions.Saturation:

                break;
        }
    }
}
