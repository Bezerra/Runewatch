using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Parent spell behaviour for one shot spells.
/// </summary>
public class SpellBehaviourOneShot : SpellBehaviourAbstract
{
    [SerializeField] protected SpellOneShotSO spell;

    public override ISpell Spell => spell;

    /// <summary>
    /// Time of spell spawn.
    /// </summary>
    public float TimeSpawned { get; set; }

    /// <summary>
    /// Time of spell impact.
    /// </summary>
    public float TimeOfImpact { get; private set; }

    /// <summary>
    /// Property for rigidbody.
    /// </summary>
    public Rigidbody Rb { get; private set; }

    /// <summary>
    /// Property por spell collider.
    /// </summary>
    public CapsuleCollider ColliderTrigger { get; private set; }

    // Whenever the spells colide with anything it sets this variable to that gameobject hit
    private GameObject lastHitGameObject;

    /// <summary>
    /// Used for pierce behaviours.
    /// </summary>
    public byte CurrentPierceHitQuantity { get; set; }

    /// <summary>
    /// Used for wall bouncing behaviours.
    /// </summary>
    public byte CurrentWallHitQuantity { get; set; }

    /// <summary>
    /// Is set to true if spells collides with something that's supposed to stop it.
    /// </summary>
    public bool DisableSpellAfterCollision { get; set; }

    /// <summary>
    /// Set to true when the spell starts moving.
    /// </summary>
    public bool SpellStartedMoving { get; set; }

    /// <summary>
    /// Set when area hover is spawned.
    /// </summary>
    public GameObject AreaHoverVFX { get; set; }

    /// <summary>
    /// Area Hit for hover vfx.
    /// </summary>
    public RaycastHit AreaHoverAreaHit { get; set; }

    /// <summary>
    /// Deals damage once with aoe hover one shot release spells.
    /// </summary>
    public bool AreaHoverDealtDamage { get; set; }

    /// <summary>
    /// Updated everytime the spell damages an enemy.
    /// </summary>
    public float LastTimeDamaged { get; set; }

    /// <summary>
    /// Disables projectile immediatly.
    /// </summary>
    public bool DisableImmediatly { get; set; }

    /// <summary>
    /// Used so all behaviours can work properly without being afected by reflection first.
    /// </summary>
    public bool ProjectileReflected { get; set; }

    /// <summary>
    /// Raycast hit of projectile reflection.
    /// </summary>
    public RaycastHit ProjectileReflectedHit { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Rb = GetComponent<Rigidbody>();
        ColliderTrigger = GetComponent<CapsuleCollider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EffectPlay();
        SpellStartedMoving = false;
        TimeOfImpact = Time.time + Mathf.Infinity; // algum bug? meter Time.time
        LastTimeDamaged = Time.time + Mathf.Infinity;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        AreaHoverAreaHit = default;
        AreaHoverVFX = null;
        SpellStartedMoving = false;
        DisableSpellAfterCollision = false;
        CurrentPierceHitQuantity = 0;
        CurrentWallHitQuantity = 0;
        lastHitGameObject = null;
        ProjectileReflected = false;
        AreaHoverDealtDamage = false;
        DisableImmediatly = false;

        if (Rb != null) Rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour()
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
            behaviour.StartBehaviour(this);
    }

    private void Update()
    {
        if (SpellStartedMoving)
        {
            foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
                behaviour.ContinuousUpdateBehaviour(this);
        }
        else
        {
            foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
                behaviour.ContinuousUpdateBeforeSpellBehaviour(this);
        }
    }

    private void FixedUpdate()
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
            behaviour.ContinuousFixedUpdateBehaviour(this);
    }

    /// <summary>
    /// Will check if it's a player or an enemy. If it's the same enemy or player hit twice in a row
    /// it will ignore that hit. Else it will hit everything normally.
    /// </summary>
    /// <param name="other">Collider of collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponentInParent<SelectionBase>(out SelectionBase component))
        {
            // If the enemy or player hit is DIFFERENT than the last one
            if (lastHitGameObject != component.gameObject)
            {
                PositionOnHit = transform.position;

                // If the enemy or player layer hit is DIFFERENT than the layer of who is casting the spell
                // (meaning enemies won't damage them selves)
                if (LayerOfWhoCast != component.gameObject.layer)
                {
                    // If this spell is hitting something for the first time
                    TimeOfImpact = Time.time;
                    foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
                        behaviour.HitTriggerBehaviour(other, this);

                    // Updates last character hit variables
                    LayerOfCharacterHit = component.gameObject.layer;
                    lastHitGameObject = component.gameObject;
                }
            }
            // If the enemy or player is the same as the last one, it will ignore this hit and the rest of the code
            else
            {
                // Ignores the rest of the code
                return;
            }
        }
        else
        {
            // If it wasn't a player or enemy it will hit normally
            if (lastHitGameObject != other.gameObject)
            {
                // If this spell is hitting something for the first time
                TimeOfImpact = Time.time;

                PositionOnHit = transform.position;

                Vector3 directionToInitialSpawn = transform.Direction(PositionOnHit);

                // Direction to do a raycast.
                // Uses directionToInitialSpawn so the hit will be a little behind the wall to prevent bugs
                Ray direction = new Ray(
                    transform.position + directionToInitialSpawn * 0.1f,
                    ((transform.position + directionToInitialSpawn * 0.1f).
                    Direction(other.ClosestPoint(transform.position))));

                // Casrts a raycast to that closest point on the collider
                if (Physics.Raycast(direction, out RaycastHit spellHitPoint))
                {
                    // This code prevents the spell colliding with multiple colliders that have the same
                    // normal. For example if a wall is divided in 4 colliders, and the player aims
                    // towards the center point of those 4 colliders, the spell will only trigger hit
                    // once and ignore the other 3 colliders
                    if (Vector3.Dot(transform.forward, spellHitPoint.normal) > 0)
                    {
                        return;
                    }

                    // If it passes the code on top, triggers hit
                    foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviourOneShot)
                        behaviour.HitTriggerBehaviour(other, this);

                    lastHitGameObject = other.transform.gameObject;
                }
            }
        }
    }



    public Vector3 TEMP { get; set; }
    public Ray TEMPRAY { get; set; }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(TEMP, 0.2f);
        Gizmos.DrawRay(TEMPRAY);
    }
}
