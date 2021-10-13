using UnityEngine;

/// <summary>
/// Monobehaviour for spell hits.
/// </summary>
public class SpellOnHitBehaviour : MonoBehaviour
{
    public SpellOnHitBehaviourSO OnHitBehaviour { get; set; }

    public float TimeSpawned { get; private set; }

    /// <summary>
    /// Runs start behaviour when enabled with with pool.
    /// </summary>
    private void OnEnable()
    {
        TimeSpawned = Time.time;
        if (OnHitBehaviour != null)
        {
            OnHitBehaviour.StartBehaviour(this);
        }
    }

    /// <summary>
    /// Keeps running on update.
    /// </summary>
    private void Update()
    {
        if (OnHitBehaviour != null)
        {
            OnHitBehaviour?.ContinuousUpdateBehaviour(this);
        }
    }
}
        
