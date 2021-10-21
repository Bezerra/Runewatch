using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell hit gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/One Shot/Spell Hit Behaviour Disable", fileName = "Spell Hit Behaviour Disable")]
sealed public class SpellOnHitBehaviourOneShotDisableSO : SpellOnHitBehaviourAbstractOneShotSO
{
    [Range(0.1f, 20)][SerializeField] private float disableAfterSeconds;

    /// <summary>
    /// Executed when the spell is enabled.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void StartBehaviour(SpellOnHitBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Executed every update.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviourOneShot parent)
    {
        if (Time.time - parent.TimeSpawned > disableAfterSeconds)
        {
            if (parent.EffectNotNull)
            {
                parent.EffectStop();

                if (parent.EffectGetAliveParticles == 0)
                {
                    parent.DisableHitSpell();
                }
            }
            else
            {
                parent.DisableHitSpell();
            }
        }
    }
}
