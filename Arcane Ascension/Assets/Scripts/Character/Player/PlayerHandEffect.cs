using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Class responsible for updating player hand effect.
/// </summary>
public class PlayerHandEffect : MonoBehaviour
{
    [SerializeField] private Transform currentEffectPosition;
    public Transform CurrentEffectPosition 
    { get => currentEffectPosition; set => currentEffectPosition = value; }

    // Spawned spells
    private GameObject spawnedSpell;
    private IList<GameObject> previousSpawnedSpell;

    // Particle system
    private IList<ParticleSystem> particlesPreviousSpawnedSpell;
    private ParticleSystem previousParticle;

    // VFX
    private IList<VisualEffect> vfxPreviousSpawnedSpell;
    private VisualEffect previousVFX;

    // Pool transform
    private Transform poolTransformParent;

    private void Awake()
    {
        previousSpawnedSpell = new List<GameObject>();
        particlesPreviousSpawnedSpell = new List<ParticleSystem>();
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
            previousParticle = null;
            previousVFX = null;

            previousSpawnedSpell.Add(spawnedSpell);

            // If particle system
            previousParticle = spawnedSpell.GetComponentInChildren<ParticleSystem>();
            if (previousParticle != null)
                particlesPreviousSpawnedSpell.Add(previousParticle);

            // If vfx
            previousVFX = spawnedSpell.GetComponentInChildren<VisualEffect>();
            if (previousVFX != null)
                vfxPreviousSpawnedSpell.Add(previousVFX);

            // Disable light
            HandEffectLightFade lightFade = 
                spawnedSpell.GetComponentInChildren<HandEffectLightFade>();

            // Adds current effect to previous effects, so it can disable them
            lightFade.DeactivateLight();

            EffectStop();
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

    private void Update()
    {
        // If there's a previows spell still active, it will disable it when it stops emitting particles

        // for particles
        if (particlesPreviousSpawnedSpell.Count > 0)
        {
            for (int i = 0; i < particlesPreviousSpawnedSpell.Count; i++)
            {
                if (particlesPreviousSpawnedSpell[i].particleCount == 0)
                {
                    previousSpawnedSpell[i].SetActive(false);
                    previousSpawnedSpell.Remove(previousSpawnedSpell[i]);
                    particlesPreviousSpawnedSpell.Remove(particlesPreviousSpawnedSpell[i]);
                    i--;
                }
            }
        }

        // for vfx
        if (vfxPreviousSpawnedSpell.Count > 0)
        {
            for (int i = 0; i < particlesPreviousSpawnedSpell.Count; i++)
            {
                if (particlesPreviousSpawnedSpell[i].particleCount == 0)
                {
                    previousSpawnedSpell[i].SetActive(false);
                    previousSpawnedSpell.Remove(previousSpawnedSpell[i]);
                    particlesPreviousSpawnedSpell.Remove(particlesPreviousSpawnedSpell[i]);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Stops an effect.
    /// </summary>
    public void EffectStop()
    {
        if (previousVFX != null)
        {
            previousVFX.Stop();
        }
        if (previousParticle != null)
        {
            previousParticle.Stop();
        }
    }
}
