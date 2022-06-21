/// <summary>
/// Updates fps options.
/// </summary>
public class FPSOption : ButtonArrowOption
{
    protected override void UpdateOption(float value)
    {
        switch (value)
        {
            case 0:
                textToUpdate.text = "Off";
                break;
            case 1:
                textToUpdate.text = "On";
                break;
        }

        PlayerUI pUI = FindObjectOfType<PlayerUI>();
        if (pUI != null)
            pUI.ControlFPSCounter((int)value);
    }
}
