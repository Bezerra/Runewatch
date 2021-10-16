using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Parent spell behaviour for one shot spells.
/// </summary>
public class SpellBehaviourOneShot : SpellBehaviourAbstract
{
    // Variables to control spell behaviour
    public float TimeSpawned { get; private set; }
    public float TimeOfImpact { get; private set; }
    public Rigidbody Rb { get; private set; }
    [SerializeField] private SphereCollider colliderNoTrigger;
    public SphereCollider ColliderNoTrigger => colliderNoTrigger;
    [SerializeField] private SphereCollider colliderTrigger;
    public SphereCollider ColliderTrigger => colliderTrigger;

    private IList<GameObject> alreadyHitGameObjects;

    /// <summary>
    /// Gets SpellBehaviourAbstract as SpellBehaviourAbstractOneShotSO.
    /// </summary>
    private SpellBehaviourAbstractOneShotSO SpellBehaviour => spell.SpellBehaviour as SpellBehaviourAbstractOneShotSO;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        alreadyHitGameObjects = new List<GameObject>();
    }

    private void OnEnable()
    {
        TimeSpawned = Time.time;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour() =>
        SpellBehaviour.StartBehaviour(this);

    private void Update() =>
        SpellBehaviour.ContinuousUpdateBehaviour(this);

    private void FixedUpdate() =>
        SpellBehaviour.ContinuousFixedUpdateBehaviour(this);

    private void OnCollisionEnter(Collision other)
    {
        if (colliderNoTrigger != null)
        {
            // Collider is enabled when the behaviour starts
            // If this spell is hitting something for the first time
            if (ColliderNoTrigger.enabled == true)
            {
                TimeOfImpact = Time.time;
                SpellBehaviour.HitNoTriggerBehaviour(other, this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (colliderTrigger != null)
        {
            if (alreadyHitGameObjects.Contains(other.gameObject) == false)
            {
                // If this spell is hitting something for the first time
                TimeOfImpact = Time.time;
                SpellBehaviour.HitTriggerBehaviour(other, this);

                alreadyHitGameObjects.Add(other.gameObject);
            }
        }
    }
}
