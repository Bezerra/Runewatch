using UnityEngine;

/// <summary>
/// Class responsible for executing chest's animation events.
/// </summary>
public class ChestAnimationEvents : MonoBehaviour
{
    private Chest parentChest;

    private void Awake()
    {
        parentChest = GetComponentInParent<Chest>();
    }

    public void ChestOpeningStartAnimationEvent()
    {
        parentChest.ChestOpeningStartAnimationEvent();
    }

    public void ChestOpenedEndAnimationEvent()
    {
        parentChest.ChestOpenedEndAnimationEvent();
    }
}
