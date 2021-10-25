using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for enemies.
/// </summary>
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
    /// <summary>
    /// Getter used to know if the enemy is picking a patrol position.
    /// </summary>
    public bool PickingPatrolPosition { get; set; }

    /// <summary>
    /// Current distance for patrols.
    /// </summary>
    public bool WalkingBackwards { get; set; }

    /// <summary>
    /// Current distance for patrols.
    /// </summary>
    public float CurrentDistance { get; set; }

    /// <summary>
    /// Current distance for patrols.
    /// </summary>
    public float DistanceToKeepFromTarget { get; set; }

    /// <summary>
    /// Getter used to know the time a final point was reached.
    /// </summary>
    public float TimePointReached { get; set; }

    /// <summary>
    /// Current waiting time after reaching a final point.
    /// </summary>
    public float CurrentWaitingTime { get; set; }

    private Vector3 playerLastKnownPosition;
    /// <summary>
    /// Sets player's last known position with a random offset depending on values distance.
    /// </summary>
    public Vector3 PlayerLastKnownPosition
    {
        get => playerLastKnownPosition;
        set
        {
            Vector3 offset = 
                new Vector3(Random.Range(-Values.Distance.x, Values.Distance.x), 0, 
                Random.Range(-Values.Distance.x, Values.Distance.x));
            playerLastKnownPosition = value + offset;
        }
    }

    // Roll properties
    public float RollTime           { get; set; }
    public float RollDelay          { get; set; }
    public Direction RollDirection  { get; set; }

    // Attack properties
    public float TimeOfLastAttack   { get; set; }
    public float AttackDelay        { get; set; }


    // General properties for ai
    /// <summary>
    /// Character controller.
    /// </summary>
    public CharacterController CharController   { get; private set; }

    /// <summary>
    /// Stats script.
    /// </summary>
    public Stats EnemyStats                 { get; private set; }

    /// <summary>
    /// Used to know if the enemy just took damage.
    /// </summary>
    public bool TookDamage                  { get; set; }

    /// <summary>
    /// Navmesh agent.
    /// </summary>
    public NavMeshAgent Agent               { get; private set; }

    private Transform currentTarget;
    /// <summary>
    /// Current target of the ai.
    /// </summary>
    public Transform CurrentTarget
    {
        get => currentTarget;
        set
        {
            currentTarget = value;
            QuantityFighting = value != null ? QuantityFighting++ : QuantityFighting--;
        }
    }

    /// <summary>
    /// Static Getter to know quantity of enemies fighting the player
    /// </summary>
    public static int QuantityFighting { get; private set; }

    /// <summary>
    /// Player script.
    /// </summary>
    public Player PlayerScript              { get; private set; }

    /// <summary>
    /// State machine property.
    /// </summary>
    public StateController<Enemy> StateMachine { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        CharController = GetComponent<CharacterController>();
        EnemyStats = GetComponent<Stats>();
        PlayerScript = FindObjectOfType<Player>();
        StateMachine = new StateController<Enemy>(this, 2);
    }

    private void Start()
    {
        Agent.speed = Values.Speed;
        StateMachine.Start(StateMachine);
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
        StateMachine.Update(StateMachine);
    }

    /// <summary>
    /// Sets TookDamage to true.
    /// </summary>
    /// <param name="temp"></param>
    private void EventTakeDamage(float temp) => TookDamage = true;
}
