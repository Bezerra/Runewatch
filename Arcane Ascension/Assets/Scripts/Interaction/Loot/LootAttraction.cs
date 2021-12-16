using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for moving a pickable item towards the player.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class LootAttraction : MonoBehaviour
{
    private Rigidbody rb;
    private LootAddForceOnSpawn addForceOnSpawn;
    private Player player;

    private bool canMoveToPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        addForceOnSpawn = GetComponent<LootAddForceOnSpawn>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        canMoveToPlayer = false;
        if (player == null) player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (addForceOnSpawn.FreezePosition)
        {
            if (other.gameObject.layer == Layers.PlayerLayerNum ||
                        other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
            {
                rb.isKinematic = true;
                addForceOnSpawn.FreezePosition = false;
                addForceOnSpawn.MovingTowardsPlayer = true;
                canMoveToPlayer = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null && canMoveToPlayer)
        {
            transform.position += transform.position.Direction(player.transform.position) * 
                Time.fixedDeltaTime * 15f;
        }
    }
}
