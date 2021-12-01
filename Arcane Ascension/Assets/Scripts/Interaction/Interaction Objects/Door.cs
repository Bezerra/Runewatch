using UnityEngine;

/// <summary>
/// Class responsible for containing behaviours of a door.
/// </summary>
public class Door : MonoBehaviour, IPassageBlock
{
    // Components
    private Animator anim;
    //private ParticleSystem lockParticles;

    /// <summary>
    /// Property with bool to open door animation.
    /// </summary>
    public bool ExecuteAnimation { get; set; }

    /// <summary>
    /// Can this door open.
    /// </summary>
    public bool CanOpen { get; set; }

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
        if (CanOpen)
            ExecuteAnimation = true;
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
        //lockParticles.Play();
    }

    /// <summary>
    /// Unblocks this passage.
    /// </summary>
    public void UnblockPassage()
    {
        CanOpen = true;
        //lockParticles.Stop();
    }
}
