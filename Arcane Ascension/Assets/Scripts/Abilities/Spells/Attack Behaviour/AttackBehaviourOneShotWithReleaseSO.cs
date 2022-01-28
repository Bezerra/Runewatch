using UnityEngine;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour One Shot With Release", 
    fileName = "Attack Behaviour One Shot With Release")]
public class AttackBehaviourOneShotWithReleaseSO : AttackBehaviourAbstractSO
{
    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    /// <summary>
    /// Attack behaviour for one shot spells with release. Instantiates the spell from a pool.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spawned spell gameobject.</param> 
    /// <param name="spell">Spell to cast.</param>
    /// <param name="character">Character that cast the spell.</param>
    /// <param name="characterStats">Character that cast the spell stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell cast.</param>
    public override void AttackKeyPress(ref GameObject currentlyCastSpell, ISpell spell, 
        Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        // This spell is only instantiated in here, it will be used on method AttackKeyRelease
        // Spawns in a distant vector, so it won't be seen for a frame before being disabled
        currentlyCastSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, DISTANTVECTOR, Quaternion.identity);
        
        // Gets behaviour of the spawned spell.
        // Starts the behaviour and passes whoCast object (stats) to the behaviour.
        spellBehaviour = currentlyCastSpell.GetComponent<SpellBehaviourOneShot>();

        // This has to happen here, so the scripts will have access to spellbehaviouroneshot variables
        spellBehaviour.WhoCast = characterStats;
    }

    /// <summary>
    /// Spawns a spell and triggers its behaviour.
    /// Used by AI.
    /// </summary>
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character (state controller) who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    public override void AttackKeyPress(ISpell spell, StateController<Enemy> character, Stats characterStats)
    {
        // This spell is only instantiated in here, it will be used on method AttackKeyRelease
        character.Controller.CurrentCastSpell =
            SpellPoolCreator.Pool.InstantiateFromPool(
                spell.Name, character.Controller.Hand.position,
                Quaternion.identity);

        // Gets behaviour of the spawned spell. Starts the behaviour and passes whoCast object (stats) to the behaviour.
        character.Controller.CurrentSpellBehaviour = 
            character.Controller.CurrentCastSpell.GetComponent<SpellBehaviourOneShot>();

        // This has to happen here, so the scripts will have access to spellbehaviouroneshot variables
        character.Controller.CurrentSpellBehaviour.WhoCast = characterStats;
    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spell being cast.</param> 
    /// <param name="spell">Cast spell.</param>
    /// <param name="character">Character who casts the spell.</param>
    /// <param name="characterStats">Character stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell.</param>
    public override void AttackKeyRelease(
        ref GameObject currentlyCastSpell, ISpell spell, Character character, 
        Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        // Now that the spell was instantiated, if the player releases fire key, it will trigger it's behaviours, etc

        // If a spell was spawned on attack press
        // Sets position and rotation and triggers its start behaviour
        if (currentlyCastSpell != null)
        {
            currentlyCastSpell.transform.SetPositionAndRotation(
                character.Hand.position, Quaternion.identity);
            spellBehaviour.TriggerStartBehaviour();
        }
    }

    /// <summary>
    /// What happens after the AI releases the attack.
    /// </summary>
    /// <param name="character"></param>
    public override void AttackKeyRelease(StateController<Enemy> character)
    {
        // If a spell was spawned on attack press
        // Sets position and rotation and triggers its start behaviour
        if (character.Controller.CurrentCastSpell != null)
        {
            character.Controller.CurrentCastSpell.transform.SetPositionAndRotation(
                character.Controller.Hand.position, Quaternion.identity);
            character.Controller.CurrentSpellBehaviour.TriggerStartBehaviour();
        }
    }
}
