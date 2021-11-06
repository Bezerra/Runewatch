using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with currency for a player.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Player Currency", fileName = "Player Currency")]
public class PlayerCurrencySO : ScriptableObject
{
    [Range(0, 10000)] [SerializeField] private int defaultGold;
    [Range(0, 10000)] [SerializeField] private int defaultArcanePower;

    public int Gold { get; private set; }
    public int ArcanePower { get; private set; }

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to gain.</param>
    public void GainCurrency(Currency currency, int amount)
    {
        if (currency == Currency.Gold) Gold += amount;
        else ArcanePower += amount;
    }

    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to spend.</param>
    public void SpendCurrency(Currency currency, int amount)
    {
        if (currency == Currency.Gold)
        {
            if (Gold - amount >= 0) Gold -= amount;
            else Gold = 0;
        }
        else
        {
            if (ArcanePower - amount > 0) ArcanePower -= amount;
            else ArcanePower = 0;
        }
    }

    /// <summary>
    /// Checks if currency can be spent.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency to spend.</param>
    /// <returns>Returns true if currency can be spent.</returns>
    public bool CanSpend(Currency currency, int amount)
    {
        if (currency == Currency.Gold)
        {
            if (Gold - amount >= 0) return true;
            else return false;
        }
        else
        {
            if (ArcanePower - amount >= 0) return true;
            else return false;
        }
    }

    private void OnEnable()
    {
        Gold = defaultGold;
        ArcanePower = defaultArcanePower;
    }
}
