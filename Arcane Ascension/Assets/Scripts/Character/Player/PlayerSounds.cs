using UnityEngine;
using System.Collections.Generic;
using ExtensionMethods;

/// <summary>
/// Class responsible for playing player's sounds.
/// </summary>
public class PlayerSounds : MonoBehaviour
{
    // Sounds
    [SerializeField] private AbstractSoundSO dashSound;
    [SerializeField] private AbstractSoundSO spendMoneySound;
    [SerializeField] private AbstractSoundSO takeDamage;
    [SerializeField] private List<SoundAssetWithType> stepSounds;

    // Components
    private PlayerAudioSources audioSources;
    private PlayerMovement playerMovement;
    private PlayerCurrency playerCurrency;
    private PlayerStats playerStats;
    private Player player;

    // Stepsounds
    private IDictionary<SurfaceType, AbstractSoundSO> stepSoundsDictionary;
    private float timeToStep;
    private readonly float STEPSOUNDDELAYWALKING = 0.6f;
    private readonly float STEPSOUNDDELAYRUNNING = 0.35f;
    private float stepSoundDelay;

    private void Awake()
    {
        audioSources = GetComponentInChildren<PlayerAudioSources>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCurrency = GetComponent<PlayerCurrency>();
        playerStats = GetComponent<PlayerStats>();
        player = GetComponent<Player>();
        timeToStep = 0;

        stepSoundsDictionary = new Dictionary<SurfaceType, AbstractSoundSO>();
        foreach(SoundAssetWithType stepSound in stepSounds)
        {
            stepSoundsDictionary.Add(stepSound.SurfaceType, stepSound.SurfaceSound);
        }
    }

    private void OnEnable()
    {
        playerMovement.EventDash += PlayDash;
        playerCurrency.EventSpendMoney += SpendMoney;
        playerMovement.EventSpeedChange += UpdateStepSoundDelay;
        playerStats.EventTakeDamage += PlayTakeDamage;
    }

    private void OnDisable()
    {
        playerMovement.EventDash -= PlayDash;
        playerCurrency.EventSpendMoney -= SpendMoney;
        playerMovement.EventSpeedChange -= UpdateStepSoundDelay;
        playerStats.EventTakeDamage -= PlayTakeDamage;
    }

    private void PlayDash() =>
        dashSound.PlaySound(audioSources.GetFreeAudioSource());

    private void SpendMoney() =>
        spendMoneySound.PlaySound(audioSources.GetFreeAudioSource());

    private void PlayTakeDamage() =>
        takeDamage.PlaySound(audioSources.GetFreeAudioSource());

    /// <summary>
    /// Updates step sound delay based on player's speed.
    /// </summary>
    /// <param name="speed">Player's speed.</param>
    private void UpdateStepSoundDelay(float speed)
    {
        if (speed > player.Values.Speed * 
            playerStats.CommonAttributes.MovementSpeedMultiplier *
            playerStats.CommonAttributes.MovementStatusEffectMultiplier)
        {
            stepSoundDelay = STEPSOUNDDELAYRUNNING;
        }
        else
        {
            stepSoundDelay = STEPSOUNDDELAYWALKING;
        }
    }

    private void Update()
    {
        // If step delay is over
        if (Time.time - timeToStep > stepSoundDelay)
        {
            // If player is not grounded or is stopped, ignores this method.
            if (playerMovement.IsPlayerStopped(0.1f) ||
                (playerMovement.IsGrounded() == false))
                return;

            if (playerMovement.IsPlayerMoving(0.1f))
            {
                Ray rayToBottom = new Ray(
                    transform.position + transform.up, 
                    (transform.position + transform.up).Direction(transform.position + -transform.up));
                if (Physics.Raycast(rayToBottom, out RaycastHit hit, 2, Layers.WallsFloor))
                {
                    if (hit.collider.TryGetComponent(out ISurface surface))
                    {
                        stepSoundsDictionary[surface.SurfaceType].PlaySound(audioSources.PlayerStepsAudioSource);
                    }
                    else
                    {
                        Debug.Log(hit.collider.gameObject.name + " doesn't have Surface script.");
                    }
                }
                timeToStep = Time.time;
            }
        } 
    }
}
