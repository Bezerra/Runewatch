using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for giving camera an impulse on dash.
/// </summary>
public class PlayerFinalCameraDashEvent : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Player player;
    private YieldInstruction wffu;
    private Vector3 defaultCameraPosition;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        wffu = new WaitForFixedUpdate();
        defaultCameraPosition = transform.localPosition;
        player = GetComponentInParent<Player>();
    }

    private void OnEnable()
    {
        playerMovement.EventCameraTiltDash += Dash;
    }

    private void OnDisable()
    {
        playerMovement.EventCameraTiltDash -= Dash;
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
        while (currentTime < player.Values.DashingTime * 0.5f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition + Vector3.up, Time.fixedDeltaTime * player.Values.CameraForceOnDash * 0.5f);
            currentTime += Time.fixedDeltaTime;
            yield return wffu;
        }
        while(transform.localPosition.y > defaultCameraPosition.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition, Time.fixedDeltaTime * player.Values.CameraForceOnDash * 0.05f);
            yield return wffu;
        }
    }
}
