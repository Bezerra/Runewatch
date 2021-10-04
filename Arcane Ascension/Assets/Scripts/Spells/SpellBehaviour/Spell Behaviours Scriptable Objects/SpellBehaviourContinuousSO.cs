using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Continuous", fileName = "Spell Behaviour Continuous")]
sealed public class SpellBehaviourContinuousSO : SpellBehaviourAbstractContinuousSO
{
    [Range(1, 50)][SerializeField] private float spellDistance;

    public override void StartBehaviour(SpellBehaviourContinuous parent)
    {
        parent.LastTimeHit = Time.time;
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        if (parent.Hand != null)
        {
            parent.transform.position = parent.Hand.position;
            parent.transform.rotation = parent.Hand.rotation;
        }

        if (Time.time > parent.LastTimeHit + parent.Spell.TimeInterval)
        {
            if (parent.WhoCast.Mana - parent.Spell.ManaCost >= 0)
            {
                float damageMultiplier = parent.WhoCast.Attributes.BaseDamage / 100f;

                RaycastHit objectHit;
                Ray forward = new Ray(parent.Eyes.position, parent.Eyes.forward);

                if (Physics.Raycast(forward, out objectHit, spellDistance)) // Creates a raycast to see if eyes are hiting something
                {
                    Vector3 direction = parent.Hand.Direction(objectHit.point); // Direction to object
                    Ray spellToObjectHit = new Ray(parent.Hand.position, direction); // Creates new ray with that direction

                    if (Physics.Raycast(spellToObjectHit, out objectHit, spellDistance + 0.1f)) // Creates raycast with new ray
                    {
                        if (objectHit.collider.gameObject.TryGetComponentInParent(out IDamageable damageable))
                        {
                            damageable.TakeDamage(damageMultiplier * parent.Spell.Damage, parent.Spell.Element);
                        }
                    }
                }
                else
                {

                }

                // Burns mana
                parent.WhoCast.ReduceMana(parent.Spell.ManaCost);
            }
            else
            {
                // Does nothing
            }

            parent.LastTimeHit = Time.time;
        }
    }

    public override void ContinuousFixedUpdateBehaviour(SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    public override void HitBehaviour(Collider other, SpellBehaviourContinuous parent)
    {
        // Left blank on purpose
    }
}
