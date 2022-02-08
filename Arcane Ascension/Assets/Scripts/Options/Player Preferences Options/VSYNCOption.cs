using UnityEngine;

/// <summary>
/// Updates screenmode options.
/// </summary>
public class VSYNCOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        switch (value)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                textToUpdate.text = "Off";
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                textToUpdate.text = "On";
                break;
        }
    }
}
