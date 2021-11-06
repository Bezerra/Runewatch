using UnityEngine;

/// <summary>
/// Class responsible for triggering potions behaviour.
/// </summary>
public class Loot : MonoBehaviour
{
    [SerializeField] private Currency lootType;
    [RangeMinMax(1f, 100000f)][SerializeField] private Vector2 amount;

    public Vector2 Amount { get => amount; set => amount = value; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum)
        {
            if (other.TryGetComponent<ICurrency>(out ICurrency currency))
            {
                currency.GainCurrency(lootType, (uint)Random.Range(amount.x, amount.y));
            }

            Destroy(gameObject);
        }
    }
}
