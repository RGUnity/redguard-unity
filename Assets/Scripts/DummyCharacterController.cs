using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 10f;
    public float gravity = -9.81f;
    public bool useMouseLook;

    private Vector3 moveVector;
    private float currentGravity = 0f;
    
    void FixedUpdate()
    {
        if (!Game.isPaused)
        {
            MovePlayer();
        }
    }
    
    void Update()
    {
        if (!Game.isPaused)
        {
            RotatePlayer();
        }
    }

    private void MovePlayer()
    {
        if (useMouseLook)
        {
            //Move the player with X and Z inputs
            float xInput = LocalScene.inputManager.horizontal;
            float zInput = LocalScene.inputManager.vertical;
            
            moveVector = transform.right * xInput + transform.forward * zInput;
            moveVector = moveVector.normalized * moveSpeed;
            moveVector += ApplyGravity();
            moveVector *= Time.deltaTime;
            
            controller.Move(moveVector);
        }

        if (useMouseLook == false)
        {
            //Move the player only along the Z axis
            float zInput = LocalScene.inputManager.vertical;
            
            moveVector = transform.forward * zInput;
            moveVector = moveVector.normalized * moveSpeed;
            
            moveVector += ApplyGravity();
            moveVector *= Time.deltaTime;
            
            controller.Move(moveVector);
            
            
        }
    }
    
    private void RotatePlayer()
    {
        if (useMouseLook)
        {
            //Rotate the player with the Mouse-X axis
            float mouseX = LocalScene.inputManager.mouseX;
            transform.Rotate(0f,mouseX * Time.deltaTime *100f, 0f);
        }
        else
        {
            //use the Horizontal buttons to rotate the player
            float xInput = LocalScene.inputManager.horizontal;
            transform.Rotate(0f,xInput * Time.deltaTime*100, 0f);
        }
    }

    private Vector3 ApplyGravity()
    {
        Vector3 gravityVector = new Vector3(0, currentGravity, 0);
        
        if (!controller.isGrounded)
        {
            currentGravity += gravity * Time.deltaTime;
        }
        return gravityVector;
    }
}
