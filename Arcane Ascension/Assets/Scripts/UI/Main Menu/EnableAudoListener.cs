using UnityEngine;

/// <summary>
/// Class responsible for enabling main menu audiolistener and disable any other
/// audio listeners that might by active.
/// </summary>
public class EnableAudoListener : MonoBehaviour
{
    [SerializeField] private AudioListener myAudioListener;

    private void Awake()
    {
        myAudioListener.enabled = false;
    }

    private void Update()
    {
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>(true);

        if (audioListeners.Length == 1)
        {
            myAudioListener.enabled = true;
            this.enabled = false;
        }

        foreach (AudioListener al in audioListeners)
        {
            if (al != myAudioListener)
            {
                al.enabled = false;
                myAudioListener.enabled = true;
                this.enabled = false;
                break;
            }
        }
    }
}
