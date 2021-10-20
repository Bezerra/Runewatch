using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Monobehaviour for one shot spell hits.
/// </summary>
public class SpellOnHitBehaviourOneShot : SpellOnHitBehaviourAbstract
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public override ISpell Spell { get; set; }
    public VisualEffect HitEffect { get; set; }
    public override float TimeSpawned { get; set; }

    private void Awake()
    {
        HitEffect = GetComponentInChildren<VisualEffect>();
    }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;

        Spell?.OnHitBehaviourOneShot.StartBehaviour(this);
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        Spell?.OnHitBehaviourOneShot.ContinuousUpdateBehaviour(this);
    }
}
        
