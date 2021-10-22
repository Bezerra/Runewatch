using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPAHH : MonoBehaviour
{
    [SerializeField] private LayerMask layerDaMascara;

    private void Start()
    {
        TEMP();
    }

    private void TEMP()
    {
        Collider[] maskCollider = Physics.OverlapSphere(transform.position, 100, layerDaMascara);

        Ray direction = new Ray(transform.position, (maskCollider[0].ClosestPoint(transform.position) - transform.position).normalized);

        RaycastHit hit;

        if (Physics.Raycast(direction, out hit, 100))
        {
            transform.rotation =
                Quaternion.LookRotation(hit.normal, hit.transform.up);

            transform.position =
                hit.point;
        }

        transform.parent = maskCollider[0].gameObject.transform;
    }
}
