using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Class used by gameobjects that can be interected with and trigger a scriptable object event.
/// </summary>
public class EventOnInteraction : MonoBehaviour, IInterectable
{
    [SerializeField] private List<EventAbstractSO> eventOnInteraction;

    /// <summary>
    /// Executes an event.
    /// </summary>
    public void Execute()
    {
        if (eventOnInteraction.Count > 0)
        {
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

    /// <summary>
    /// Executes destroy coroutine.
    /// </summary>
    public void DestroyInvoker() =>
        StartCoroutine(DestroyInvokerCoroutine());

    /// <summary>
    /// Destroys gameobject.
    /// </summary>
    /// <returns>WFFU.</returns>
    private IEnumerator DestroyInvokerCoroutine()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
