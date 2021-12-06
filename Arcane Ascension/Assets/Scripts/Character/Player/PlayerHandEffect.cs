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

    // Pool transform
    private Transform poolTransformParent;

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
            HandEffectLightFade lightFade = spawnedSpell.GetComponentInChildren<HandEffectLightFade>();

            // Adds current effect to previous effects, so it can disable them
            lightFade.DeactivateLight();
            previousSpawnedSpell.Add(spawnedSpell);
            vfxPreviousSpawnedSpell.Add(previousVFX);

            previousVFX.Stop();
        }

        if (spell.Prefab.Item4 != null)
        {
            // If there's a spell on player's hand already, moves it to pool parent
            if (spawnedSpell != null)
                spawnedSpell.transform.parent = poolTransformParent;

            // Starts a new effect
            spawnedSpell =
                SpellHandEffectPoolCreator.Pool.InstantiateFromPool(
                    spell.Name, Vector3.zero, Quaternion.identity);

            // Sets pool transform parent
            if (poolTransformParent == null)
                poolTransformParent = spawnedSpell.transform.parent;

            // Sets spell parent as this game object.
            spawnedSpell.transform.parent = 
                currentEffectPosition.transform;

            // Resets position and rotation
            spawnedSpell.transform.localPosition = Vector3.zero;
            spawnedSpell.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void FixedUpdate()
    {
        // If there's a previows spell still active, it will disable it when it stops emitting particles
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
