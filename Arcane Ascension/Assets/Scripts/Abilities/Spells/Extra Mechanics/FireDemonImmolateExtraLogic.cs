using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for growing particles radius depending on a spell AoE.
/// </summary>
public class FireDemonImmolateExtraLogic : MonoBehaviour
{
    // Immolate radius growth
    [SerializeField] private ParticleSystem risingRings;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private ParticleSystem fireParticles;
    private ParticleSystem.ShapeModule smokeShape;
    private ParticleSystem.ShapeModule fireShape;

    private EnemyBoss fireDemon;

    // Safe zones
    [SerializeField] private List<GameObject> safeZones;

    // Better to have this default scriptable object to know default resistance
    [SerializeField] private EnemyStatsSO fireDemonSecondStateStats;

    // Spell
    private SpellBehaviourOneShot spell;
    private float enemyDefaultDamageResistance;
    private StatsSO enemyStats;

    [Range(-1, 0f)] [SerializeField] private float extraResistanceWhileCasting = -0.6f;

    private YieldInstruction wffu;

    private void Awake()
    {
        smokeShape = smokeParticles.shape;
        fireShape = fireParticles.shape;
        spell = GetComponent<SpellBehaviourOneShot>();
        enemyDefaultDamageResistance = 
            fireDemonSecondStateStats.DamageResistanceStatusEffectMultiplier;
        wffu = new WaitForFixedUpdate();
        fireDemon = FindObjectOfType<EnemyBoss>();
    }

    private void OnEnable()
    {
        int safeZoneCount = fireDemon.ExecutedFirstMechanic ? 4 : 2;

        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale = 
            new Vector3(0, risingRings.transform.localScale.y, 0);

        System.Random random = new System.Random();
        safeZones.Shuffle(random);

        for (int i = 0; i < safeZoneCount; i++)
        {
            safeZones[i].SetActive(true);
        }
        
        StartCoroutine(SetExtraResistanceCoroutine());
    }

    private IEnumerator SetExtraResistanceCoroutine()
    {
        yield return wffu;

        if (spell.WhoCast != null)
        {
            enemyStats = spell.WhoCast.CommonAttributes;
            enemyStats.DamageResistanceStatusEffectMultiplier = extraResistanceWhileCasting;
        }
    }

    private void OnDisable()
    {
        // Disables all save zones
        for (int i = 0; i < safeZones.Count; i++)
        {
            safeZones[i].SetActive(false);
        }

        if (spell.WhoCast != null)
        {
            enemyStats.DamageResistanceStatusEffectMultiplier =
                enemyDefaultDamageResistance;
        }

        // Resets variables
        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale =
            new Vector3(0, risingRings.transform.localScale.y, 0);
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
}
