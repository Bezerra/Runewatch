using UnityEngine;

/// <summary>
/// Scritable object responsible for getting 3 random spells from all spells list.
/// </summary>
[CreateAssetMenu(menuName = "Events/Event Get One Dropped Spell",
    fileName = "Event Get One Dropped Spell")]
public class EventGetOneDroppedSpellSO : EventAbstractSO
{
    // Scriptable object that saves spells result
    [SerializeField] private RandomAbilitiesToChooseSO abilitiesToChose;

    /// <summary>
    /// Updates drop abilities asset with the dropped spell.
    /// </summary>
    /// <param name="invoker">Who invoked this event.</param>
    public override void Execute(AbstractEventOnInteraction invoker)
    {
        abilitiesToChose.DroppedSpell = invoker.gameObject.GetComponent<SpellScroll>().DroppedSpell;
    }
}
