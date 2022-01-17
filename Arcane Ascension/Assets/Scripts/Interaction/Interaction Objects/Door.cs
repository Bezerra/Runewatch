using UnityEngine;

/// <summary>
/// Class responsible for containing behaviours of a door.
/// </summary>
public class Door : MonoBehaviour, IPassageBlock
{
    [SerializeField] private GameObject hasOpenned1, hasOpenned2;
    [SerializeField] private GameObject locked;

    // Components
    private Animator anim;
    //[SerializeField] private ParticleSystem lockParticles;

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
        //lockParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start() =>
        CanOpen = true;

    private void Update() =>
        anim.SetBool("Execute", ExecuteAnimation);

    /// <summary>
    /// Opens door if possible.
    /// </summary>
    public void Open()
    {
        if (CanOpen && IsDoorRoomFullyLoaded)
            ExecuteAnimation = true;
        hasOpenned1.SetActive(false);
        hasOpenned2.SetActive(false);
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
    /// Blocks this passage.
    /// </summary>
    public void BlockPassage()
    {
        CanOpen = false;
        locked.SetActive(true);
        //lockParticles.Play();
    }

    /// <summary>
    /// Unblocks this passage.
    /// </summary>
    public void UnblockPassage()
    {
        CanOpen = true;
        locked.SetActive(false);
        print("Unlock Doors");

        //lockParticles.Stop();
    }
}
