using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to attack player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Random Attack", fileName = "Action Random Attack")]
sealed public class ActionAttack : FSMAction
{
    public override void Execute(StateController aiCharacter)
    {
        Attack(aiCharacter);
    }

    /// <summary>
    /// If attack is not in cooldown and enemy is looking towards the player, the enemy
    /// casts a random spell from the available spells.
    /// </summary>
    /// <param name="aiCharacter">AI Character.</param>
    private void Attack(StateController aiCharacter)
    {
        // If it's not in attack delay
        if (Time.time - aiCharacter.EnemyScript.TimeOfLastAttack > aiCharacter.EnemyScript.AttackDelay)
        {
            if (aiCharacter.EnemyScript.CurrentTarget != null)
            {
                // If the enemy is looking towards the player (with tolerance)
                if (aiCharacter.EnemyScript.transform.IsLookingTowards(aiCharacter.EnemyScript.CurrentTarget.position, 20))
                {
                    int randomSpell = Random.Range(0, aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells.Count);

                    ISpell spell = aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell];

                    if (spell.CastType == SpellCastType.OneShotCast)
                    {
                        aiCharacter.EnemyScript.AllValues.CharacterStats.AvailableSpells[randomSpell].
                            AttackBehaviour.AttackKeyPress(spell, aiCharacter, aiCharacter.EnemyScript.EnemyStats);
                    }

                    aiCharacter.EnemyScript.TimeOfLastAttack = Time.time;

                    aiCharacter.EnemyScript.AttackDelay = Random.Range(
                        aiCharacter.EnemyScript.Values.AttackDelay.x, aiCharacter.EnemyScript.Values.AttackDelay.y);
                }
            }
        }
    }

    public override void OnEnter(StateController aiCharacter)
    {
        aiCharacter.EnemyScript.TimeOfLastAttack = Time.time;

        aiCharacter.EnemyScript.AttackDelay = Random.Range(
            aiCharacter.EnemyScript.Values.AttackDelay.x, aiCharacter.EnemyScript.Values.AttackDelay.y);
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
