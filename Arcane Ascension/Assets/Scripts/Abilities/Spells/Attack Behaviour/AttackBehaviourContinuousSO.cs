using UnityEngine;

/// <summary>
/// Scriptable object for continuous attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour Continuous", fileName = "Attack Behaviour Continuous")]
public class AttackBehaviourContinuousSO : AttackBehaviourAbstractContinuousSO
{
    /// <summary>
    /// Spawns a spell until the player releases the fire key.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spell being cast.</param> 
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public override void AttackKeyPress(ref GameObject currentlyCastSpell, ISpell spell, 
        Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        currentlyCastSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.Hand.position,
                Quaternion.identity);

        // Spellbehaviour will continue running until
        // the player releases cast spell button 

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        spellBehaviour = currentlyCastSpell.GetComponent<SpellBehaviourContinuous>();
        spellBehaviour.WhoCast = characterStats;
        spellBehaviour.TriggerStartBehaviour();
    }

    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by AI.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character (state controller) who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    public override void AttackKeyPress(
        ISpell spell, StateController<Enemy> character, Stats characterStats)
    {
        // Temp blank
    }

    /// <summary>
    /// Disables continuous spell monobehaviour.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spell being cast.</param>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public override void AttackKeyRelease(
        ref GameObject currentlyCastSpell, ISpell spell, Character character, 
        Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        DisableSpell(spellBehaviour);
    }
}
