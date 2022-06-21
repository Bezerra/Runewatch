using System.Collections;
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
        // Waits sometime before running
        yield return new WaitForSeconds(3);

        YieldInstruction wfs = new WaitForSeconds(0.1f);

        bool deactivatedBarsThisFrame = false;
        Collider[] enemyColliders;

        while (true)
        {
            if (PlayerPrefs.GetFloat(PPrefsOptions.EnemyHealthBar.ToString(), 1) == 0)
            {
                yield return wfs;

                // Deactivates bars for all enemies around the player once
                if (deactivatedBarsThisFrame == false)
                {
                    enemyColliders = Physics.OverlapSphere(transform.position, 
                        20, enemyHealthBarLayer);

                    if (enemyColliders.Length > 0)
                    {
                        for (int i = 0; i < enemyColliders.Length; i++)
                        {
                            if (enemyColliders[i].TryGetComponent(
                                out EnemyHealthBar healthBar))
                            {
                                healthBar.EnableEnemyHealthBar(false);
                            }
                        }
                    }
                    deactivatedBarsThisFrame = true;
                }

                continue;
            }

            deactivatedBarsThisFrame = false;
            enemyColliders = Physics.OverlapSphere(transform.position, 20, 
                enemyHealthBarLayer);

            if (enemyColliders.Length > 0)
            {
                for (int i = 0; i < enemyColliders.Length; i++)
                {
                    if (enemyColliders[i].TryGetComponent(
                        out EnemyHealthBar healthBar))
                    {
                        if (transform.IsLookingTowards(
                            enemyColliders[i].transform.position, true, 15))
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
