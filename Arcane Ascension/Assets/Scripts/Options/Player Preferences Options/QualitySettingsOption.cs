using UnityEngine;

/// <summary>
/// Updates quality settings options.
/// </summary>
public class QualitySettingsOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        switch (value)
        {
            case 0:
                QualitySettings.SetQualityLevel(0);
                textToUpdate.text = "Low";
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                textToUpdate.text = "Medium";
                break;
            case 2:
                QualitySettings.SetQualityLevel(2);
                textToUpdate.text = "High";
                break;
        }
    }
}
