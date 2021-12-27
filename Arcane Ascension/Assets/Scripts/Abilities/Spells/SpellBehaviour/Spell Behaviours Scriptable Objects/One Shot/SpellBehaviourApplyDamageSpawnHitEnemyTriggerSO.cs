using UnityEngine;

/// <summary>
/// Scriptable object responsible for spawning hit of AoE hover spell.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage Spawn Hit Enemy Trigger",
    fileName = "Spell Behaviour Apply Damage Spawn Hit Enemy Trigger")]
public class SpellBehaviourApplyDamageSpawnHitEnemyTriggerSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Needed to run other behaviours
        parent.SpellStartedMoving = true;
        parent.TimeOfImpact = Time.time;
        parent.TimeSpawned = Time.time;

        parent.transform.position = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;
        parent.PositionOnHit = parent.AreaHoverAreaHit.point + parent.AreaHoverAreaHit.normal;
    }

    public override void ContinuousUpdateBeforeSpellBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        LayerMask layerToHit = parent.LayerOfWhoCast == 
            Layers.PlayerLayer ? Layers.EnemyLayer : Layers.PlayerLayer;

        Collider[] characterInRange = Physics.OverlapSphere(parent.transform.position,
            parent.Spell.AreaOfEffect, layerToHit);

        if (characterInRange.Length > 0)
        {
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

            parent.Spell.DamageBehaviour.Damage(parent);

            parent.DisableSpell();
        }
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
