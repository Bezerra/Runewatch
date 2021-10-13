
/// <summary>
/// Parent spell behaviour for continuous spells.
/// </summary>
public class SpellBehaviourContinuous : SpellBehaviourAbstract
{
    // Variables to control spell behaviour
    public float LastTimeHit { get; set; }

    /// <summary>
    /// Gets SpellBehaviourAbstract as SpellBehaviourAbstractContinuousSO.
    /// </summary>
    private SpellBehaviourAbstractContinuousSO SpellBehaviour => spell.SpellBehaviour as SpellBehaviourAbstractContinuousSO;

    /// <summary>
    /// Method called after instantiating the spell.
    /// Must be called manually through this method instead of OnEnable or Start in order to prevent bugs.
    /// </summary>
    public override void TriggerStartBehaviour() =>
        SpellBehaviour.StartBehaviour(this);

    private void Update()
    {
        // If who cast doesn't have enough mana, it will immediatly cancel the spell
        if (WhoCast != null)
        {
            if (WhoCast.Mana - spell.ManaCost <= 0)
                SpellBehaviour.DisableSpell(this);
        }

        SpellBehaviour.ContinuousUpdateBehaviour(this);
    }

    private void FixedUpdate() =>
        SpellBehaviour.ContinuousFixedUpdateBehaviour(this);
}
