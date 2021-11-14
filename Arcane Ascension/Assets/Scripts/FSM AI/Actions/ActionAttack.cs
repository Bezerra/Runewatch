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
                // If the enemy needs to be at a certain range
                if (ai.Controller.CurrentlySelectedSpell.NeedsToBeInRangeToAttack)
                {
                    // If that range's distance is not met, it will ignore the rest of the method
                    if (Vector3.Distance(ai.Controller.transform.position, ai.Controller.CurrentTarget.transform.position) >
                        ai.Controller.CurrentlySelectedSpell.Range)
                    {
                        return;
                    }
                }

                // If the enemy is looking towards the player (with tolerance)
                if (ai.Controller.transform.IsLookingTowards(
                    ai.Controller.CurrentTarget.position, true, ai.Controller.Values.FireAllowedAngle))
                {
                    if (ai.Controller.CurrentlySelectedSpell.Spell.CastType == SpellCastType.OneShotCast)
                    {
                        ai.Controller.CurrentlySelectedSpell.Spell.
                            AttackBehaviour.AttackKeyPress(ai.Controller.CurrentlySelectedSpell.Spell, 
                            ai, ai.Controller.EnemyStats);
                    }

                    ai.Controller.TimeOfLastAttack = Time.time;

                    ai.Controller.AttackDelay = Random.Range(
                        ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);

                    // Gets new random spell
                    int randomSpell = Random.Range(0, ai.Controller.EnemyStats.EnemyAttributes.AvailableSpells.Count);
                    ai.Controller.CurrentlySelectedSpell =
                        ai.Controller.EnemyStats.EnemyAttributes.AvailableSpells[randomSpell];
                }
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Gets new random spell
        int randomSpell = Random.Range(0, ai.Controller.EnemyStats.EnemyAttributes.AvailableSpells.Count);
        ai.Controller.CurrentlySelectedSpell =
            ai.Controller.EnemyStats.EnemyAttributes.AvailableSpells[randomSpell];

        ai.Controller.TimeOfLastAttack = Time.time;

        // Sets a new attack delay
        ai.Controller.AttackDelay = Random.Range(
            ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.Agent.isStopped = false;
    }
}
