using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for moving a pickable item towards the player.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class LootAttraction : MonoBehaviour
{
    private Rigidbody rb;
    private Player player;

    private bool canMoveToPlayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        canMoveToPlayer = false;
        if (player == null) player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            canMoveToPlayer = true;
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
