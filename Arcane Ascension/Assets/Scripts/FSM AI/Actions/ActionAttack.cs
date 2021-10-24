using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to attack player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Random Attack", fileName = "Action Random Attack")]
sealed public class ActionAttack : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        Attack(ai);
    }

    /// <summary>
    /// If attack is not in cooldown and enemy is looking towards the player, the enemy
    /// casts a random spell from the available spells.
    /// </summary>
    /// <param name="ai">AI Character.</param>
    private void Attack(StateController<Enemy> ai)
    {
        // If it's not in attack delay
        if (Time.time - ai.Controller.TimeOfLastAttack > ai.Controller.AttackDelay)
        {
            if (ai.Controller.CurrentTarget != null)
            {
                // If the enemy is looking towards the player (with tolerance)
                if (ai.Controller.transform.IsLookingTowards(ai.Controller.CurrentTarget.position, 20))
                {
                    int randomSpell = Random.Range(0, ai.Controller.AllValues.CharacterStats.AvailableSpells.Count);

                    ISpell spell = ai.Controller.AllValues.CharacterStats.AvailableSpells[randomSpell];

                    if (spell.CastType == SpellCastType.OneShotCast)
                    {
                        ai.Controller.AllValues.CharacterStats.AvailableSpells[randomSpell].
                            AttackBehaviour.AttackKeyPress(spell, ai, ai.Controller.EnemyStats);
                    }

                    ai.Controller.TimeOfLastAttack = Time.time;

                    ai.Controller.AttackDelay = Random.Range(
                        ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);
                }
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        ai.Controller.TimeOfLastAttack = Time.time;

        ai.Controller.AttackDelay = Random.Range(
            ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
