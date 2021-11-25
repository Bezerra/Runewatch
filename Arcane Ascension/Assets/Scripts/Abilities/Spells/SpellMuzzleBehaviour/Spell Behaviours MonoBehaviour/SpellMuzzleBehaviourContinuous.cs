using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Monobehaviour for spell muzzles.
/// </summary>
public class SpellMuzzleBehaviourContinuous : SpellMuzzleBehaviourAbstract
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public override ISpell Spell { get; set; }
    public SpellBehaviourContinuous SpellMonoBehaviour { get; set; }
    public VisualEffect MuzzleEffect { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MuzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        EffectPlay();

        if (Spell != null)
        {
            foreach (SpellMuzzleBehaviourAbstractContinuousSO muzzleBehaviour in Spell.MuzzleBehaviourContinuous)
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
            foreach (SpellMuzzleBehaviourAbstractContinuousSO muzzleBehaviour in Spell.MuzzleBehaviourContinuous)
                muzzleBehaviour.ContinuousUpdateBehaviour(this);
        }
    }
}
