using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for growing particles radius depending on a spell AoE.
/// </summary>
public class FireDemonImmolateExtraLogic : MonoBehaviour, IReset
{
    // Immolate radius growth
    [SerializeField] private ParticleSystem risingRings;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private ParticleSystem fireParticles;
    private ParticleSystem.ShapeModule smokeShape;
    private ParticleSystem.ShapeModule fireShape;

    // Safe zones
    [SerializeField] private List<GameObject> safeZones;

    // Spell
    private SpellBehaviourOneShot spell;

    private void Awake()
    {
        smokeShape = smokeParticles.shape;
        fireShape = fireParticles.shape;
        spell = GetComponent<SpellBehaviourOneShot>();
    }

    private void OnEnable()
    {
        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale = 
            new Vector3(0, risingRings.transform.localScale.y, 0);

        System.Random random = new System.Random();
        safeZones.Shuffle(random);

        for (int i = 0; i < safeZones.Count / 2; i++)
        {
            safeZones[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < safeZones.Count; i++)
        {
            safeZones[i].SetActive(false);
        }
    }

    private void Update()
    {
        float radius = spell.Spell.AreaOfEffect;
        smokeShape.radius = radius;
        fireShape.radius = radius;
        risingRings.transform.localScale =
            new Vector3(radius / risingRings.startSize, 
                risingRings.transform.localScale.y,
                radius / risingRings.startSize);
    }

    public void ResetAfterPoolDisable()
    {
        for (int i = 0; i < safeZones.Count; i++)
        {
            safeZones[i].SetActive(false);
        }

        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale =
            new Vector3(0, risingRings.transform.localScale.y, 0);
    }
}
