using UnityEngine;

/// <summary>
/// Interface implemented by currency types.
/// </summary>
public interface ICurrency
{
    /// <summary>
    /// type of currency.
    /// </summary>
    CurrencyType CurrencyType { get; }

    /// <summary>
    /// Amount this currency contains.
    /// </summary>
    Vector2 Amount { get; set; }

    /// <summary>
    /// Multiplier of the amount.
    /// </summary>
    float AmountMultiplier { get; set; }
}
