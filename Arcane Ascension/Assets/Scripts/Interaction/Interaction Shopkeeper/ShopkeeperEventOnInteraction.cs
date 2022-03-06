using System;
using UnityEngine;

/// <summary>
/// Class used by shopkeeper inventory gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class ShopkeeperEventOnInteraction : AbstractEventOnInteraction, IInterectable, 
    IInteractableWithSound, IFindPlayer
{
    [Header("Sound to be played when interected with")]
    [SerializeField] private LootAndInteractionSoundType lootAndInteractionSoundType;
    public LootAndInteractionSoundType LootAndInteractionSoundType => lootAndInteractionSoundType;

    /// <summary>
    /// Property with player currency to be used on external classes.
    /// </summary>
    public PlayerCurrency PlayerCurrency { get; private set; }

    // Item price
    [Range(1, 100000)] [SerializeField] private int price;

    /// <summary>
    /// Price of the item.
    /// </summary>
    public int Price => price;

    /// <summary>
    /// Shopkeeper that spawn this item.
    /// </summary>
    public Shopkeeper ShopkeeperOfItem { get; set; }

    /// <summary>
    /// Which slot was it spawned in.
    /// </summary>
    public int NumberOfSlot { get; set; }

    protected override void Awake()
    {
        base.Awake();
        PlayerCurrency = FindObjectOfType<PlayerCurrency>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
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

                OnItemBought(this);
                gameObject.SetActive(false);
                PlayerCurrency.SpendCurrency(CurrencyType.Gold, price);
            }
        }
        else
        {
            Debug.Log("Has no event to execute");
        }
    }

    public override void FindPlayer(Player player)
    {
        base.FindPlayer(player);
        PlayerCurrency = player.GetComponent<PlayerCurrency>();
    }

    protected virtual void OnItemBought(ShopkeeperEventOnInteraction itemBought) => 
        ItemBought?.Invoke(itemBought);
    public event Action<ShopkeeperEventOnInteraction> ItemBought;
}
