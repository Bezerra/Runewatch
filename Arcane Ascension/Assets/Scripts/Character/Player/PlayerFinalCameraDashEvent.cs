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

        while (transform.localPosition.y < defaultCameraPosition.y + 1)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition + Vector3.up, Time.fixedDeltaTime * player.Values.CameraForceOnDash);

            yield return wffu;
        }

        while(transform.localPosition.y > defaultCameraPosition.y)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, defaultCameraPosition, Time.fixedDeltaTime * player.Values.CameraForceOnDash * 2f);
            yield return wffu;
        }
    }
}
