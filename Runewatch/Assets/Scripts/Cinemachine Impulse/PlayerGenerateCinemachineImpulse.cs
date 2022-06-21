using UnityEngine;
using Cinemachine;

/// <summary>
/// Class responsible for generating a cinemachine source impulse.
/// Used by player to generate impulses.
/// </summary>
public class PlayerGenerateCinemachineImpulse : GenerateCinemachineImpulse
{
    // Components
    private PlayerCastSpell castSpell;
    private PlayerStats playerStats;

    // Both impulse sources for different shots
    [SerializeField] private CinemachineImpulseSource oneShotImpulseSource;
    [SerializeField] private CinemachineImpulseSource ontDamageReceiveImpulseSource;

    protected override void Awake()
    {
        base.Awake();
        castSpell = GetComponentInChildren<PlayerCastSpell>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable()
    {
        castSpell.EventStartScreenShake += GenerateCastImpulse;
        playerStats.EventTakeDamagePosition += GenerateTakeDamageImpulse;
    }

    private void OnDisable()
    {
        castSpell.EventStartScreenShake -= GenerateCastImpulse;
        playerStats.EventTakeDamagePosition -= GenerateTakeDamageImpulse;
    }

    private void GenerateTakeDamageImpulse(Vector3 damageDirection)
    {
        if (damageDirection != Vector3.zero)
            ontDamageReceiveImpulseSource.GenerateImpulse(mainCam.transform.forward);
    }

    /// <summary>
    /// Generates impulse or starts coroutine depending on the type of shot.
    /// </summary>
    /// <param name="castType">Cast type.</param>
    private void GenerateCastImpulse(SpellCastType castType) =>
        oneShotImpulseSource.GenerateImpulse(mainCam.transform.forward);
}
