using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for spawning shopkeeper items.
/// </summary>
public class Shopkeeper : MonoBehaviour, IFindPlayer
{
    private readonly int numberOfVarietyOfItemsToSell = 4;

    [Header("This number will be automatic after implementing the rest of the stuff")]
    [Range(1, 7)][SerializeField] private byte numberOfInventorySlots;

    // Slots must be active on awake
    private IList<ShopkeeperInventorySlot> allInventorySlots;

    // Spawned items
    private IList<GameObject> itemsSpawned;

    private int numberOfItemsSold;
    private bool shopkeeperSpawned;

    // Components
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        allInventorySlots = GetComponentsInChildren<ShopkeeperInventorySlot>();
        itemsSpawned = new List<GameObject>();

        if (numberOfInventorySlots > allInventorySlots.Count) numberOfInventorySlots = (byte)allInventorySlots.Count;

        // Disables all
        foreach (ShopkeeperInventorySlot slot in allInventorySlots)
        {
            slot.gameObject.SetActive(false);
        } 
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        SpawnShopkeeper();
    }

    public void SpawnShopkeeper()
    {
        // Enables X number of slots depending on the allowed number of slots
        for (int i = 0; i < numberOfInventorySlots; i++)
        {
            allInventorySlots[i].gameObject.SetActive(true);

            // Gets random item
            int randomItemIndex = Random.Range(0, numberOfVarietyOfItemsToSell);

            // Spawn item from pool
            itemsSpawned.Add(ShopkeeperLootPoolCreator.Pool.InstantiateFromPool(
                randomItemIndex,
                transform.position, Quaternion.identity));

            // Rotates the item towards shopkeeper forward
            itemsSpawned[i].transform.SetPositionAndRotation(
                allInventorySlots[i].ItemModelParent.position,
                allInventorySlots[i].ItemModelParent.rotation);

            // Updates canvas with item price
            allInventorySlots[i].UpdateInformation(
                itemsSpawned[i].GetComponent<ShopkeeperEventOnInteraction>().Price.ToString());
        }
        shopkeeperSpawned = true;
    }

    private void FixedUpdate()
    {
        if (shopkeeperSpawned || player != null)
        {
            // If player is close
            if (Vector3.Distance(transform.position, player.transform.position) < 25)
            {
                for (int i = 0; i < numberOfInventorySlots; i++)
                {
                    // If item wasn't bought yet, updates item position to match inventory slots position
                    if (itemsSpawned[i].activeSelf)
                        itemsSpawned[i].transform.position = allInventorySlots[i].ItemModelParent.position;
                    else
                    {
                        if (allInventorySlots[i] != null)
                        {
                            numberOfItemsSold++;
                            Destroy(allInventorySlots[i].gameObject);
                        }
                    }
                }

                if (numberOfInventorySlots == numberOfItemsSold)
                    Destroy(gameObject);
            }
        }
    }

    public void FindPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayerLost()
    {
        player = null;
    }
}
