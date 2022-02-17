using UnityEngine;

/// <summary>
/// Class responsible for executing chest's animation events.
/// </summary>
public class ChestAnimationEvents : MonoBehaviour, IReset
{
    private Chest parentChest;

    [Header("Objects to reset")]
    [SerializeField] private GameObject locked;
    [SerializeField] private GameObject lockBreak;
    [SerializeField] private GameObject shake;
    [SerializeField] private GameObject opening;
    [SerializeField] private GameObject persistent;

    private void Awake() =>
        parentChest = GetComponentInParent<Chest>();

    public void ChestOpeningStartAnimationEvent() =>
        parentChest.ChestOpeningStartAnimationEvent();

    public void ChestOpenedEndAnimationEvent() =>
        parentChest.ChestOpenedEndAnimationEvent();

    public void PlayShakeVFX() =>
        shake.SetActive(true);

    public void PlayOpeningVFX() =>
        opening.SetActive(true);

    public void PlayPersistentVFX() =>
        persistent.SetActive(true);

    public void PlayLockedVFX() =>
        locked.SetActive(true);

    public void PlayBreakLockVFX()
    {
        locked.SetActive(false);
        lockBreak.SetActive(true);
    }

    /// <summary>
    /// When pool disables the gameobjects on the end of a scene.
    /// </summary>
    public void ResetAfterPoolDisable()
    {
        locked.SetActive(true);
        lockBreak.SetActive(false);
        shake.SetActive(false);
        opening.SetActive(false);
        persistent.SetActive(false);
    }
}
