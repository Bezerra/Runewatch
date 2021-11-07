/// <summary>
/// Interface implemented by passive abilities.
/// </summary>
public interface IPassive: IAbility
{
    PassiveType PassiveType { get; }
    void Execute(PlayerStats playerStats);
}
