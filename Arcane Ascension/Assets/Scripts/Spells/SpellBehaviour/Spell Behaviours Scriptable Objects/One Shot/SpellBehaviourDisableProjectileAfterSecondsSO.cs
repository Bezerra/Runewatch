using UnityEngine;

/// <summary>
/// Scriptable object responsible for disabling projectile after seconds.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/One Shot/Spell Behaviour Disable Projectile After Seconds", 
    fileName = "Spell Behaviour Disable Projectile After Seconds")]
sealed public class SpellBehaviourDisableProjectileAfterSecondsSO : SpellBehaviourAbstractOneShotSO
{
    [Header("This variables is used to disable the spell after X seconds")]
    [Range(1, 10)] [SerializeField] private float disableAfterSeconds;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        if (parent.SpellStartedMoving)
        {
            if (parent.Rb.velocity != Vector3.zero)
            {
                if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                {
                    DisableSpell(parent);
                } 
            }
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
