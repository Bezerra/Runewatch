using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for player's currency behaviour.
/// </summary>
public class PlayerCurrency : MonoBehaviour, IUseCurrency, ISaveable
{
    private Player player;

    /// <summary>
    /// Property to get gold (item 1) and arcane power (item 2).
    /// </summary>
    public (int, int) Quantity => 
        (player.AllValues.Currency.Gold, player.AllValues.Currency.ArcanePower);

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private IEnumerator Start()
    {
        // Lets everything load on player first
        YieldInstruction wffu = new WaitForFixedUpdate();
        yield return wffu;
        yield return wffu;

        // Initial gold with passives
        GainCurrency(CurrencyType.Gold, 
            FindObjectOfType<CharacterSaveDataController>().SaveData.Wealth);
    }

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to gain.</param>
    public void GainCurrency(CurrencyType currency, int amount)
    {
        player.AllValues.Currency.GainCurrency(currency, amount);
        OnEventCurrencyUpdate(
            player.AllValues.Currency.Gold, player.AllValues.Currency.ArcanePower);
    }

    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to spend.</param>
    public void SpendCurrency(CurrencyType currency, int amount)
    {
        if (CanSpend(currency, amount))
        {
            OnEventSpendMoney();
            OnEventCurrencyUpdate(
                player.AllValues.Currency.Gold, player.AllValues.Currency.ArcanePower);
        }
        player.AllValues.Currency.SpendCurrency(currency, amount);
    }

    /// <summary>
    /// Checks if currency can be spent.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency to spend.</param>
    /// <returns>Returns true if currency can be spent.</returns>
    public bool CanSpend(CurrencyType currency, int amount) =>
        player.AllValues.Currency.CanSpend(currency, amount);

    /// <summary>
    /// Saves currency.
    /// </summary>
    /// <param name="saveData">SaveData to save to.</param>
    public void SaveCurrentData(RunSaveData saveData)
    {
        // currency
        if (this != null) // Do not remove <
        {
            saveData.PlayerSavedData.Gold = player.AllValues.Currency.Gold;
            saveData.PlayerSavedData.ArcanePower = player.AllValues.Currency.ArcanePower;
        }
    }

    /// <summary>
    /// Loads currency.
    /// </summary>
    /// <param name="saveData">SaveData to load from.</param>
    /// <returns>WFFU.</returns>
    public IEnumerator LoadData(RunSaveData saveData)
    {
        yield return new WaitForFixedUpdate();

        // Currency
        if (this != null) // Do not remove <
        {
            player.AllValues.Currency.GainCurrency(CurrencyType.Gold, saveData.PlayerSavedData.Gold);
            player.AllValues.Currency.GainCurrency(CurrencyType.ArcanePower, saveData.PlayerSavedData.ArcanePower);
        }
    }

    // Registered on playersounds
    protected virtual void OnEventSpendMoney() => EventSpendMoney?.Invoke();
    public event Action EventSpendMoney;

    // Registered on PlayerUI
    protected virtual void OnEventCurrencyUpdate(float gold, float ap) => 
        EventCurrencyUpdate?.Invoke(gold, ap);
    public event Action<float, float> EventCurrencyUpdate;
}
