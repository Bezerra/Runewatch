using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Monobehaviour for one shot spell hits.
/// </summary>
public class SpellOnHitBehaviourContinuous : SpellOnHitBehaviourAbstract
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public override ISpell Spell { get; set; }
    public override float TimeSpawned { get; set; }
    public SpellBehaviourContinuous SpellMonoBehaviour { get; set; }
    public VisualEffect HitEffect { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        HitEffect = GetComponentInChildren<VisualEffect>();
    }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;
        EffectPlay();

        if (Spell != null)
        {
            foreach (SpellOnHitBehaviourAbstractContinuousSO onHitBehaviour in Spell.OnHitBehaviourContinuous)
                onHitBehaviour.StartBehaviour(this);
        }
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        if (Spell != null)
        {
            foreach (SpellOnHitBehaviourAbstractContinuousSO onHitBehaviour in Spell.OnHitBehaviourContinuous)
                onHitBehaviour.ContinuousUpdateBehaviour(this);
        }
    }
}
