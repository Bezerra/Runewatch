using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using ExtensionMethods;

public class RoomOcclusion : MonoBehaviour
{
    // Components
    private MeshRenderer[] childMeshRenderers;
    private ParticleSystem[] childParticleSystems;

    // Coroutines
    private YieldInstruction wffu;
    private IEnumerator coroutine;

    private void Awake()
    {
        childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        childParticleSystems = GetComponentsInChildren<ParticleSystem>();
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
        float time = Time.time;
        while(true)
        {
            for (int i = 0; i < childMeshRenderers.Length; i++)
            {
                childMeshRenderers[i].enabled = true;
                yield return wffu;
            }
            
            for (int i = 0; i < childParticleSystems.Length; i++)
            {
                childParticleSystems[i].Play();
                yield return wffu;
            }

            break;
        }
        Debug.Log(Time.time - time);
    }

    private IEnumerator DisableRenderersCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < childMeshRenderers.Length; i++)
            {
                childMeshRenderers[i].enabled = false;
                yield return wffu;
            }

            for (int i = 0; i < childParticleSystems.Length; i++)
            {
                childParticleSystems[i].Clear(true);
                childParticleSystems[i].Stop();
                yield return wffu;
            }

            break;
        }
    }
}
