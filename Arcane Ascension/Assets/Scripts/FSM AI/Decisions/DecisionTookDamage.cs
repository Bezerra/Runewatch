using UnityEngine;

/// <summary>
/// Decision that checks if enemy took damage.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Took Damage", fileName = "Decision Took Damage")]
public class DecisionTookDamage : FSMDecision
{
    public override bool CheckDecision(StateController aiCharacter)
    {
        bool tookDamage = aiCharacter.EnemyScript.TookDamage;

        if (tookDamage && aiCharacter.EnemyScript.PlayerScript != null)
        {
            if (aiCharacter.CurrentTarget == null)
                aiCharacter.CurrentTarget = FindObjectOfType<Player>().Eyes.transform;
        }
        return tookDamage;
    }

    public override void OnEnter(StateController aiCharacter)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController aiCharacter)
    {
        aiCharacter.EnemyScript.TookDamage = false;
    }
}
