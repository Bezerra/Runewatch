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
    public byte CurrentPierceHitQuantity { get; set; }
    public byte CurrentWallHitQuantity { get; set; }
    public Rigidbody Rb { get; private set; }
    public SphereCollider ColliderTrigger { get; private set; }

    private GameObject lastHitGameObject;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ColliderTrigger = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        CurrentPierceHitQuantity = 0;
        CurrentWallHitQuantity = 0;
        lastHitGameObject = null;
        TimeSpawned = Time.time;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour()
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviour)
            behaviour.StartBehaviour(this);
    }

    private void Update()
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviour)
            behaviour.ContinuousUpdateBehaviour(this);
    }

    private void FixedUpdate()
    {
        foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviour)
            behaviour.ContinuousFixedUpdateBehaviour(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lastHitGameObject != other.gameObject)
        {
            // If this spell is hitting something for the first time
            TimeOfImpact = Time.time;
            foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviour)
                behaviour.HitTriggerBehaviour(other, this);

            lastHitGameObject = other.gameObject;
        }
    }
}
