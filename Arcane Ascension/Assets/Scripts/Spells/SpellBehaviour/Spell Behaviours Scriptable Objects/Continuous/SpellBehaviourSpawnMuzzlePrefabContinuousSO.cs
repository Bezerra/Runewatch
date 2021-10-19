using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Spawn Muzzle Prefab", 
    fileName = "Spell Behaviour Spawn Muzzle Prefab")]
sealed public class SpellBehaviourSpawnMuzzlePrefabContinuousSO : SpellBehaviourAbstractContinuousSO
{
    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        // Creates spell muzzle prefab
        // Update() will run from its monobehaviour script
        // Creates spell muzzle prefab
        GameObject spellMuzzleBehaviourGameObject = SpellMuzzlePoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.transform.position,
            Quaternion.LookRotation(parent.transform.forward, parent.transform.up));

        // Gets muzzle behaviour from it
        if (spellMuzzleBehaviourGameObject.TryGetComponent<SpellMuzzleBehaviourContinuous>
            (out SpellMuzzleBehaviourContinuous muzzleBehaviour))
        {
            if (muzzleBehaviour.Spell != parent.Spell)
            {
                muzzleBehaviour.Spell = parent.Spell;
                muzzleBehaviour.SpellMonoBehaviour = parent;
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
