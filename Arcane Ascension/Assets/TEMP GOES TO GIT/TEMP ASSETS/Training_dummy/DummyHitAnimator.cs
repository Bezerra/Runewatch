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
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= HitAnimation;
    }

    private void HitAnimation()
    {
        anim.SetTrigger("Hit");
        StartCoroutine(ResetTrigger());
    }

    private IEnumerator ResetTrigger()
    {
        yield return null;
        anim.ResetTrigger("Hit");
    }
}
