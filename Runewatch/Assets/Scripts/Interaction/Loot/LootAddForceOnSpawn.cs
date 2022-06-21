using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Class responsible for adding a force on spawned items.
/// </summary>
public class LootAddForceOnSpawn : MonoBehaviour
{
    public bool FreezePosition { get; set; }
    private float directionOfSpawn;

    // Components
    private Rigidbody rb;
    private SphereCollider sphereCollider;

    [Range(0.1f, 2f)][SerializeField] private float colliderCollisionSize = 0.5f;

    [SerializeField] private bool isAttractable;
    [EnableIf("isAttractable", true)]
    [Range(2f, 30f)] [SerializeField] private float colliderAttractionSize = 3f;

    [SerializeField] private bool dontRotateObjectOnIdle;

    // Gravity
    private float forceValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;

        // Force should never be less than this number (so it won't cause bugs
        // on trigger enter)
        forceValue = Random.Range(1050f, 1150f);
        directionOfSpawn = Random.Range(-250, 250f);
        sphereCollider.radius = 0;

        rb.AddForce(new Vector3(directionOfSpawn, forceValue, directionOfSpawn));
        StartCoroutine(GrowColliderAfterTime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.FloorNum)
        {
            if (rb.velocity.y < 0.1f)
            {
                rb.isKinematic = true;

                if (isAttractable)
                    sphereCollider.radius = colliderAttractionSize;
            }
        }
    }

    private IEnumerator GrowColliderAfterTime()
    {
        while (rb.velocity.y < 0.1f) yield return null;
        sphereCollider.radius = colliderCollisionSize;
    }

    private void FixedUpdate()
    {
        if (dontRotateObjectOnIdle) return;

        transform.Rotate(0, 10 * Time.fixedDeltaTime, 0, Space.Self);
    }
}
