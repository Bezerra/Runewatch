using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Class responsible for updating player hand effect.
/// </summary>
public class PlayerHandEffect : MonoBehaviour
{
    [SerializeField] private Transform currentEffectPosition;

    private GameObject spawnedSpell;
    private IList<GameObject> previousSpawnedSpell;
    private IList<VisualEffect> vfxPreviousSpawnedSpell;

    private void Awake()
    {
        previousSpawnedSpell = new List<GameObject>();
        vfxPreviousSpawnedSpell = new List<VisualEffect>();
    }

    /// <summary>
    /// Updates current player hand effect with a spell's hand effect.
    /// </summary>
    /// <param name="spell">Spell with the desired effect.</param>
    public void UpdatePlayerHandEffect(ISpell spell)
    {
        // If there's an active effect, it stops it
        if (spawnedSpell != null)
        {
            VisualEffect previousVFX = spawnedSpell.GetComponentInChildren<VisualEffect>();

            previousSpawnedSpell.Add(spawnedSpell);
            vfxPreviousSpawnedSpell.Add(previousVFX);

            previousVFX.Stop();
        }

        if (spell.Prefab.Item4 != null)
        {
            // Starts a new effect
            spawnedSpell =
                SpellHandEffectPoolCreator.Pool.InstantiateFromPool(
                    spell.Name, Vector3.zero, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        if (spawnedSpell != null)
        {
            spawnedSpell.transform.position = currentEffectPosition.position;
            //spawnedSpell.transform.SetPositionAndRotation(
            //    currentEffectPosition.position, currentEffectPosition.rotation);
        }

        // If there's a previows spell still active, it will disable it when it stops emitting particles
        if (vfxPreviousSpawnedSpell.Count > 0)
        {
            Debug.Log("Run");
            for (int i = 0; i < vfxPreviousSpawnedSpell.Count; i++)
            {
                if (vfxPreviousSpawnedSpell[i].aliveParticleCount == 0)
                {
                    previousSpawnedSpell[i].SetActive(false);
                    previousSpawnedSpell.Remove(previousSpawnedSpell[i]);
                    vfxPreviousSpawnedSpell.Remove(vfxPreviousSpawnedSpell[i]);
                    i--;
                }
            }
        }
    }
}
