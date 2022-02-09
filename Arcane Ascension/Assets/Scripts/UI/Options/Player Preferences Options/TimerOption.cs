/// <summary>
/// Updates timer options.
/// </summary>
public class TimerOption : ButtonArrowOption
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
            pUI.ControlTimer((int)value);
    }
}
