using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;

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
    [SerializeField] private List<GameObject> safeZonesGO;
    private List<SafeZone> safeZones;

    // Better to have this default scriptable object to know default resistance
    [SerializeField] private EnemyStatsSO fireDemonSecondStateStats;

    // Spell
    private SpellBehaviourOneShot spell;
    
    private StatsSO enemyStats;
    private YieldInstruction wffu;

    private bool rotatedTowardsPlayer;

    private void Awake()
    {
        smokeShape = smokeParticles.shape;
        fireShape = fireParticles.shape;
        spell = GetComponent<SpellBehaviourOneShot>();
        
        wffu = new WaitForFixedUpdate();
        fireDemon = FindObjectOfType<EnemyBoss>();

        safeZones = new List<SafeZone>();
        foreach (GameObject go in safeZonesGO)
            safeZones.Add(go.GetComponent<SafeZone>());
    }

    private void OnEnable()
    {
        rotatedTowardsPlayer = false;

        if (fireDemon == null) fireDemon = FindObjectOfType<EnemyBoss>();

        if (fireDemon != null)
        {
            foreach (SafeZone sz in safeZones)
                sz.Boss = fireDemon;
        }

        int safeZoneCount = 4;

        if (fireDemon != null) 
            safeZoneCount = fireDemon.ExecutedFirstMechanic ? 3 : 2;

        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale = 
            new Vector3(0, risingRings.transform.localScale.y, 0);

        System.Random random = new System.Random();
        safeZonesGO.Shuffle(random);

        for (int i = 0; i < safeZoneCount; i++)
        {
            safeZonesGO[i].SetActive(true);
        }
        
        StartCoroutine(SetExtraResistanceCoroutine());
    }

    private IEnumerator SetExtraResistanceCoroutine()
    {
        yield return wffu;

        if (spell.WhoCast != null)
        {
            enemyStats = spell.WhoCast.CommonAttributes;
        }
    }

    private void OnDisable()
    {
        // Disables all save zones
        for (int i = 0; i < safeZonesGO.Count; i++)
        {
            safeZonesGO[i].SetActive(false);
        }

        // Resets variables
        smokeShape.radius = 0;
        fireShape.radius = 0;
        risingRings.transform.localScale =
            new Vector3(0, risingRings.transform.localScale.y, 0);

        rotatedTowardsPlayer = false;
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

        if (fireDemon != null && fireDemon.CurrentTarget != null &&
            rotatedTowardsPlayer == false)
        {
            transform.LookAtY(fireDemon.CurrentTarget);
            rotatedTowardsPlayer = true;
        }
    }
}
