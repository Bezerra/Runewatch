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

    public override bool CheckDecision(StateController<Enemy> ai)
    {
        return LookForPlayer(ai);
    }

    /// <summary>
    /// Looks for player.
    /// </summary>
    /// <param name="ai">AI Controller.</param>
    /// <returns>True if it detects a player without a wall in front.</returns>
    private bool LookForPlayer(StateController<Enemy> ai)
    {
        Collider[] colliders = Physics.OverlapSphere(
            ai.Controller.transform.position, ai.Controller.Values.VisionRange, playerLayer);

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (ai.Controller.transform.CanSee(colliders[i].transform, layersToCheck))
                {
                    if (colliders[i].TryGetComponentInParent<Player>(out Player player))
                    {
                        ai.Controller.CurrentTarget = player.Eyes.transform;
                    }
                    else
                    {
                        ai.Controller.CurrentTarget = colliders[i].transform;
                    }
                    
                    return true;
                }
            } 
        }

        ai.Controller.CurrentTarget = null;
        return false;
    }

    public override void OnEnter(StateController<Enemy> aiCharacter)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> aiCharacter)
    {
        // Left blank on purpose
    }
}
