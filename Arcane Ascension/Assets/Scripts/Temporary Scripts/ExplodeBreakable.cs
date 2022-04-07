using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ExplodeBreakable : MonoBehaviour
{
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] float radius;

    [SerializeField] bool insertVFX = false;
    [EnableIf("insertVFX", true)]
    [SerializeField] ParticleSystem VFXtoAdd;

    private void OnEnable()
    {
        ExplodePieces();

        if (insertVFX && VFXtoAdd != null)
            InsertVFX();
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

    private void InsertVFX()
    {
        foreach (Transform t in transform)
        {
            var mr = t.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                ParticleSystem ps = Instantiate<ParticleSystem>(VFXtoAdd, t);
                ps.gameObject.name = "Death VFX";
                var newShape = ps.shape;
                newShape.meshRenderer = mr;
            }
        }
    }


}
