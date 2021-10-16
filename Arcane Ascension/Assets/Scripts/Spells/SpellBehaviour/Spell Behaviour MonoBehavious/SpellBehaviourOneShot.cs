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
    public byte CurrentHitQuantity { get; set; }
    public Rigidbody Rb { get; private set; }
    public SphereCollider ColliderTrigger { get; private set; }

    private IList<GameObject> alreadyHitGameObjects;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        ColliderTrigger = GetComponent<SphereCollider>();
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
        if (alreadyHitGameObjects.Contains(other.gameObject) == false)
        {
            // If this spell is hitting something for the first time
            TimeOfImpact = Time.time;
            foreach (SpellBehaviourAbstractOneShotSO behaviour in spell.SpellBehaviour)
                behaviour.HitTriggerBehaviour(other, this);

            alreadyHitGameObjects.Add(other.gameObject);
        }
    }
}
