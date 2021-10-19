using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Spawn On Hit Prefab", 
    fileName = "Spell Behaviour Spawn On Hit Prefab")]
sealed public class SpellBehaviourSpawnOnHitPrefabContinuousSO : SpellBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Creates spell muzzle prefab
        // Update() will run from its monobehaviour script
        // Creates spell muzzle prefab
        GameObject spellOnHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.transform.position,
            Quaternion.LookRotation(parent.transform.forward, parent.transform.up));

        // Gets muzzle behaviour from it
        if (spellOnHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourContinuous>
            (out SpellOnHitBehaviourContinuous onHitBehaviour))
        {
            if (onHitBehaviour.Spell != parent.Spell)
            {
                onHitBehaviour.Spell = parent.Spell;
            }
        }
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }
}
