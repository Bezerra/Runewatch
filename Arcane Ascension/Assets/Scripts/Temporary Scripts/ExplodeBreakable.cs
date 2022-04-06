using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBreakable : MonoBehaviour
{
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] float radius;

    private void OnEnable()
    {
        
        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }
        }
    }
}
