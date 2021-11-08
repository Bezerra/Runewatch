using UnityEngine;

/// <summary>
/// Monobehaviour for spell muzzles.
/// </summary>
public class SpellMuzzleBehaviourOneShot : SpellMuzzleBehaviourAbstract
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public override ISpell Spell { get; set; }

    public override float TimeSpawned { get; set; }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;
        EffectPlay();

        if (Spell != null)
        {
            foreach (SpellMuzzleBehaviourAbstractOneShotSO muzzleBehaviour in Spell.MuzzleBehaviourOneShot)
                muzzleBehaviour.StartBehaviour(this);
        }
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        if (Spell != null)
        {
            foreach (SpellMuzzleBehaviourAbstractOneShotSO muzzleBehaviour in Spell.MuzzleBehaviourOneShot)
                muzzleBehaviour.ContinuousUpdateBehaviour(this);
        }
    }
}
