using UnityEngine;

/// <summary>
/// Monobehaviour for spell muzzles.
/// </summary>
public class SpellMuzzleBehaviour : MonoBehaviour
{
    /// <summary>
    /// This variable is set on spell behaviour after the spell is cast.
    /// </summary>
    public ISpell Spell { get; set; }

    public float TimeSpawned { get; private set; }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;

        Spell?.MuzzleBehaviour.StartBehaviour(this);
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        Spell?.MuzzleBehaviour.ContinuousUpdateBehaviour(this);
    }
}