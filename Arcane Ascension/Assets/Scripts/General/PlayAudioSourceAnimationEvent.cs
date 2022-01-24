using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSourceAnimationEvent : MonoBehaviour
{
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
