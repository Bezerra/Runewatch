using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates a slider option.
/// </summary>
public class SliderOption : PlayerPreferencesOption
{
    [SerializeField] private string optionToUpdate;
    [SerializeField] private float defaultValue;
    [SerializeField] private Slider slider;

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        float value = PlayerPrefs.GetFloat(optionToUpdate, defaultValue);
        slider.value = value;
    }

    public override void UpdateValue(float value)
    {
        PlayerPrefs.SetFloat(optionToUpdate, value);
    }
}
