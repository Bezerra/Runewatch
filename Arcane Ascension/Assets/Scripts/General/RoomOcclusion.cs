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
    private IEnumerator coroutine;

    private void Awake()
    {
        childMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        childParticleSystems = GetComponentsInChildren<ParticleSystem>();

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
        while(true)
        {
            for (int i = 0; i < childMeshRenderers.Length; i++)
            {
                childMeshRenderers[i].enabled = true;
                yield return null;
            }
            
            for (int i = 0; i < childParticleSystems.Length; i++)
            {
                childParticleSystems[i].Play();
                yield return null;
            }

            break;
        }
    }

    private IEnumerator DisableRenderersCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < childMeshRenderers.Length; i++)
            {
                childMeshRenderers[i].enabled = false;
                yield return null;
            }

            for (int i = 0; i < childParticleSystems.Length; i++)
            {
                childParticleSystems[i].Clear(true);
                childParticleSystems[i].Stop();
                yield return null;
            }

            break;
        }
    }
}
