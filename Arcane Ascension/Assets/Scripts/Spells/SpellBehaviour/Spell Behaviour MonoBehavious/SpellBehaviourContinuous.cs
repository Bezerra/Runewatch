using UnityEngine;

/// <summary>
/// Parent spell behaviour for continuous spells.
/// </summary>
public class SpellBehaviourContinuous : SpellBehaviourAbstract
{
    [SerializeField] protected SpellContinuousSO spell;

    public override ISpell Spell => spell;

    // Variables to control spell behaviour
    public float LastTimeHit { get; set; }

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
        }

        foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
            behaviour.ContinuousUpdateBehaviour(this);
    }

    private void FixedUpdate()
    {
        foreach (SpellBehaviourAbstractContinuousSO behaviour in spell.SpellBehaviourContinuous)
            behaviour.ContinuousFixedUpdateBehaviour(this);
    }

    /// <summary>
    /// Immediatly disables spell gameobject.
    /// </summary>
    /// <param name="parent">Spell parent.</param>
    public override void DisableSpell(SpellBehaviourAbstract parent)
    {
        foreach (SpellBehaviourAbstractSO behaviour in spell.SpellBehaviourContinuous)
            behaviour.DisableSpell(parent);
    }
}
