using UnityEngine;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot With Release", 
    fileName = "Attack Behaviour One Shot With Release")]
public class AttackBehaviourOneShotWithReleaseSO : AttackBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Attack behaviour for one shot spells. Instantiates the spell from a pool and triggers its start behaviour.
    /// </summary>
    /// <param name="playerCastSpell">Script from where the spell was cast from.</param> 
    /// <param name="spell">Spell to cast.</param>
    /// <param name="character">Character that cast the spell.</param>
    /// <param name="characterStats">Character that cast the spell stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell cast.</param>
    public override void AttackKeyPress(ref GameObject currentlyCastSpell, ISpell spell, 
        Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        currentlyCastSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.
                Hand.position,
                Quaternion.identity);
    }

    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by AI.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character (state controller) who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    public override void AttackKeyPress(ISpell spell, StateController character, Stats characterStats)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Triggered when attack key is released.
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

        // If a spell was spawned on attack press
        if (currentlyCastSpell != null)
        {
            currentlyCastSpell.transform.SetPositionAndRotation(character.Hand.position, Quaternion.identity);

            // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
            spellBehaviour = currentlyCastSpell.GetComponent<SpellBehaviourOneShot>();
            spellBehaviour.WhoCast = characterStats;
            spellBehaviour.TriggerStartBehaviour();
        }
    }
}
