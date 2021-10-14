using UnityEngine;

/// <summary>
/// Abstract class responsible for disabling the spell hit gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/Spell Hit Behaviour Disable", fileName = "Spell Hit Behaviour Disable")]
sealed public class SpellOnHitBehaviourDisable : SpellOnHitBehaviourSO
{
    [Range(0, 20)][SerializeField] private byte disableAfterSeconds;

    /// <summary>
    /// Executed when the spell is enabled.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void StartBehaviour(SpellOnHitBehaviour parent)
    {
        // Left blank on purpose
    }

    /// <summary>
    /// Executed every update.
    /// </summary>
    /// <param name="parent">Parent SpellOnHit monobehaviour.</param>
    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviour parent)
    {
        if (Time.time - parent.TimeSpawned > disableAfterSeconds)
            DisableHitSpell(parent);
    }
}
