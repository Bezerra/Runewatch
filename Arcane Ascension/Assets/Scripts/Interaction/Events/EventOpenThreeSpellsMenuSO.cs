using UnityEngine;

/// <summary>
/// Scritable object responsible for opening three spells canvas.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Open Three Spells Menu",
    fileName = "Event Open Three Spells Menu")]
public class EventOpenThreeSpellsMenuSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        FindObjectOfType<AbilitiesCanvas>().EnableThreeSpellCanvas();
    }
}
