using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezerraDreamSpellSpawner : MonoBehaviour
{
    [Header("One Shot With Release spells will spawn in 0, 0, 0")]
    [SerializeField] private SpellSO spell;
    [SerializeField] private float timeToSpawn;
    [Range(0f,1f)][SerializeField] private float timeScale;

    private PlayerStats characterStats;

    private void Awake()
    {
        characterStats = GetComponent<PlayerStats>();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToSpawn);

            GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, transform.position,
                Quaternion.LookRotation(transform.forward, transform.up));

            // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
            SpellBehaviourAbstract spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
            spellBehaviour.WhoCast = characterStats;
            spellBehaviour.TriggerStartBehaviour();
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }
}
