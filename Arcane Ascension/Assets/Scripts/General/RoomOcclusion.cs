using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using ExtensionMethods;
using System.Linq;

public class RoomOcclusion : MonoBehaviour
{
    // Components
    private Transform[] childOccludees;

    // Coroutines
    private YieldInstruction wffu;
    private IEnumerator coroutine;

    private void Awake()
    {
        childOccludees = GetComponentsInChildren<Transform>(true).Where(i => i.CompareTag("ChildOccludee")).ToArray();
        wffu = new WaitForFixedUpdate();

        StartCoroutine(DisableRenderersCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            this.StartCoroutineWithReset(ref coroutine, EnableRenderersCoroutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            this.StartCoroutineWithReset(ref coroutine, DisableRenderersCoroutine());
        }
    }

    private IEnumerator EnableRenderersCoroutine()
    {
        for (int i = 0; i < childOccludees.Length; i++)
        {
            childOccludees[i].gameObject.SetActive(true);
            yield return wffu;
        }
    }

    private IEnumerator DisableRenderersCoroutine()
    {
        for (int i = 0; i < childOccludees.Length; i++)
        {
            childOccludees[i].gameObject.SetActive(false);
            yield return wffu;
        }
    }
}
