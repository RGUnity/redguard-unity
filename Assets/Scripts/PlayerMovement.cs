using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float groundMagnet = -4f;
    
    [Header("Basic Movement")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float velocitySmoothing = 12;
    [SerializeField] private float turnSpeed = 3.7f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float fallSpeedMultiplier = 2f;
    [SerializeField] private float longJumpDistance = 6f;
    [SerializeField] private float shortJumpDistance = 4f;
    [SerializeField] private float longJumpThreshold = 2.8f;
    [SerializeField] private float shortJumpThreshold = 0.5f;
    
    [Header("Slope Sliding")]
    [SerializeField] private float slopeSlideSpeed = 5f;
    [SerializeField] private float slopeAlignmentSpeed = 3f;
    
    [FormerlySerializedAs("_movingPlatformTag")]
    [Header("Moving Platforms")]
    [SerializeField] private string movingPlatformTag;
    [SerializeField] private string rotatingPlatformTag;
    
    // General CC properties
    private bool _isGrounded;
    private bool _isSliding; 
    private Vector3 _velocity;
    
    // Surface information
    private float _surfaceAngle;
    private Vector3 _surfaceNormal;
    private Vector3 _surfaceContact;
    private Vector3 _surfaceDownhill;
    private Vector3 _forwardOnSurface;
    private Vector3 _rightOnSurface;
    
    private bool _isOnMovingPlatform; 
    private bool _isOnRotatingPlatform; 
    private float _pointRotation;
    
    // Other variables
    private InputManager _input;
    private Vector3 _smoothVelocity;
    private float _speed;
    private Transform _currentPlatform;

    private float maxHeight;


    private void Start()
    {
        _input = LocalScene.inputManager;
    }

    private void FixedUpdate()
    {
        GroundCheck();
        GetSurfaceVectors();
        
        // Slope sliding
        if (_surfaceAngle > characterController.slopeLimit)
        {
            _isSliding = true;
            SlideDownhill();
        }
        else
        {
            _isSliding = false;
        }

        // Basic movement
        if (_isGrounded && !_isSliding)
        {
            Debug.DrawRay(_surfaceContact, _forwardOnSurface, Color.yellow);
            
            // Movement when moveModifier is pressed / is true
            if (_input.moveModifier)
            {
                WalkMode();
            }
            // Movement when moveModifier is NOT pressed / is false
            else
            {
                RunMode();
            }
            
            // Apply smoothing with MoveTowards
            _smoothVelocity = Vector3.MoveTowards(_smoothVelocity, _velocity, (1/velocitySmoothing));
            _velocity.z = _smoothVelocity.z;
            _velocity.x = _smoothVelocity.x;
            
            if (_input.jump)
            {
                Jump();
                maxHeight = 0;
            }
        }

        // Y Speed
        if (!_isGrounded)
        {
            if (characterController.velocity.y > 0)
            {
                // Apply gravity
                _velocity.y += gravity / 1000;
                //print(_velocity.y);
            }
            else
            {
                // Apply gravity
                _velocity.y += gravity / 1000 * fallSpeedMultiplier;
            }
        }
        else
        {
            // Apply ground magnet
            _velocity.y += groundMagnet;
        }
        
        
        // Stick to moving and rotating platforms
        StickToPlatforms();
        
        
        // Move the controller
        characterController.Move(_velocity);
        
        // I think this is in meters per second?
        _speed = characterController.velocity.magnitude;

        if (transform.position.y > maxHeight)
        {
            maxHeight = transform.position.y;
            print(maxHeight);
        }
    }

    private void WalkMode()
    {
        if (_input.move.y != 0)
        {
            // Walk forward or backwards
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed);
                    
            // Turn the player
            float yRotation = _input.move.x * turnSpeed/2 * Time.deltaTime;
            transform.Rotate(0, yRotation, 0);
        }
        else if (_input.move.x != 0
                 && _input.move.y == 0)
        {
            // Strafe left or right
            _velocity = _rightOnSurface * (_input.move.x * walkSpeed);
        }
        else
        {
            _velocity = Vector3.zero;
        }
    }
    
    private void RunMode()
    {
        // Run Forward
        if (_input.move.y > 0)
        {
            _velocity = _forwardOnSurface * (_input.move.y * runSpeed);
        }
        // Walk backwards
        else if (_input.move.y < 0)
        {
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed);
        }
        else
        {
            _velocity = Vector3.zero;
        }
                
        // Turn the player
        float yRotation = _input.move.x * turnSpeed/2 * Time.deltaTime * 60;
        transform.Rotate(0, yRotation, 0);
    }
    
    private void Jump()
    {
        if (_input.move.y > 0
            && _speed > longJumpThreshold)
        {
            // Long forward jump 
            _velocity = _forwardOnSurface * longJumpDistance;
            _velocity.y = jumpHeight;
            _isGrounded = false;
            print("Long jump");
        }
        else if(_input.move.y > 0
                && _speed > shortJumpThreshold)
        {
            // Short forward jump
            _velocity = _forwardOnSurface * shortJumpDistance;
            _velocity.y = jumpHeight;
            _isGrounded = false;
            print("Short jump");
        }
        else
        {
            // Standing jump
            _velocity = Vector3.zero;
            _smoothVelocity = Vector3.zero;
            _velocity.y = jumpHeight;
            _isGrounded = false;
            
            print("Standing jump");
        }
    }

    private void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, 0.18f, Vector3.down, out RaycastHit hit, 0.85f, groundLayers))
        {
            _isGrounded = true;
            
            if (hit.collider.CompareTag(movingPlatformTag))
            {
                _isOnMovingPlatform = true;
                _isOnRotatingPlatform = false;
                _currentPlatform = hit.transform;
            }
            else if (hit.collider.CompareTag(rotatingPlatformTag))
            {
                _isOnMovingPlatform = false;
                _isOnRotatingPlatform = true;
                _currentPlatform = hit.transform;
            }
            else
            {
                _isOnMovingPlatform = false;
                _isOnRotatingPlatform = false;
                _currentPlatform = null;
            }
            
        }
        else
        {
            _isGrounded = false;
            
            _isOnMovingPlatform = false;
            _isOnRotatingPlatform = false;
            _currentPlatform = null;
        }
    }
    
    private void GetSurfaceVectors()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f, groundLayers))
        {
            _surfaceAngle = Vector3.Angle(Vector3.up, hit.normal);
            _surfaceNormal = hit.normal;
            _surfaceContact = hit.point;
            _surfaceDownhill = Vector3.Cross((Vector3.Cross( Vector3.up, _surfaceNormal)), _surfaceNormal).normalized;
            _forwardOnSurface = Vector3.Cross(transform.right, hit.normal).normalized;
            _rightOnSurface = Vector3.Cross(_surfaceNormal, _forwardOnSurface);
        }
    }
    
    private void SlideDownhill()
    {
        // This Vector points downhill
        Debug.DrawRay(_surfaceContact, _surfaceDownhill, Color.green);
        
        _velocity = _surfaceDownhill * slopeSlideSpeed;
        
        // Rotate the player towards the slideDirection
        Vector3 flatSurfaceDownhill = new Vector3(_surfaceDownhill.x, 0, _surfaceDownhill.z);
        
        Debug.DrawRay(_surfaceContact, flatSurfaceDownhill, Color.blue);
        Quaternion stepRotation = Quaternion.LookRotation(flatSurfaceDownhill, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, stepRotation, slopeAlignmentSpeed * Time.deltaTime);
    }

    private void StickToPlatforms()
    {
        // Moving platforms
        if (_isOnMovingPlatform)
        {
            if (_currentPlatform.parent.TryGetComponent(out TEST_MovingPlatform platform))
            {
                _velocity += platform.moveStep;
            }
        }
        
        // Rotating platforms
        if (_isOnRotatingPlatform)
        {
            if (_currentPlatform.parent.TryGetComponent(out TEST_RotatingPlatform platform))
            {
                //transform.RotateAround(_platformPosition, Vector3.up, _pointRotation);
                Vector3 newPosition = RotateVectorAroundTransform(transform.position, _currentPlatform, Vector3.up, platform.yStepRotation);
            
                Vector3 movementVector = newPosition - transform.position;
                //print("Current position: " + transform.position + ".  New position: " + newPosition);
                Debug.DrawRay(transform.position, movementVector*10, Color.magenta);
                _velocity += movementVector;
                
                transform.Rotate(transform.up, platform.yStepRotation);
            }
        }
    }
    
    public Vector3 RotateVectorAroundTransform(Vector3 vector, Transform transform, Vector3 axis, float angle)
    {
        // Step 1: Calculate the vector from the transform's position to the vector
        Vector3 vectorToRotate = vector - transform.position;

        // Step 2: Convert the vector from world space to local space
        Vector3 localVector = Quaternion.Inverse(transform.rotation) * vectorToRotate;

        // Step 3: Rotate the vector using the desired rotation
        Quaternion rotation = Quaternion.AngleAxis(angle, axis);
        Vector3 rotatedVector = rotation * localVector;

        // Step 4: Convert the rotated vector back to world space
        Vector3 worldRotatedVector = transform.rotation * rotatedVector;

        // Step 5: Add the rotated vector to the transform's position
        Vector3 finalPosition = transform.position + worldRotatedVector;

        return finalPosition;
    }
    

    
    private void OnControllerColliderHit(ControllerColliderHit hit )
    {
        // if (hit.collider.CompareTag(movingPlatformTag))
        // {
        //     _isOnMovingPlatform = true;
        //     _isOnRotatingPlatform = false;
        //     _currentPlatform = hit.transform;
        //     
        //     if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_MovingPlatform pf))
        //     {
        //         _pointMovement = pf.moveStep;
        //     }
        // }
        // else if (hit.collider.CompareTag(rotatingPlatformTag))
        // {
        //     _isOnMovingPlatform = false;
        //     _isOnRotatingPlatform = true;
        //     _currentPlatform = hit.transform;
        //     
        //     if (hit.rigidbody != null)
        //     {
        //         
        //         if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_RotatingPlatform pf))
        //         {
        //             _pointRotation = pf.yStepRotation;
        //         }
        //         _platformPosition = hit.rigidbody.transform.position;
        //     }
        // }
        // else
        // {
        //     _isOnMovingPlatform = false;
        //     _isOnRotatingPlatform = false;
        //     _currentPlatform = null;
        // }
    }
}

