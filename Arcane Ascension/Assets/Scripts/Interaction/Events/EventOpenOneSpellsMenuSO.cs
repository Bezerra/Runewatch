using UnityEngine;

/// <summary>
/// Scritable object responsible for opening one spells canvas.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Open One Spells Menu",
    fileName = "Event Open One Spells Menu")]
public class EventOpenOneSpellsMenuSO : EventAbstractSO
{
    public override void Execute(EventOnInteraction invoker)
    {
        FindObjectOfType<AbilitiesCanvas>().EnableOneSpellCanvas();
    }
}
