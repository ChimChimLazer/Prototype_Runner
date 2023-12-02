using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;


    [SerializeField] bool grounded;

    private float horizontalInput;
    private float verticalInput;

    public float playerHeight;
    Vector3 moveForce;

    public Rigidbody rb;
    public Transform playerCamera;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerRoation();
        groundCheck();
        jump();
    }
    private void FixedUpdate()
    {
        Move();
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
        if (Input.GetButtonDown("Jump") && grounded)
        {
            print("Jump");
            rb.AddForce(0, 10, 0, ForceMode.Impulse);
        }
    }

    private void groundCheck(){
        grounded = Physics.Raycast(transform.position, -Vector3.up, playerHeight);
        Debug.DrawRay(transform.position, -Vector3.up, Color.red, playerHeight);
    }
}
