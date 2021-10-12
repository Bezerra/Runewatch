using UnityEngine;

/// <summary>
/// Abstract class responsible for disabling the spell hit gameobject.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Hit Behaviour/Spell Hit Behaviour Disable", fileName = "Spell Hit Behaviour Disable")]
sealed public class SpellOnHitBehaviourDisable : SpellOnHitBehaviourSO
{
    [Range(0, 20)][SerializeField] private byte disableAfterSeconds;

    public override void StartBehaviour(SpellOnHitBehaviour parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviour parent)
    {
        if (Time.time - parent.TimeSpawned > disableAfterSeconds)
            DisableSpell(parent);
    }
}
