/// <summary>
/// Interface implemented by passive abilities.
/// </summary>
public interface IPassive
{
    string Name { get; }
    string Description { get; }
    int PassiveTier { get; }
    PassiveType PassiveType { get; }
    void Execute(PlayerStats playerStats);
}
