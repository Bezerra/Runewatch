/// <summary>
/// Updates show damage options.
/// </summary>
public class ShowDamageOption : ButtonArrowOption
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
    }
}
