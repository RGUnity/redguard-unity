using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MovingPlatform : MonoBehaviour
{   
    [Header("Global Settings")]
    public Transform movementRoot;
    public Transform rotationRoot;
    
    [Header("Moving Platform")]
    [SerializeField] private bool allowMovement;
    
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform moveTargetA;
    [SerializeField] private Transform moveTargetB;
    
    [Header("Rotating Platform")]
    [SerializeField] private bool allowRotation;
    [SerializeField] private float rotationSpeed = 1;

    [HideInInspector] public Vector3 linearVelocity;
    [HideInInspector] public float angularVelocityY;
    
    private Transform _currentTarget;
    
    // This script must be higher than PlayerMovement in the script execution order...
    // ...otherwise the player's movement will lag behind by one frame 
    
    
    private void Start()
    {
        if (allowMovement
            && moveTargetA != null
            && moveTargetB != null
            && moveSpeed > 0)
        {
            _currentTarget = moveTargetA;
        }
        else
        {
            allowMovement = false;
        }

        if (allowRotation
            && rotationSpeed > 0)
        {
            
        }
        else
        {
            allowRotation = false;
        }
    }
    
    private void FixedUpdate()
    {
        if (allowMovement)
        {
            if (movementRoot.position == moveTargetA.position)
            {
                _currentTarget = moveTargetB;
            }
            else
            {
                if (movementRoot.position == moveTargetB.position)
                {
                    _currentTarget = moveTargetA;
                }
            }
            MovePlatform();
        }

        if (allowRotation)
        {
            RotatePlatform();
        }
    }
    
    private void MovePlatform()
    {
        Vector3 newPosition = Vector3.MoveTowards(movementRoot.position, _currentTarget.position, moveSpeed/100);
        
        // linearVelocity is used by the player to move along with platforms
        linearVelocity = newPosition - movementRoot.position;
        
        movementRoot.position = newPosition;
    }

    private void RotatePlatform()
    {
        // angularVelocityY is used by the player to rotate with platforms
        angularVelocityY = rotationSpeed;
        
        rotationRoot.Rotate(Vector3.up, rotationSpeed);
    }
}
