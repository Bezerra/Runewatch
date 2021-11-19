using System.Collections;
using UnityEngine;
using Cinemachine;
using ExtensionMethods;

/// <summary>
/// Class responsible for generating a cinemachine source impulse.
/// Used by player to generate impulses.
/// </summary>
public class PlayerGenerateCinemachineImpulse : GenerateCinemachineImpulse
{
    // Components
    private PlayerCastSpell castSpell;
    private PlayerStats playerStats;

    // Coroutines
    private IEnumerator screenShakeCoroutine;
    private YieldInstruction continuousCastWFS;

    // Both impulse sources for different shots
    [SerializeField] private CinemachineImpulseSource oneShotImpulseSource;
    [SerializeField] private CinemachineImpulseSource continuousImpulseSource;
    [SerializeField] private CinemachineImpulseSource ontDamageReceiveImpulseSource;

    protected override void Awake()
    {
        base.Awake();
        castSpell = GetComponent<PlayerCastSpell>();
        playerStats = GetComponent<PlayerStats>();
        continuousCastWFS = new WaitForSeconds(0.2f);
    }

    private void OnEnable()
    {
        castSpell.EventStartScreenShake += GenerateCastImpulse;
        castSpell.EventCancelScreenShake += CancelScreenShake;
        playerStats.EventTakeDamagePosition += GenerateTakeDamageImpulse;
    }

    private void OnDisable()
    {
        castSpell.EventStartScreenShake -= GenerateCastImpulse;
        castSpell.EventCancelScreenShake -= CancelScreenShake;
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
    private void GenerateCastImpulse(SpellCastType castType)
    {
        if (castType != SpellCastType.ContinuousCast)
        {
            oneShotImpulseSource.GenerateImpulse(mainCam.transform.forward);
        }
        else
        {
            this.StartCoroutineWithReset(ref screenShakeCoroutine, ScreenShakeCoroutine());
        }
    }

    /// <summary>
    /// Starts shaking screen until a method stops it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScreenShakeCoroutine()
    {
        while (true)
        {
            yield return continuousCastWFS;
            continuousImpulseSource.GenerateImpulse(mainCam.transform.forward);
        }
    }
        
    /// <summary>
    /// Stops screen shake coroutine.
    /// </summary>
    private void CancelScreenShake()
    {
        if (screenShakeCoroutine != null) 
            StopCoroutine(screenShakeCoroutine);
    }
}
