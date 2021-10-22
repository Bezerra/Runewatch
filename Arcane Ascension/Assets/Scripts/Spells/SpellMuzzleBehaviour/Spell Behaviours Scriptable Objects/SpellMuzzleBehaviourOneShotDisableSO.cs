using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell muzzle gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Muzzle Behaviour/One Shot/Spell Muzzle Behaviour Disable", 
    fileName = "Spell Muzzle Behaviour Disable")]
public class SpellMuzzleBehaviourOneShotDisableSO : SpellMuzzleBehaviourAbstractOneShotSO
{
    [Range(0, 20)] [SerializeField] private byte disableAfterSeconds;

    /// <summary>
    /// Executed when the spell is enabled.
    /// </summary>
    /// <param name="parent">Parent Spell Muzzle monobehaviour.</param>
    public override void StartBehaviour(SpellMuzzleBehaviourOneShot parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Executed every update.
    /// </summary>
    /// <param name="parent">Parent Spell Muzzle monobehaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellMuzzleBehaviourOneShot parent)
    {
        if (Time.time - parent.TimeSpawned > disableAfterSeconds)
        {
            if (parent.EffectNotNull)
            {
                parent.EffectStop();

                if (parent.EffectGetAliveParticles == 0)
                    parent.DisableMuzzleSpell();

                return;
            }
        }

        if (parent.EffectNotNull)
        {
            parent.DisableMuzzleSpell();
        }

        // Safety measure if too much time passes and the effect didn't get disabled
        if (Time.time - parent.TimeSpawned > disableAfterSeconds * 3)
        {
            parent.DisableMuzzleSpell();
        }
    }
}
