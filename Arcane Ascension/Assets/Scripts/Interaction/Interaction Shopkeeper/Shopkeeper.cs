using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for spawning shopkeeper items.
/// </summary>
public class Shopkeeper : MonoBehaviour
{
    private readonly int numberOfVarietyOfItemsToSell = 3;

    // Slots must be active on awake
    private IList<ShopkeeperInventorySlot> allInventorySlots;

    // Spawned items
    private IList<GameObject> itemsSpawned;

    private int numberOfItemsSold;
    private byte numberOfInventorySlots;

    private LoopGameplayMusic gameplayMusic;
    private ShopkeeperMusic shopkeeperMusic;


    private void Awake()
    {
        allInventorySlots = GetComponentsInChildren<ShopkeeperInventorySlot>(true);
        gameplayMusic = FindObjectOfType<LoopGameplayMusic>();
        shopkeeperMusic = FindObjectOfType<ShopkeeperMusic>();
    }

    private void OnEnable()
    {
        numberOfItemsSold = 0;

        itemsSpawned = new List<GameObject>();

        numberOfInventorySlots = 4; // Initial slots
        numberOfInventorySlots += FindObjectOfType<CharacterSaveDataController>().SaveData.Dealer;

        if (numberOfInventorySlots > allInventorySlots.Count)
            numberOfInventorySlots = (byte)allInventorySlots.Count;

        // Disables all
        foreach (ShopkeeperInventorySlot slot in allInventorySlots)
        {
            slot.gameObject.SetActive(false);
        }

        StartCoroutine(SpawnShopKeeperCoroutine());
        
        // This logic applies only if the player has spawned
        if (gameplayMusic.HasPlayerSpawned)
        {
            gameplayMusic.FadeOutVolume();
            shopkeeperMusic.FadeInVolume();
        }
    }

    private void OnDisable()
    {
        // This logic applies only if the player has spawned
        if (gameplayMusic.HasPlayerSpawned)
        {
            gameplayMusic.FadeInVolume();
            shopkeeperMusic.FadeOutVolume();
        }
    }

    private IEnumerator SpawnShopKeeperCoroutine()
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
            GameObject itemToSpawn =
                ShopkeeperLootPoolCreator.Pool.InstantiateFromPool(
                randomItemIndex, transform.position, Quaternion.identity);

            // Adds to spawn items list
            itemsSpawned.Add(itemToSpawn);

            // Sets item's shopkeeper
            if (itemToSpawn.TryGetComponent(out ShopkeeperEventOnInteraction item))
                item.ShopkeeperOfItem = this;

            // Subscribes to buy event
            item.ItemBought += ItemBought;
            item.NumberOfSlot = i;

            // Rotates the item towards shopkeeper forward
            itemsSpawned[i].transform.SetPositionAndRotation(
                allInventorySlots[i].ItemModelParent.position,
                allInventorySlots[i].ItemModelParent.rotation);

            // Updates canvas with item price
            allInventorySlots[i].UpdateInformation(
                itemsSpawned[i].GetComponent<ShopkeeperEventOnInteraction>().
                Price.ToString());
        }

        for (int i = 0; i < numberOfInventorySlots; i++)
        {
            // Updates item position to match inventory slots position
            if (itemsSpawned[i].activeSelf)
            {
                itemsSpawned[i].transform.position =
                    allInventorySlots[i].ItemModelParent.position;
            }
        }
    }

    private void ItemBought(ShopkeeperEventOnInteraction itemBought)
    {
        numberOfItemsSold++;
        allInventorySlots[itemBought.NumberOfSlot].gameObject.SetActive(false);

        if (numberOfInventorySlots == numberOfItemsSold)
        {
            gameObject.SetActive(false);
        }

        itemBought.ItemBought -= ItemBought;
    }
}
