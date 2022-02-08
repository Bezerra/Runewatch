using UnityEngine;

/// <summary>
/// Updates resolution options.
/// </summary>
public class ResolutionButtonOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        switch (value)
        {
            case 0:
                if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
                    Screen.SetResolution(1280, 720, true);
                else
                    Screen.SetResolution(1280, 720, false);
                textToUpdate.text = "1280 x 720";
                break;
            case 1:
                if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
                    Screen.SetResolution(1600, 900, true);
                else
                    Screen.SetResolution(1600, 900, false);
                textToUpdate.text = "1600 x 900";
                break;
            case 2:
                if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
                    Screen.SetResolution(1920, 1080, true);
                else
                    Screen.SetResolution(1920, 1080, false);
                textToUpdate.text = "1920 x 1080";
                break;
        }
    }
}
