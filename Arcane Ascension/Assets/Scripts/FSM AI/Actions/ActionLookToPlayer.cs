using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Action to chase player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Actions/Action Look To Player", fileName = "Action Look To Player")]
sealed public class ActionLookToPlayer : FSMAction
{
    public override void Execute(StateController<Enemy> ai)
    {
        RotateToPlayer(ai);
    }

    private void RotateToPlayer(StateController<Enemy> ai)
    {
        if (ai.Controller.MovingCloseToAttack)
            return;

        if (ai.Controller.CurrentTarget != null)
        {
            if (ai.Controller.RunningBackwards == false)
            {
                // If the enemy is in the middle of an attack that requires him to stop
                if (ai.Controller.IsAttackingWithStoppingTime)
                {
                    // If the attack is of type one shot cast with release
                    if (ai.Controller.CurrentlySelectedSpell.Spell.CastType == SpellCastType.OneShotCastWithRelease)
                    {
                        // If the time passed while attacking has already reached the time to
                        // start showing the spell area, then it will ignore the rest of the method.
                        if (Time.time - ai.Controller.TimeEnemyStoppedWhileAttacking >
                            ai.Controller.CurrentlySelectedSpell.StoppingTime *
                            ai.Controller.CurrentlySelectedSpell.PercentageStoppingTimeTriggerAoESpell)
                        {
                            return;
                        }
                    }
                }

                // Rotates to player
                ai.Controller.transform.LookAtYLerp(ai.Controller.CurrentTarget, ai.Controller.Values.RotationSpeed);
            }
        }
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }
}
