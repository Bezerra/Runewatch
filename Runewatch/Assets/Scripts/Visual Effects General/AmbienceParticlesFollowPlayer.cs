using UnityEngine;

/// <summary>
/// Class responsible for making ambience particles follow player
/// </summary>
public class AmbienceParticlesFollowPlayer : MonoBehaviour, IFindPlayer
{
    private Player playerTransform;

    private void Awake() =>
        FindPlayer(FindObjectOfType<Player>());

    private void FixedUpdate()
    {
        if (playerTransform != null)
            transform.position = playerTransform.transform.position;
    }

    public void FindPlayer(Player player)
    {
        if (player == null) return;
        playerTransform = player;
    }

    public void PlayerLost(Player player)
    {
        // Left blank on purpose
    }
}
