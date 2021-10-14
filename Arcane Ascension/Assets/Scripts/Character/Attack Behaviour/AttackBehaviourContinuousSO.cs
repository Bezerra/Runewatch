using UnityEngine;

/// <summary>
/// Scriptable object for continuous attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour Continuous", fileName = "Attack Behaviour Continuous")]
public class AttackBehaviourContinuousSO : AttackBehaviourAbstractSO
{
    /// <summary>
    /// Spawns a spell until the player releases the fire key.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public override void Attack(ISpell spell, Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.Hand.position,
                Quaternion.identity);

        // Spellbehaviour will continue running until
        // the player releases cast spell button 

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourContinuous>();
        spellBehaviour.WhoCast = characterStats;
    }

    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by AI.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character (state controller) who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    public override void Attack(ISpell spell, StateController character, Stats characterStats)
    {
        // Temp blank
    }
}
