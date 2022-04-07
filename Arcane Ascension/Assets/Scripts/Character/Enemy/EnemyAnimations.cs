using UnityEngine;
using System.Collections;
using ExtensionMethods;

/// <summary>
/// Class responsible for handling a monster animations and animation events.
/// </summary>
public class EnemyAnimations : MonoBehaviour, IEnemyAnimator
{
    [SerializeField] private GameObject brokenEnemy;

    // Components
    private Animator anim;
    private Enemy enemy;
    private EnemyStats enemyStats;
    private Transform enemyModel;
    private EnemySounds enemySounds;
    private EnemyHealthBar enemyHealthBar;
    private SkinnedMeshRenderer meshRender;

    // Coroutines
    private IEnumerator shakeCoroutine;
    private YieldInstruction wffu;
    private readonly float TIMETOSHAKE = 0.25f;
    private float currentShakingTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyModel = GetComponent<Transform>();
        enemy = GetComponentInParent<Enemy>();
        enemyStats = GetComponentInParent<EnemyStats>();
        enemySounds = GetComponentInParent<EnemySounds>();
        enemyHealthBar = transform.parent.GetComponentInChildren<EnemyHealthBar>();
        meshRender = GetComponentInChildren<SkinnedMeshRenderer>();
        wffu = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += StartShakeCoroutine;
        enemyStats.EventDeath += TriggerDeath;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= StartShakeCoroutine;
        enemyStats.EventDeath -= TriggerDeath;
    }

    private void Update()
    {
        Vector3 movementDirection = 
            enemy.transform.InverseTransformDirection(enemy.Agent.velocity);

        anim.SetFloat("VelocityZ", movementDirection.z);
        anim.SetFloat("VelocityX", movementDirection.x);
    }

    /// <summary>
    /// Triggers death animation.
    /// </summary>
    /// <param name="emptyVariable">Empty variable.</param>
    private void TriggerDeath(Stats emptyVariable) =>
        anim.SetTrigger("Death");

    /// <summary>
    /// Destroys enemy root.
    /// </summary>
    public void DestroyEnemyAnimationEvent() =>
        Destroy(GetComponentInParent<SelectionBase>().gameObject);

    /// <summary>
    /// Triggers enemy attack.
    /// </summary>
    /// <param name="enemyAttackType">Type of the attack.</param>
    public void TriggerAttack(EnemyAttackTypeAnimations enemyAttackType)
    {
        switch(enemyAttackType)
        {
            case EnemyAttackTypeAnimations.Melee:
                anim.SetTrigger("AttackMelee");
                break;
            case EnemyAttackTypeAnimations.OneShotSpell:
                anim.SetTrigger("AttackSpell");
                break;
            case EnemyAttackTypeAnimations.OneShotSpellWithRelease:
                anim.SetTrigger("AttackReleaseChannelingSpell");
                break;
            case EnemyAttackTypeAnimations.Channeling:
                anim.SetTrigger("AttackChannelingSpell");
                break;
        }
    }

    /// <summary>
    /// Executes AttackKeyPress of the current spell.
    /// </summary>
    public void OnKeyPressAnimationEvent()
    {
        enemy.CurrentlySelectedSpell.Spell.
            AttackBehaviour.AttackKeyPress(enemy.CurrentlySelectedSpell.Spell,
            enemy.StateMachine, enemy.EnemyStats);

        // If it's not a one shot cast with release, it executes AttackKeyRelease behaviour
        if (enemy.CurrentlySelectedSpell.Spell.CastType != SpellCastType.OneShotCastWithRelease)
        {
            OnKeyReleaseAnimationEvent();
        }

        // If the enemy has a spell that needs the enemy to stop while attacking
        // It will update a new timer and ignore the rest of the method
        if (enemy.CurrentlySelectedSpell.EnemyStopsOnAttack)
        {
            enemy.IsAttackingWithStoppingTime = true;
            enemy.TimeEnemyStoppedWhileAttacking = Time.time;
            return;
        }

        // Else it will reset variables
        AfterKeyReleaseAnimationEvent();
    }

    /// <summary>
    /// Executes AttackKeyRelease of the current spell.
    /// </summary>
    public void OnKeyReleaseAnimationEvent()
    {
        enemy.CurrentlySelectedSpell.Spell.
            AttackBehaviour.AttackKeyRelease(enemy.StateMachine);
    }

    /// <summary>
    /// Right after releasing attack key is released.
    /// Resets variables.
    /// </summary>
    public void AfterKeyReleaseAnimationEvent()
    {
        // Gets new random spell
        int randomSpell = enemy.Random.RandomWeight(enemy.EnemyStats.AvailableSpellsWeight);
        enemy.CurrentlySelectedSpell =
             enemy.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];

        // Updates time of last attack delay, so it starts a new delay for attack
        enemy.TimeOfLastAttack = Time.time;

        // Gets a new attack delay
        enemy.AttackDelay = Random.Range(
                enemy.EnemyStats.EnemyAttributes.AttackingDelay.x, 
                enemy.EnemyStats.EnemyAttributes.AttackingDelay.y);

        // Agent can move again
        enemy.Agent.speed = enemy.Values.Speed *
            enemyStats.CommonAttributes.MovementSpeedMultiplier *
            enemyStats.CommonAttributes.MovementStatusEffectMultiplier;

        enemy.Agent.isStopped = false;

        enemy.IsAttackingWithStoppingTime = false;

        enemy.StateMachine.AllowedToChangeState = true;

        enemy.CanRunAttackStoppedLoop = true;

        enemy.CanRunAttackLoop = true;
    }

    public bool BlockAllowedToChangeStateAnimationEvent() =>
        enemy.StateMachine.AllowedToChangeState = false;

    public bool UnblockAllowedToChangeStateAnimationEvent() =>
        enemy.StateMachine.AllowedToChangeState = true;

    /// <summary>
    /// Starts shake coroutine.
    /// </summary>
    private void StartShakeCoroutine() =>
        this.StartCoroutineWithReset(ref shakeCoroutine, ShakeCoroutine());

    /// <summary>
    /// Shakes enemy for x seconds.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator ShakeCoroutine()
    {
        Vector3 defaultPosition = enemyModel.transform.localPosition;
        currentShakingTime = Time.time;
        while (Time.time - currentShakingTime < TIMETOSHAKE)
        {
            enemyModel.transform.localPosition = defaultPosition;
            float randomNumber = Random.Range(-0.1f, 0.1f);
            enemyModel.transform.localPosition = defaultPosition + new Vector3(randomNumber, -randomNumber, randomNumber);
            yield return wffu;
        }
        enemyModel.transform.localPosition = defaultPosition;
    }

    /// <summary>
    /// Spawns broken enemy.
    /// </summary>
    public void Break()
    {
        meshRender.enabled = false;
        brokenEnemy.SetActive(true);
        brokenEnemy.transform.parent = null;
    }

    /// <summary>
    /// Animation event used to play enemy voice.
    /// </summary>
    public void PlayVoice() =>
        enemySounds.PlayVoice();

    /// <summary>
    /// Animation event used to play steps from an audio scriptab le object.
    /// </summary>
    public void PlayMovementSoundStep() =>
        enemySounds.PlayMovementSoundStep();

    /// <summary>
    /// Animation event used to play loop sounds directly from audiosource.
    /// </summary>
    public void PlayMovementSoundConstant() =>
        enemySounds.PlayMovementSoundConstant();

    /// <summary>
    /// Animation event used to deactivate health bar.
    /// </summary>
    public void DeactivateHealthBar() =>
        enemyHealthBar.gameObject.SetActive(false);

    /// <summary>
    /// Resets the class.
    /// </summary>
    public void ResetAll()
    {
        OnDisable();
        Awake();
        OnEnable();
    }

    public void TriggerEnragedAnimation() =>
        anim.SetBool("Enraged", true);
}
