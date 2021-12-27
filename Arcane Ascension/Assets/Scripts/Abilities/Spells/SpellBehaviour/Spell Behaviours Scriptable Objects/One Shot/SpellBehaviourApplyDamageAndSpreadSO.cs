using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for applying damage, and spread to another enemy.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage And Spread", 
    fileName = "Spell Behaviour Apply Damage And Spread")]
sealed public class SpellBehaviourApplyDamageAndSpreadSO : SpellBehaviourAbstractOneShotSO
{
    [Space(20)]
    [Range(2, 20)] [SerializeField] private byte hitQuantity;
    [Range(1, 15f)] [SerializeField] private float spreadRange = 10f;

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
        if (parent.TriggerSpread)
        {
            // Checks which layer should it damage
            LayerMask layerToHit = parent.WhoCast.CommonAttributes.Type ==
                CharacterType.Player ? Layers.EnemyLayer : Layers.PlayerLayer;

            // Creates a sphere collider to check every enemy in front of the player
            Collider[] enemyColliders =
                Physics.OverlapSphere(parent.transform.position, spreadRange,
                layerToHit);

            // If it found enemies
            if (enemyColliders.Length > 0)
            {
                parent.TriggerSpread = false;
                
                parent.HomingTarget = enemyColliders[Random.Range(0, enemyColliders.Length)].transform;
            }
            else
            {
                parent.Rb.velocity = Vector3.zero;
                parent.DisableImmediatly = true;
            }
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Guides the spell towards the homing target
        if (parent.HomingTarget != null)
        {
            Vector3 direction = parent.transform.position.Direction(parent.HomingTarget.position);
            parent.Rb.velocity = direction * parent.Spell.Speed;
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Applies damage with a modifier
        parent.Spell.DamageBehaviour.Damage(parent, other);

        parent.Rb.velocity = Vector3.zero;
        parent.HomingTarget = null;
        parent.TriggerSpread = true;

        // Adds a hit counter
        if (++parent.CurrentPierceHitQuantity >= hitQuantity)
        {
            parent.Rb.velocity = Vector3.zero;
            parent.DisableImmediatly = true;
        }
    }
}
