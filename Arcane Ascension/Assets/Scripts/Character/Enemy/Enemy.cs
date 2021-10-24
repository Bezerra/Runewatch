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

    // Movement properties
    public float WaitingTime { get; set; }
    public float Distance {get; set; }
    public float TimePointReached {get; set; }
    public bool ReachedPoint {get; set; }

    // Roll properties
    public float RollTime { get; set; }
    public float RollDelay { get; set; }
    public Direction RollDirection { get; set; }

    // Attack properties
    public float TimeOfLastAttack { get; set; }
    public float AttackDelay { get; set; }

    // General properties for ai
    public CharacterController Controller { get; private set; }
    public Stats EnemyStats { get; private set; }
    public bool TookDamage { get; set; }
    public NavMeshAgent Agent { get; private set; }
    public Transform CurrentTarget { get; set; }
    public Player PlayerScript { get; private set; }
    public StateController StateMachine { get; private set; }
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<CharacterController>();
        EnemyStats = GetComponent<Stats>();
        PlayerScript = FindObjectOfType<Player>();
        StateMachine = new StateController(this);
    }

    private void Start()
    {
        Agent.speed = Values.Speed;
        StateMachine.Start();
    }

    private void OnEnable()
    {
        EnemyStats.EventTakeDamage += EventTakeDamage;
    }

    private void OnDisable()
    {
        EnemyStats.EventTakeDamage -= EventTakeDamage;
    }

    private void Update()
    {
        StateMachine.Update();
    }

    /// <summary>
    /// Sets TookDamage to true.
    /// </summary>
    /// <param name="temp"></param>
    private void EventTakeDamage(float temp) => TookDamage = true;

}
