/// <summary>
/// Interface implemented by all passives.
/// </summary>
public interface IPassive<T> : IAbility
{
    T PassiveType { get; }
}
