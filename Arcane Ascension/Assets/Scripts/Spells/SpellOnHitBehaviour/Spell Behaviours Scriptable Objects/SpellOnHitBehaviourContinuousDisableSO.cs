using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell muzzle gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/Continuous/Spell On Hit Behaviour Disable", 
    fileName = "Spell On Hit Behaviour Disable")]
public class SpellOnHitBehaviourContinuousDisableSO : SpellOnHitBehaviourAbstractContinuousSO
{
    [Range(0f, 10f)] [SerializeField] private float disableAfterSeconds;

    public override void StartBehaviour(SpellOnHitBehaviourContinuous parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// While spell ray is active, it will update muzzle position.
    /// Else it will stop the effect (for ex: when the player stops pressing attack key).
    /// </summary>
    /// <param name="parent"></param>
    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviourContinuous parent)
    {
        if (Time.time - parent.TimeSpawned > disableAfterSeconds)
        {
            if (parent.HitEffect != null)
            {
                parent.HitEffect.Stop();

                if (parent.HitEffect.aliveParticleCount == 0)
                    parent.DisableHitSpell();

                return;
            }
        }

        if (parent.HitEffect == null)
        {
            parent.DisableHitSpell();
        }
    }
}
