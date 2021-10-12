using UnityEngine;

/// <summary>
/// Monobehaviour for spell hits.
/// </summary>
public class SpellOnHitBehaviour : MonoBehaviour
{
    public SpellOnHitBehaviourSO OnHitBehaviour { get; set; }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable() =>
        OnHitBehaviour.StartBehaviour(this);

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update() =>
        OnHitBehaviour.ContinuousUpdateBehaviour(this);
}
