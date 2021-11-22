using UnityEngine;

/// <summary>
/// Class responsible for playing enemy sounds.
/// </summary>
public class EnemySounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO takeDamage;

    private EnemyAudioSources audioSources;
    private EnemyStats enemyStats;

    private void Awake()
    {
        audioSources = GetComponentInChildren<EnemyAudioSources>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void OnEnable()
    {
        enemyStats.EventTakeDamage += PlayTakeDamage;
    }

    private void OnDisable()
    {
        enemyStats.EventTakeDamage -= PlayTakeDamage;
    }

    private void PlayTakeDamage() =>
        takeDamage.PlaySound(audioSources.EnemyHitAudioSource);

}
