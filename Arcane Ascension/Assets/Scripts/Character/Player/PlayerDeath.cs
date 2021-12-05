using UnityEngine;

/// <summary>
/// Class responsible for what happens when player dies.
/// </summary>
public class PlayerDeath : MonoBehaviour, IFindInput
{
    [SerializeField] private GameObject[] gameobjectsToDisableInitially;

    // Components to disable on death
    private PlayerGenerateCinemachineImpulse playerGenerateCMImpulse;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerSpells playerSpells;
    private PlayerCastSpell playerCastSpell;
    private PlayerControlEnemyHealthBar playerEnemyHealthBar;
    private PlayerInteraction playerInteraction;
    private PlayerStats playerStats;

    // Components
    private Animator deathAnimator;
    private PlayerFindMe playerFindMe;
    private IInput input;

    private void Awake()
    {
        playerGenerateCMImpulse = GetComponentInParent<PlayerGenerateCinemachineImpulse>();
        characterController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        playerCamera = GetComponentInParent<PlayerCamera>();
        playerSpells = GetComponentInParent<PlayerSpells>();
        playerCastSpell = GetComponentInParent<PlayerCastSpell>();
        playerEnemyHealthBar = GetComponentInParent<PlayerControlEnemyHealthBar>();
        playerInteraction = GetComponentInParent<PlayerInteraction>();
        playerFindMe = GetComponentInParent<PlayerFindMe>();
        playerStats = GetComponentInParent<PlayerStats>();
        deathAnimator = GetComponent<Animator>();
        input = FindObjectOfType<PlayerInputCustom>();
    }

    private void OnEnable()
    {
        playerStats.EventDeath += OnDeath;
    }

    private void OnDisable()
    {
        playerStats.EventDeath += OnDeath;
    }

    private void OnDeath(Stats stats)
    {
        input.SwitchActionMapToNone();

        foreach (GameObject go in gameobjectsToDisableInitially)
            go.SetActive(false);

        deathAnimator.SetTrigger("Death");
    }

    /// <summary>
    /// This "destroys" the player, so all classes that have IFindPlayer implemented
    /// will trigger PlayerLost().
    /// </summary>
    public void EndOfDeathAnimationEvent()
    {
        playerGenerateCMImpulse.enabled = false;
        characterController.enabled = false;
        playerFindMe.enabled = false;
        playerMovement.enabled = false;
        playerCamera.enabled = false;
        playerSpells.enabled = false;
        playerCastSpell.enabled = false;
        playerEnemyHealthBar.enabled = false;
        playerInteraction.enabled = false;
        playerStats.enabled = false;
    }

    public void FindInput()
    {
        input = FindObjectOfType<PlayerInputCustom>();
    }

    public void LostInput()
    {
        // Left blank on purpose
    }
}
