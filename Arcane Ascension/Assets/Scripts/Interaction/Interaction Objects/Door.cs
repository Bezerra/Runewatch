using UnityEngine;

public class Door : ObjectEventOnInteraction, IPassageBlock, IInteractableWithAnimation
{
    // Components
    private Animator anim;

    public bool ExecuteAnimation { get; set; }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Execute", ExecuteAnimation);
    }

    public void Open()
    {
        ExecuteAnimation = true;
    }

    public void Close()
    {
        ExecuteAnimation = false;
    }
}
