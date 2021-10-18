using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Spawn Muzzle Prefab", fileName = "Spell Behaviour Spawn Muzzle Prefab")]
sealed public class SpellBehaviourSpawnMuzzlePrefabOneShotSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Creates spell muzzle
        // Update() will run from its monobehaviour script
        if (parent.Spell.MuzzleBehaviourOneShot != null)
        {
            // Creates spell muzzle
            GameObject spellMuzzleBehaviourGameObject = SpellMuzzlePoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.transform.position,
            Quaternion.LookRotation(parent.transform.forward, parent.transform.up));

            if (spellMuzzleBehaviourGameObject.TryGetComponent<SpellMuzzleBehaviourOneShot>(out SpellMuzzleBehaviourOneShot muzzleBehaviour))
            {
                muzzleBehaviour.Spell = parent.Spell;
            }
        }
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
