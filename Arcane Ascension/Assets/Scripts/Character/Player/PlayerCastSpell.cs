using System.Collections;
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

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerStats = GetComponent<Stats>();
        playerSpells = GetComponent<PlayerSpells>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        input.CastSpell += Cast;
        input.StopCastSpell += DisableSpell;
    }

    private void OnDisable()
    {
        input.CastSpell -= Cast;
        input.StopCastSpell -= DisableSpell;
    }

    /// <summary>
    /// Casts a spell. Happens when the player presses fire key.
    /// </summary>
    private void Cast()
    {
        // If spell is not in cooldown
        // Important for OneShot spells (continuous don't have cooldown)
        if (playerSpells.CooldownOver(playerSpells.ActiveSpell))
        {
            // If player has enough mana to cast the active spell
            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost >= 0)
            {
                spellBehaviour = null;
                if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCast)
                {
                    playerSpells.ActiveSpell.AttackBehaviourOneShot.Attack(playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);
                }
                else if (playerSpells.ActiveSpell.CastType == SpellCastType.ContinuousCast)
                {
                    playerSpells.ActiveSpell.AttackBehaviourContinuous.Attack(playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);
                }
            }
        }
    }

    /// <summary>
    /// Disables continuous cast parent behaviour gameobject.
    /// </summary>
    private void DisableSpell()
    {
        if (playerSpells.ActiveSpell.CastType == SpellCastType.ContinuousCast)
        {
            playerSpells.ActiveSpell.AttackBehaviourContinuous.DisableSpell(spellBehaviour);
        }
    }
}
