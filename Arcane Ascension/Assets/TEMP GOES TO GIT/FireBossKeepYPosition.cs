using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBossKeepYPosition : MonoBehaviour
{
    private Vector3 pos;

    private void Awake()
    {
        pos = new Vector3(0, -2, 0);
    }

    private void FixedUpdate()
    {
        transform.localPosition = pos;
    }
}
