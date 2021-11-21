using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Scriptable object with enemies lists and types.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Enemy List", fileName = "Enemy List")]
public class AvailableListOfEnemiesToSpawnSO : ScriptableObject
{
    [TableList]
    [SerializeField] private List<EnemyTypeAndPrefabsInformation> enemyList;

    private Dictionary<EnemySpawnType, IList<GameObject>> enemyDictionary;

    private void OnEnable()
    {
        enemyDictionary = new Dictionary<EnemySpawnType, IList<GameObject>>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyDictionary.Add(enemyList[i].EnemySpawnType, enemyList[i].EnemyPrefabs);
        }
    }

    /// <summary>
    /// Gets an enemy from a list depending on enemy spawn time.
    /// </summary>
    /// <param name="enemySpawnType">Enemy spawn type to get from a list.</param>
    /// <returns>Returns an enemy gameobject.</returns>
    public GameObject SpawnEnemy(EnemySpawnType enemySpawnType)
    {
        IList<GameObject> enemyList = enemyDictionary[enemySpawnType];
        return enemyList[Random.Range(0, enemyList.Count)];
    }
}
