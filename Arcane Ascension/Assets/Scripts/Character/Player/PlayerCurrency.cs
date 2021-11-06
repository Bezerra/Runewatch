using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for player's currency behaviour.
/// </summary>
public class PlayerCurrency : MonoBehaviour, ICurrency, ISaveable
{
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to gain.</param>
    public void GainCurrency(Currency currency, uint amount) =>
        player.AllValues.Currency.GainCurrency(currency, amount);

    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to spend.</param>
    public void SpendCurrency(Currency currency, uint amount) =>
        player.AllValues.Currency.SpendCurrency(currency, amount);

    public void SaveCurrentData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator LoadData(SaveData saveData)
    {
        throw new System.NotImplementedException();
    }
}
