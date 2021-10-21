using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning hit prefab.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Hit Prefab", 
    fileName = "Spell Behaviour Spawn Hit Prefab")]
public class SpellBehaviourSpawnHitPrefabOneShotSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
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
        // Calculates rotation of the spell to cast
        Quaternion spellLookRotation;
        if (Physics.Raycast(parent.transform.position,
            (other.ClosestPoint(parent.transform.position) - parent.transform.position).normalized,
            out RaycastHit spellHitPoint))
        {
            spellLookRotation =
                Quaternion.LookRotation(spellHitPoint.normal, other.transform.up);
        }
        else
        {
            spellLookRotation =
                Quaternion.LookRotation(
                    (other.transform.position - parent.WhoCast.transform.position).normalized, other.transform.up);
        }

        // Creates spell hit
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviourOneShot != null &&
            other.gameObject.layer != parent.WhoCast.gameObject.layer)
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, parent.transform.position,
                spellLookRotation);

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(out SpellOnHitBehaviourOneShot onHitBehaviour))
            {
                onHitBehaviour.Spell = parent.Spell;
            }
        }
    }
}
