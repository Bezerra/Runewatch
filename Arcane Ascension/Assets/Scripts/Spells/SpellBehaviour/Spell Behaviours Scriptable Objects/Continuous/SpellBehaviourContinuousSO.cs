using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's continuous spawning behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Continuous/Spell Behaviour Continuous", 
    fileName = "Spell Behaviour Continuous")]
sealed public class SpellBehaviourContinuousSO : SpellBehaviourAbstractContinuousSO
{
    [Range(1, 50)][SerializeField] private float spellMaxDistance;

    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        parent.EffectPlay();
    }

    /// <summary>
    /// Lers a line from player's hand to walls or forward.
    /// </summary>
    /// <param name="parent">Parent spell behaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Sets line first point
        parent.LineRender.SetPosition(0, parent.Hand.position);

        // Creates rays from eyes to forward
        Ray eyesForward = new Ray(parent.Eyes.position, parent.Eyes.forward);
        Vector3 eyesTarget;
        if (Physics.Raycast(eyesForward, out RaycastHit objectHit, spellMaxDistance))
        {
            // If it collides against a wall gets a point
            eyesTarget = objectHit.point;
        }
        else
        {
            // Else it will go forward until spell's max distance
            eyesTarget = parent.Eyes.position + parent.Eyes.forward * spellMaxDistance;
        }
        // Now, it creates a ray from HAND to eyes previous target
        Ray handForward = new Ray(parent.Hand.position, parent.Hand.position.Direction(eyesTarget));
        Vector3 finalTarget;
        if (Physics.Raycast(handForward, out RaycastHit handObjectHit, spellMaxDistance))
        {
            parent.HitPoint = handObjectHit;

            // If it its something, then it will lerp spell distance into that point
            finalTarget = handObjectHit.point;

            parent.CurrentSpellDistance = 
                Mathf.Lerp(parent.CurrentSpellDistance, handObjectHit.distance, parent.Spell.Speed * Time.deltaTime);

            // If the point reached the target, it gets its collider
            if (parent.CurrentSpellDistance.Similiar(handObjectHit.distance, 0.5f))
            {
                if (parent.DamageableTarget != objectHit.collider)
                    parent.DamageableTarget = objectHit.collider;
            }
        }
        else
        {
            parent.HitPoint = default;

            // Else it will grow forward until spell's max distance
            finalTarget = eyesTarget;

            if (parent.CurrentSpellDistance < spellMaxDistance)
                parent.CurrentSpellDistance += parent.Spell.Speed * Time.deltaTime * 2; // Faster growth

            // If parent had a damageable target it sets it to null
            if (parent.DamageableTarget != null)
                parent.DamageableTarget = null;
        }

        // Renderers line second point with the distance being updated
        parent.LineRender.SetPosition(1, 
            parent.Hand.position + parent.Hand.position.Direction(finalTarget) * parent.CurrentSpellDistance);
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
