using UnityEngine;

/// <summary>
/// Scriptable object responsible for self healing.
/// </summary>
[CreateAssetMenu(menuName = "Spells/Spell Behaviour/Spell Behaviour Self Heal", fileName = "Spell Behaviour Self Heal")]
sealed public class SpellBehaviourOneShotSelfHealSO : SpellBehaviourAbstractOneShotSO
{
    [SerializeField] private StatsType healingStats;

    public override void StartBehaviour(SpellBehaviourOneShot parent)
    {
        IHealable character = parent.WhoCast;
        character.Heal(parent.Spell.Damage, healingStats);
    }

    public override void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // Disables spell after it reached max time
        if (Time.time - parent.TimeSpawned > parent.Spell.MaxTime)
            parent.gameObject.SetActive(false);
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
