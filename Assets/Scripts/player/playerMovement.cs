using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class handles the movement of the player as well most features that use collision
public class playerMovement : MonoBehaviour
{
    private float playerSpeed; // the X and Z velocity of the player added together.
    
    [Header("Movement Speed")]
    public float sprintSpeed; // Players movement speed when sprinting.
    public float walkSpeed; // Players movement speed when walking.
    public float crouchSpeed; // Players movement speed when crouchwalking.
    public float wallRunningSpeed; // Players movement speed when wall running.

    private float moveSpeed; // Float that scores players movement speed for there current state.

    [Header("Drag")]
    public float groundedDrag; // ammout of drag applies to the players rigidbody when on the ground.
    public float airDrag; // ammout of drag applies to the players rigidbody when in the air.


    [Header("Jumping")]

    public float jumpForce; // Amount of force applied when jumping.

    public float jumpBufferTime; // the window of time before landing on the ground the player has to buffer a jump.
    private float jumpBufferTimer; // Timer for jump buffering.

    public float CoyoteTime; // the window of time after falling where the player can still jump.
    private float CoyoteTimer; // timer for coyote time.

    private bool grounded; // Check if the player is grounded.
    private bool OldGrounded; // Check if the player was grounded on the last frame.
    private bool OldWallRunning; // Check if the player was wall running on the last frame.

    // All of the movement states the player can be in
    public enum MoveState
    {
        Walk,
        Run,
        crouch,
        wallRunning,
        Idle,
        Slide,
    }
    public MoveState moveState; // users current moveState
    private MoveState oldState; // users moveState lastframe

    [Header("Sliding")]
    public float slideThreshold; // Speed needed to preform a slide
    public float slideCooldownTime; // Cooldown time between slides
    private float slideCooldown; // Timer for the cooldown between slides
    public float slideDrag; // Drag applies to the player when sliding
    public float slideForce; // Force applied when sliding

    [Header("Wall Running")]
    public float wallRunSpeedNeeded; // Speed needed to perfrom a wallrun
    public float wallRunCoolDownTime; // Cooldown time between wallruns
    public float wallRunMomentum; // Force applies to the player when jumping from a wallrun
    public float wallRunGrabForce; // Small amount of upwards force applied when initally walling to move player to a better place on the wall

    public static float wallRunCameraRotation; // Target rotation of the camera (Used when camera rotates when wallrunning and unwallrunning)
    public float wallRunRotationAmount; // Amount of camera rotation when wall running

    private bool wallRunReady; // Bool that stores if the player can wallrun
    private bool wallRunning = false; // Bool that stores if the player is wallrunning
    private float wallRunCoolDown; // Timer for wallrun cooldown

    private float horizontalInput; // Stores Horizontal movement input
    private float verticalInput; // Stores Vertical movement input

    [Header("Power Up Pads & Zones")]
    public float jumpPadForce; // Force applies when standing on a jump pad
    private float jumpPadCheck; // Stops coyote time from alloing a second jump when boncing off a jump pad

    public float boostPadForce; // Force applies when standing on a boost pad
    public float boostFOV; // FOV increase when the user stands on a boost pad

    private Vector3 padBoost; // Dirrection jump or boost pad will applie force 

    [Header("Player Stats")]
    // Player Height and width if used for raycasts
    public float playerHeight; // Player Hight
    public float playerWidth; // Player Width
    
    public static float playerFOV; // Players current FOV target
    public float FOV; // Players standard FOV

    private int currentCheckpoint; // Current checkpoint the player will spawn at

    [Header("References")]
    public Rigidbody rb; // Players rigidbody
    public Transform playerCamera; // Players camera
    private GameObject runningOnWall; // Wall the player is running on
    public gameGUI GUI; // Game GUI

    // Ran on first frame
    void Start()
    {
        // Initalise players cooldowns and movespeed
        moveSpeed = walkSpeed;
        wallRunCoolDown = wallRunCoolDownTime;
        slideCooldown = slideCooldownTime;

        loadSettings(); // Loads players FOV values from settings
        jumpBufferTimer = 0;
        Time.timeScale = 1; // Initalise timescale

        currentCheckpoint = 0;
        jumpPadCheck = 0;

        // Intalise variable that store if the player has grounded or wallrunning on the last frame
        OldGrounded = grounded;
        OldWallRunning = wallRunning;
    }

