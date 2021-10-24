using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Decision that checks if the enemy can see the player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Can See Player", fileName = "Decision Can See Player")]
sealed public class DecisionCanSeePlayer : FSMDecision
{
    [SerializeField] private LayerMask playerLayer;

    [Header("Layers that will be checked. For ex: Walls, Player, Floor, Objects, Player layer MUST be here")]
    [SerializeField] private LayerMask layersToCheck;

    public override bool CheckDecision(StateController aiController)
    {
        return LookForPlayer(aiController);
    }

    /// <summary>
    /// Looks for player.
    /// </summary>
    /// <param name="aiController">AI Controller.</param>
    /// <returns>True if it detects a player without a wall in front.</returns>
    private bool LookForPlayer(StateController aiController)
    {
        Collider[] colliders = Physics.OverlapSphere(
            aiController.EnemyScript.transform.position, aiController.EnemyScript.Values.VisionRange, playerLayer);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (aiController.EnemyScript.transform.CanSee(colliders[i].transform, layersToCheck))
                {
                    if (colliders[i].TryGetComponentInParent<Player>(out Player player))
                    {
                        aiController.EnemyScript.CurrentTarget = player.Eyes.transform;
                    }
                    else
                    {
                        aiController.EnemyScript.CurrentTarget = colliders[i].transform;
                    }
                    
                    return true;
                }
            } 
        }

        aiController.EnemyScript.CurrentTarget = null;
        return false;
    }

    public override void OnEnter(StateController aiCharacter)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController aiCharacter)
    {
        // Left blank on purpose
    }
}
