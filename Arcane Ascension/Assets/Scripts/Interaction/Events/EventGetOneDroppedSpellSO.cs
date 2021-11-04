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

    public override void Execute(EventOnInteraction invoker)
    {
        abilitiesToChose.SpellResult[0] = invoker.gameObject.GetComponent<DroppedSpell>().SpellDropped;
    }
}
