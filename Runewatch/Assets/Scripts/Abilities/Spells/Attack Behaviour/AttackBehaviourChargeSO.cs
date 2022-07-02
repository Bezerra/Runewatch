using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Scriptable object for one shot attacks.
/// </summary>
[CreateAssetMenu(menuName = "Attack Behaviour/Attack Behaviour Charge", fileName = "Attack Behaviour Charge")]
public class AttackBehaviourChargeSO : AttackBehaviourAbstractSO
{
    [SerializeField] float minChargeDamage = .5f;
    [SerializeField] float maxChargeDamage = 2f;
    [SerializeField] float fullChargeTime = 2f;


    private Vector3 DISTANTVECTOR = new Vector3(10000, 10000, 10000);

    float charge = 0;
    float timeStart;

    /// <summary>
    /// Attack behaviour for one shot spells. Instantiates the spell from a pool and triggers its start behaviour.
    /// </summary>
    /// <param name="currentlyCastSpell">Current spell being cast.</param> 
    /// <param name="spell">Spell to cast.</param>
    /// <param name="character">Character that cast the spell.</param>
    /// <param name="characterStats">Character that cast the spell stats.</param>
    /// <param name="spellBehaviour">Behaviour of the spell cast.</param>
    public override void AttackKeyPress(ref GameObject currentlyCastSpell, ISpell spell, 
        Character character, Stats characterStats, ref SpellBehaviourAbstract spellBehaviour)
    {
        Debug.Log("Start Charge");
        timeStart = Time.time;

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
    public override void AttackKeyPress(
        ISpell spell, StateController<Enemy> character, Stats characterStats)
    {

    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    /// <summary>
    /// What happens after the player releases attack key.
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
        float timeCharge = Time.time - timeStart;
        
        Debug.Log("Release Charge: " + CalcCharge(timeCharge));

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
        // Left blank on purpose
    }

    private float CalcCharge(float chargeTime)
    {
        chargeTime = Mathf.Min(chargeTime, fullChargeTime);
        return (FloatExtensions.Remap(0, fullChargeTime, minChargeDamage, maxChargeDamage, chargeTime));
    }
}
