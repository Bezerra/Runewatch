using System.Collections;
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
    private YieldInstruction wffu;
    private IEnumerator changeEffectCoroutine;

    private void Awake()
    {
        wffu = new WaitForFixedUpdate();
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

        // Starts a new effect
        spawnedSpell =
            SpellHandEffectPoolCreator.Pool.InstantiateFromPool(
                spell.Name, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (spawnedSpell != null)
        {
            spawnedSpell.transform.position = currentEffectPosition.position;
        }

        if (vfxPreviousSpawnedSpell.Count > 0)
        {
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
