using UnityEngine;

/// <summary>
/// Class responsible for playing enemy sounds.
/// </summary>
public class EnemySounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO takeDamage;

    private AudioSources audioSources;
    private EnemyStats enemyStats;

    private void Awake()
    {
        audioSources = GetComponentInChildren<AudioSources>();
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

    private void PlayTakeDamage(float emptyVar) =>
        takeDamage.PlaySound(audioSources.GetFreeAudioSource());

}
