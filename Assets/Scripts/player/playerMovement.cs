using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    private float playerSpeed;
    
    [Header("Movement Speed")]
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float wallRunningSpeed;

    private float moveSpeed;

    [Header("Drag")]
    public float groundedDrag;
    public float airDrag;


    [Header("Jumping")]

    public float jumpForce;

    public float jumpBufferTime;
    private float jumpBufferTimer;

    public float CoyoteTime;
    private float CoyoteTimer;

    private bool grounded;
    private bool OldGrounded;
    private bool OldWallRunning;

    // MoveState enum
    public enum MoveState
    {
        Walk,
        Run,
        crouch,
        wallRunning,
        Idle,
        Slide,
    }
    public MoveState moveState;
    private MoveState oldState;

    [Header("Sliding")]
    public float slideThreshold;
    public float slideCooldownTime;
    private float slideCooldown;
    public float slideDrag;
    public float slideForce;

    [Header("Wall Running")]
    public float wallRunSpeedNeeded;
    public float wallRunCoolDownTime;
    public float wallRunMomentum;
    public float wallRunGrabForce;

    public static float wallRunCameraRotation;
    public float wallRunRotationAmount;

    private bool wallRunReady;
    private bool wallRunning = false;
    private float wallRunCoolDown;

    private float horizontalInput;
    private float verticalInput;

    [Header("Power Up Pads & Zones")]
    public float jumpPadForce;
    private float jumpPadCheck;

    public float boostPadForce;
    public float boostFOV;
    private Vector3 padBoost;

    [Header("Player Stats")]
    public float playerHeight;
    public float playerWidth;
    private Vector3 moveForce;
    private Vector3 slideForceApplied;
    public static float playerFOV;
    public float FOV;

    private int currentCheckpoint;

    [Header("References")]
    public Rigidbody rb;
    public Transform playerCamera;
    private GameObject runningOnWall;
    public gameGUI GUI;

    void Start()
    {
        moveSpeed = walkSpeed;
        wallRunCoolDown = wallRunCoolDownTime;
        slideCooldown = slideCooldownTime;
        playerFOV = FOV;
        jumpBufferTimer = 0;

        currentCheckpoint = 0;
        jumpPadCheck = 0;
        OldGrounded = grounded;
        OldWallRunning = wallRunning;
    }
    void Update()
    {
        playerSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);

        if (wallRunning && playerSpeed < wallRunSpeedNeeded) {
            exitWallRun();
        }
        if (wallRunCoolDown < wallRunCoolDownTime)
        {
            wallRunCoolDown += Time.deltaTime;
        } else
        {
            wallRunReady = !grounded && playerSpeed > wallRunSpeedNeeded && moveState != MoveState.Walk;
        }

        if (!wallRunning)
        {
            Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
            transform.rotation = playerRotation;
        }

        if (moveState != MoveState.Slide && slideCooldown < slideCooldownTime)
        {
            slideCooldown += Time.deltaTime;
        }

        groundCheck();
        jump();
        jumpBufferingAndCoyoteTime();
        moveStateHandler();
    }
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        moveForce = ((transform.forward * verticalInput) + (transform.right * horizontalInput));            
        
        rb.AddForce(moveForce.normalized * moveSpeed * 10, ForceMode.Force);
    }
    void jump()
    {
        if (grounded || wallRunning || CoyoteTimer > 0) {
            if (Input.GetButtonDown("Jump") || jumpBufferTimer > 0)
            {
                jumpBufferTimer = 0;
                CoyoteTimer = 0;
                
                if (wallRunning)
                {
                    rb.AddForce(0, jumpForce * (float)1.5, 0, ForceMode.Impulse);
                    rb.AddForce(transform.forward * wallRunMomentum, ForceMode.Impulse);
                    exitWallRun();
                    grounded = false;
                }
                else
                {
                    rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                    grounded = false;
                }
            }
        } else
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferTimer = jumpBufferTime;
            }
        }
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

    void jumpBufferingAndCoyoteTime()
    {
        // Jump Buffering
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Coyote Time
        if (OldGrounded != grounded || OldWallRunning != wallRunning)
        {
            if (grounded == false && wallRunning == false) 
            {
                if (!Input.GetButton("Jump") && jumpPadCheck <= 0)
                {
                    CoyoteTimer = CoyoteTime;
                }
            }
            

        } else if (CoyoteTimer > 0)
        {
            CoyoteTimer -= Time.deltaTime;
        }

        // Jump Pad Coyote Time Fix
        if (jumpPadCheck > 0)
        {
            jumpPadCheck -= Time.deltaTime;
        }
        OldGrounded = grounded;
        OldWallRunning = wallRunning;
    }

    void groundCheck(){
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
    }
    void moveStateHandler()
    {
        // Checks if players can stand up when crouching
        RaycastHit hit;
        bool headBlocked = false;
        if (moveState == MoveState.crouch || moveState == MoveState.Slide)
        {
            headBlocked = 
                Physics.Raycast(transform.position - transform.right, Vector3.up, out hit, playerHeight - 0.2f) || 
                Physics.Raycast(transform.position + transform.right, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position - transform.forward, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position + transform.forward, Vector3.up, out hit, playerHeight - 0.2f) ||
                Physics.Raycast(transform.position, Vector3.up, out hit, playerHeight - 0.2f);
        }
        if (wallRunning == true)
        {
            moveState = MoveState.wallRunning;
        }
        else if ((Input.GetKey(KeyCode.LeftControl)) || moveState == MoveState.Slide || headBlocked && grounded)
        {
            if (playerSpeed > slideThreshold && slideCooldown >= slideCooldownTime)
            {
                moveState = MoveState.Slide;
            }
            else
            {
                moveState = MoveState.crouch;
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Vertical") >= 0))
        {
            moveState = MoveState.Run;
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            moveState = MoveState.Walk;
        } else
        {
            moveState = MoveState.Idle;
        }
        
        // checks if state has been changed
        if (moveState != oldState)
        {
            if(moveState == MoveState.Slide) 
            {
                slideForceApplied = (transform.forward);
                rb.AddForce(slideForceApplied.normalized * slideForce, ForceMode.Impulse);
            }
            OnStateChange(moveState, oldState);
            oldState = moveState;
        }
    }
    void OnStateChange(MoveState newState, MoveState lastState)
    {
        // Exiting State
        switch (lastState)
        {
            case MoveState.Slide:
                if (newState != MoveState.crouch)
                {
                    rb.transform.localScale = new Vector3(1, 1, 1);
                }
                slideCooldown = 0;
                break;
            case MoveState.crouch:
                if (newState != MoveState.Slide)
                {
                    rb.transform.localScale = new Vector3(1, 1, 1);
                }
                break;
        }
        
        // Entering State
        switch(newState)
        {
            case MoveState.crouch:
                if (lastState != MoveState.Slide)
                {
                    rb.transform.localScale = new Vector3(1, (float)0.5, 1);
                    rb.AddForce(0, -14, 0, ForceMode.Impulse);
                }

                moveSpeed = crouchSpeed;
                break;
            case MoveState.Slide:
                if (lastState != MoveState.crouch)
                {
                    rb.transform.localScale = new Vector3(1, (float)0.5, 1);
                    rb.AddForce(0, -14, 0, ForceMode.Impulse);
                }

                moveSpeed = 0;
                break;
            case MoveState.Walk:
                moveSpeed = walkSpeed;
                break;
            case MoveState.Run:
                moveSpeed = sprintSpeed;
                break;
            case MoveState.wallRunning: 
                moveSpeed = wallRunningSpeed;
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && wallRunReady)
        {
            Vector3 playerDirectionRight = transform.right;

            if (Physics.Raycast(transform.position, playerDirectionRight, playerWidth)){
                wallRunCameraRotation = wallRunRotationAmount;
            } 
            else if (Physics.Raycast(transform.position, -playerDirectionRight, playerWidth)){
                wallRunCameraRotation = -wallRunRotationAmount;
            }

            runningOnWall = collision.gameObject;
            startWallRun();

        } else if (collision.gameObject.tag == "CheckPoint")
        {
            
            checkpointFX checkpoint = collision.gameObject.GetComponent<checkpointFX>();

            if (checkpoint.checkpointNumber > currentCheckpoint)
            {
                currentCheckpoint = checkpoint.checkpointNumber;
                checkpoint.activeLight();
                playerRespawning respawn = gameObject.GetComponentInParent<playerRespawning>();
                respawn.spawnPoint = collision.transform.position + new Vector3(0, 1, 0);
                respawn.spawnRotation = Quaternion.Euler(0, collision.transform.localRotation.eulerAngles.y, 0);
            }
        }
    }

    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag == "Jump Pad")
        {
            padBoost = (trigger.transform.up);
            rb.AddForce(padBoost * jumpPadForce, ForceMode.VelocityChange);
            jumpPadCheck = 0.1f;

        }
    }

    void OnTriggerStay(Collider trigger)
    {
        if (trigger.gameObject.tag == "Boost Pad")
        {
            padBoost = (trigger.transform.right);
            rb.AddForce(padBoost * boostPadForce * 100 * Time.deltaTime, ForceMode.Acceleration);
            playerFOV = boostFOV;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == runningOnWall)
        {
            exitWallRun();
        }
        
    }

    void OnTriggerExit(Collider trigger)
    {
        if (trigger.gameObject.tag == "Boost Pad")
        {
            playerFOV = FOV;
        }
        if (trigger.gameObject.tag == "Timer Start")
        {
            GUI.startTimer();
        }
    }

    void startWallRun()
    {
        wallRunning = true;
        rb.useGravity = false;

        rb.AddForce(transform.up * wallRunGrabForce, ForceMode.Impulse);
    }
    void exitWallRun()
    {
        runningOnWall = null;
        wallRunning = false;
        rb.useGravity = true;

        wallRunCameraRotation = 0;

        wallRunCoolDown = 0;
    }
}
