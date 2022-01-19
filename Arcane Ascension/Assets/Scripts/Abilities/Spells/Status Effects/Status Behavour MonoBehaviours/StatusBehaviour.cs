using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Class responsible for running status logic.
/// </summary>
public class StatusBehaviour : MonoBehaviour
{
    private readonly Vector3 STATUSOFFSET = new Vector3(0, 0.5f, 0f);

    /// <summary>
    /// Parent spell of this 
    /// </summary>
    public ISpell Spell { get; private set; }

    /// <summary>
    /// Spell behaviour that created this status.
    /// </summary>
    public SpellBehaviourAbstract ParentSpell { get; private set; }

    /// <summary>
    /// Time of spawn.
    /// </summary>
    public float TimeSpawned { get; set; }

    /// <summary>
    /// Property to know if this status effect is currently taking effect or not yet.
    /// Set only on status behaviours start behaviours.
    /// </summary>
    public bool EffectActive { get; set; }

    private GameObject statusEffectGameobject;

    /// <summary>
    /// Effect type.
    /// </summary>
    public StatusEffectType EffectType
    {
        set
        {
            if (value != StatusEffectType.Null)
            {
                statusEffectGameobject =
                    StatusEffectPoolCreator.Pool.InstantiateFromPool(value.ToString());
            }
        }
    }

    /// <summary>
    /// Post process on player camera.
    /// </summary>
    public Volume PostProcessEffect { get; set; }

    /// <summary>
    /// Last time the effect was applied.
    /// </summary>
    public float LastTimeHit { get; set; }

    private Stats characterHit;

    /// <summary>
    /// Which character is being affected by this spell.
    /// </summary>
    public Stats CharacterHit
    {
        get => characterHit;
        private set
        {
            characterHit = value;
            if (characterHit != null)
                characterHit.EventDeath += DisableStatusGameObject;
        }
    }

    /// <summary>
    /// Which character cast the spell of this status.
    /// </summary>
    public Stats WhoCast { get; private set; }

    private void OnDisable()
    {
        if(characterHit != null)
            characterHit.EventDeath -= DisableStatusGameObject;

        statusEffectGameobject?.SetActive(false);
        statusEffectGameobject = null;
        EffectActive = false;
        TimeSpawned = 0;
        EffectType = StatusEffectType.Null;

        Initialize(null, null, null, null);
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable/Start in order to prevent bugs.
    /// </summary>
    public void TriggerStartBehaviour()
    {
        if(Spell != null && WhoCast != null && characterHit != null)
        {
            TimeSpawned = Time.time;
            Spell.StatusBehaviour.StartBehaviour(this);
        }
        else
        {
            DisableStatusGameObject();
        }
    }

    private void Update()
    {
        // To reach update, spell has to be different than null, so it doesn't need check in here
        Spell.StatusBehaviour.ContinuousUpdateBehaviour(this);
    }

    private void FixedUpdate()
    {
        // To reach fixed update, spell has to be different than null, so it doesn't need check in here
        transform.position = characterHit.transform.position;

        if(statusEffectGameobject != null)
            statusEffectGameobject.transform.position = 
                transform.position + STATUSOFFSET;
    }

    /// <summary>
    /// Disables status gameobject.
    /// </summary>
    /// <param name="emptyVariable">Variable to match on death event.</param>
    public void DisableStatusGameObject(Stats emptyVariable = null) =>
        gameObject.SetActive(false);

    /// <summary>
    /// Sets crucial variables.
    /// </summary>
    /// <param name="spell">Spell cast.</param>
    /// <param name="whoCast">Who cast the spell</param>
    /// <param name="characterHit">Who was hit by the spell.</param>
    /// <param name="parentSpell">Spell behaviour that created this status.</param>
    public void Initialize(ISpell spell, Stats whoCast, Stats characterHit, 
        SpellBehaviourAbstract parentSpell)
    {
        Spell = spell;
        WhoCast = whoCast;
        CharacterHit = characterHit;
        ParentSpell = parentSpell;
    }
}