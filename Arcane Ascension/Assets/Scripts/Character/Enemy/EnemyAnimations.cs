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
        anim.SetFloat("Movement", enemy.Agent.velocity.magnitude);
    }

    /// <summary>
    /// Triggers enemy attack.
    /// </summary>
    /// <param name="enemyAttackType">Type of the attack.</param>
    public void TriggerAttack(EnemyAttackType enemyAttackType)
    {
        switch(enemyAttackType)
        {
            case EnemyAttackType.Melee:
                anim.SetTrigger("AttackMelee");
                break;
            case EnemyAttackType.OneShotSpell:
                anim.SetTrigger("AttackSpell");
                break;
            case EnemyAttackType.OneShotSpellWithRelease:
                anim.SetTrigger("AttackChannelingSpell");
                break;
        }
    }

    /// <summary>
    /// Executes AttackKeyPress of the current spell.
    /// </summary>
    public void ReleaseAttackAnimationEvent()
    {
        enemy.CurrentlySelectedSpell.Spell.
            AttackBehaviour.AttackKeyPress(enemy.CurrentlySelectedSpell.Spell,
            enemy.StateMachine, enemy.EnemyStats);
    }

    /// <summary>
    /// Starts shake coroutine.
    /// </summary>
    /// <param name="emptyVar"></param>
    private void StartShakeCoroutine(float emptyVar) =>
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
