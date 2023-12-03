using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{

    [SerializeField] float playerSpeed;

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

    private bool wallRunning = false;
    public float wallRunSpeedNeeded;

    private float horizontalInput;
    private float verticalInput;

    public float playerHeight;
    private Vector3 moveForce;

    private GameObject runningOnWall;
    public Rigidbody rb;
    public Transform playerCamera;

    void Start()
    {

    }
    void Update()
    {
        playerSpeed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);

        if (wallRunning && playerSpeed < wallRunSpeedNeeded) {
            exitWallRun();
        }

        Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
        transform.rotation = playerRotation;

        groundCheck();
        jump();
        moveStateHandler();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        moveForce = ((transform.forward * verticalInput) + (transform.right * horizontalInput));            
        
        rb.AddForce(moveForce.normalized * moveSpeed * 10, ForceMode.Force);
    }
    private void jump()
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
                grounded = false;
            }
        }
    }

    private void groundCheck(){
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
    }
    private void moveStateHandler()
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
    private void OnStateChange(MoveState newState, MoveState lastState)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !grounded && playerSpeed > wallRunSpeedNeeded && moveState != MoveState.Walk)
        {
            runningOnWall = collision.gameObject;
            startWallRun();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == runningOnWall)
        {
            exitWallRun();
        }

    }

    private void startWallRun()
    {
        wallRunning = true;
        rb.useGravity = false;
    }
    private void exitWallRun()
    {
        runningOnWall = null;
        wallRunning = false;
        rb.useGravity = true;
    }
}
