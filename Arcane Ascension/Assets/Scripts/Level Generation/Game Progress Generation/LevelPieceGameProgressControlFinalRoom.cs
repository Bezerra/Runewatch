using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for controlling what happens inside a normal level piece room.
/// </summary>
public class LevelPieceGameProgressControlFinalRoom : LevelPieceGameProgressControlAbstract
{
    private RunSaveData saveData;
    private NextFloorPortalSpawnPoint nextFloorPortalSpawnPoint;

    protected override void Awake()
    {
        base.Awake();
        saveData = FindObjectOfType<RunSaveDataController>().SaveData;
        nextFloorPortalSpawnPoint =
            GetComponentInChildren<NextFloorPortalSpawnPoint>();
    }

    protected override void Start()
    {
        if (saveData.DungeonSavedData.Floor % 3 == 0)
        {
            BossRoom();
        }
        else
        {
            ChangeFloorRoom();
        }

        // Base start happens before initial logic, so if it's not a boss room,
        // it will destroy enemy spawn points first, so base start won't spawn enemies
        base.Start();
    }

    /// <summary>
    /// If it's a boss room, disables portal stuff.
    /// </summary>
    private void BossRoom()
    {
        if (nextFloorPortalSpawnPoint != null)
        {
            nextFloorPortalSpawnPoint.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// If it's a portal room, destroys boss stuff.
    /// </summary>
    private void ChangeFloorRoom()
    {
        if (enemySpawnPoints.Count > 0)
        {
            foreach(EnemySpawnPoint enemySpawn in enemySpawnPoints)
            {
                Destroy(enemySpawn.gameObject);
            }

            enemySpawnPoints = null;
        }
    }

    /// <summary>
    /// Triggered after boss dies.
    /// </summary>
    /// <param name="enemyStats">Boss stats.</param>
    protected override void BossDeath(Stats enemyStats)
    {
        base.BossDeath(enemyStats);
        StartCoroutine(EnablePortal());
    }

    /// <summary>
    /// Coroutine that enables a portal.
    /// Will wait for player to leave the portal zone to enable it.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator EnablePortal()
    {
        while(true)
        {
            Collider[] playerCollider =
                Physics.OverlapSphere(
                    nextFloorPortalSpawnPoint.transform.position, 3.5f, Layers.PlayerLayer);

            if (playerCollider.Length == 0)
            {
                nextFloorPortalSpawnPoint.gameObject.SetActive(true);
                break;
            }

            yield return null;
        }
    }
}
