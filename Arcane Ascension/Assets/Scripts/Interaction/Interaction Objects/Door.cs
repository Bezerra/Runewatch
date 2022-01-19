using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for containing behaviours of a door.
/// </summary>
public class Door : MonoBehaviour, IPassageBlock
{
    [SerializeField] private GameObject hasOpenned1;
    [SerializeField] private GameObject hasOpenned2;
    [SerializeField] private GameObject locked;

    // Components
    private Animator anim;

    /// <summary>
    /// Property with bool to open door animation.
    /// </summary>
    public bool ExecuteAnimation { get; set; }

    /// <summary>
    /// Can this door open.
    /// </summary>
    public bool CanOpen { get; set; }

    /// <summary>
    /// Property to know if the room behind this door is fully loaded.
    /// </summary>
    public bool IsDoorRoomFullyLoaded { get; set; }

    private bool doorAnimationIsOver;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        CanOpen = true;
        locked.SetActive(false);
        doorAnimationIsOver = true;
    }

    private void Update() =>
        anim.SetBool("Execute", ExecuteAnimation);

    /// <summary>
    /// Opens door if possible.
    /// </summary>
    public void Open()
    {
        if (CanOpen && IsDoorRoomFullyLoaded)
        {
            ExecuteAnimation = true;
            hasOpenned1.SetActive(false);
            hasOpenned2.SetActive(false);
        }
    }

    public void AnimationRunningAnimationEvent() => doorAnimationIsOver = false;
    public void AnimationOverAnimationEvent() => doorAnimationIsOver = true;


    /// <summary>
    /// Closes door.
    /// </summary>
    public void Close()
    {
        ExecuteAnimation = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
            Open();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
            Close();
    }

    /// <summary>
    /// Enables block symbol.
    /// </summary>
    public void EnableBlockSymbol()
    {
        if (locked != null)
            StartCoroutine(EnableBlockSymbolCoroutine());
    }

    private IEnumerator EnableBlockSymbolCoroutine()
    {
        while (locked.activeSelf == false)
        {
            if (doorAnimationIsOver)
                locked.SetActive(true);

            yield return null;
        }
    }

    /// <summary>
    /// Disables block symbol.
    /// </summary>
    public void DisableBlockSymbol()
    {
        if (locked != null)
            locked.SetActive(false);
    }
}
