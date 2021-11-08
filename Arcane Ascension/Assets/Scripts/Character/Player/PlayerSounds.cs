using UnityEngine;

/// <summary>
/// Class responsible for playing player's sounds.
/// </summary>
public class PlayerSounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO dashSound;
    [SerializeField] private AbstractSoundSO spendMoneySound;

    private PlayerAudioSources audioSources;
    private PlayerMovement playerMovement;
    private PlayerCurrency playerCurrency;

    private void Awake()
    {
        audioSources = GetComponentInChildren<PlayerAudioSources>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCurrency = GetComponent<PlayerCurrency>();
    }

    private void OnEnable()
    {
        playerMovement.EventDash += PlayDash;
        playerCurrency.EventSpendMoney += SpendMoney;
    }

    private void OnDisable()
    {
        playerMovement.EventDash -= PlayDash;
        playerCurrency.EventSpendMoney -= SpendMoney;
    }

    private void PlayDash() =>
        dashSound.PlaySound(audioSources.GetFreeAudioSource());

    private void SpendMoney() =>
        spendMoneySound.PlaySound(audioSources.GetFreeAudioSource());
}
