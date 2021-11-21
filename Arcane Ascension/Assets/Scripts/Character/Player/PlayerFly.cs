using UnityEngine;

/// <summary>
/// Class responsible for applying Fly behaviour through a cheat code.
/// </summary>
public class PlayerFly : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private Player player;
    private PlayerStats playerStats;

    // Movement variables
    private Vector3 directionPressed;
    private float speed;
    public float Speed { get => speed; private set => speed = value; }
    public bool Running { get; private set; }

    public bool CheatApplied { get; set; }

    private void Awake()
    {
        player = GetComponent<Player>();
        playerStats = GetComponent<PlayerStats>();
        input = FindObjectOfType<PlayerInputCustom>();
        Speed = player.Values.Speed;
    }

    private void OnEnable()
    {
        input.Run += Run;
    }

    private void OnDisable()
    {
        input.Run -= Run;
    }

    /// <summary>
    /// Updates player's velocity if run is pressed or released.
    /// </summary>
    private void Run(bool condition)
    {
        if (CheatApplied)
        {
            if (condition)
            {
                Running = true;
                Speed = player.Values.RunningSpeed * playerStats.CommonAttributes.MovementSpeedMultiplier;
            }
            else
            {
                Running = false;
                Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;
            }
        }
    }

    private void Update()
    {
        if (CheatApplied)
        {
            // Movement Directions
            Vector3 sideMovement = input.Movement.x * Speed * transform.right;
            Vector3 forwardMovement = input.Movement.y * Speed * player.Eyes.transform.forward;
            directionPressed = sideMovement + forwardMovement;
            if (input.Movement == Vector2.zero) directionPressed = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (CheatApplied)
        {
            transform.position += directionPressed * speed * Time.fixedDeltaTime;
        }
    }
}
