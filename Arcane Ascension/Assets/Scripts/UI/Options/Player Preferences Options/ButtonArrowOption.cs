using UnityEngine;
using TMPro;

/// <summary>
/// Updates button arrow option value.
/// </summary>
public abstract class ButtonArrowOption : PlayerPreferencesOption
{
    [SerializeField] protected TextMeshProUGUI textToUpdate;
    [SerializeField] private string optionToUpdate;
    [SerializeField] protected Vector2 limits;
    [Header("Must be a number between x and y limits")]
    [SerializeField] private int defaultValue;
    private float value;

    public void IncrementValue()
    {
        if (value + 1 <= limits.y) value++;
        else value = limits.x;
        UpdateValue(value);
    }

    public void DecrementValue()
    {
        if (value - 1 >= limits.x) value--;
        else value = limits.y;
        UpdateValue(value);
    }

    protected override void UpdateValueToMatchPlayerPrefs()
    {
        float value = PlayerPrefs.GetFloat(optionToUpdate, defaultValue);
        UpdateOption(value);
    }

    public override void UpdateValue(float value)
    {
        UpdateOption(value);
        PlayerPrefs.SetFloat(optionToUpdate, value);
    }

    protected abstract void UpdateOption(float value);
}
