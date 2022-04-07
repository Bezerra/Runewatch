using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEnemy : MonoBehaviour
{
    [SerializeField] GameObject brokenEnemy;
    [SerializeField] float timeToDestroy = 5;

    public void Break()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        brokenEnemy.SetActive(true);
        brokenEnemy.transform.parent = null;

        Destroy(brokenEnemy, timeToDestroy);
    }
}
