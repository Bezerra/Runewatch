/// <summary>
/// Interface implemented by objects with spell behaviours.
/// </summary>
public interface ISpellBehaviour
{
    SpellBehaviourAbstractSO SpellBehaviour{ get; }
    SpellCastType CastType { get; }
    float Speed { get; }
    float Cooldown { get; }
    float CooldownCounter { get; set; }
    public SpellOnHitBehaviourSO OnHitBehaviour { get; }
    public AttackBehaviourAbstractSO AttackBehaviour { get; }
}
