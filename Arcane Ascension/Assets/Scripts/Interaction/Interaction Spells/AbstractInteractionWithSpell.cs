using UnityEngine;

/// <summary>
/// Abstract class for objects that only interact with spells.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public abstract class AbstractInteractionWithSpell : MonoBehaviour, IInteractionWithSpell
{
    [Space(10)]
    [Header("True if you want the spell to interact with any element hit")]
    [SerializeField] private bool ignoreElement;

    [Space(10)]
    [SerializeField] private ElementType elementToInteractWith;

    protected Animator anim;

    private void Awake() =>
        anim = GetComponent<Animator>();

    /// <summary>
    /// If this object matches the desired spell element, it will trigger an action.
    /// </summary>
    /// <param name="element">Element to check against</param>
    public void ExecuteInteraction(ElementType element)
    {
        if (ignoreElement)
        {
            ActionToTake();
            return;
        }

        if (elementToInteractWith == element)
        {
            ActionToTake();
        }
    }

    /// <summary>
    /// What
    /// </summary>
    protected abstract void ActionToTake();
}
