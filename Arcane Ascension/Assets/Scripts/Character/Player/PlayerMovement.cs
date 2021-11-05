using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private PlayerInputCustom input;
    private Player player;
    private PlayerCastSpell playerCastSpell;
    private CharacterController characterController;
    private PlayerStats playerStats;

    // Movement
    private Vector3 direction;
    public float Speed { get; private set; }
    private bool castingContinuousSpell;

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

    // Character controller collider, don't change these values
    private readonly float CONTROLLERRADIUSDEFAULT = 0.75f;
    private readonly float CONTROLLERRADIUSONAIR = 0.5f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        player = GetComponent<Player>();
        playerCastSpell = GetComponent<PlayerCastSpell>();
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
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
        playerCastSpell.EventAttack += ReduceSpeedOnContinuousAttack;
        playerCastSpell.EventCancelAttack += NormalSpeedAfterContinuousAttack;
    }

    private void OnDisable()
    {
        input.Jump -= JumpPress;
        input.Dash -= Dash;
        input.Run += Run;
        playerCastSpell.EventAttack -= ReduceSpeedOnContinuousAttack;
        playerCastSpell.EventCancelAttack -= NormalSpeedAfterContinuousAttack;
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

        if (characterController.isGrounded)
        {
            if (characterController.radius != CONTROLLERRADIUSDEFAULT)
                characterController.radius =
                    Mathf.Lerp(characterController.radius, CONTROLLERRADIUSDEFAULT, Time.deltaTime * 50f);
        }
        else
        {
            characterController.radius = CONTROLLERRADIUSONAIR;
        }
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
            // If player is running it divides this value so dash is always the same
            lastDirectionPressed = Speed == 
                player.Values.Speed ? direction : direction.normalized * player.Values.Speed;
            canDash = false;
            OnEventDash();

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
        characterController.Move(dashCurrentValue * Time.fixedDeltaTime * finalDirection);

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
        if (castingContinuousSpell == false)
        {
            if (condition) Speed = player.Values.RunningSpeed * playerStats.CommonAttributes.MovementSpeedMultiplier;
            else Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;
            OnEventRun(condition);
        }
    }

    /// <summary>
    /// Updates speed variable.
    /// </summary>
    public void UpdateSpeed() =>
        Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;

    private void ReduceSpeedOnContinuousAttack(SpellCastType spellCastType)
    {
        if (spellCastType == SpellCastType.ContinuousCast)
        {
            castingContinuousSpell = true;
            Speed = playerStats.CommonAttributes.MovementSpeedMultiplier * player.Values.Speed * 0.5f;
        }
    }

    private void NormalSpeedAfterContinuousAttack()
    {
        castingContinuousSpell = false;
        Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;
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

            // Starts incrementing gravity until it reaches its peak
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

    protected virtual void OnEventDash() => EventDash?.Invoke();
    public event Action EventDash;

    protected virtual void OnEventRun(bool condition) => EventRun?.Invoke(condition);
    public event Action<bool> EventRun;
}
