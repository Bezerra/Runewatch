using UnityEngine;

/// <summary>
/// Class responsible for triggering currency behaviour.
/// </summary>
public class Currency : MonoBehaviour, ICurrency
{
    [Header("Currency default information")]
    [SerializeField] private CurrencyInformationSO currencySO;

    [Header("Variable used to create a sound")]
    [SerializeField] private LootAndInteractionSoundType lootType;

    /// <summary>
    /// Amount contained on this currency.
    /// </summary>
    public Vector2 Amount { get => currencySO.Amount; set => currencySO.Amount = value; }

    /// <summary>
    /// Type of this currency.
    /// </summary>
    public CurrencyType CurrencyType => currencySO.CurrencyType;

    /// <summary>
    /// Multiplier of the amount. This is set by mobs that drop gold if the
    /// player has Pickpocket passive.
    /// </summary>
    public float AmountMultiplier { get; set; }

    /// <summary>
    /// Resets this amount.
    /// </summary>
    private void OnDisable()
    {
        Amount = currencySO.Amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (other.TryGetComponent<IUseCurrency>(out IUseCurrency currency))
            {
                float amountToGain = Random.Range(Amount.x, Amount.y);

                currency.GainCurrency(currencySO.CurrencyType,
                    (int)(amountToGain + (AmountMultiplier * amountToGain * 0.01f)));
            }
            LootSoundPoolCreator.Pool.InstantiateFromPool(
                lootType.ToString(), transform.position, Quaternion.identity);

            transform.parent.gameObject.SetActive(false);
        }
    }
}
