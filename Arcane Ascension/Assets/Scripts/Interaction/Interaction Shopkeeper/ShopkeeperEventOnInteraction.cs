using UnityEngine;

/// <summary>
/// Class used by shopkeeper inventory gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class ShopkeeperEventOnInteraction : AbstractEventOnInteraction, IInterectable
{
    /// <summary>
    /// Property with player currency to be used on external classes.
    /// </summary>
    public PlayerCurrency PlayerCurrency { get; private set; }

    // Item price
    [Range(1, 100000)] [SerializeField] private int price;

    /// <summary>
    /// Property with this item's price.
    /// </summary>
    public int Price => price;

    protected override void Awake()
    {
        base.Awake();
        PlayerCurrency = FindObjectOfType<PlayerCurrency>();
    }

    /// <summary>
    /// Executes an event.
    /// </summary>
    public override void Execute()
    {
        if (eventOnInteraction.Count > 0)
        {
            playerInteraction.LastObjectInteracted = this.gameObject;

            if (PlayerCurrency.CanSpend(CurrencyType.Gold, price))
            {
                foreach (EventAbstractSO eve in eventOnInteraction)
                {
                    if (eve != null)
                    {
                        eve.Execute(this);
                    }
                }

                PlayerCurrency.SpendCurrency(CurrencyType.Gold, price);
                eventOnInteraction.Clear();
                Destroy(GetComponentInParent<ShopkeeperInventorySlot>().gameObject);
            }
        }
        else
        {
            Debug.Log("Has no event to execute");
        }
    }
}
