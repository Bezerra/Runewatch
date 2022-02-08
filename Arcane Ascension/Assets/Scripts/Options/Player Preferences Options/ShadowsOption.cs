using UnityEngine;

/// <summary>
/// Updates shadows options.
/// </summary>
public class ShadowsOption : ButtonArrowOption
{
    [SerializeField] private string vsyncPlayerPrefsName;
    [SerializeField] private string qualitySettingsPlayerPrefsName;

    protected override void UpdateOption(float value)
    {
        float vSyncOption = PlayerPrefs.GetFloat(vsyncPlayerPrefsName, 1);
        float qualitySettingsOption = 
            PlayerPrefs.GetFloat(qualitySettingsPlayerPrefsName, 2);

        switch (value)
        {
            // No shadows, it will update quality level to no shadows assets
            case 0:
                switch (qualitySettingsOption)
                {
                    case 0:
                        QualitySettings.SetQualityLevel(3);
                        break;
                    case 1:
                        QualitySettings.SetQualityLevel(4);
                        break;
                    case 2:
                        QualitySettings.SetQualityLevel(5);
                        break;
                }
                textToUpdate.text = "Off";
                break;

            // Shadows, it will update quality level to shadows assets
            case 1:
                switch (qualitySettingsOption)
                {
                    case 0:
                        QualitySettings.SetQualityLevel(0);
                        break;
                    case 1:
                        QualitySettings.SetQualityLevel(1);
                        break;
                    case 2:
                        QualitySettings.SetQualityLevel(2);
                        break;
                }
                textToUpdate.text = "On";
                break;
        }

        // Updates vsync again after changing quality levels
        QualitySettings.vSyncCount = (int)vSyncOption;
    }
}
