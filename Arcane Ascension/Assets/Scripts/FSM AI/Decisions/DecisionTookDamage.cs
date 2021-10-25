using UnityEngine;

/// <summary>
/// Decision that checks if enemy took damage.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Took Damage", fileName = "Decision Took Damage")]
public class DecisionTookDamage : FSMDecision
{
    public override bool CheckDecision(StateController<Enemy> ai)
    {
        bool tookDamage = ai.Controller.TookDamage;

        if (tookDamage && ai.Controller.PlayerScript != null)
        {
            if (ai.Controller.CurrentTarget == null)
                ai.Controller.CurrentTarget = ai.Controller.PlayerScript.Eyes.transform;
        }
        return tookDamage;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        ai.Controller.TookDamage = false;
    }
}
