using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private Player player;
    private CharacterController characterController;

    // Movement
    private Vector3 direction;
    public float Speed { get; private set; }

    // Dashing
    private float dashCurrentValue;
    private Vector3 lastDirectionPressed;
    private float dashingTimer;
    private bool canDash;
    private bool dashing;

    // Jump
    private YieldInstruction wffu;
    private YieldInstruction jumpTime;

    private IEnumerator jumpingCoroutine;
    private IEnumerator fallingCoroutine;

    // Gravity
    private float gravityIncrement;
    private readonly float DEFAULTGRAVITYINCREMENT = 1;    
    private readonly float GRAVITY = 100f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        wffu = new WaitForFixedUpdate();
        jumpTime = new WaitForSeconds(player.Values.JumpTime);
        Speed = player.Values.Speed;
        dashing = false;
        gravityIncrement = DEFAULTGRAVITYINCREMENT;
        dashCurrentValue = player.Values.DashDefaultValue;
    }

    private void OnEnable()
    {
        input.Jump += JumpPress;
        input.Dash += Dash;
        input.Run += Run;
    }

    private void OnDisable()
    {
        input.Jump -= JumpPress;
        input.Dash -= Dash;
        input.Run += Run;
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
        Vector3 sideMovement = input.Movement.x * Speed * transform.right;
        Vector3 forwardMovement = input.Movement.y * Speed * transform.forward;
        direction = sideMovement + forwardMovement;
        if (input.Movement == Vector2.zero) direction = Vector3.zero;
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

        if (dashing)
        {
            Dashing();

            // Cancels everything below this code
            return;
        }

        if (characterController.isGrounded)
            canDash = true;

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
    /// If player presses dash it will check if dash is possible.
    /// Player can't be dashing, must have touched the floor once, and must be pressing a direction.
    /// </summary>
    private void Dash()
    {
        if (dashing == false && (direction.x != 0 || direction.z != 0) && canDash)
        {
            dashing = true;
            dashingTimer = Time.time;
            lastDirectionPressed = direction;
            canDash = false;

            // Cancels jump and gravity
            if (jumpingCoroutine != null)
            {
                StopCoroutine(jumpingCoroutine);
                gravityIncrement = DEFAULTGRAVITYINCREMENT;
                jumpingCoroutine = null;
            }
        }  
    }

    /// <summary>
    /// Player dashes.
    /// </summary>
    private void Dashing()
    {
        canDash = false;

        // Decrements dash force smoothly
        if (dashCurrentValue - Time.fixedDeltaTime * player.Values.DashingTimeReducer > 1)
            dashCurrentValue -= Time.fixedDeltaTime * player.Values.DashingTimeReducer;
        else
            dashCurrentValue = 1;

        // Calculates dash direction
        Vector3 sideMovement = lastDirectionPressed.x * Vector3.right;
        Vector3 forwardMovement = lastDirectionPressed.z * Vector3.forward;
        Vector3 finalDirection = sideMovement + forwardMovement;
        
        // Dashes
        characterController.Move(finalDirection * dashCurrentValue * Time.fixedDeltaTime);

        // Cancels dash and resets dash value
        if (Time.time - dashingTimer > player.Values.DashingTime)
        {
            dashCurrentValue = player.Values.DashDefaultValue;
            dashing = false;
            return;
        }
    }

    /// <summary>
    /// Updates player's velocity.
    /// </summary>
    private void Run(bool condition)
    {
        if (condition) Speed = player.Values.RunningSpeed;
        else Speed = player.Values.Speed;
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
