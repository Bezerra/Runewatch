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

    public CharacterController Controller { get; private set; }
    public Stats EnemyStats { get; private set; }

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
}
