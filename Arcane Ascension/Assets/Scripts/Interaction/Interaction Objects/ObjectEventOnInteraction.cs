using UnityEngine;

/// <summary>
/// Class used by pickable item gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class ObjectEventOnInteraction : AbstractEventOnInteraction, IInterectable
{
    /// <summary>
    /// Executes a list of events.
    /// </summary>
    public override void Execute()
    {
        if (eventOnInteraction.Count > 0)
        {
            // To prevent null errors if reference is lost
            if (playerInteraction == null)
                playerInteraction = FindObjectOfType<PlayerInteraction>();

            if (playerInteraction != null)
            {
                playerInteraction.LastObjectInteracted = this.gameObject;
                foreach (EventAbstractSO eve in eventOnInteraction)
                {
                    if (eve != null)
                    {
                        eve.Execute(this, playerInteraction);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Has no event to execute");
        }
    }
}
