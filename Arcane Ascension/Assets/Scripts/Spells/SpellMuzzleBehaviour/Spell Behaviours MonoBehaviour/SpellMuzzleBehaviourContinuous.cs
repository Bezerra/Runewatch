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
    public override float TimeSpawned { get; set; }
    public SpellBehaviourContinuous SpellMonoBehaviour { get; set; }
    public VisualEffect MuzzleEffect { get; private set; }

    private void Awake()
    {
        MuzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        Spell?.MuzzleBehaviourContinuous.StartBehaviour(this);
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        Spell?.MuzzleBehaviourContinuous.ContinuousUpdateBehaviour(this);
    }
}
