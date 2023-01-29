using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCharacterController : MonoBehaviour
{
    public CharacterController controller;
    public float moveSppeed = 10f;
    public bool useMouseLook;
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Vector3 move = new Vector3(x, 0f, z).normalized;
        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 zMove = transform.forward * z;

        
        // Movement
        if (move.magnitude >= 0.1f && useMouseLook)
        {
            //Move the player along with X and Z inputs
            controller.SimpleMove(move * (moveSppeed*30 * Time.deltaTime));
        }

        if (useMouseLook == false)
        {
            //Move the player only along the Z axis
            controller.SimpleMove(zMove * (moveSppeed *30* Time.deltaTime));
        }

        
        // Rotation
        if (useMouseLook)
        {
            //Rotate the player with the Mouse-X axis
            transform.Rotate(0f,Input.GetAxis("Mouse X")*1.5f, 0f);
        }
        else
        {
            //use the Horizontal buttons to rotate the player
            transform.Rotate(0f,Input.GetAxisRaw("Horizontal")*2, 0f);
        }
    }
}
