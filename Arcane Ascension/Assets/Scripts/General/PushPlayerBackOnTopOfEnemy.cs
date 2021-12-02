using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Class responsible for growing a collider on top of the enemy to push the player back.
/// </summary>
public class PushPlayerBackOnTopOfEnemy : MonoBehaviour
{
    [SerializeField] private BoxCollider colliderToGrow;
    [SerializeField] private BoxCollider growUntil;

    private Enemy enemy;

    private YieldInstruction wffu;
    private IEnumerator pushPlayerBackCoroutine;

    private void Awake()
    {
        wffu = new WaitForFixedUpdate();
        colliderToGrow.size = new Vector3(0, colliderToGrow.size.y, 0);
        colliderToGrow.enabled = false;
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == Layers.PlayerLayerNum ||
            collision.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            enemy.TouchingPlayerWithBlockCollider = true;
        }
    }

    private void Update()
    {
        Debug.Log(enemy.TouchingPlayerWithBlockCollider);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == Layers.PlayerLayerNum ||
            collision.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            enemy.TouchingPlayerWithBlockCollider = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
        {
            colliderToGrow.size = new Vector3(0, colliderToGrow.size.y, 0);
            colliderToGrow.enabled = false;
            this.StartCoroutineWithReset(ref pushPlayerBackCoroutine, PushPlayerBackCoroutine());
        }
    }

    private IEnumerator PushPlayerBackCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        colliderToGrow.enabled = true;

        while (colliderToGrow.size.x < growUntil.size.x * 1.5f)
        {
            yield return wffu;
            colliderToGrow.size += new Vector3(12f, 0f, 12f) * Time.fixedDeltaTime;
        }
        colliderToGrow.size = new Vector3(0, colliderToGrow.size.y, 0);
        colliderToGrow.enabled = false;
    }
}
