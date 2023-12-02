using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float crouchSpeed;

    public float jumpForce;
    public float downForce;

    public enum MoveState
    {
        Walk,
        Run,
        crouch,
    }
    public MoveState moveState;
    private MoveState oldState;

    private bool grounded;

    private float horizontalInput;
    private float verticalInput;

    public float playerHeight;
    private Vector3 moveForce;

    public Rigidbody rb;
    public Transform playerCamera;

    void Start()
    {

    }
    void Update()
    {
        playerRoation();
        groundCheck();
        jump();
        moveStateHandler();
    }
    private void FixedUpdate()
    {
        Move();
        AddDownForce();
    }

    private void playerRoation()
    {
        Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
        transform.rotation = playerRotation;
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
        if (Input.GetButtonDown("Jump") && grounded){
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            grounded = false;
        }
    }
    private void AddDownForce()
    {
        if (!grounded)
        {
            rb.AddForce(0, -downForce, 0, ForceMode.Force);
        }
    }

    private void groundCheck(){
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
    }
    private void moveStateHandler()
    {
        
        if (Input.GetKey(KeyCode.LeftControl) && grounded)
        {
            moveState = MoveState.crouch;
            moveSpeed = crouchSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxisRaw("Vertical") >= 0))
        {
            moveState = MoveState.Run;
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveState = MoveState.Walk;
            moveSpeed = walkSpeed;
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
                break;
        }
    }
}
