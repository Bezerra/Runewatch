using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;

/// <summary>
/// Scriptable object with enemies lists and types.
/// </summary>
[CreateAssetMenu(menuName = "Create Once/Enemy List", fileName = "Enemy List")]
public class AvailableListOfEnemiesToSpawnSO : ScriptableObject
{
    [Tooltip("Probability of spawning an enemy with the same element of the dungeon")]
    [Range(0, 100)] [SerializeField] private int chanceOnSameEnemyElement; 

    [TableList]
    [SerializeField] private List<EnemyTypeAndPrefabsInformation> enemyList;

    private Dictionary<EnemySpawnType, IList<ElementAndEnemyPrefab>> enemyDictionary;

    System.Random random;

    private void OnEnable()
    {
        enemyDictionary = new Dictionary<EnemySpawnType, IList<ElementAndEnemyPrefab>>();
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyDictionary.Add(enemyList[i].EnemySpawnType, enemyList[i].EnemyInformation);
        }
    }

    /// <summary>
    /// Gets an enemy from a list depending on enemy spawn time.
    /// </summary>
    /// <param name="enemySpawnType">Enemy spawn type to get from a list.</param>
    /// <returns>Returns an enemy gameobject.</returns>
    public GameObject SpawnEnemy(EnemySpawnType enemySpawnType)
    {
        if (random == null) random = new System.Random();

        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        ElementType dungeonElement;

        // Works for normal scenes and test isolated scenes.
        dungeonElement = levelGenerator != null ? levelGenerator.Element : ElementType.Ignis;

        // Creates a list of enemy weights 
        IList<int> enemyWeights = new List<int>();
        for (int i = 0; i < enemyDictionary[enemySpawnType].Count; i++)
        {
            if (enemyDictionary[enemySpawnType][i].Element == dungeonElement)
                enemyWeights.Add(chanceOnSameEnemyElement);
            else  
                enemyWeights.Add((100 - chanceOnSameEnemyElement) / 7); // rest divided by variety of elements
        }

        IList<ElementAndEnemyPrefab> enemyFinalList = enemyDictionary[enemySpawnType];

        //return enemyList[Random.Range(0, enemyList.Count)].Prefab;
        return enemyFinalList[random.RandomWeight(enemyWeights)].Prefab;
    }
}
