using UnityEngine;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot", fileName = "Attack Behaviour One Shot")]
public class AttackBehaviourOneShotSO : AttackBehaviourAbstractOneShotSO
{
    /// <summary>
    /// Attack behaviour for one shot spells. Instantiates the spell from a pool and triggers its start behaviour.
    /// </summary>
    /// <param name="spell">Spell to cast.</param>
    /// <param name="character">Character that cast the spell.</param>
    /// <param name="characterStats">Character that cast the spell stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell cast.</param>
    public override void AttackKeyPress(ISpell spell, Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.
                Hand.position,
                Quaternion.identity);

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
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
    public override void AttackKeyPress(ISpell spell, StateController character, Stats characterStats)
    {
        Vector3 finalDirection =
                        character.transform.position +
                        (character.CurrentTarget.position - character.transform.position);

        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.EnemyScript.
                Hand.position,
                Quaternion.identity);

        spawnedSpell.transform.LookAt(finalDirection);

        SpellBehaviourOneShot spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
        spellBehaviour.WhoCast = characterStats;
        spellBehaviour.TriggerStartBehaviour();
    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    /// <param name="spellBehaviour">Parent spell behaviour.</param>
    public override void AttackKeyRelease(
        ISpell spell, Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        // Left blank on purpose
    }
}
