using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning hit of AoE hover spell.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Spawn Hit Aoe Hover Spell",
    fileName = "Spell Behaviour Spawn Hit Aoe Hover Spell")]
public class SpellBehaviourSpawnHitOnAoEHoverSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Needed to run other behaviours
        parent.SpellStartedMoving = true;
        parent.TimeOfImpact = Time.time;
        parent.TimeSpawned = Time.time;

        parent.transform.position = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;
        parent.PositionOnHit = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;

        // Spawns hit in direction of collider hit normal
        GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.AreaHoverAreaHit.point,
            Quaternion.identity);

        if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviourOneShot>(out SpellOnHitBehaviourOneShot onHitBehaviour))
        {
            // Sets hit Spell to this spell
            if (onHitBehaviour.Spell != parent.Spell)
                onHitBehaviour.Spell = parent.Spell;
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
