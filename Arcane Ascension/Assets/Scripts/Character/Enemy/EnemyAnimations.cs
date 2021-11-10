using UnityEngine;

/// <summary>
/// Class responsible for handling a monster animations.
/// </summary>
public class EnemyAnimations : MonoBehaviour
{
    private Animator anim;

    private Enemy enemy;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        anim.SetFloat("Movement", enemy.Agent.velocity.magnitude);
    }
}
