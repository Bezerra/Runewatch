using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for casting a spell.
/// </summary>
public class PlayerCastSpell : MonoBehaviour
{
    private PlayerInputCustom input;
    private Stats playerStats;
    private PlayerSpells playerSpells;
    private Player player;

    private SpellBehaviourContinuous spellBehaviourContinuous;

    [SerializeField] private AttackBehaviourAbstract attackBehaviour;

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
        input.StopCastSpell += StopContinuousCast;
    }

    private void OnDisable()
    {
        input.CastSpell -= Cast;
        input.StopCastSpell -= StopContinuousCast;
    }

    /// <summary>
    /// Casts a spell. Happens when the player presses fire key.
    /// </summary>
    private void Cast()
    {
        // If spell is not in cooldown
        // Important for OneShot spells (continuous don't have cooldown)
        if (playerSpells.ActiveSpell?.CooldownCounter == playerSpells.ActiveSpell?.Cooldown)
        {
            // If player has enough mana to cast the active spell
            if (playerStats.Mana - playerSpells.ActiveSpell?.ManaCost >= 0)
            {
                //CastSpell(playerSpells.ActiveSpell);
                attackBehaviour.Attack(playerSpells.ActiveSpell, player, playerStats);
            }
        }
    }

    /// <summary>
    /// Disables continuous cast parent behaviour gameobject.
    /// </summary>
    private void StopContinuousCast()
    {
        if (spellBehaviourContinuous != null)
        {
            spellBehaviourContinuous.DisableSpell(spellBehaviourContinuous);
            spellBehaviourContinuous = null;
        }
    }

    /// <summary>
    /// Cast spell logic. Depends if the spell is SpellCastType.OneShot or ContinuousCast.
    /// </summary>
    /// <param name="spell">ISpell to cast.</param>
    /// <returns>Null.</returns>
    private void CastSpell(ISpell spell)
    {
        GameObject spawnedSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, player.
                Hand.position,
                Quaternion.identity);

        // Spellbehaviour will continue running until
        // the player releases cast spell button or 
        if (spell.CastType == SpellCastType.ContinuousCast)
        {
            // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
            spellBehaviourContinuous = spawnedSpell.GetComponent<SpellBehaviourContinuous>();
            spellBehaviourContinuous.WhoCast = playerStats;
            spellBehaviourContinuous.TriggerStartBehaviour();
        }
        else if (spell.CastType == SpellCastType.OneShotCast)
        {   
            // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
            SpellBehaviourOneShot spellBehaviour = spawnedSpell.GetComponent<SpellBehaviourOneShot>();
            spellBehaviour.WhoCast = playerStats;
            spellBehaviour.TriggerStartBehaviour();
        }
    }
}
