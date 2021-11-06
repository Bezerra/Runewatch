/// <summary>
/// Interface implemented by characters with Mana stats.
/// </summary>
public interface IMana: IHealable
{
    /// <summary>
    /// Current mana of a character.
    /// </summary>
    float Mana { get; }

    /// <summary>
    /// Gets max mana of a character.
    /// </summary>
    float MaxMana { get; }

    /// <summary>
    /// Reduces mana of a character.
    /// </summary>
    /// <param name="amount">Amount to reduce.</param>
    void ReduceMana(float amount);
}
