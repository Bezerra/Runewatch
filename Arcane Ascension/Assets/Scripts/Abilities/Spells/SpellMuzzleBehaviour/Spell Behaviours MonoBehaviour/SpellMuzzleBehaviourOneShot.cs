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

    public Transform Eyes { get; set; }

    public Transform Hand { get; set; }

    public Stats WhoCast { get; set; }

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

    private void OnDisable()
    {
        Spell = null;
        TimeSpawned = Time.time;
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

    /// <summary>
    /// Keeps running on fixed update.
    /// </summary>
    private void FixedUpdate()
    {
        if (Spell != null)
        {
            foreach (SpellMuzzleBehaviourAbstractOneShotSO muzzleBehaviour in Spell.MuzzleBehaviourOneShot)
                muzzleBehaviour.ContinuousFixedUpdateBehaviour(this);
        }
    }

    /// <summary>
    /// Keeps running on late update.
    /// </summary>
    private void LateUpdate()
    {
        if (Spell != null)
        {
            foreach (SpellMuzzleBehaviourAbstractOneShotSO muzzleBehaviour in Spell.MuzzleBehaviourOneShot)
                muzzleBehaviour.ContinuousLateUpdateBehaviour(this);
        }
    }
}
