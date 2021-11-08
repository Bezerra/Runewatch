using UnityEngine;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Muzzle Prefab", 
    fileName = "Spell Behaviour Spawn Muzzle Prefab")]
sealed public class SpellBehaviourSpawnMuzzlePrefabOneShotSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Creates spell muzzle prefab
        // Update() will run from its monobehaviour script
        GameObject spellMuzzleBehaviourGameObject = SpellMuzzlePoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.Hand.position,
            Quaternion.LookRotation(parent.Hand.forward, parent.Hand.up));

        // Gets muzzle behaviour from it
        if (spellMuzzleBehaviourGameObject.TryGetComponent<SpellMuzzleBehaviourOneShot>(out SpellMuzzleBehaviourOneShot muzzleBehaviour))
        {
            if (muzzleBehaviour.Spell != parent.Spell)
                muzzleBehaviour.Spell = parent.Spell;

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
