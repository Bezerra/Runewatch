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
        if (ai.Controller.IsAttackingWithStoppingTime &&
            ai.Controller.CanRunAttackStoppedLoop)
        {
            // Agent can't move
            ai.Controller.Agent.isStopped = true;
            ai.AllowedToChangeState = false;

            if (Time.time - ai.Controller.TimeEnemyStoppedWhileAttacking >    
                ai.Controller.CurrentlySelectedSpell.StoppingTime)
            {
                if (ai.Controller.CurrentlySelectedSpell.Spell.CastType == SpellCastType.OneShotCastWithRelease)
                {
                    ai.Controller.CanRunAttackStoppedLoop = false;
                    ai.Controller.Animation.TriggerAttack(EnemyAttackTypeAnimations.OneShotSpellWithRelease);
                }
                else
                {
                    // If it is not a one shot cast with release, it will reset variables immediatly

                    // Gets new random spell
                    int randomSpell = ai.Controller.Random.RandomWeight(ai.Controller.EnemyStats.AvailableSpellsWeight);
                    ai.Controller.CurrentlySelectedSpell =
                         ai.Controller.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];

                    // Updates time of last attack delay, so it starts a new delay for attack
                    ai.Controller.TimeOfLastAttack = Time.time;

                    // Gets a new attack delay
                    ai.Controller.AttackDelay = Random.Range(
                            ai.Controller.EnemyStats.EnemyAttributes.AttackingDelay.x,
                            ai.Controller.EnemyStats.EnemyAttributes.AttackingDelay.y);

                    ai.Controller.Agent.isStopped = false;

                    ai.Controller.IsAttackingWithStoppingTime = false;

                    ai.AllowedToChangeState = true;

                    ai.Controller.CanRunAttackLoop = true;
                }
            }
            return;
        }
        
        // Block of code if the enemy is not stopped in the middle of an attack ---
        // If it's not in attack delay
        if (Time.time - ai.Controller.TimeOfLastAttack > ai.Controller.AttackDelay &&
            ai.Controller.CanRunAttackLoop)
        {
            if (ai.Controller.CurrentTarget != null)
            {
                // If too much time has passed and the enemy wasn't able to attack
                if (Time.time - ai.Controller.TimeOfLastAttack > ai.Controller.AttackDelay + 3)
                {
                    // Gets new random spell
                    int randomSpell = ai.Controller.Random.RandomWeight
                        (ai.Controller.EnemyStats.AvailableSpellsWeight);
                    ai.Controller.CurrentlySelectedSpell =
                        ai.Controller.EnemyStats.EnemyAttributes.AllEnemySpells[randomSpell];
                }

                // If that range's distance is not met, it will ignore the rest of the method
                // Adds a small value as compensation, so it can fail precise distance check safely
                if (Vector3.Distance(ai.Controller.transform.position, 
                    ai.Controller.CurrentTarget.transform.position) >
                    ai.Controller.CurrentlySelectedSpell.Range + 0.02f)
                {
                    return;
                }

                // If the enemy is looking towards the player (with tolerance)
                if (ai.Controller.transform.IsLookingTowards(
                    ai.Controller.CurrentTarget.position, true, ai.Controller.Values.FireAllowedAngle))
                {
                    ai.Controller.CanRunAttackLoop = false;

                    ai.Controller.Animation.TriggerAttack(
                        ai.Controller.CurrentlySelectedSpell.EnemyAttackType);
                    // ATTACK IS EXECUTED THROUGH AN ANIMATION EVENT WHILE INSIDE THIS ANIMATION
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
            ai.Controller.EnemyStats.EnemyAttributes.AttackingDelay.x,
            ai.Controller.EnemyStats.EnemyAttributes.AttackingDelay.y);

        ai.Controller.IsAttackingWithStoppingTime = false;

        ai.Controller.TimeEnemyStoppedWhileAttacking = 0;

        ai.Controller.CanRunAttackStoppedLoop = true;

        ai.Controller.CanRunAttackLoop = true;
    }

    /// <summary>
    /// Resets variables and sets speed back to normal.
    /// </summary>
    /// <param name="ai"></param>
    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.IsAttackingWithStoppingTime = false;
        ai.Controller.TimeEnemyStoppedWhileAttacking = 0;
        ai.Controller.Agent.isStopped = false;
        ai.AllowedToChangeState = true;
        ai.Controller.CanRunAttackStoppedLoop = true;
        ai.Controller.CanRunAttackLoop = true;
    }
}
