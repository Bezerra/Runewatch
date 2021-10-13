using UnityEngine;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot", fileName = "Attack Behaviour One Shot")]
public class AttackBehaviourOneShotSO : AttackBehaviourAbstractSO
{
    public override void Attack(ISpell spell, Character character, Stats characterStats)
    {
        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.
                Hand.position,
                Quaternion.identity);

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        SpellBehaviourOneShot spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
        spellBehaviour.WhoCast = characterStats;
        spellBehaviour.TriggerStartBehaviour();
    }
}
