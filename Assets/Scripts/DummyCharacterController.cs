using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public float moveSpeed = 10f;
    public bool useMouseLook;
    
    
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Vector3 move = new Vector3(x, 0f, z).normalized;
        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 zMove = transform.forward * z;

        
        if (move.magnitude >= 0.1f && useMouseLook)
        {
            //Move the player along with X and Z inputs
            controller.SimpleMove(move * (moveSpeed * 30 * Time.deltaTime));
        }

        if (useMouseLook == false)
        {
            //Move the player only along the Z axis
            controller.SimpleMove(zMove * (moveSpeed * 30 * Time.deltaTime));
        }
    }

    private void RotatePlayer()
    {
        if (useMouseLook)
        {
            //Rotate the player with the Mouse-X axis
            transform.Rotate(0f,Input.GetAxis("Mouse X")* Time.deltaTime *1.5f, 0f);
        }
        else
        {
            //use the Horizontal buttons to rotate the player
            transform.Rotate(0f,Input.GetAxisRaw("Horizontal")* Time.deltaTime*100, 0f);
        }
    }
}
