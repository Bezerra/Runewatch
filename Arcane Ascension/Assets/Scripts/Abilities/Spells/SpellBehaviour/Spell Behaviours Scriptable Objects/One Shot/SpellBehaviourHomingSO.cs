using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object responsible for moving the spell towards a target.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Homing", 
    fileName = "Spell Behaviour Homing")]
sealed public class SpellBehaviourHomingSO : SpellBehaviourAbstractOneShotSO
{
    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        Collider[] enemyColliders = 
            Physics.OverlapSphere(parent.transform.position, 
            parent.Spell.MaximumDistance, Layers.EnemyLayer);

        if (enemyColliders.Length > 0)
        {
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].TryGetComponent(out Enemy enemy))
                {
                    if (parent.WhoCast.transform.IsLookingTowards(
                        enemyColliders[i].transform.position))
                    {
                        if (parent.transform.CanSee(enemyColliders[i].transform, Layers.EnemyWithWallsFloor))
                        {
                            parent.HomingTarget = enemy.Eyes;
                            Debug.Log("got eyes");
                        }
                    }
                }
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
        if (parent.HomingTarget != null)
        {
            parent.transform.rotation = Quaternion.LookRotation(parent.HomingTarget.position);
        }
    }

    public override void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }
}
