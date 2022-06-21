using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with currency for a player.
/// </summary>
[InlineEditor]
[CreateAssetMenu(menuName = "Create Once", fileName = "Currency")]
public class CurrencySO : ScriptableObject
{
    [Range(0, 10000)] [SerializeField] private int defaultGold;
    [Range(0, 10000)] [SerializeField] private int defaultArcanePower;

    public int DefaultGold => defaultGold;
    public int DefaultArcanePower => defaultArcanePower;

    public int Gold { get; private set; }
    public int ArcanePower { get; private set; }

    private CharacterSaveDataController characterSaveDataController;

    /// <summary>
    /// Gains currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to gain.</param>
    public void GainCurrency(CurrencyType currency, int amount)
    {
        if (currency == CurrencyType.Gold)
        {
            Gold += amount;
        }
        else
        {
            ArcanePower += amount;
        }
    }

    /// <summary>
    /// Spends currency.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount to spend.</param>
    public void SpendCurrency(CurrencyType currency, int amount)
    {
        if (currency == CurrencyType.Gold)
        {
            if (Gold - amount >= 0) Gold -= amount;
            else Gold = 0;
        }
        else
        {
            if (ArcanePower - amount > 0) ArcanePower -= amount;
            else ArcanePower = 0;
            if (characterSaveDataController == null)
            {
                characterSaveDataController =
                    FindObjectOfType<CharacterSaveDataController>();

                characterSaveDataController.SaveData.ArcanePower = ArcanePower;
            }
            else
            {
                characterSaveDataController.SaveData.ArcanePower = ArcanePower;
            }
        }
    }

    /// <summary>
    /// Checks if currency can be spent.
    /// </summary>
    /// <param name="currency">Type of currency.</param>
    /// <param name="amount">Amount of currency to spend.</param>
    /// <returns>Returns true if currency can be spent.</returns>
    public bool CanSpend(CurrencyType currency, int amount)
    {
        if (currency == CurrencyType.Gold)
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

    public void ResetCurrency()
    {
        Gold = 0;
        ArcanePower = 0;
    }

    private void OnEnable()
    {
        ResetCurrency();
        characterSaveDataController =
            FindObjectOfType<CharacterSaveDataController>();
    }
}
