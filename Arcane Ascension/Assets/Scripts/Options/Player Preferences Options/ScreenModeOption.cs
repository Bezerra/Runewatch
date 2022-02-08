using UnityEngine;

/// <summary>
/// Updates screenmode options.
/// </summary>
public class ScreenModeOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        FullScreenMode windowMode = default;
        switch (value)
        {
            case 0:
                windowMode = FullScreenMode.Windowed;
                textToUpdate.text = "Windowed";
                break;
            case 1:
                windowMode = FullScreenMode.FullScreenWindow;
                textToUpdate.text = "Fullscreen";
                break;
        }
        Screen.fullScreenMode = windowMode;
    }
}
