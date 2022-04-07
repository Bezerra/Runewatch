using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExplodeBreakable : MonoBehaviour
{
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] float radius;


    private void OnEnable()
    {
        ExplodePieces();
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
