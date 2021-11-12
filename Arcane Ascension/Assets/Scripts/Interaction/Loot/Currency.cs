using UnityEngine;

/// <summary>
/// Class responsible for triggering currency behaviour.
/// </summary>
public class Currency : MonoBehaviour
{
    [SerializeField] private CurrencyInformationSO currencySO;
    [SerializeField] private LootAndInteractionSoundType lootType;

    public Vector2 Amount { get => currencySO.Amount; set => currencySO.Amount = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (other.TryGetComponent<IUseCurrency>(out IUseCurrency currency))
            {
                currency.GainCurrency(currencySO.CurrencyType, 
                    (int)Random.Range(currencySO.Amount.x, currencySO.Amount.y));
            }
            LootSoundPoolCreator.Pool.InstantiateFromPool(lootType.ToString(), transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        }
    }
}
