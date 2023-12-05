using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{

    private float playerSpeed;

    private float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;
    public float wallRunningSpeed;

    public float jumpForce;
    public float downForce;

    public enum MoveState
    {
        Walk,
        Run,
        crouch,
        wallRunning,
    }
    public MoveState moveState;
    private MoveState oldState;

    private bool grounded;

    private bool walkRunReady;
    private bool wallRunning = false;
    private float wallRunCoolDown;
    public float wallRunSpeedNeeded;
    public float wallRunCoolDownTime;

    private float horizontalInput;
    private float verticalInput;

    public float playerHeight;
    public float playerWidth;
    private Vector3 moveForce;

    private GameObject runningOnWall;
    public Rigidbody rb;
    public Transform playerCamera;

    void Start()
    {
        moveSpeed = walkSpeed;
        wallRunCoolDown = wallRunCoolDownTime;
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
            walkRunReady = !grounded && playerSpeed > wallRunSpeedNeeded && moveState != MoveState.Walk;
        }

        if (!wallRunning)
        {
            Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
            transform.rotation = playerRotation;
        }

        groundCheck();
        jump();
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
        if (Input.GetButtonDown("Jump") && (grounded || wallRunning)){
            if (grounded)
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                grounded = false;
            } 
            else if (wallRunning)
            {
                rb.AddForce(0, jumpForce*(float)1.5, 0, ForceMode.Impulse);
                exitWallRun();
                grounded = false;
            }
        }
    }

    void groundCheck(){
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
    }
    void moveStateHandler()
    {
        if (wallRunning == true)
        {
            moveState = MoveState.wallRunning;
        }
        else if (Input.GetKey(KeyCode.LeftControl) && grounded)
        {
            moveState = MoveState.crouch;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Vertical") >= 0))
        {
            moveState = MoveState.Run;
        }
        else
        {
            moveState = MoveState.Walk;
        }
        
        // checks if state has been changed
        if (moveState != oldState)
        {
            OnStateChange(moveState, oldState);
            oldState = moveState;
        }
    }
    void OnStateChange(MoveState newState, MoveState lastState)
    {
        switch (lastState)
        {
            case MoveState.crouch:
                rb.transform.localScale = new Vector3 (1,1,1);
                break;
        }
        switch(newState)
        {
            case MoveState.crouch:
                rb.transform.localScale = new Vector3(1, (float)0.5, 1);
                rb.AddForce(0,-14,0, ForceMode.Impulse);

                moveSpeed = crouchSpeed;
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
        if (collision.gameObject.layer == 8 && walkRunReady)
        {
            Vector3 playerDirectionRight = transform.right;

            if (Physics.Raycast(transform.position, playerDirectionRight, playerWidth)){
                Debug.Log("Wallrun right");
            } 
            else if (Physics.Raycast(transform.position, -playerDirectionRight, playerWidth)){
                Debug.Log("Wallrun left");
            }

            runningOnWall = collision.gameObject;
            startWallRun();
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == runningOnWall)
        {
            exitWallRun();
        }
    }

    void startWallRun()
    {
        wallRunning = true;
        rb.useGravity = false;
    }
    void exitWallRun()
    {
        runningOnWall = null;
        wallRunning = false;
        rb.useGravity = true;

        wallRunCoolDown = 0;
    }
}
