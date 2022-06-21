/// <summary>
/// Interface implemented by characters with health.
/// </summary>
public interface IHealth: IHealable
{
    /// <summary>
    /// Gets current health.
    /// </summary>
    float Health { get; }

    /// <summary>
    /// Gets max health of a character.
    /// </summary>
    float MaxHealth { get; }
}
