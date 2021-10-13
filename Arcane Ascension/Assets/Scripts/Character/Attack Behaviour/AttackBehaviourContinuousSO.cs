using UnityEngine;

/// <summary>
/// Scriptable object for continuous attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour Continuous", fileName = "Attack Behaviour Continuous")]
public class AttackBehaviourContinuousSO : AttackBehaviourAbstractSO
{
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
}
