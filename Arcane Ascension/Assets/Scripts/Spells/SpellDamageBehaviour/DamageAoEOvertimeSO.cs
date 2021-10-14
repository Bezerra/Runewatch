using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for creating a spell's damage behaviour.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Damage Behaviour/Spell Damage Behaviour Forward", fileName = "Spell Damage AoEOvertime")]
public class DamageAoEOvertimeSO : DamageBehaviourAbstractSO
{
    public override void Damage(Collider other, SpellBehaviourAbstract parent)
    {
        Collider[] collisions = Physics.OverlapSphere(
                    other.ClosestPoint(parent.transform.position), parent.Spell.AreaOfEffect, Layers.EnemyWithWalls);

        // Adds all IDamageable characters found to a list
        for (int i = 0; i < collisions.Length; i++)
        {
            // Creates a ray from spell to hit
            Ray dir = new Ray(
                        parent.transform.position,
                        (collisions[i].transform.position - parent.transform.position).normalized);

            if (Physics.Raycast(dir, out RaycastHit characterHit, parent.Spell.AreaOfEffect * 0.5f, Layers.EnemyWithWalls))
            {
                // If the collider is an IDamageable (meaning there wasn't a wall in the ray path)
                if (characterHit.collider.TryGetComponentInParent<IDamageable>(out IDamageable character) &&
                    characterHit.collider.TryGetComponentInParent<Stats>(out Stats stats))
                {
                    // If the target is different than who cast the spell
                    if (stats != parent.WhoCast)
                    {
                        character.TakeDamageOvertime(
                            parent.WhoCast.Attributes.BaseDamageMultiplier * parent.Spell.Damage,
                            parent.Spell.Element,
                            parent.Spell.TimeInterval,
                            parent.Spell.MaxTime);
                    }
                }
            }
        }
    }
}
