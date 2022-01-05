using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class DISABLEVFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (VisualEffect effect in GetComponentsInChildren<VisualEffect>())
            if (effect != null)
                effect.Stop();

        foreach (ParticleSystem effect in GetComponentsInChildren<ParticleSystem>())
            if (effect != null)
                effect.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
