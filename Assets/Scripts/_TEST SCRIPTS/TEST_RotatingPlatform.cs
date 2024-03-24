using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_RotatingPlatform : MonoBehaviour
{   
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 1;

    public float yStepRotation;
    
    
    private void FixedUpdate()
    {
        //RotateAroundAxis(Vector3.up, speed);
        
        transform.Rotate(Vector3.up, speed);

        yStepRotation = speed;
    }

    private void RotateAroundAxis(Vector3 axis, float angle)
    {
        angle *= Mathf.Deg2Rad; // Convert angle to radians
        //angle *= Time.deltaTime * 60; // Apply deltaTime
        Quaternion deltaRotation = Quaternion.AngleAxis(1, axis);
        
        // stepRotation is used by the player to rotate around the pivot
        yStepRotation = deltaRotation.eulerAngles.y;
        
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(targetRotation);
        float test = deltaRotation.y * Mathf.Rad2Deg;
        
        print("Platform at Frame " + Time.frameCount + " moves by " + deltaRotation.y);
    }
}
