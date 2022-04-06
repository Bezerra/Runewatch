using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEnemy : MonoBehaviour
{
    [SerializeField] private GameObject brokenEnemy;

    public void Break()
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        brokenEnemy.SetActive(true);
    }
}
