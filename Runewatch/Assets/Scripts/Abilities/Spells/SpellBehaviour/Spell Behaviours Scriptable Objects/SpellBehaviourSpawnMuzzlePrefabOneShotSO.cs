using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Muzzle Prefab", 
    fileName = "Spell Behaviour Spawn Muzzle Prefab")]
sealed public class SpellBehaviourSpawnMuzzlePrefabOneShotSO : SpellBehaviourAbstractSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Creates spell muzzle prefab
        // Update() will run from its monobehaviour script
        GameObject spellMuzzleBehaviourGameObject = SpellMuzzlePoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.Hand.position,
            Quaternion.LookRotation(parent.Eyes.forward, parent.Eyes.up));

        // Gets muzzle behaviour from it
        if (spellMuzzleBehaviourGameObject.TryGetComponent<SpellMuzzleBehaviourOneShot>(
            out SpellMuzzleBehaviourOneShot muzzleBehaviour))
        {
            muzzleBehaviour.Spell = parent.Spell;
            muzzleBehaviour.Eyes = parent.Eyes;
            muzzleBehaviour.Hand = parent.Hand;
            muzzleBehaviour.WhoCast = parent.WhoCast;
            //muzzleBehaviour.MuzzlePlayerSpawnOffset = muzzlePlayerSpawnOffset;

            if (parent.Spell.Sounds.Muzzle != null)
            {
                muzzleBehaviour.Spell.Sounds.Muzzle.PlaySound(muzzleBehaviour.AudioS);
            }
        }
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
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
