using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosAreaSpell : MonoBehaviour
{
    public float SpellRange { get; set; }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 0.5f, 0.5f);
        Gizmos.DrawSphere(transform.position, SpellRange);
    }
}
