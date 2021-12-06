using UnityEngine;

/// <summary>
/// Class responsible for making ambience particles follow player
/// </summary>
public class AmbienceParticlesFollowPlayer : MonoBehaviour, IFindPlayer
{
    private Player playerTransform;

    private void Awake() =>
        FindPlayer();

    private void FixedUpdate()
    {
        if (playerTransform != null)
            transform.position = playerTransform.transform.position;
    }

    public void FindPlayer() =>
        playerTransform = FindObjectOfType<Player>();

    public void PlayerLost()
    {
        // Left blank on purpose
    }
}
