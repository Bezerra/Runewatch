using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Class used by gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class EventOnInteraction : MonoBehaviour, IInterectable
{
    [SerializeField] private List<EventAbstractSO> eventOnInteraction;

    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    /// <summary>
    /// Executes an event.
    /// </summary>
    public void Execute()
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
