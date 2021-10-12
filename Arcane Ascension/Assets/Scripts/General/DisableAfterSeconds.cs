using System.Collections;
using UnityEngine;

/// <summary>
/// Disables this game object after x seconds.
/// </summary>
public class DisableAfterSeconds : MonoBehaviour
{
    [SerializeField] private float secondsToWait;
    private YieldInstruction wfs;

    private void Awake()
    {
        wfs = new WaitForSeconds(secondsToWait);
    }

    private void OnEnable()
    {
        StartCoroutine(DisableAfterSecs());
    }

    private IEnumerator DisableAfterSecs()
    {
        yield return wfs;
        gameObject.SetActive(false);
    }
}
