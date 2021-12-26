using UnityEngine;

/// <summary>
/// Class responsible for running status logic.
/// </summary>
public class StatusBehaviour : MonoBehaviour
{
    /// <summary>
    /// Parent spell of this 
    /// </summary>
    public ISpell Spell { get; private set; }

    /// <summary>
    /// Time of spawn.
    /// </summary>
    public float TimeSpawned { get; set; }

    /// <summary>
    /// Property to know if this status effect is currently taking effect or not yet.
    /// Set only on status behaviours start behaviours.
    /// </summary>
    public bool EffectActive { get; set; }

    /// <summary>
    /// VFX of the effect.
    /// </summary>
    public GameObject PrefabVFX { get; set; }

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

        PrefabVFX = null;
        EffectActive = false;
        TimeSpawned = 0;

        Initialize(null, null, null);
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
    public void Initialize(ISpell spell, Stats whoCast, Stats characterHit)
    {
        Spell = spell;
        WhoCast = whoCast;
        CharacterHit = characterHit;
    }
}