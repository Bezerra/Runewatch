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
    public virtual void StartBehaviour(SpellBehaviourOneShot parent)
    {
        // Turns collidersphere on because it was turned off when the spell hit something
        if (parent.ColliderNoTrigger != null)
            parent.ColliderNoTrigger.enabled = true;

        // Creates spell muzzle
        // Update() will run from its monobehaviour script
        if (parent.Spell.MuzzleBehaviour != null)
        {
            // Creates spell muzzle
            GameObject spellMuzzleBehaviourGameObject = SpellMuzzlePoolCreator.Pool.InstantiateFromPool(
            parent.Spell.Name, parent.transform.position,
            Quaternion.LookRotation(parent.transform.forward, parent.transform.up));

            if (spellMuzzleBehaviourGameObject.TryGetComponent<SpellMuzzleBehaviour>(out SpellMuzzleBehaviour muzzleBehaviour))
            {
                muzzleBehaviour.Spell = parent.Spell;
            }
        }

        // Starts cooldown of the spell
        if (parent.WhoCast.TryGetComponent<PlayerSpells>(out PlayerSpells playerSpells))
            playerSpells.StartSpellCooldown(playerSpells.ActiveSpell);

        // Takes mana from player
        parent.WhoCast.ReduceMana(parent.Spell.ManaCost);
    }

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
    public virtual void HitNoTriggerBehaviour(Collision other, SpellBehaviourOneShot parent)
    {
        parent.Spell.DamageBehaviour.Damage(other, parent);

        // Creates spell hit
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviour != null)
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, parent.transform.position,
                Quaternion.LookRotation(other.contacts[0].normal, other.collider.transform.up));

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviour>(out SpellOnHitBehaviour onHitBehaviour))
            {
                onHitBehaviour.Spell = parent.Spell;
            }
        }

        parent.ColliderNoTrigger.enabled = false;
        // After the spell is set to zero velocity, it will start a counter to disable it on update behaviour
        parent.Rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Executes on hit.
    /// </summary>
    /// <param name="other">Collider.</param>
    public virtual void HitTriggerBehaviour(Collider other, SpellBehaviourOneShot parent)
    {
        parent.Spell.DamageBehaviour.Damage(other, parent);

        // Creates spell hit
        // Update() will run from its monobehaviour script
        if (parent.Spell.OnHitBehaviour != null)
        {
            // Spawns hit in direction of collider hit normal
            GameObject onHitBehaviourGameObject = SpellHitPoolCreator.Pool.InstantiateFromPool(
                parent.Spell.Name, parent.transform.position,
                Quaternion.LookRotation(
                    (parent.WhoCast.transform.position - parent.transform.position).normalized, other.transform.up));

            if (onHitBehaviourGameObject.TryGetComponent<SpellOnHitBehaviour>(out SpellOnHitBehaviour onHitBehaviour))
            {
                onHitBehaviour.Spell = parent.Spell;
            }
        }

        // After the spell is set to zero velocity, it will start a counter to disable it on update behaviour
        parent.Rb.velocity = Vector3.zero;
    }
}
