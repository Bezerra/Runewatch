using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for handling a monster animations.
/// </summary>
public class EnemyAnimations : MonoBehaviour
{
    private Animator anim;

    private Enemy enemy;
    private EnemyStats enemyStats;

    [SerializeField] private Transform enemyModel;

    // Coroutines
    private IEnumerator shakeCoroutine;
    private YieldInstruction wffu;
    private readonly float TIMETOSHAKE = 0.25f;
    private float currentShakingTime;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
        enemyStats = GetComponent<EnemyStats>();
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

    private void StartShakeCoroutine(float emptyVar)
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = ShakeCoroutine();
        StartCoroutine(shakeCoroutine);
    }

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
