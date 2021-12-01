using UnityEngine;

public class Door : MonoBehaviour, IInterectableWithSound, IPassageBlock
{
    // Components
    private Animator anim;

    public bool OpenDoor { get; set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("OpenDoor", OpenDoor);
    }

    public void Open()
    {
        OpenDoor = true;
    }

    public void Close()
    {
        OpenDoor = false;
    }

    public LootAndInteractionSoundType LootAndInteractionSoundType => 
        throw new System.NotImplementedException();
}
