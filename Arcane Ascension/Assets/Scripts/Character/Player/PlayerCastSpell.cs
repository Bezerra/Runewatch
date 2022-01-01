using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Class responsible for casting a spell.
/// </summary>
public class PlayerCastSpell : MonoBehaviour
{
    // Components
    private IInput input;
    private PlayerStats playerStats;
    private PlayerSpells playerSpells;
    private Player player;

    // Current cast spell behaviour
    private SpellBehaviourAbstract spellBehaviour;
    private GameObject currentlyCastSpell;
    public bool CurrentlyCasting => currentlyCastSpell == null ? false : true;

    private float lastTimeSpellWasCast;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerStats = GetComponent<PlayerStats>();
        playerSpells = GetComponent<PlayerSpells>();
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Coroutine that runs every X seconds.
    /// If the player has not enough mana to cast the active spell, 
    /// it cancels shake, cancels attack and resets casting spells variables.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        YieldInstruction wfs = new WaitForSeconds(0.2f);

        while(playerSpells.ActiveSpell == null)
        {
            yield return wfs;
        }

        // Will only run this after the player obtains a spell
        while (true)
        {
            yield return wfs;

            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost < 0)
            {
                OnEventCancelAttack();
                currentlyCastSpell = null;
                currentlyCastSpell = null;
            }
        }
    }

    private void OnEnable()
    {
        input.CastSpell += AttackKeyPress;
        input.StopCastSpell += AttackKeyRelease;
        input.CastBasicSpell += SecondaryAttackKeyPress;
    }

    private void OnDisable()
    {
        input.CastSpell -= AttackKeyPress;
        input.StopCastSpell -= AttackKeyRelease;
        input.CastBasicSpell -= SecondaryAttackKeyPress;
    }

    /// <summary>
    /// A fixed spell cast for every spells.
    /// </summary>
    /// <returns></returns>
    public bool SpellCastOnFixedDelay()
    {
        if (Time.time - lastTimeSpellWasCast > 0.15f)
            return false;
        return true;
    }

    /// <summary>
    /// Casts secondary basic spell.
    /// </summary>
    private void SecondaryAttackKeyPress()
    {
        // If main spell is not in cooldown
        if (playerSpells.CooldownOver(playerSpells.SecondarySpell) &&
            currentlyCastSpell == null)
        {
            if (SpellCastOnFixedDelay())
                return;
            lastTimeSpellWasCast = Time.time;

            playerSpells.SecondarySpell.AttackBehaviour.AttackKeyPress(
                    ref currentlyCastSpell, playerSpells.SecondarySpell, player, playerStats, ref spellBehaviour);

            OnEventAttack(playerSpells.SecondarySpell.CastType);
            OnEventStartCooldown(playerSpells.SecondarySpell);

            currentlyCastSpell = null;
        }
    }

    /// <summary>
    /// Casts a spell. Happens when the player presses attack key.
    /// </summary>
    private void AttackKeyPress()
    {
        if (playerSpells.ActiveSpell == null)
            return;

        // If spell is not in cooldown
        // Important for OneShot spells (continuous don't have cooldown)
        if (playerSpells.CooldownOver(playerSpells.ActiveSpell) &&
        playerSpells.CooldownOver(playerSpells.SecondarySpell))
        {
            if (SpellCastOnFixedDelay())
                return;

            // If player has enough mana to cast the active spell
            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost > 0)
            {
                if (SpellCastOnFixedDelay())
                    return;
                lastTimeSpellWasCast = Time.time;

                currentlyCastSpell = null;

                playerSpells.ActiveSpell.AttackBehaviour.AttackKeyPress(
                    ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

                // Mana and cooldown on oneshot
                if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCast)
                {
                    OnEventStartCooldown(playerSpells.ActiveSpell);
                    OnEventSpendMana(playerSpells.ActiveSpell.ManaCost);
                }

                // Attack Events
                // Screen Shake Events
                if (playerSpells.ActiveSpell.CastType != SpellCastType.OneShotCastWithRelease)
                {
                    // For One Shot and Continuous
                    OnEventAttack(playerSpells.ActiveSpell.CastType);
                    OnEventStartScreenShake(playerSpells.ActiveSpell.CastType);
                }
            }
            else
            {
                OnEventCancelAttack();
            }
        }
    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    public void AttackKeyRelease()
    {
        if (playerSpells.ActiveSpell == null)
            return;

        if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCastWithRelease)
        {
            // If player has a spell being prepared in hand and CDs are over
            if (playerSpells.CooldownOver(playerSpells.ActiveSpell) &&
                playerSpells.CooldownOver(playerSpells.SecondarySpell) &&
                currentlyCastSpell != null)
            {
                // If he has enough mana, casts spell, spends mana and updates cd.
                if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost > 0)
                {
                    playerSpells.ActiveSpell.AttackBehaviour.AttackKeyRelease(
                        ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

                    OnEventStartCooldown(playerSpells.ActiveSpell);
                    OnEventSpendMana(playerSpells.ActiveSpell.ManaCost);
                    OnEventAttack(playerSpells.ActiveSpell.CastType);
                }
            }
            else
            {
                // If player has not mana, ignores this method
                currentlyCastSpell = null;
                return;
            }
        }
        else
        {
            playerSpells.ActiveSpell.AttackBehaviour.AttackKeyRelease(
                ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

            // For the rest of spells, invokes events.
            OnEventCancelAttack();
        }

        spellBehaviour = null;
        currentlyCastSpell = null;
    }

    protected virtual void OnEventStartCooldown(ISpell spell) => EventStartCooldown?.Invoke(spell);
    public event Action<ISpell> EventStartCooldown;

    protected virtual void OnEventSpendMana(float amount) => EventSpendMana?.Invoke(amount);
    public event Action<float> EventSpendMana;

    protected virtual void OnEventAttack(SpellCastType castType) => EventAttack?.Invoke(castType);
    public event Action<SpellCastType> EventAttack;
    protected virtual void OnEventCancelAttack() => EventCancelAttack.Invoke();
    public event Action EventCancelAttack;

    // Registered on PlayerGenerateCinemachineImpulse
    protected virtual void OnEventStartScreenShake(SpellCastType castType) => EventStartScreenShake?.Invoke(castType);
    public event Action<SpellCastType> EventStartScreenShake;
}
