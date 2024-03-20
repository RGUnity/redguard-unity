using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CC_Rigidbody_01 : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float groundDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayers;
    
    [SerializeField] private bool isGrounded = true;
    
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    
    private bool isReadyToJump = true;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayers);
        
        horizontalInput = LocalScene.inputManager.horizontal;
        verticalInput = LocalScene.inputManager.vertical;

        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        if (LocalScene.inputManager.jump)
        {
            if (isGrounded && isReadyToJump)
            {
                Jump();
                isReadyToJump = false;
            
                Invoke(nameof(ResetJump),  jumpCooldown);
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = transform.forward * verticalInput;
        
        // with forward and sideways movement:
        //moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }
        
    }

    private void RotatePlayer()
    {
        //use the Horizontal buttons to rotate the player
        float xInput = LocalScene.inputManager.horizontal;
        //transform.Rotate(0f,xInput * Time.deltaTime*100, 0f);

        if (isGrounded)
        {
            RotateAroundAxis(transform.up, xInput * 4000 * Time.deltaTime);
        }
        
    }
    
    void RotateAroundAxis(Vector3 axis, float angle)
    {
        angle = angle * Mathf.Deg2Rad; // Convert angle to radians
        Quaternion deltaRotation = Quaternion.AngleAxis(angle, axis);
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(targetRotation);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        print("Jump now");
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }
}
