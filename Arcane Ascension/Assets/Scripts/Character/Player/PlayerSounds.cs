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
    [SerializeField] private List<SoundAssetWithType> stepSounds;

    // Components
    private PlayerAudioSources audioSources;
    private PlayerMovement playerMovement;
    private PlayerCurrency playerCurrency;

    // Stepsounds
    private IDictionary<SurfaceType, AbstractSoundSO> stepSoundsDictionary;
    private float timeToStep;
    private readonly float STEPSOUNDDELAY = 0.5f;

    private void Awake()
    {
        audioSources = GetComponentInChildren<PlayerAudioSources>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCurrency = GetComponent<PlayerCurrency>();
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

    private void Update()
    {
        if (Time.time - timeToStep > STEPSOUNDDELAY)
        {
            if (playerMovement.IsPlayerMoving())
            {
                Ray rayToBottom = new Ray(transform.position + Vector3.up, (transform.position + Vector3.up).Direction(Vector3.down));
                if (Physics.Raycast(rayToBottom, out RaycastHit hit, 3, Layers.WallsFloor))
                {
                    if (hit.collider.TryGetComponent(out ISurface surface))
                    {
                        stepSoundsDictionary[surface.SurfaceType].PlaySound(audioSources.PlayerStepsAudioSource);
                    }
                }
                timeToStep = Time.time;
            }
        } 
    }
}
