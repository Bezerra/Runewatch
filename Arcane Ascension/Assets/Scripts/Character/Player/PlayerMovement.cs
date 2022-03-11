using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class responsible for handing all player's movement.
/// </summary>
public class PlayerMovement : MonoBehaviour, IFindInput
{
    // Components
    private IInput              input;
    private Player              player;
    private CharacterController characterController;
    private PlayerStats         playerStats;

    // Movement
    private float   movementX;
    private float   movementZ;
    private Vector3 directionPressed;
    private float   speed;
    public float    Speed { get => speed; 
        private set { speed = value; OnEventSpeedChange(Speed); } }
    public bool     Running { get; private set; }
    private Vector3 positionOnLastCalculation;
    public float   InCombatSpeed { get; private set; }
    private float   timeThatLeftFloor;

    // Dash
    private float   dashCurrentValue;
    private Vector3 lastDirectionPressed;
    private float   dashingTimer;
    public bool    Dashing { get; private set; }
    public float    CurrentTimeToGetCharge { get; private set; }
    private float   inCombatDashDelay;
    private NavMeshObstacle navmeshObstacle;
    private int layerBeforeDash;
    private readonly int DASHINGLAYER = 24;
    [SerializeField] private Collider bodyToDamage;

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
    private readonly float  CONTROLLERRADIUSDEFAULT = 0.92f;
    private readonly float  CONTROLLERRADIUSONAIR = 0.5f;

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        player = GetComponent<Player>();
        characterController = GetComponent<CharacterController>();
        playerStats = GetComponent<PlayerStats>();
        wffu = new WaitForFixedUpdate();
        jumpTime = new WaitForSeconds(player.Values.JumpTime);
        Dashing = false;
        navmeshObstacle = GetComponent<NavMeshObstacle>();
        gravityIncrement = DEFAULTGRAVITYINCREMENT;
        dashCurrentValue = player.Values.DashInitialForce;
        CurrentTimeToGetCharge = 0;
        InCombatSpeed = player.Values.OutOfCombatSpeedMultiplier;
        inCombatDashDelay = player.Values.OutOfCombatDashDelayMultiplier;
    }

    private void OnEnable()
    {
        LostInput();
        input.Jump += JumpPress;
        input.Dash += Dash;
        input.Run += Run;
        playerStats.EventSpeedUpdate += UpdateSpeed;
        LevelPieceGameProgressControlAbstract.EventPlayerInCombat += InCombat;
    }

    private void Start()
    {
        Run();
    }

    private void OnDisable()
    {
        LostInput();
        playerStats.EventSpeedUpdate -= UpdateSpeed;
        LevelPieceGameProgressControlAbstract.EventPlayerInCombat -= InCombat;
    }

    /// <summary>
    /// Called if player enters or leaves combat.
    /// </summary>
    /// <param name="condition"></param>
    private void InCombat(bool condition)
    {
        if (condition)
        {
            InCombatSpeed = 1f;
            inCombatDashDelay = 1f;
        }
        else
        {
            InCombatSpeed = player.Values.OutOfCombatSpeedMultiplier;
            inCombatDashDelay = player.Values.OutOfCombatDashDelayMultiplier;
        }
    }

    private void Update()
    {
        // Movement Directions
        movementX = Mathf.Lerp(movementX, input.Movement.x, Time.deltaTime * speed * 1.5f);
        movementZ = Mathf.Lerp(movementZ, input.Movement.y, Time.deltaTime * speed * 1.5f);
        Vector3 sideMovement = movementX * speed * transform.right;
        Vector3 forwardMovement = movementZ * speed * transform.forward;
        directionPressed = sideMovement + forwardMovement;
        directionPressed *= InCombatSpeed;

        /*
        // Controls character radius to prevent getting stuck on edges after jumping
        if (IsInAirAfterTime())
        {
            characterController.radius = CONTROLLERRADIUSONAIR;
        }
        else
        {
            // Character radius
            if (characterController.radius != CONTROLLERRADIUSDEFAULT)
                characterController.radius =
                    Mathf.Lerp(characterController.radius, CONTROLLERRADIUSDEFAULT, Time.deltaTime * 50f);
        }
        */

        // Dash counter. Updates dash timer and charges
        if (playerStats.DashCharge < playerStats.PlayerAttributes.MaxDashCharge)
        {
            CurrentTimeToGetCharge -= Time.deltaTime * inCombatDashDelay;
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
        if (jumpingCoroutine == null && IsGrounded() == false && Dashing == false)
        {
            if (fallingCoroutine == null)
            {
                fallingCoroutine = FallingCoroutine();
                StartCoroutine(fallingCoroutine);
                // Increments gravity smoothly
            }
        }

        // Removes run if the player stopped
        //if (directionPressed.Lesser(Vector3.zero, 0.5f))
        //{
        //    Run(false);
        //}

        // Happens when player is Dashing
        if (Dashing)
        {
            DashingLogic();
            return;
        }

        // Happens if player pressed jump.
        // Will keep pushing player upwards while the time is passing.
        // Needs to happen here, on fixed update, or jump will be canceled.
        // After the jumping time has reached it's limit, a coroutine used to increment
        // gravity will start running.
        if (jumpingCoroutine != null)
        {
            directionPressed.y = player.Values.JumpForce;
        }

        // Applies gravity
        directionPressed.y -= GRAVITY * gravityIncrement * Time.fixedDeltaTime;

        // Movement. Calculates movement after everything else.
        characterController.Move(directionPressed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Chekcs if player is moving.
    /// </summary>
    /// <param name="distanceOfPreviousPosition">Distance of previous position.</param>
    /// <returns>Retruns true if player is moving.</returns>
    public bool IsPlayerMoving(float distanceOfPreviousPosition = 0.1f)
    {
        if (Vector3.Distance(transform.position, positionOnLastCalculation) > distanceOfPreviousPosition)
        {
            positionOnLastCalculation = transform.position;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets player velocity.
    /// </summary>
    public float GetVelocity => 
        characterController.velocity.magnitude;

    /// <summary>
    /// Checks if player is stopped.
    /// </summary>
    /// <param name="maximumVelocityToConsider">Maximum velocity the player has to be considered as stopped.</param>
    /// <returns>Retruns true if player is stopped.</returns>
    public bool IsPlayerStopped(float maximumVelocityToConsider = 0.1f)
    {
        if (characterController.velocity.magnitude < maximumVelocityToConsider) return true;
        return false;
    }

    /// <summary>
    /// If player presses jump, starts jumping coroutine.
    /// </summary>
    private void JumpPress()
    {
        if (jumpingCoroutine == null && IsGrounded())
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
        if (Dashing == false && input.Movement.magnitude > 0.5f &&
            playerStats.DashCharge > 0)
        {
            Dashing = true;
            dashingTimer = Time.time;
            layerBeforeDash = gameObject.layer;

            Vector3 directionPressedMultiplied = directionPressed.normalized *
                player.Values.DashContinuousForce;

            // Multiplies direction pressed by speed modifiers.
            lastDirectionPressed = 
                directionPressedMultiplied * 
                player.Values.Speed * 
                playerStats.CommonAttributes.MovementSpeedMultiplier *
                playerStats.CommonAttributes.MovementStatusEffectMultiplier;

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
    private void DashingLogic()
    {
        // Decrements dash force smoothly
        float minimumDashForce = 3;

        if (dashCurrentValue - Time.fixedDeltaTime * player.Values.DashingTimeReducer > minimumDashForce)
            dashCurrentValue -= Time.fixedDeltaTime * player.Values.DashingTimeReducer;
        else
            dashCurrentValue = minimumDashForce;

        // Calculates dash Direction
        Vector3 sideMovement = lastDirectionPressed.x * Vector3.right;
        Vector3 forwardMovement = lastDirectionPressed.z * Vector3.forward;
        Vector3 finalDirection = sideMovement + forwardMovement;

        // Disables variables to prevent player from colliding with enemies
        gameObject.layer = DASHINGLAYER;
        bodyToDamage.enabled = false;
        navmeshObstacle.enabled = false;

        // Dashes
        characterController.Move(dashCurrentValue * Time.fixedDeltaTime * finalDirection);

        // Checks if player is colliding with an enemy while Dashing
        Collider[] enemyCollision = Physics.OverlapSphere(transform.position,
            CONTROLLERRADIUSDEFAULT, Layers.EnemyLayer);

        // Cancels dash and resets dash value
        if (Time.time - dashingTimer > player.Values.DashingTime &&
            enemyCollision.Length == 0)
        {
            dashCurrentValue = player.Values.DashInitialForce;
            Dashing = false;
            gameObject.layer = layerBeforeDash;
            bodyToDamage.enabled = true;
            navmeshObstacle.enabled = true;
            return;
        }
    }

    /// <summary>
    /// Updates player's velocity if run is pressed or released.
    /// </summary>
    private void Run()
    {
        if (Running == false)
        {
            Running = true;
            Speed = player.Values.RunningSpeed *
                playerStats.CommonAttributes.MovementSpeedMultiplier *
                playerStats.CommonAttributes.MovementStatusEffectMultiplier *
                InCombatSpeed;
        }
        else
        {
            Running = false;
            Speed = player.Values.Speed *
                playerStats.CommonAttributes.MovementSpeedMultiplier *
                playerStats.CommonAttributes.MovementStatusEffectMultiplier *
                InCombatSpeed;
        }
    }

    /// <summary>
    /// Updates speed variable.
    /// </summary>
    private void UpdateSpeed(float speed)
    {
        // Updates speed
        Speed = speed;

        if (Running)
        {
            Speed = player.Values.RunningSpeed *
                playerStats.CommonAttributes.MovementSpeedMultiplier *
                playerStats.CommonAttributes.MovementStatusEffectMultiplier *
                InCombatSpeed;
        }
        else
        {
            Speed = player.Values.Speed *
                playerStats.CommonAttributes.MovementSpeedMultiplier *
                playerStats.CommonAttributes.MovementStatusEffectMultiplier *
                InCombatSpeed;
        }
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

            // 0.2f is the magic number that always works for our desired falling speed

            // Starts incrementing gravity until it reaches its peak
            if (gravityIncrement >= 0.2f / Time.fixedDeltaTime) gravityIncrement = 0.2f / Time.fixedDeltaTime;
            else gravityIncrement += player.Values.GravityIncrement;

            // Breaks the coroutine if the character touches the floor
            if (IsGrounded())
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
        Ray rayToFloor = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(rayToFloor, 0.35f, Layers.WallsFloor))
        {
            // Gravity on a ramp for example
            gravityIncrement = 0.25f / Time.fixedDeltaTime;
        }
        else
        {
            // Gravity if there is no floor beneath,
            // it will increment smoothly from a low value
            gravityIncrement = 0.0025f;
        }
        
        // Starts incrementing gravity every fixed update
        while (true)
        {
            // 0.2f is the magic number that always works for our desired falling speed

            // Increments gravity
            if (gravityIncrement >= 0.2f / Time.fixedDeltaTime) gravityIncrement = 0.2f / Time.fixedDeltaTime;
            else gravityIncrement += player.Values.GravityIncrement;

            yield return wffu;

            // Breaks the coroutine if the character touches the floor
            if (IsGrounded())
            {
                break;
            }
        }

        gravityIncrement = DEFAULTGRAVITYINCREMENT;
        fallingCoroutine = null;
    }

    /// <summary>
    /// Checks if player is grounded.
    /// </summary>
    /// <returns>Returns true if player is grounded.</returns>
    public bool IsGrounded()
    {
        if (characterController.isGrounded)
        {
            timeThatLeftFloor = Time.time;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if player is in air after leaving the ground.
    /// </summary>
    /// <param name="timeLimit">Time after leaving the ground.</param>
    /// <returns>Retruns true if player is in air.</returns>
    public bool IsInAirAfterTime(float timeLimit = 0.25f)
    {
        if (Time.time - timeThatLeftFloor > timeLimit)
            return true;
        return false;
    }

    // Subscribed on PlayerSounds, PlayerDashEffect, PlayerCamera
    protected virtual void OnEventDash() => EventDash?.Invoke();
    public event Action EventDash;

    // Subscribed on PlayerSounds
    protected virtual void OnEventSpeedChange(float speed) => EventSpeedChange?.Invoke(speed);
    public event Action<float> EventSpeedChange;

    public void FindInput()
    {
        if (input != null)
        {
            input.Jump -= JumpPress;
            input.Dash -= Dash;
            input.Run -= Run;
        }

        input = FindObjectOfType<PlayerInputCustom>();
        input.Jump += JumpPress;
        input.Dash += Dash;
        input.Run += Run;
    }

    public void LostInput()
    {
        if (input != null)
        {
            input.Jump -= JumpPress;
            input.Dash -= Dash;
            input.Run -= Run;
        }
    }    
}
