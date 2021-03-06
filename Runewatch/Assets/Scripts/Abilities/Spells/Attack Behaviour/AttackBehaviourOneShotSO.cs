using UnityEngine;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot", fileName = "Attack Behaviour One Shot")]
public class AttackBehaviourOneShotSO : AttackBehaviourAbstractSO
{
    /// <summary>
    /// Attack behaviour for one shot spells. Instantiates the spell from a pool and triggers its start behaviour.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spell being cast.</param> 
    /// <param name="spell">Spell to cast.</param>
    /// <param name="character">Character that cast the spell.</param>
    /// <param name="characterStats">Character that cast the spell stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell cast.</param>
    public override void AttackKeyPress(ref GameObject currentlyCastSpell, ISpell spell, 
        Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        currentlyCastSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.Hand.position,
                Quaternion.identity);

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        spellBehaviour = currentlyCastSpell.GetComponent<SpellBehaviourOneShot>();
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
        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.Controller.Hand.position,
                Quaternion.identity);

        SpellBehaviourOneShot spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
        spellBehaviour.WhoCast = characterStats;
        spellBehaviour.TriggerStartBehaviour();
    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    /// <summary>
    /// What happens after the player releases attack key.
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
        // Left blank on purpose
    }

    /// <summary>
    /// What happens after the AI releases the attack.
    /// </summary>
    /// <param name="character"></param>
    public override void AttackKeyRelease(StateController<Enemy> character)
    {
        // Left blank on purpose
    }
}
