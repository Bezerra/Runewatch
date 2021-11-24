using System;

/// <summary>
/// Interface implemented by classes with currency.
/// </summary>
public interface IUseCurrency
{
    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency.</param>
    void SpendCurrency(CurrencyType currency, int amount);

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency.</param>
    void GainCurrency(CurrencyType currency, int amount);

    /// <summary>
    /// Checks if currency can be spent.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency to spend.</param>
    /// <returns>Returns true if currency can be spent.</returns>
    bool CanSpend(CurrencyType currency, int amount);

    /// <summary>
    /// Quantity of gold and arcane power.
    /// </summary>
    (int, int) Quantity { get; }

    event Action<float, float> EventCurrencyUpdate;
}
