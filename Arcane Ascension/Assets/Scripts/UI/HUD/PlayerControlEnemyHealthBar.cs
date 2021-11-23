using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for activating enemy health bars when the player is looking towards them.
/// </summary>
public class PlayerControlEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private LayerMask enemyHealthBarLayer;

    private IEnumerator Start()
    {
        YieldInstruction wfs = new WaitForSeconds(0.1f);

        Collider[] enemyColliders;

        while (true)
        {
            enemyColliders = Physics.OverlapSphere(transform.position, 20, enemyHealthBarLayer);

            if (enemyColliders.Length > 0)
            {
                for (int i = 0; i < enemyColliders.Length; i++)
                {
                    if (enemyColliders[i].TryGetComponent(out EnemyHealthBar healthBar))
                    {
                        if (transform.IsLookingTowards(enemyColliders[i].transform.position, true, 15))
                        {
                            healthBar.EnableEnemyHealthBar(true);
                        }
                        else
                        {
                            healthBar.EnableEnemyHealthBar(false);
                        }
                    }
                }
            }
            yield return wfs;
        }
    }
}
