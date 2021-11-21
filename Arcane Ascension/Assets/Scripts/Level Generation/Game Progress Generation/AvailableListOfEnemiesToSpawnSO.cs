using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural Generation/Enemy List", fileName = "Enemy List")]
public class AvailableListOfEnemiesToSpawnSO : ScriptableObject
{
    [SerializeField] private List<EnemyWeight> enemyList;
    //public 
}
