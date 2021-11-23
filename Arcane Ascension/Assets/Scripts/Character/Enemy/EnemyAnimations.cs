using UnityEngine;
using System.Collections;
using ExtensionMethods;

/// <summary>
/// Class responsible for handling a monster animations.
/// </summary>
public class EnemyAnimations : MonoBehaviour, IEnemyAnimator
{
    // Components
    private Animator anim;
    private Enemy enemy;
    private EnemyStats enemyStats;
    private Transform enemyModel;

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
        wffu = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += StartShakeCoroutine;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= StartShakeCoroutine;
    }

    private void Update()
    {
        Vector3 movementZ = enemy.Agent.velocity.z * enemy.transform.forward;
        Vector3 movementX = enemy.Agent.velocity.x * enemy.transform.right;
        Vector3 movementDirection = movementZ + movementX;

        anim.SetFloat("VelocityZ", movementDirection.z);
        anim.SetFloat("VelocityX", movementDirection.x);
    }

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
                enemy.Values.AttackDelay.x, enemy.Values.AttackDelay.y);

        // Agent can move again
        enemy.Agent.speed = enemy.Values.Speed;

        enemy.IsAttackingWithStoppingTime = false;

        enemy.StateMachine.AllowedToChangeState = true;

        enemy.CanRunAttackStoppedLoop = true;

        enemy.CanRunAttackLoop = true;
    }



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
}
