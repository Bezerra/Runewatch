using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to attack player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Random Attack", fileName = "Action Random Attack")]
sealed public class ActionAttack : FSMAction
{
    private float timeOfAttack;

    public override void Execute(StateController aiCharacter)
    {
        Attack(aiCharacter);
    }

    private void OnEnable()
    {
        timeOfAttack = Time.time;
    }

    /// <summary>
    /// If attack is not in cooldown and enemy is looking towards the player, the enemy
    /// casts a random spell from the available spells.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    private void Attack(StateController aiCharacter)
    {
        // If it's not in attack delay
        if (Time.time - timeOfAttack > aiCharacter.EnemyScript.Values.AttackDelay)
        {
            if (aiCharacter.CurrentTarget != null)
            {
                // If the enemy is looking towards the player (with tolerance)
                if (aiCharacter.transform.IsLookingTowards(aiCharacter.CurrentTarget.position, 20))
                {
                    int randomSpell = Random.Range(0, aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells.Count);

                    if (aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell].CastType == SpellCastType.OneShotCast)
                    {
                        Vector3 finalDirection =
                        aiCharacter.transform.position +
                        (aiCharacter.CurrentTarget.position - aiCharacter.transform.position);

                        GameObject spell = SpellPoolCreator.Pool.InstantiateFromPool(
                                aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell].Name,
                                aiCharacter.EnemyScript.Hand.position,
                                Quaternion.identity);

                        spell.transform.LookAt(finalDirection);
                        SpellBehaviourOneShot spellBehaviour = spell.GetComponent<SpellBehaviourOneShot>();
                        spellBehaviour.WhoCast = aiCharacter.EnemyScript.GetComponent<Stats>(); // Passes whoCast to the behaviour
                        spellBehaviour.TriggerStartBehaviour();

                        timeOfAttack = Time.time;
                    }
                }
            }
        }
    }

    public override void OnEnter(StateController aiCharacter)
    {
        timeOfAttack = Time.time;
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
