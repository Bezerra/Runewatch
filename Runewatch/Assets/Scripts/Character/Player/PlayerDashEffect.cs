using UnityEngine;

/// <summary>
/// Class responsible for enabling and disabling dash effect.
/// </summary>
public class PlayerDashEffect : MonoBehaviour, IFindPlayer
{
    private PlayerMovement playerMovement;
    private ParticleSystem particles;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (playerMovement != null)
            playerMovement.EventDash += DashEffect;
    }

    private void OnDisable() =>
        PlayerLost();

    private void Start()
    {
        particles.Stop();
    }

    public void FindPlayer(Player player)
    {
        if (playerMovement != null)
        {
            PlayerLost(player);
            playerMovement = null;
        }

        if (playerMovement == null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.EventDash += DashEffect;
        }
    }

    public void PlayerLost(Player player = null)
    {
        if (playerMovement != null)
            playerMovement.EventDash -= DashEffect;
    }

    private void DashEffect()
    {
        particles.Play();
    }
}
