using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for giving camera an impulse on dash.
/// </summary>
public class PlayerFinalCameraDashEvent : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private YieldInstruction wffu;
    private Vector3 defaultCameraPosition;
    private float dashHalfTime;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        wffu = new WaitForFixedUpdate();
        defaultCameraPosition = transform.localPosition;
        dashHalfTime = GetComponentInParent<Player>().Values.DashingTime * 0.5f;
    }

    private void OnEnable()
    {
        playerMovement.EventDash += Dash;
    }

    private void OnDisable()
    {
        playerMovement.EventDash -= Dash;
    }

    private void Dash() => 
        StartCoroutine(DashCoroutine());

    /// <summary>
    /// Moves camera up while dashing starts, then moves camera back to original position.
    /// </summary>
    /// <returns>Wffu.</returns>
    private IEnumerator DashCoroutine()
    {
        float currentTime = 0;
        while (currentTime < dashHalfTime)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition + Vector3.up, Time.fixedDeltaTime * 2.5f);
            currentTime += Time.fixedDeltaTime;
            yield return wffu;
        }
        while(transform.localPosition.y > defaultCameraPosition.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition, Time.fixedDeltaTime * 1.5f);
            yield return wffu;
        }
    }
}
