using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezerraDreamSpellSpawner : MonoBehaviour
{
    [SerializeField] private SpellSO spell;
    [SerializeField] private float timeToSpawn;
    [Range(0f,1f)][SerializeField] private float timeScale;

    private Character character;
    private Stats characterStats;
    private SpellBehaviourAbstract spellBehaviour;

    private void Awake()
    {
        character = FindObjectOfType<Player>();
        characterStats = character.GetComponent<Stats>();
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
            spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
            spellBehaviour.WhoCast = characterStats;

            // Moves the spell forward
            spawnedSpell.GetComponent<Rigidbody>().velocity = transform.forward * spell.Speed;
        }
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }
}
