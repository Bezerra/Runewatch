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
        // Block of code to stop an enemy in the middle of an attack ---
        // If attack stopping time is not over yet (this code always happens
        // after the enemy initiated an attack, never at the beggining of the state)
        if (ai.Controller.IsAttackingWithStoppingTime)
        {
            // Agent can't move
            ai.Controller.Agent.speed = 0;
            ai.AllowedToChangeState = false;

            if (Time.time - ai.Controller.TimeEnemyStoppedWhileAttacking > ai.Controller.CurrentlySelectedSpell.StoppingTime)
            {
                if (ai.Controller.CurrentlySelectedSpell.Spell.CastType == SpellCastType.OneShotCastWithRelease)
                {
                    ai.Controller.Animation.TriggerAttack(EnemyAttackTypeAnimations.OneShotSpellWithRelease);
                }

                // Gets new random spell
                int randomSpell = ai.Controller.Random.RandomWeight(ai.Controller.EnemyStats.AvailableSpellsWeight);
                ai.Controller.CurrentlySelectedSpell =
                     ai.Controller.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];

                // Updates time of last attack delay, so it starts a new delay for attack
                ai.Controller.TimeOfLastAttack = Time.time;

                // Gets a new attack delay
                ai.Controller.AttackDelay = Random.Range(
                        ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);

                // Agent can move again
                ai.Controller.Agent.speed = ai.Controller.Values.Speed;

                ai.Controller.IsAttackingWithStoppingTime = false;

                ai.AllowedToChangeState = true;
            }
            return;
        }
        
        // Block of code if the enemy is not stopped in the middle of an attack ---
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
                    ai.Controller.Animation.TriggerAttack(
                        ai.Controller.CurrentlySelectedSpell.EnemyAttackType);
                    // ATTACK IS EXECUTED THROUGH AN ANIMATION EVENT WHILE INSIDE THIS ANIMATION

                    // If the enemy has a spell that needs the enemy to stop while attacking
                    // It will update a new timer and ignore the rest of the method
                    if (ai.Controller.CurrentlySelectedSpell.EnemyStopsOnAttack)
                    {
                        ai.Controller.IsAttackingWithStoppingTime = true;
                        ai.Controller.TimeEnemyStoppedWhileAttacking = Time.time;
                        return;
                    }

                    ai.Controller.TimeOfLastAttack = Time.time;

                    ai.Controller.AttackDelay = Random.Range(
                        ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);

                    // Gets new random spell
                    // Gets new random spell
                    int randomSpell = ai.Controller.Random.RandomWeight(ai.Controller.EnemyStats.AvailableSpellsWeight);
                    ai.Controller.CurrentlySelectedSpell =
                         ai.Controller.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];
                }
            }
        }
    }

    /// <summary>
    /// Resets variables.
    /// </summary>
    /// <param name="ai"></param>
    public override void OnEnter(StateController<Enemy> ai)
    {
        // Gets new random spell
        int randomSpell = ai.Controller.Random.RandomWeight(ai.Controller.EnemyStats.AvailableSpellsWeight);
        ai.Controller.CurrentlySelectedSpell =
             ai.Controller.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];

        ai.Controller.TimeOfLastAttack = Time.time;

        // Sets a new attack delay
        ai.Controller.AttackDelay = Random.Range(
            ai.Controller.Values.AttackDelay.x, ai.Controller.Values.AttackDelay.y);

        ai.Controller.IsAttackingWithStoppingTime = false;
        ai.Controller.TimeEnemyStoppedWhileAttacking = 0;
    }

    /// <summary>
    /// Resets variables and sets speed back to normal.
    /// </summary>
    /// <param name="ai"></param>
    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.IsAttackingWithStoppingTime = false;
        ai.Controller.TimeEnemyStoppedWhileAttacking = 0;
        ai.Controller.Agent.speed = ai.Controller.Values.Speed;
        ai.AllowedToChangeState = true;
    }
}
