/// <summary>
/// Interface implemented by classes with currency.
/// </summary>
public interface ICurrency
{
    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency.</param>
    void SpendCurrency(Currency currency, int amount);

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency.</param>
    void GainCurrency(Currency currency, int amount);

    /// <summary>
    /// Checks if currency can be spent.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency to spend.</param>
    /// <returns>Returns true if currency can be spent.</returns>
    bool CanSpend(Currency currency, int amount);
}
