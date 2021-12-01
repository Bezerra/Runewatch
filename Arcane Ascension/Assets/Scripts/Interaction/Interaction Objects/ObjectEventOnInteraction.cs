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
            playerInteraction.LastObjectInteracted = this.gameObject;
            foreach (EventAbstractSO eve in eventOnInteraction)
            {
                if (eve != null)
                {
                    eve.Execute(this);
                }
            }
        }
        else
        {
            Debug.Log("Has no event to execute");
        }
    }
}
