using UnityEngine;

/// <summary>
/// Parent spell behaviour for continuous spells.
/// </summary>
public class SpellBehaviourContinuous : SpellBehaviourAbstract
{
    [SerializeField] protected SpellContinuousSO spell;

    public override ISpell Spell => spell;

    /// <summary>
    /// Used to control everytime the spell hits something.
    /// </summary>
    public float LastTimeHit { get; set; }
    public LineRenderer LineRender { get; private set; }
    public float CurrentSpellDistance { get; set; }
    public Collider DamageableTarget { get; set; }
    public RaycastHit HitPoint { get; set; }
    /// <summary>
    /// Used to control hit spawn on wall.
    /// </summary>
    public float LastTimeSpellWallHit { get; set; }

    private void Awake()
    {
        LineRender = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        LastTimeHit = Time.time;
        LastTimeSpellWallHit = Time.time;
    }

    private void OnDisable()
    {
        LastTimeSpellWallHit = 0;
        CurrentSpellDistance = 0;
        HitPoint = default;
        DamageableTarget = null;
    }

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour()
    {
        foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
            behaviour.StartBehaviour(this);
    }

    private void Update()
    {
        // If who cast doesn't have enough mana, it will immediatly cancel the spell
        if (WhoCast != null)
        {
            if (WhoCast.Mana - spell.ManaCost <= 0)
            {
                foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
                    behaviour.DisableSpell(this);
            }
            else
            {
                foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
                    behaviour.ContinuousUpdateBehaviour(this); 
            }
        }

        // Every spell.Cooldown  time, the spell will update last hit current time
        // This time is used inside behaviours to have control of the update times
        if (Time.time > LastTimeHit + Spell.Cooldown)
        {
            LastTimeHit = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // If who cast doesn't have enough mana, it will immediatly cancel the spell
        if (WhoCast != null)
        {
            foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
                behaviour.ContinuousFixedUpdateBehaviour(this);
        }
    }
}
