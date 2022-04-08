using UnityEngine;

public class ExplodeBreakable : MonoBehaviour
{
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float radius;
    [SerializeField] private float timeToDestroy = 5;

    private void OnEnable()
    {
        ExplodePieces();
        //Destroy(gameObject, timeToDestroy);
    }

    private void ExplodePieces()
    {
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float force = Random.Range(minForce, maxForce);
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}
