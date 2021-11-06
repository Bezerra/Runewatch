using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with currency for a player.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once/Player Currency", fileName = "Player Currency")]
public class PlayerCurrencySO : ScriptableObject
{
    [Range(0, 10000)] [SerializeField] private uint defaultGold;
    [Range(0, 10000)] [SerializeField] private uint defaultArcanePower;

    public uint Gold { get; private set; }
    public uint ArcanePower { get; private set; }

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to gain.</param>
    public void GainCurrency(Currency currency, uint amount)
    {
        if (currency == Currency.Gold) Gold += amount;
        else ArcanePower += amount;

        Debug.Log(Gold);
        Debug.Log(ArcanePower);
    }

    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to spend.</param>
    public void SpendCurrency(Currency currency, uint amount)
    {
        if (currency == Currency.Gold) Gold -= amount;
        else ArcanePower -= amount;
        Debug.Log(Gold);
        Debug.Log(ArcanePower);
    }

    private void OnEnable()
    {
        Gold = defaultGold;
        ArcanePower = defaultArcanePower;
    }
}
