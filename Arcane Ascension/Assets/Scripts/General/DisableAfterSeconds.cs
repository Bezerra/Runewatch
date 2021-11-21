using System.Collections;
using UnityEngine;
using ExtensionMethods;

/// <summary>
/// Disables this game object after x seconds.
/// </summary>
public class DisableAfterSeconds : MonoBehaviour
{
    [SerializeField] private float secondsToWait;
    private YieldInstruction wfs;

    private IEnumerator disableAfterSecs;

    private void Awake()
    {
        wfs = new WaitForSeconds(secondsToWait);
    }

    private void OnEnable()
    {
        this.StartCoroutineWithReset(ref disableAfterSecs, DisableAfterSecs());
    }

    private IEnumerator DisableAfterSecs()
    {
        yield return wfs;
        gameObject.SetActive(false);
    }
}
