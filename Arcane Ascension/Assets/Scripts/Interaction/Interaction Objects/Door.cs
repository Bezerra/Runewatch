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

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        CanOpen = true;
        locked.SetActive(false);
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
            locked.SetActive(true);
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
