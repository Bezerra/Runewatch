using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    /// <summary>
    /// Values of enemy.
    /// </summary>
    public EnemyValuesSO Values => AllValues.CharacterValues as EnemyValuesSO;

    /// <summary>
    /// All values of enemy.
    /// </summary>
    public EnemyCharacterSO AllValues => allValues as EnemyCharacterSO;

    // Roll variables
    public float RollTime { get; set; }
    public float RollDelay { get; set; }
    public Direction RollDirection { get; set; }

    // General properties for ai
    public CharacterController Controller { get; private set; }
    public Stats EnemyStats { get; private set; }
    public bool TookDamage { get; set; }

    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<CharacterController>();
        EnemyStats = GetComponent<Stats>();
    }

    private void Start()
    {
        agent.speed = Values.Speed;
    }

    private void OnEnable()
    {
        EnemyStats.EventTakeDamage += EventTakeDamage;
    }

    private void OnDisable()
    {
        EnemyStats.EventTakeDamage -= EventTakeDamage;
    }

    /// <summary>
    /// Sets TookDamage to true.
    /// </summary>
    /// <param name="temp"></param>
    private void EventTakeDamage(float temp) => TookDamage = true;

}
