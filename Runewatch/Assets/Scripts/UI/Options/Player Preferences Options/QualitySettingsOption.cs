using UnityEngine;

/// <summary>
/// Updates quality settings options.
/// </summary>
public class QualitySettingsOption : ButtonArrowOption
{
    [SerializeField] private PPrefsOptions vsyncPlayerPrefsName;
    [SerializeField] private PPrefsOptions shadowsPlayerPrefsName;

    protected override void UpdateOption(float value)
    {
        float vSyncOption = PlayerPrefs.GetFloat(vsyncPlayerPrefsName.ToString(), 1);
        float shadowsOption = PlayerPrefs.GetFloat(shadowsPlayerPrefsName.ToString(), 1);

        switch (value)
        {
            case 0:
                if (shadowsOption == 1)
                    QualitySettings.SetQualityLevel(0);
                else
                    QualitySettings.SetQualityLevel(3);
                textToUpdate.text = "Low";
                break;
            case 1:
                if (shadowsOption == 1)
                    QualitySettings.SetQualityLevel(1);
                else
                    QualitySettings.SetQualityLevel(4);
                textToUpdate.text = "Medium";
                break;
            case 2:
                if (shadowsOption == 1)
                    QualitySettings.SetQualityLevel(2);
                else
                    QualitySettings.SetQualityLevel(5);
                textToUpdate.text = "High";
                break;
        }

        // Updates vsync again after changing quality levels
        QualitySettings.vSyncCount = (int)vSyncOption;
    }
}
