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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (other.TryGetComponent<IUseCurrency>(out IUseCurrency currency))
            {
                currency.GainCurrency(currencySO.CurrencyType, 
                    (int)Random.Range(Amount.x, Amount.y));
            }
            LootSoundPoolCreator.Pool.InstantiateFromPool(
                lootType.ToString(), transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        }
    }
}
