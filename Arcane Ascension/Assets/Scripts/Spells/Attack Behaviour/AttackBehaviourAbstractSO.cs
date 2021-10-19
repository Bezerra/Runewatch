using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object for attack behaviours.
/// </summary>
[InlineEditor]
public abstract class AttackBehaviourAbstractSO : ScriptableObject
{
    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by player.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public abstract void Attack(ISpell spell, Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour);

    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by AI.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character (state controller) who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    public abstract void Attack(ISpell spell, StateController character, Stats characterStats);

    /// <summary>
    /// Disables spell gameobject.
    /// </summary>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public void DisableSpell(SpellBehaviourAbstract spellBehaviour)
    {
        if (spellBehaviour != null)
            spellBehaviour.DisableSpell();
    }
}
