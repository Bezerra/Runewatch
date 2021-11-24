using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System;
using ExtensionMethods;

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
    /// Checks for a new direction if back movement is blocked.
    /// </summary>
    public Direction DirectionIfBackBlocked { get; set; }

    /// <summary>
    /// Property to know if the enemy is walking backwards.
    /// </summary>
    public bool WalkingBackwards { get; set; }

    /// <summary>
    /// Property to know if the enemy is running backwards.
    /// </summary>
    public bool RunningBackwards { get; set; }

    /// <summary>
    /// Exact time the enemy stopped walking backwards.
    /// </summary>
    public float TimeStoppedWalkingBackwards { get; set; }

    /// <summary>
    /// Current distance for patrols.
    /// </summary>
    public float CurrentDistance { get; set; }

    /// <summary>
    /// Current minimum distance to keep from target.
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
                new Vector3(UnityEngine.Random.Range(-Values.Distance.x, Values.Distance.x), 0, 
                UnityEngine.Random.Range(-Values.Distance.x, Values.Distance.x));
            playerLastKnownPosition = value + offset;
        }
    }

    private EnemySpell currentlySelectedSpell;
    /// <summary>
    /// Current spell the enemy has equiped.
    /// </summary>
    public EnemySpell CurrentlySelectedSpell
    {
        get => currentlySelectedSpell;
        set
        {
            currentlySelectedSpell = value;
            currentlySelectedSpell.Range = 
                UnityEngine.Random.Range(
                    currentlySelectedSpell.RandomRange.x, 
                    currentlySelectedSpell.RandomRange.y);
        }
    }

    /// <summary>
    /// Property to know if the enemy is running an attack action with stopping time.
    /// </summary>
    public bool IsAttackingWithStoppingTime { get; set; }

    /// <summary>
    /// Property to know how much time has passed since the enemy started the current attack.
    /// </summary>
    public float TimeEnemyStoppedWhileAttacking { get; set; }

    /// <summary>
    /// Keeps track of the last time the enemy attacked.
    /// </summary>
    public float TimeOfLastAttack { get; set; }

    /// <summary>
    /// The delay of each attack.
    /// </summary>
    public float AttackDelay { get; set; }

    /// <summary>
    /// True if the enemy isn't currently inside an attack.
    /// </summary>
    public bool CanRunAttackLoop { get; set; }

    /// <summary>
    /// True if the enemy isn't currently channeling a spell.
    /// </summary>
    public bool CanRunAttackStoppedLoop { get; set; }

    /// <summary>
    /// Property with side movement current time.
    /// </summary>
    public float SideMovingTime { get; set; }

    /// <summary>
    /// Property with side movement delay.
    /// </summary>
    public float SideMovementDelay { get; set; }

    /// <summary>
    /// Property with current side movement direction.
    /// </summary>
    public Direction SideMovementDirection  { get; set; }

    /// <summary>
    /// Used to know if the enemy just took damage.
    /// </summary>
    public bool TookDamage { get; set; }

    /// <summary>
    /// Static Getter to know quantity of enemies fighting the player
    /// </summary>
    public static int QuantityFighting { get; private set; }

    /// <summary>
    /// Property with player body transform.
    /// </summary>
    public Transform PlayerBody { get; set; }

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
    /// Stats script.
    /// </summary>
    public EnemyStats EnemyStats { get; private set; }

    /// <summary>
    /// Navmesh agent.
    /// </summary>
    public NavMeshAgent Agent { get; private set; }

    /// <summary>
    /// Player script.
    /// </summary>
    public Player PlayerScript              { get; private set; }

    /// <summary>
    /// State machine property.
    /// </summary>
    public StateController<Enemy> StateMachine { get; private set; }

    /// <summary>
    /// Property set on attack behaviours.
    /// </summary>
    public GameObject CurrentCastSpell { get; set; }

    /// <summary>
    /// Property set on attack behaviours.
    /// </summary>
    public SpellBehaviourAbstract CurrentSpellBehaviour { get; set; }

    /// <summary>
    /// Enemy animations.
    /// </summary>
    public IEnemyAnimator Animation { get; private set; }

    // Coroutines
    private IEnumerator takeDamageCoroutine;
    private YieldInstruction wfsAfterBeingHit;

    public System.Random Random;
    private void Awake()
    {
        Animation = GetComponentInChildren<IEnemyAnimator>();
        Agent = GetComponent<NavMeshAgent>();
        EnemyStats = GetComponent<EnemyStats>();
        PlayerScript = FindObjectOfType<Player>();
        StateMachine = new StateController<Enemy>(this, 2);
        wfsAfterBeingHit = new WaitForSeconds(0.25f);
        Random = new System.Random();
    }

    private void Start()
    {
        CurrentlySelectedSpell = EnemyStats.EnemyAttributes.AllEnemySpells[0];
        Agent.speed = Values.Speed * EnemyStats.CommonAttributes.MovementSpeedMultiplier *
            EnemyStats.CommonAttributes.MovementStatusEffectMultiplier;
        StateMachine.Start(StateMachine);
    }

    /// <summary>
    /// Updates agent speed.
    /// </summary>
    public void UpdateSpeed() =>
        Agent.speed = Values.Speed * EnemyStats.CommonAttributes.MovementSpeedMultiplier *
            EnemyStats.CommonAttributes.MovementStatusEffectMultiplier;

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
    private void EventTakeDamage()
    {
        this.StartCoroutineWithReset(ref takeDamageCoroutine, TakeDamageCoroutine());
        TookDamage = true;
    }

    /// <summary>
    /// Stops enemy and updates speed back to default after x seconds.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator TakeDamageCoroutine()
    {
        Agent.speed = 0;
        yield return wfsAfterBeingHit;
        UpdateSpeed();
    }
}
