using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 10f;
    public float gravity = -9.81f;
    public bool useMouseLook;

    private float currentGravity = 0f;
    void FixedUpdate()
    {
        if (!GameManager.isGamePaused)
        {
            MovePlayer();
        }
    }
    
    void Update()
    {
        if (!GameManager.isGamePaused)
        {
            RotatePlayer();
        }
    }

    private void MovePlayer()
    {
        float xInput = Input.GetAxis("Horizontal") * moveSpeed;
        float zInput = Input.GetAxis("Vertical") * moveSpeed;
        //Vector3 move = new Vector3(x, 0f, z).normalized;
        
        
        if (useMouseLook)
        {
            Vector3 moveVector = transform.right * xInput + transform.forward * zInput;
            moveVector += ApplyGravity();
            moveVector *= Time.deltaTime;
            //Move the player along with X and Z inputs
            controller.Move(moveVector);
        }

        if (useMouseLook == false)
        {
            //Move the player only along the Z axis
            Vector3 zMoveVector = transform.forward * zInput;
            zMoveVector += ApplyGravity();
            zMoveVector *= Time.deltaTime;
            controller.Move(zMoveVector);
        }
    }
    
    private void RotatePlayer()
    {
        if (useMouseLook)
        {
            //Rotate the player with the Mouse-X axis
            transform.Rotate(0f,Input.GetAxis("Mouse X")* Time.deltaTime *100f, 0f);
        }
        else
        {
            //use the Horizontal buttons to rotate the player
            transform.Rotate(0f,Input.GetAxisRaw("Horizontal")* Time.deltaTime*100, 0f);
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
