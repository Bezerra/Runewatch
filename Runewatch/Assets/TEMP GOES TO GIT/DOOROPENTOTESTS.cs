using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOOROPENTOTESTS : MonoBehaviour
{
    // Components
    private Animator anim;
    //private ParticleSystem lockParticles;

    /// <summary>
    /// Property with bool to open door animation.
    /// </summary>
    public bool ExecuteAnimation { get; set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        //lockParticles = GetComponentInChildren<ParticleSystem>();
    }


    private void Update() =>
        anim.SetBool("Execute", ExecuteAnimation);

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
            ExecuteAnimation = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.PlayerLayerNum ||
            other.gameObject.layer == Layers.InvisiblePlayerLayerNum)
            ExecuteAnimation = false;
    }
}
