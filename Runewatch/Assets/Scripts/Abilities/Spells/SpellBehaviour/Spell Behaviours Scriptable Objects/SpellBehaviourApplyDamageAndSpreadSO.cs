using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for applying damage, and spread to another enemy.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Apply Damage And Spread", 
    fileName = "Spell Behaviour Apply Damage And Spread")]
sealed public class SpellBehaviourApplyDamageAndSpreadSO : SpellBehaviourAbstractSO
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
        if (parent.DisableSpellAfterCollision)
        {
            parent.Rb.velocity = Vector3.zero;
            return;
        }

        if (parent.TriggerSpread)
        {
            // Checks which layer should it damage
            LayerMask layerToHit = parent.WhoCast.CommonAttributes.Type ==
                CharacterType.Player ? Layers.EnemySensiblePoint : Layers.PlayerLayer;

            // Creates a sphere collider to check every enemy in front of the player
            Collider[] enemyColliders =
                Physics.OverlapSphere(parent.transform.position, spreadRange,
                layerToHit);

            // Creates a list for colliders found
            IList<Collider> collidersList = new List<Collider>();
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].TryGetComponentInParent(out SelectionBase colliderHit))
                {
                    if (parent.CharacterHit != null)
                    {
                        if (parent.CharacterHit.gameObject.TryGetComponentInParent(out SelectionBase characterHit))
                        {
                            // If collider selection base is different than the current character being damaged
                            if (colliderHit != characterHit)
                            {
                                collidersList.Add(enemyColliders[i]);
                            }
                        }
                    }
                }  
            }

            // If it found enemies
            // Sets a new homing target
            if (collidersList.Count > 0)
            {
                parent.HomingTarget = collidersList[Random.Range(0, collidersList.Count)].transform;
                parent.TriggerSpread = false;
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
            parent.transform.rotation =
                Quaternion.LookRotation(parent.transform.Direction(parent.HomingTarget.position));
            parent.Rb.velocity = parent.transform.forward * parent.Spell.Speed;
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Applies damage with a modifier
        parent.Spell.DamageBehaviour.Damage(parent, other: other);

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
