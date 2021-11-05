/// <summary>
/// Interface implemented by passive abilities.
/// </summary>
public interface IPassive
{
    int PassiveTier { get; }
    PassiveType PassiveType { get; }
}
