using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class responsible for handing all player's movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    // Components
    private PlayerInputCustom   input;
    private Player              player;
    private PlayerCastSpell     playerCastSpell;
    private CharacterController characterController;
    private PlayerStats         playerStats;

    // Movement
    private bool    castingContinuousSpell;
    private Vector3 directionPressed;
    public float    Speed { get; private set; }
    public bool Running { get; private set; }
    private Vector3 positionOnLastCalculation;

    // Dash
    private float   dashCurrentValue;
    private Vector3 lastDirectionPressed;
    private float   dashingTimer;
    private bool    dashing;
    public float    CurrentTimeToGetCharge { get; private set; }

    // Jump
    private YieldInstruction wffu;
    private YieldInstruction jumpTime;

    private IEnumerator jumpingCoroutine;
    private IEnumerator fallingCoroutine;

    // Gravity
    private float           gravityIncrement;
    private readonly float  DEFAULTGRAVITYINCREMENT = 1;    
    private readonly float  GRAVITY = 100f;

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
        CurrentTimeToGetCharge = 0;
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

    private void Update()
    {
        // Movement Directions
        Vector3 sideMovement = input.Movement.x * Speed * transform.right;
        Vector3 forwardMovement = input.Movement.y * Speed * transform.forward;
        directionPressed = sideMovement + forwardMovement;
        if (input.Movement == Vector2.zero) directionPressed = Vector3.zero;

        // Controls character radius to prevent getting stuck on edges after jumping
        if (characterController.isGrounded)
        {
            // Character radius
            if (characterController.radius != CONTROLLERRADIUSDEFAULT)
                characterController.radius =
                    Mathf.Lerp(characterController.radius, CONTROLLERRADIUSDEFAULT, Time.deltaTime * 50f); 
        }
        else
        {
            characterController.radius = CONTROLLERRADIUSONAIR;
        }

        // Dash counter. Updates dash timer and charges
        if (playerStats.DashCharge < playerStats.PlayerAttributes.MaxDashCharge)
        {
            CurrentTimeToGetCharge -= Time.deltaTime;
            if (CurrentTimeToGetCharge <= 0)
            {
                playerStats.DashCharge++;
                CurrentTimeToGetCharge = player.Values.TimeToGetDashCharge;
            }
        }
    }

    /// <summary>
    /// Should rearrange this stuff.
    /// </summary>
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

        // Happens when player is dashing
        if (dashing)
        {
            Dashing();
            return;
        }

        // Happens if player pressed jump.
        // Will keep pushing player upwards while the time is passing.
        // Needs to happen here, on fixed update, or jump will be canceled.
        // After the jumping time has reached it's limit, a coroutine with gravity will start running.
        if (jumpingCoroutine != null)
        {
            directionPressed.y = player.Values.JumpForce;
        }

        // Gravity. Calculates gravity after jumping.
        directionPressed.y -= GRAVITY * gravityIncrement * Time.fixedDeltaTime;

        // Movement. Calculates movement after everything else.
        characterController.Move(directionPressed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Chekcs if player is moving.
    /// </summary>
    /// <returns>Retruns true if player is moving.</returns>
    public bool IsPlayerMoving()
    {
        if (Vector3.Distance(transform.position, positionOnLastCalculation) > 0.5f)
        {
            positionOnLastCalculation = transform.position;
            return true;
        }
        return false;
    }

    /// <summary>
    /// If player presses jump, starts jumping coroutine.
    /// </summary>
    private void JumpPress()
    {
        if (jumpingCoroutine == null && characterController.isGrounded)
        {
            jumpingCoroutine = JumpingCoroutine();
            StartCoroutine(jumpingCoroutine);
        }
    }

    /// <summary>
    /// If player presses dash it will check if dash is possible.
    /// Player must have a dash, and must be pressing a Direction.
    /// If this method is executed, a variable is turned to true and dash will begin.
    /// </summary>
    private void Dash()
    {
        // Player must have a dash, and must be pressing a Direction.
        if (dashing == false && (directionPressed.x != 0 || directionPressed.z != 0) &&
            playerStats.DashCharge > 0)
        {
            dashing = true;
            dashingTimer = Time.time;

            // If player is running it divides this value so dash is always the same
            lastDirectionPressed = Speed == 
                player.Values.Speed ? directionPressed : 
                directionPressed.normalized * 
                player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;

            OnEventDash();

            playerStats.DashCharge--;

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
        // Decrements dash force smoothly
        if (dashCurrentValue - Time.fixedDeltaTime * player.Values.DashingTimeReducer > 1)
            dashCurrentValue -= Time.fixedDeltaTime * player.Values.DashingTimeReducer;
        else
            dashCurrentValue = 1;

        // Calculates dash Direction
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
    /// Updates player's velocity if run is pressed or released.
    /// </summary>
    private void Run(bool condition)
    {
        if (castingContinuousSpell == false)
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
            OnEventRun(condition);
        }
    }

    /// <summary>
    /// Updates speed variable.
    /// </summary>
    public void UpdateSpeed() =>
        Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;

    /// <summary>
    /// Reduces speed if the player is attacking with a continuous spell.
    /// </summary>
    /// <param name="spellCastType"></param>
    private void ReduceSpeedOnContinuousAttack(SpellCastType spellCastType)
    {
        if (spellCastType == SpellCastType.ContinuousCast)
        {
            castingContinuousSpell = true;
            Speed = playerStats.CommonAttributes.MovementSpeedMultiplier * player.Values.Speed * 0.5f;
        }
    }

    /// <summary>
    /// Turns speed back to normal after continuous attack is over.
    /// </summary>
    private void NormalSpeedAfterContinuousAttack()
    {
        castingContinuousSpell = false;
        if (Running) Speed = player.Values.RunningSpeed * playerStats.CommonAttributes.MovementSpeedMultiplier;
        else Speed = player.Values.Speed * playerStats.CommonAttributes.MovementSpeedMultiplier;

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
            if (gravityIncrement >= 0.2f / Time.fixedDeltaTime) gravityIncrement = 0.2f / Time.fixedDeltaTime;
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
    /// Increments gravity value while player is falling ---------(WITHOUT JUMPING)----------.
    /// (Gravity while jumping is executed on fixed update).
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator FallingCoroutine()
    {
        // Starts incrementing gravity every fixed update
        while (true)
        {
            yield return wffu;

            // Increments gravity
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

    // Registered on PlayerSounds and PlayerFinalCameraDashEvent
    protected virtual void OnEventDash() => EventDash?.Invoke();
    public event Action EventDash;

    // Registere on DelayedCamera
    protected virtual void OnEventRun(bool condition) => EventRun?.Invoke(condition);
    public event Action<bool> EventRun;
}
