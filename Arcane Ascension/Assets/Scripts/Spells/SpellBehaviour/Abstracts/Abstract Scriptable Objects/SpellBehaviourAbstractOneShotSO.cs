using UnityEngine;

/// <summary>
/// Abstract scriptable object used to create one shot spell behaviours.
/// </summary>
public abstract class SpellBehaviourAbstractOneShotSO: SpellBehaviourAbstractSO
{
    [Header("This variables is used to disable the spell after colliding with something")]
    [Range(1, 10)] [SerializeField] private float disableAfterSecondsAfterCollision;

    [Header("This variables is used to disable the spell after X seconds")]
    [Range(1, 10)] [SerializeField] private float disableAfterSeconds;

    /// <summary>
    /// Executes on start.
    /// </summary>
    public abstract void StartBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on update.
    /// </summary>
    public virtual void ContinuousUpdateBehaviour(SpellBehaviourOneShot parent)
    {
        // If the spell hits something
        if (parent.Rb.velocity == Vector3.zero)
        {
            if (Time.time - parent.TimeOfImpact > disableAfterSecondsAfterCollision)
                DisableSpell(parent);
        }
        else
        {
            if (Time.time - parent.TimeSpawned > disableAfterSeconds)
                DisableSpell(parent);
        }
    }

    /// <summary>
    /// Executes on fixed update.
    /// </summary>
    public abstract void ContinuousFixedUpdateBehaviour(SpellBehaviourOneShot parent);

    /// <summary>
    /// Executes on hit.
    /// </summary>
    /// <param name="other">Collider.</param>
    public virtual void HitBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        parent.Spell.DamageBehaviour.Damage(other, parent);

        // Creates spell hit
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviour != null)
        {
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, parent.transform.position,
                Quaternion.identity);

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviour>(out SpellOnHitBehaviour onHitBehaviour))
            {
                onHitBehaviour.OnHitBehaviour = parent.Spell.OnHitBehaviour;
            }
        }

        parent.ColliderSphere.enabled = false;
        parent.Rb.velocity = Vector3.zero;
    }
}
