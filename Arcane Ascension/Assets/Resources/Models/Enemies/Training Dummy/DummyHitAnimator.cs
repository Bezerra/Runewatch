using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHitAnimator : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Animator anim;

    private void Awake()
    {
        enemyStats = GetComponentInParent<EnemyStats>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += HitAnimation;
        enemyStats.EventDeath += DeathAnimation;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= HitAnimation;
        enemyStats.EventDeath -= DeathAnimation;
    }

    private void HitAnimation()
    {
        anim.SetTrigger("Hit");
        StartCoroutine(ResetTrigger());
    }

    private void DeathAnimation(Stats stats)
        => anim.SetTrigger("Death");

    public void DestroyOnAnimationEvent() =>
        Destroy(GetComponentInParent<SelectionBase>().gameObject);

    private IEnumerator ResetTrigger()
    {
        yield return null;
        anim.ResetTrigger("Hit");
    }
}
