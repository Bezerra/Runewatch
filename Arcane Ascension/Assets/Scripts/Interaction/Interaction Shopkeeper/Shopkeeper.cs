using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for spawning shopkeeper items.
/// </summary>
public class Shopkeeper : MonoBehaviour
{
    [Header("Let slots active to true")]
    [SerializeField] private ShopkeeperEventOnInteraction[] possibleItemsToSell;

    [Header("This number will be automatic after implementing the rest of the stuff")]
    [Range(1, 7)][SerializeField] private byte numberOfInventorySlots;

    // Slots must be active on awake
    private IList<ShopkeeperInventorySlot> allInvetorySlots;

    private void Awake()
    {
        allInvetorySlots = GetComponentsInChildren<ShopkeeperInventorySlot>();
        if (numberOfInventorySlots > allInvetorySlots.Count) numberOfInventorySlots = (byte)allInvetorySlots.Count;

        // Disables all
        foreach (ShopkeeperInventorySlot slot in allInvetorySlots)
        {
            slot.gameObject.SetActive(false);
        }

        // Enables X number of slots depending on the allowed number of slots
        for (int i = 0; i < numberOfInventorySlots; i++)
        {
            allInvetorySlots[i].gameObject.SetActive(true);

            // Gets random item
            int randomItemIndex = UnityEngine.Random.Range(0, possibleItemsToSell.Length);

            // Spawns item in slot
            GameObject itemSpawned =
                Instantiate(
                    possibleItemsToSell[randomItemIndex].gameObject,
                transform.position, Quaternion.identity);

            // Rotates the item towards shopkeeper forward
            itemSpawned.transform.SetPositionAndRotation(
                allInvetorySlots[i].ItemModelParent.position,
                allInvetorySlots[i].ItemModelParent.rotation);

            // Sets parent
            itemSpawned.transform.parent = allInvetorySlots[i].ItemModelParent;

            // Updates canvas with item price
            allInvetorySlots[i].UpdateInformation(possibleItemsToSell[randomItemIndex].Price.ToString());
        }
    }
}
