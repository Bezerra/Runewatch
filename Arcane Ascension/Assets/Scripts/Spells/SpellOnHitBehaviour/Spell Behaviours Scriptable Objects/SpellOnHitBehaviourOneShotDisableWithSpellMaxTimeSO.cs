using UnityEngine;

/// <summary>
/// Scriptable Object responsible for disabling the spell hit gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/One Shot/Spell Hit Behaviour Disable With Spell Max Time", 
    fileName = "Spell Hit Behaviour Disable With Spell Max Time")]
sealed public class SpellOnHitBehaviourOneShotDisableWithSpellMaxTimeSO : SpellOnHitBehaviourAbstractOneShotSO
{
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
        if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime)
            parent.DisableHitSpell();
    }
}
