using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private Player player;
    private CharacterController characterController;

    private Vector3 direction;

    private float speed;

    // Jump //////////////////////////////////////////
    private YieldInstruction wffu;
    private YieldInstruction jumpTime;

    private IEnumerator jumpingCoroutine;
    private IEnumerator fallingCoroutine;

    private float gravityIncrement;
    private readonly float DEFAULTGRAVITYINCREMENT = 1;
    /// ///////////////////////////////////////////////
    
    private readonly float GRAVITY = 100f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        wffu = new WaitForFixedUpdate();
        jumpTime = new WaitForSeconds(player.Values.JumpTime);
        speed = player.Values.Speed;
    }

    private void Start()
    {
        gravityIncrement = DEFAULTGRAVITYINCREMENT;
    }

    private void OnEnable()
    {
        input.Jump += JumpPress;
        input.Run += Run;
    }

    private void OnDisable()
    {
        input.Jump -= JumpPress;
        input.Run -= Run;
    }

    private void JumpPress()
    {
        if (jumpingCoroutine == null && characterController.isGrounded)
        {
            jumpingCoroutine = JumpingCoroutine();
            StartCoroutine(jumpingCoroutine);
        }
    }

    private void Update()
    {
        Vector3 sideMovement = input.Movement.x * speed * transform.right;
        Vector3 forwardMovement = input.Movement.y * speed * transform.forward;
        direction = sideMovement + forwardMovement;
    }

    private void FixedUpdate()
    {
        // Happens if player is falling (without jumping)
        if (jumpingCoroutine == null && characterController.isGrounded == false)
        {
            if (fallingCoroutine == null)
            {
                fallingCoroutine = FallingCoroutine();
                StartCoroutine(fallingCoroutine);
            }
        }

        // Happens if player pressed jump and JumpingCoroutine is running
        // Will keep pushing player upwards while the time is passing
        if (jumpingCoroutine != null)
        {
            direction.y = player.Values.JumpForce;
        }

        // Gravity
        direction.y -= GRAVITY * gravityIncrement * Time.fixedDeltaTime;

        // Movement
        characterController.Move(direction * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Increments player's speed.
    /// </summary>
    /// <param name="running">True if running, else it's false.</param>
    private void Run(bool running)
    {
        if (running) speed = player.Values.RunningSpeed;
        else speed = player.Values.Speed;
    }

    /// <summary>
    /// Jumps and increments gravity value.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator JumpingCoroutine()
    {
        // Resets gravity increment
        gravityIncrement = 0.01f;

        // Waits until jumping time passes
        yield return jumpTime;

        // Starts incrementing gravity every fixed update
        while (true)
        {
            yield return wffu;

            if (gravityIncrement >= 20) gravityIncrement = 20;
            else gravityIncrement += player.Values.GravityIncrement;

            // Breaks the coroutine if the character touches the floor
            if (characterController.isGrounded)
            {
                break;
            }
        }

        gravityIncrement = DEFAULTGRAVITYINCREMENT;
        jumpingCoroutine = null;
    }

    /// <summary>
    /// Increments gravity value while player is falling (WITHOUT JUMPING).
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator FallingCoroutine()
    {
        // Starts incrementing gravity every fixed update
        while (true)
        {
            yield return wffu;

            if (gravityIncrement >= 20) gravityIncrement = 20;
            else gravityIncrement += player.Values.GravityIncrement;

            // Breaks the coroutine if the character touches the floor
            if (characterController.isGrounded)
            {
                break;
            }
        }

        gravityIncrement = DEFAULTGRAVITYINCREMENT;
        fallingCoroutine = null;
    }
}
