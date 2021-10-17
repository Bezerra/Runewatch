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

                    ISpell spell = aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell];

                    if (spell.CastType == SpellCastType.OneShotCast)
                    {
                        aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell].
                            AttackBehaviourOneShot.Attack(spell, aiCharacter, aiCharacter.EnemyScript.EnemyStats);
                    }
                    else if (spell.CastType == SpellCastType.OneShotCast)
                    {
                        aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell].
                            AttackBehaviourContinuous.Attack(spell, aiCharacter, aiCharacter.EnemyScript.EnemyStats);
                    }

                        timeOfAttack = Time.time;
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
