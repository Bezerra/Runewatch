using UnityEngine;

public class Door : ObjectEventOnInteraction, IPassageBlock, IInteractableWithAnimation
{
    // Components
    private Animator anim;
    private Vector3 checkForPlayerSize;

    private float lastTimeOpened;

    public bool ExecuteAnimation { get; set; }

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        checkForPlayerSize = new Vector3(2, 5, 2);
    }

    private void Update()
    {
        anim.SetBool("Execute", ExecuteAnimation);

        if (ExecuteAnimation)
        {
            if (Time.time - lastTimeOpened > 2f)
            {
                lastTimeOpened = Time.time;
                Close();
            }
        }
    }

    public void Open()
    {
        ExecuteAnimation = true;
        lastTimeOpened = Time.time;
    }

    public void Close()
    {
        Collider[] playerHit = Physics.OverlapBox(
            transform.position, checkForPlayerSize, 
            Quaternion.identity, Layers.PlayerNormalAndInvisibleLayer);
        Debug.Log(playerHit.Length);
        if (playerHit.Length == 0)
        {
            ExecuteAnimation = false;
        }
    }
}
