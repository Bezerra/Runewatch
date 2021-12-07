using UnityEngine;

/// <summary>
/// Class responsible for controlling what happens inside a normal level piece room.
/// </summary>
public class LevelPieceGameProgressControl : LevelPieceGameProgressControlAbstract
{
    // Variables to keep track of progress
    // Shopkeeper
    private ShopkeeperGizmosMesh[] shopkeeper;
    public bool RoomSpawnsShopkeeper { get; set; }

    // Chests
    private ChestGizmosMesh chest;
    public AbilityType AbilityType { get; set; }
    public bool RoomSpawnsChest { get; set; }
    private GameObject spawnedChest;
    private Chest chestScript;

    protected override void Awake()
    {
        base.Awake();
        shopkeeper = GetComponentsInChildren<ShopkeeperGizmosMesh>(true);
        chest = GetComponentInChildren<ChestGizmosMesh>(true);
    }

    /// <summary>
    /// Spawns chests after generating level.
    /// </summary>
    public void SpawnChestAfterGeneration()
    {
        if (RoomSpawnsChest)
        {
            if (AbilityType == AbilityType.Spell)
            {
                spawnedChest = RunProgressPoolCreator.Pool.InstantiateFromPool(
                    "Spell Chest", chest.transform.position,
                    chest.transform.rotation);
            }
            else
            {
                spawnedChest = RunProgressPoolCreator.Pool.InstantiateFromPool(
                    "Passive Chest", chest.transform.position,
                    chest.transform.rotation);
            }
        }
        if (spawnedChest.TryGetComponent(out chestScript))
        {
            chestScript.CanOpen = false;
        }
    }

    /// <summary>
    /// Blocks/unblocks all exits.
    /// Triggered when the player enters a room or defeats all enemies.
    /// </summary>
    protected override void BlockUnblockExits(bool condition)
    {
        if (exitBlockers.Count > 0)
        {
            if (condition)
            {
                foreach (BoxCollider exitBlock in exitBlockers)
                {
                    exitBlock.enabled = true;
                }

                foreach (ContactPointDoor contactPoint in contactPointsDoors)
                {
                    contactPoint.BlockPassage();
                    contactPoint.ClosePassage();
                }
            }
            else
            {
                foreach (BoxCollider exitBlock in exitBlockers)
                {
                    exitBlock.enabled = false;
                }

                foreach (ContactPointDoor contactPoint in contactPointsDoors)
                {
                    // Keeps passage closed but unblocks it
                    contactPoint.ClosePassage();
                    contactPoint.UnblockPassage();
                }

                // If this piece is supposed to spawn shopkeeper, spawns it
                if (RoomSpawnsShopkeeper && shopkeeper != null)
                {
                    for (int i = 0; i < shopkeeper.Length; i++)
                    {
                        Collider[] collisions = Physics.OverlapSphere(
                            shopkeeper[i].transform.position, 4,
                            Layers.PlayerNormalAndInvisibleLayer);

                        if (collisions.Length == 0)
                        {
                            RunProgressPoolCreator.Pool.InstantiateFromPool(
                                "Shopkeeper", shopkeeper[i].transform.position,
                                shopkeeper[i].transform.rotation);

                            break;
                        }
                    }
                }

                // If this piece is supposed to spawn a chest, spawns it
                // Type of chest randomly set on the end of level generator
                if (RoomSpawnsChest && chestScript != null)
                {
                    chestScript.CanOpen = true;
                    chestScript.GetComponentInChildren<MinimapIcon>(true).SetIconActive(true);
                }
            }
        }
        else
        {
            // If this piece is supposed to spawn shopkeeper, spawns it
            if (RoomSpawnsShopkeeper && shopkeeper != null)
            {
                for (int i = 0; i < shopkeeper.Length; i++)
                {
                    Collider[] collisions = Physics.OverlapSphere(
                        shopkeeper[i].transform.position, 4,
                        Layers.PlayerNormalAndInvisibleLayer);

                    if (collisions.Length == 0)
                    {
                        RunProgressPoolCreator.Pool.InstantiateFromPool(
                            "Shopkeeper", shopkeeper[i].transform.position,
                            shopkeeper[i].transform.rotation);

                        break;
                    }
                }
            }

            // If this piece is supposed to spawn a chest, spawns it
            // Type of chest randomly set on the end of level generator
            if (RoomSpawnsChest && chestScript != null)
            {
                chestScript.CanOpen = true;
            }
        }
    }
}
