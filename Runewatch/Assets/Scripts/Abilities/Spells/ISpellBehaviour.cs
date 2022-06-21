/// <summary>
/// Interface implemented by objects with spell behaviours.
/// </summary>
public interface ISpellBehaviour: ISpellOneShotBehaviour
{
    SpellCastType CastType { get; }
    float Speed { get; }
    float Cooldown { get; }
    float CooldownCounter { get; set; }
    float MaximumDistance { get; }

    /// <summary>
    /// Spell's attack behaviour.
    /// </summary>
    AttackBehaviourAbstractSO AttackBehaviour { get; }
}
