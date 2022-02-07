using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates a slider option.
/// </summary>
public class SliderOption : PlayerPreferencesOption
{
    [SerializeField] private string optionToUpdate;
    [SerializeField] private Slider slider;

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        slider.value = PlayerPrefs.GetFloat(optionToUpdate, 1);
    }

    public override void UpdateValue(float value)
    {
        PlayerPrefs.SetFloat(optionToUpdate, value);
    }
}
