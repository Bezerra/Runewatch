using UnityEngine;

/// <summary>
/// Class responsible for triggering currency behaviour.
/// </summary>
public class Currency : MonoBehaviour
{
    [SerializeField] private CurrencySO currencySO;

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

            gameObject.SetActive(false);
        }
    }
}
