using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for adding a force on spawned items.
/// </summary>
public class LootAddForceOnSpawn : MonoBehaviour
{
    private bool freezePosition;
    private Vector3 positionOnTriggerEnter;
    private bool canDetectCollision;
    private float directionOfSpawn;

    // Components
    private Rigidbody rb;

    // Gravity
    private float gravityIncrement;
    private YieldInstruction wffu;
    private YieldInstruction forceTime;
    private float forceValue;
    private IEnumerator addForceCoroutine;

    private void Awake()
    {
        wffu = new WaitForFixedUpdate();
        forceTime = new WaitForSeconds(1);
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        freezePosition = false;
        canDetectCollision = false;
        forceValue = Random.Range(3, 6f);
        directionOfSpawn = Random.Range(-1f, 1f);

        this.StartCoroutineWithReset(ref addForceCoroutine, AddForceCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.WallsNum)
        {
            directionOfSpawn = 0;
        }

        // Stops object in place
        if (rb.velocity.y < 0 && canDetectCollision)
        {
            if (other.gameObject.layer == Layers.FloorNum)
            {
                rb.isKinematic = true;
                freezePosition = true;
                positionOnTriggerEnter = transform.position;
                if (addForceCoroutine != null) StopCoroutine(addForceCoroutine);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 10 * Time.fixedDeltaTime, 0, Space.Self);

        if (rb.velocity.y < 0)
            canDetectCollision = true;

        if (freezePosition)
        {
            transform.position = positionOnTriggerEnter;
            return;
        }

        // Moves object upwards
        if (addForceCoroutine != null)
        {
            transform.position += 
                new Vector3(directionOfSpawn, forceValue, directionOfSpawn) * Time.fixedDeltaTime;
        }
    }

    private IEnumerator AddForceCoroutine()
    {
        // Resets gravity increment
        gravityIncrement = 0.01f;

        // Waits until force time passes
        yield return forceTime;

        // Starts incrementing gravity every fixed update
        while (true)
        {
            yield return wffu;

            // Starts incrementing gravity until it reaches its peak
            if (gravityIncrement >= 0.2f / Time.fixedDeltaTime) gravityIncrement = 0.2f / Time.fixedDeltaTime;
            else gravityIncrement += 0.1f;

            if (freezePosition)
                break;
        }        
    }
}
