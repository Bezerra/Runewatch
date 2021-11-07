using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for spawning shopkeeper items.
/// </summary>
public class Shopkeeper : MonoBehaviour
{
    private readonly int numberOfVarietyOfItemsToSell = 4;

    [Header("This number will be automatic after implementing the rest of the stuff")]
    [Range(1, 7)][SerializeField] private byte numberOfInventorySlots;

    // Slots must be active on awake
    private IList<ShopkeeperInventorySlot> allInventorySlots;

    // Spawned items
    private IList<GameObject> itemsSpawned;

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

        // Enables X number of slots depending on the allowed number of slots
        for (int i = 0; i < numberOfInventorySlots; i++)
        {
            allInventorySlots[i].gameObject.SetActive(true);

            // Gets random item
            int randomItemIndex = UnityEngine.Random.Range(0, numberOfVarietyOfItemsToSell);

            // Spawn item from pool
            itemsSpawned.Add(ShopkeeperLootPoolCreator.Pool.InstantiateFromPool(
                randomItemIndex,
                transform.position, Quaternion.identity));

            // Rotates the item towards shopkeeper forward
            itemsSpawned[i].transform.SetPositionAndRotation(
                allInventorySlots[i].ItemModelParent.position,
                allInventorySlots[i].ItemModelParent.rotation);

            // Sets parent
            //itemSpawned.transform.parent = allInventorySlots[i].ItemModelParent;

            // Updates canvas with item price
            allInventorySlots[i].UpdateInformation(itemsSpawned[i].GetComponent<ShopkeeperEventOnInteraction>().Price.ToString());
        }
    }

    private void FixedUpdate()
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
                    if(allInventorySlots[i] != null)
                    {
                        Destroy(allInventorySlots[i].gameObject);
                    }
                }
            }
        }
    }
}
