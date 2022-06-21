/// <summary>
/// Class for burnable objects.
/// </summary>
public class Burnable : AbstractInteractionWithSpell
{
    protected override void ActionToTake()
    {
        anim.SetTrigger("Burn");
    }
}
