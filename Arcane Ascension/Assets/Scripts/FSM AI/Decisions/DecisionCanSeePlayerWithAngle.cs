using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Decision that checks if the enemy can see the player.
/// </summary>
[CreateAssetMenu(menuName = "FSM/Decisions/Decision Can See Player With Angle", fileName = "Decision Can See Player With Angle")]
sealed public class DecisionCanSeePlayerWithAngle : FSMDecision
{
    [SerializeField] private LayerMask playerLayer;

    [Header("Layers that will be checked. For ex: Walls, Player, Floor, Objects, Player layer MUST be here")]
    [SerializeField] private LayerMask layersToCheck;

    [SerializeField] private float angle = 60f;

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
                    if (ai.Controller.transform.IsLookingTowards(colliders[i].transform, angle))
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
        }
        return false;
    }

    public override void OnEnter(StateController<Enemy> ai)
    {
        // Left blank on purpose
    }

    public override void OnExit(StateController<Enemy> ai)
    {
        if (ai.Controller.CurrentTarget != null)
        {
            ai.Controller.PlayerLastKnownPosition = ai.Controller.CurrentTarget.position;
        }

        ai.Controller.CurrentTarget = null;
    }
}
