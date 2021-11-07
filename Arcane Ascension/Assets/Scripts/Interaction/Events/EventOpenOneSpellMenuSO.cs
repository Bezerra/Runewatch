using UnityEngine;

/// <summary>
/// Scritable object responsible for opening one spells canvas.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Open One Spell Menu",
    fileName = "Event Open One Spell Menu")]
public class EventOpenOneSpellMenuSO : EventAbstractSO
{
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        FindObjectOfType<AbilitiesCanvas>().EnableOneSpellCanvas();
    }
}
