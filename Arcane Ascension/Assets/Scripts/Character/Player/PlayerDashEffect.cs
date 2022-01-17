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

    private void OnEnable() =>
        playerMovement.EventDash += DashEffect;

    private void OnDisable() =>
        PlayerLost();

    private void Start()
    {
        particles.Stop();
    }

    public void FindPlayer()
    {
        if (playerMovement != null)
        {
            PlayerLost();
            playerMovement = null;
        }

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            playerMovement.EventDash += DashEffect;
        }
    }

    public void PlayerLost()
    {
        if (playerMovement != null)
            playerMovement.EventDash -= DashEffect;
    }

    private void DashEffect()
    {
        particles.Play();
    }
}
