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
    void SpendCurrency(Currency currency, uint amount);

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency.</param>
    void GainCurrency(Currency currency, uint amount);
}
