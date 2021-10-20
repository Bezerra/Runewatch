using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object responsible for bouncing on hit.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Bounce On Hit", 
    fileName = "Spell Behaviour Bounce On Hit")]
public class SpellBehaviourBounceOnHitSO : SpellBehaviourAbstractOneShotSO
{
    [Header("Walls and floor layer numbers")]
    [SerializeField] private List<int> layersToStopTheSpell;
    [Range(2, 20)] [SerializeField] private byte hitQuantity;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
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
        if (layersToStopTheSpell.Contains(other.gameObject.layer))
        {
            if (++parent.CurrentWallHitQuantity < hitQuantity)
            {
                RotateProjectile(other, parent);
            }
            else
            {
                parent.DisableSpellAfterCollision = true;
                parent.Rb.velocity = Vector3.zero;
            }
        }
    }

    private void RotateProjectile(Collider other, SpellBehaviourOneShot parent)
    {
        if (Physics.Raycast(parent.transform.position,
            (other.ClosestPoint(parent.transform.position) - parent.transform.position).normalized,
            out RaycastHit spellHitPoint))
        {
            Vector3 reflection = Vector3.Reflect(parent.Rb.velocity, spellHitPoint.normal).normalized;

            parent.Rb.velocity = reflection * parent.Spell.Speed;
        }
    }
}
