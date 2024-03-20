using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CC_Rigidbody_02 : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private bool useMouseLook;

    private bool isGrounded = true;
    private Vector3 moveVector;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        RotatePlayer();
        
        
    }

    private void Update()
    {
        if (isGrounded && LocalScene.inputManager.jump)
        {
            Jump();
        }

        if (isGrounded)
        {
            MovePlayer();
            
            float zInput = LocalScene.inputManager.vertical;
            
            moveVector = transform.forward * zInput;
            moveVector = moveVector.normalized * moveSpeed;
            
            moveVector *= Time.deltaTime;
            
            //rb.MovePosition();
        }
    }

    private void MovePlayer()
    {
        
    }
    
    private void RotatePlayer()
    {
        if (useMouseLook)
        {
            //Rotate the player with the Mouse-X axis
            float mouseX = LocalScene.inputManager.mouseX;
            //transform.Rotate(0f,mouseX * Time.deltaTime *100f, 0f);
            
            RotateAroundAxis(transform.up, mouseX * 4000 * Time.deltaTime);
        }
        else
        {
            //use the Horizontal buttons to rotate the player
            float xInput = LocalScene.inputManager.horizontal;
            //transform.Rotate(0f,xInput * Time.deltaTime*100, 0f);
            
            RotateAroundAxis(transform.up, xInput * 4000 * Time.deltaTime);
        }
    }

    private void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, 0.3f, -transform.up, out RaycastHit hit, 1.1f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        Vector3 jumpVector = (transform.forward + transform.up).normalized * 400;
        
        print("Jump!");
        isGrounded = false;
        rb.AddForce(jumpVector);
    }
    
    void RotateAroundAxis(Vector3 axis, float angle)
    {
        angle = angle * Mathf.Deg2Rad; // Convert angle to radians
        Quaternion deltaRotation = Quaternion.AngleAxis(angle, axis);
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(targetRotation);
    }
}
