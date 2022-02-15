using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shake;
    [SerializeField] private ParticleSystem opening;
    [SerializeField] private ParticleSystem persistent;

    public void PlayShake()
    {
        shake.Play();
    }
}
