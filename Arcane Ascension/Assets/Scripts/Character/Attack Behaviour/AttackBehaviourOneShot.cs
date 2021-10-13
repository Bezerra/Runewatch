using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot", fileName = "Attack Behaviour One Shot")]
public class AttackBehaviourOneShot : AttackBehaviourAbstract
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
