using UnityEngine;

/// <summary>
/// Class responsible for playing enemy sounds.
/// </summary>
public class EnemySounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO takeDamage;
    [SerializeField] private AbstractSoundSO movementAudioAsset;
    
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

    public void PlayMovementSoundConstant() =>
        audioSources.MovementAudioSource.Play();

    public void PlayMovementSoundStep() =>
        movementAudioAsset.PlaySound(audioSources.MovementAudioSource);

    public void PlayVoice() =>
        audioSources.VoiceAudioSource.Play();
}
