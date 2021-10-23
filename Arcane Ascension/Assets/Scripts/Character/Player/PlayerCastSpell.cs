using System;
using UnityEngine;

/// <summary>
/// Class responsible for casting a spell.
/// </summary>
public class PlayerCastSpell : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private Stats playerStats;
    private PlayerSpells playerSpells;
    private Player player;

    // Current cast spell behaviour
    private SpellBehaviourAbstract spellBehaviour;
    private GameObject currentlyCastSpell;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerStats = GetComponent<Stats>();
        playerSpells = GetComponent<PlayerSpells>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        input.CastSpell += AttackKeyPress;
        input.StopCastSpell += AttackKeyRelease;
    }

    private void OnDisable()
    {
        input.CastSpell -= AttackKeyPress;
        input.StopCastSpell -= AttackKeyRelease;
    }

    /// <summary>
    /// Casts a spell. Happens when the player presses attack key.
    /// </summary>
    private void AttackKeyPress()
    {
        // If spell is not in cooldown
        // Important for OneShot spells (continuous don't have cooldown)
        if (playerSpells.CooldownOver(playerSpells.ActiveSpell))
        {
            // If player has enough mana to cast the active spell
            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost >= 0)
            {
                playerSpells.ActiveSpell.AttackBehaviour.AttackKeyPress(
                    ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

                if (playerSpells.ActiveSpell.CastType != SpellCastType.OneShotCastWithRelease)
                    OnEventAttack(playerSpells.ActiveSpell.CastType);
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
    private void AttackKeyRelease()
    {
        // If spell is not in cooldown
        // So this will one be triggered when attackKeyPress is triggered aswell
        if (playerSpells.CooldownOver(playerSpells.ActiveSpell))
        {
            playerSpells.ActiveSpell.AttackBehaviour.AttackKeyRelease(
                ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

            currentlyCastSpell = null;
            spellBehaviour = null;

            if (playerSpells.ActiveSpell.CastType != SpellCastType.OneShotCastWithRelease)
                OnEventCancelAttack();
            else
                OnEventAttack(playerSpells.ActiveSpell.CastType);
        }
    }

    // Registered on DelayedCamera for screen shake
    protected virtual void OnEventAttack(SpellCastType castType) => EventAttack?.Invoke(castType);
    public event Action<SpellCastType> EventAttack;
    // Registered on DelayedCamera for screen shake
    protected virtual void OnEventCancelAttack() => EventCancelAttack.Invoke();
    public event Action EventCancelAttack;
}