    // Called every frame
    void Update()
    {
        // Calculate players current speed
        playerSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);

        // Exits wall running of the player is wallruning and is too slow
        if (wallRunning && playerSpeed < wallRunSpeedNeeded) {
            exitWallRun();
        }

        // Add time to delta time to wallRunCoolDown if it has not been reached
        // If cooldown has been reached check if the player meets the other conditions required to wallrun
        // These other conditions are:
        // Player must be in the air
        // Player must be moving fast enough to wallrun
        // Player must not be walking
        if (wallRunCoolDown < wallRunCoolDownTime)
        {
            wallRunCoolDown += Time.deltaTime;
        } else
        {
            wallRunReady = !grounded && playerSpeed > wallRunSpeedNeeded && moveState != MoveState.Walk;
        }

        // Move player to match where the camera is facing if not wallruning
        if (!wallRunning)
        {
            Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
            transform.rotation = playerRotation;
        }

        // Add time.deltaTime to slide cooldown if its not been met and the player isn't sliding
        if (moveState != MoveState.Slide && slideCooldown < slideCooldownTime)
        {
            slideCooldown += Time.deltaTime;
        }

        groundCheck(); // Updates groundCheck
        jump(); // Allows the player to jump
        jumpBufferingAndCoyoteTime(); // Hangles junp buffering and coyote time
        moveStateHandler(); // Manages the players movement state
    }

    // Called on a fixed frame rate
    void FixedUpdate()
    {
        // Handles player movement
        Move();
    }

    // Loads settings for the players FOV
    void loadSettings()
    {
        userSettings data = SaveSystem.loadUserSettings();

        FOV = data.FOV;
        boostFOV += data.FOV;
        playerFOV = FOV;
    }

    // Handles player movement
    void Move()
    {
        Vector3 moveForce;
        // Gets users vertical and horizontal input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        // Calculate move direction
        moveForce = ((transform.forward * verticalInput) + (transform.right * horizontalInput));            
        
        // Adds force to the player for movement
        rb.AddForce(moveForce.normalized * moveSpeed * 10, ForceMode.Force);
    }

    // Handles Jumping
    void jump()
    {
        // Checks if user can jump
        if (grounded || wallRunning || CoyoteTimer > 0) {
            // If the user Jumps or has buffered a jump
            if (Input.GetButtonDown("Jump") || jumpBufferTimer > 0)
            {
                // Resets Jump buffer and Coyote time
                jumpBufferTimer = 0;
                CoyoteTimer = 0;
                
                if (wallRunning)
                {
                    // Applies force for wallrunning Jump
                    rb.AddForce(0, jumpForce * (float)1.5, 0, ForceMode.Impulse);
                    rb.AddForce(transform.forward * wallRunMomentum, ForceMode.Impulse);
                    exitWallRun();
                    grounded = false;
                }
                else
                {
                    // Applies force for regular Jump
                    rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                    grounded = false;
                }
            }
        } else
        {
            // Buffer Jump
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimer = jumpBufferTime;
            }
        }

        // Handles drag applies to the player
        if (grounded || wallRunning)
        {
            if (moveState == MoveState.Slide)
            {
                rb.drag = slideDrag;

            } else
            {
                rb.drag = groundedDrag;
            }
        } else
        {
            rb.drag = airDrag;
        }
    }

    // Handles Jump Buffering and coyote time
    void jumpBufferingAndCoyoteTime()
    {
        // Decreases jump buffering timer when it is active
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // If the player was grounded or wallrunning last frame
        if (OldGrounded != grounded || OldWallRunning != wallRunning)
        {
            // If the player is no longer grounded or wallrunning
            if (grounded == false && wallRunning == false) 
            {
                // Check that the player didn't already jump or bonce off a bonce pad
                if (!Input.GetButton("Jump") && jumpPadCheck <= 0)
                {
                    // Start coyote time timer
                    CoyoteTimer = CoyoteTime;
                }
            }
            

        } else if (CoyoteTimer > 0)
        {
            // Run out coyote time timer when its activeated 
            CoyoteTimer -= Time.deltaTime;
        }

        // Jump Pad Coyote Time Fix
        if (jumpPadCheck > 0)
        {
            jumpPadCheck -= Time.deltaTime;
        }
        // Set old grounded and wallrunning to the new values ready for the next frame
        OldGrounded = grounded;
        OldWallRunning = wallRunning;
    }

    // Checks if the player is grounded
    void groundCheck()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
    }

    // Handles movement States
    void moveStateHandler()
    {
        // Checks if players can stand up when crouching
        RaycastHit hit;
        bool headBlocked = false; // Initalise headblock 
        // If the player is crouching or slideing check if there head will be blocked if they stand up
        if (moveState == MoveState.crouch || moveState == MoveState.Slide)
        {
            // use 5 raycasts to check if the players head is blocked
            // One ray cast is in the middle of the player
            // The other 4 are off set just behind, just in front, just left and right of the player
            // All of these ray casts casts upwards
            headBlocked = 
                Physics.Raycast(transform.position - transform.right, Vector3.up, out hit, playerHeight - 0.2f) || 
                Physics.Raycast(transform.position + transform.right, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position - transform.forward, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position + transform.forward, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position, Vector3.up, out hit, playerHeight - 0.2f);
        }
        // If the player is wallrunning set the there moveState to wallrunning
        if (wallRunning == true)
        {
            moveState = MoveState.wallRunning;

        } // checks if the player is crouching or if there head is blocked
        else if ((Input.GetKey(KeyCode.LeftControl)) || moveState == MoveState.Slide || headBlocked && grounded)
        {
            // Checks if the player is sliding or crouching
            if (playerSpeed > slideThreshold && slideCooldown >= slideCooldownTime)
            {
                moveState = MoveState.Slide;
            }
            else
            {
                moveState = MoveState.crouch;
            }
        } // Checks if the player is running
        else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Vertical") >= 0))
        {
            moveState = MoveState.Run;
        } // Checks if the player is walking
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            moveState = MoveState.Walk;
        } else
        {
            // if no of the other movement states are set then set the state to idle
            moveState = MoveState.Idle;
        }
        
        // checks if state has been changed
        if (moveState != oldState)
        {
            // Adds force to the player when sliding
            if(moveState == MoveState.Slide) 
            {
                Vector3 slideForceApplied;
                slideForceApplied = (transform.forward);
                rb.AddForce(slideForceApplied.normalized * slideForce, ForceMode.Impulse);
            }
            OnStateChange(moveState, oldState);
            oldState = moveState;
        }
    }

    // Function handles what happens when the player changes movement state
    // Gets called when the player changes movestate
    void OnStateChange(MoveState newState, MoveState lastState)
    {
        // Switch statment when the player exits a state
        switch (lastState)
        {
            case MoveState.Slide:
                // Change player to normal size
                if (newState != MoveState.crouch)
                {
                    rb.transform.localScale = new Vector3(1, 1, 1);
                }
                // start slide cooldown
                slideCooldown = 0;
                break;
            case MoveState.crouch:
                // Changes player to normal size
                if (newState != MoveState.Slide)
                {
                    rb.transform.localScale = new Vector3(1, 1, 1);
                }
                break;
        }

        // Switch statment for when player enters a state
        switch (newState)
        {
            case MoveState.crouch:
                if (lastState != MoveState.Slide)
                {
                    // Changes player to be half sized
                    rb.transform.localScale = new Vector3(1, (float)0.5, 1);
                    // Add downwards force player remains on the ground
                    rb.AddForce(0, -14, 0, ForceMode.Impulse);
                }
                // Set movespeed to crouch speed
                moveSpeed = crouchSpeed;
                break;
            case MoveState.Slide:
                if (lastState != MoveState.crouch)
                {
                    // Changes player to be half sized
                    rb.transform.localScale = new Vector3(1, (float)0.5, 1);
                    // Add downwards force player remains on the ground
                    rb.AddForce(0, -14, 0, ForceMode.Impulse);
                }
                // Sets movement speed to 0 so the user cannot walk while crouching
                moveSpeed = 0;
                break;
            case MoveState.Walk:
                // Set movespeed to walk speed
                moveSpeed = walkSpeed;
                break;
            case MoveState.Run:
                // Set movespeed to sprint speed
                moveSpeed = sprintSpeed;
                break;
            case MoveState.wallRunning:
                // Set movespeed to wallrunning speed
                moveSpeed = wallRunningSpeed;
                break;
        }
    }

    // Called the frame the player collides with a collider
    void OnCollisionEnter(Collision collision)
    {
        // if the collider is a wall
        if (collision.gameObject.layer == 8 && wallRunReady)
        {
            // sets wall run dirrection
            Vector3 playerDirectionRight = transform.right;

            // checks what side of the player the wall is and sets the camera rotation
            if (Physics.Raycast(transform.position, playerDirectionRight, playerWidth)){
                wallRunCameraRotation = wallRunRotationAmount;
            } 
            else if (Physics.Raycast(transform.position, -playerDirectionRight, playerWidth)){
                wallRunCameraRotation = -wallRunRotationAmount;
            }

            // set runningOnWall to the be the wall
            runningOnWall = collision.gameObject;
            // Starts a wall run
            startWallRun();

        // if the collider is a checkpoint
        } else if (collision.gameObject.tag == "CheckPoint")
        {
            
            // gets the checkpointFX script from the checkpoint
            checkpointFX checkpoint = collision.gameObject.GetComponent<checkpointFX>();

            // Checks if the checkpoint hasn't been activated and its not a checkpoint before the currently activated one
            if (checkpoint.checkpointNumber > currentCheckpoint)
            {
                // Change current checkpoint to this checkpoint
                currentCheckpoint = checkpoint.checkpointNumber;
                // Turn the checkpoint light green
                checkpoint.activeLight();
                // Get player repawn script
                playerRespawning respawn = gameObject.GetComponentInParent<playerRespawning>();

                // Sets new player respawn point and rotation
                respawn.spawnPoint = collision.transform.position + new Vector3(0, 1, 0);
                respawn.spawnRotation = Quaternion.Euler(0, collision.transform.localRotation.eulerAngles.y, 0);
            }
        }
    }

    // This is called the frame the player collides with a trigger
    void OnTriggerEnter(Collider trigger)
    {
        // Checks if the trigger is a jump pad
        if (trigger.gameObject.tag == "Jump Pad")
        {
            // adds jump bad boost to the player
            padBoost = (trigger.transform.up);
            rb.AddForce(padBoost * jumpPadForce, ForceMode.VelocityChange);
            jumpPadCheck = 0.1f;

        }
    }

    // This is called every frame the player collides with a trigger
    void OnTriggerStay(Collider trigger)
    {
        // Check if the trigger is a boost pad
        if (trigger.gameObject.tag == "Boost Pad")
        {
            // adds boost pad force to player
            padBoost = (trigger.transform.right);
            rb.AddForce(padBoost * boostPadForce * 100 * Time.deltaTime, ForceMode.Acceleration);
            playerFOV = boostFOV;
        }
    }

    // This is called the frame the player exits a collider
    void OnCollisionExit(Collision collision)
    {
        // checks if the collider a wal the player is running on
        if (collision.gameObject == runningOnWall)
        {
            // exits the wall run
            exitWallRun();
        }
        
    }

    // Called the frame the plaery exits a trigger
    void OnTriggerExit(Collider trigger)
    {
        // checks if the trigger is a boost pad
        if (trigger.gameObject.tag == "Boost Pad")
        {
            //  Sets players FOV back to normal
            playerFOV = FOV;

        } // checks if the trigger is a timerStart area
        else if (trigger.gameObject.tag == "Timer Start")
        {
            // Starts timer
            GUI.startTimer();
        }
    }

    // Called when the player starts a wall run
    void startWallRun()
    {
        // Set wall running to true
        wallRunning = true;
        // Stops the players rigidbody from using gravity
        rb.useGravity = false;

        // Adds small amout of upwards force to put the player slightly higher up the wall
        rb.AddForce(transform.up * wallRunGrabForce, ForceMode.Impulse);
    }

    // called when the player exits a wall run
    void exitWallRun()
    {
        // Initalises runnOnWall and wallRunning
        runningOnWall = null;
        wallRunning = false;

        // Enables gravity
        rb.useGravity = true;

        // Intalises camera rotation
        wallRunCameraRotation = 0;

        // Starts wall run cooldown
        wallRunCoolDown = 0;
    }
}
