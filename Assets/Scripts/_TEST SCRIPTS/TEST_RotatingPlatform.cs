using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_RotatingPlatform : MonoBehaviour
{   
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateAroundAxis(Vector3.up, speed * Time.fixedDeltaTime);
    }
    
    void RotateAroundAxis(Vector3 axis, float angle)
    {
        angle = angle * Mathf.Deg2Rad; // Convert angle to radians
        Quaternion deltaRotation = Quaternion.AngleAxis(angle, axis);
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(targetRotation);
    }
}
