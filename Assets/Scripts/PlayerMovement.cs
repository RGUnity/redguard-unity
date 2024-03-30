using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private CharacterController cc;
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

    [Header("Ledge Climbing")] 
    [SerializeField] private Vector3 localRaycastOrigin = new Vector3(0, 2.25f, 0.25f);
    [SerializeField] private float highLedgeStart = 2.1f;
    [SerializeField] private float lowLedgeStart = 0.9f;
    
    
    // General CC properties
    private bool _isGrounded;
    private bool _isSliding; 
    private Vector3 _velocity;
    
    // Surface information
    private Vector3 _surfaceNormal;
    private float _slopeAngle;
    private Vector3 _surfaceContact;
    private Vector3 _surfaceDownhill;
    private Vector3 _forwardOnSurface;
    private Vector3 _rightOnSurface;
    
    private bool _isOnMovingPlatform; 
    private float _pointRotation;
    
    // Other variables
    private InputManager _input;
    private Vector3 _smoothVelocity;
    private float _speed;
    private TEST_MovingPlatform _currentPlatform;
    private bool _inFrontOfLowLedge;
    private bool _inFrontOfHighLedge;
    private Vector3 _ledgeTargetPosition;
    private bool _isClimbingUpLedge;
    private bool _isHanginOnLedge;
    private Vector3 _playerRootPosition;
    


    private void Start()
    {
        _input = LocalScene.inputManager;
    }

    private void FixedUpdate()
    {
        float playerRootHeight = transform.position.y - cc.height / 2 + cc.center.y - cc.skinWidth;
        _playerRootPosition = new Vector3(transform.position.x, playerRootHeight, transform.position.z);
        
        GroundCheck();
        
        // Slope sliding
        if (_isGrounded
            && _slopeAngle > cc.slopeLimit)
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
            //Debug.DrawRay(_surfaceContact, _forwardOnSurface, Color.yellow);
            
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
                _input.jump = false;
            }
        }
        
        if (!_isGrounded)
        {
            
            if (cc.velocity.y > 0)
            {
                // Apply gravity when jumping up
                _velocity.y += gravity / 1000;
            }
            else
            {
                // Apply gravity when falling down
                _velocity.y += gravity / 1000 * fallSpeedMultiplier;
            }

            if (IsNearHighLedge())
            {
                AttachToLedge();
            }
        }
        else
        {
            StickToPlatforms();
            
            // Apply ground magnet
            _velocity.y += groundMagnet;
        }
        
        // Apply the velocity to the CC  
        cc.Move(_velocity);
        
        //Debug.DrawRay(transform.position - Vector3.up, characterController.velocity.normalized, Color.green);
        
        // I think this is in meters per second?
        _speed = cc.velocity.magnitude;
    }

    private void WalkMode()
    {
        if (_input.move.y != 0)
        {
            // Walk forward or backwards
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed /60);
                    
            // Turn the player
            float yRotation = _input.move.x * turnSpeed* Time.deltaTime * 60;
            transform.Rotate(0, yRotation, 0);
        }
        else if (_input.move.x != 0
                 && _input.move.y == 0)
        {
            // Strafe left or right
            _velocity = _rightOnSurface * (_input.move.x * walkSpeed /60);
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
            _velocity = _forwardOnSurface * (_input.move.y * runSpeed /60);
        }
        // Walk backwards
        else if (_input.move.y < 0)
        {
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed /60);
        }
        else
        {
            _velocity = Vector3.zero;
        }
                
        // Turn the player
        float yRotation = _input.move.x * turnSpeed * Time.deltaTime * 60;
        transform.Rotate(0, yRotation, 0);
    }
    
    private void Jump()
    {
        if (IsNearLowLedge())
        {
            ClimbLowerLedge();
            return;
        }
        
        if (_input.move.y > 0
            && _speed > longJumpThreshold)
        {
            // Long forward jump 
            _velocity = _forwardOnSurface * longJumpDistance /10;
            _velocity.y = jumpHeight /10;
            _isGrounded = false;
        }
        else if(_input.move.y > 0
                && _speed > shortJumpThreshold)
        {
            // Short forward jump
            _velocity = _forwardOnSurface * shortJumpDistance /10;
            _velocity.y = jumpHeight /10;
            _isGrounded = false;
        }
        else
        {
            // Standing jump
            _velocity = Vector3.zero;
            _smoothVelocity = Vector3.zero;
            _velocity.y = jumpHeight /10;
            _isGrounded = false;
        }
    }

    private void GroundCheck()
    {
        Vector3 surfaceDownhill1 = default;
        Vector3 surfaceDownhill2 = default;

        
        if (Physics.SphereCast(transform.position, 0.34f, Vector3.down, out RaycastHit sphereHit, 0.7f, groundLayers))
        {
            _isGrounded = true;
            surfaceDownhill1 = Vector3.Cross((Vector3.Cross( Vector3.up, _surfaceNormal)), _surfaceNormal).normalized;
            
            if (sphereHit.collider.CompareTag("Moving Platform"))
            {
                if (sphereHit.transform.TryGetComponent(out TEST_MovingPlatform platform))
                {
                    _currentPlatform = platform;
                    _isOnMovingPlatform = true;
                }
            }
            else
            {
                _currentPlatform = null;
                _isOnMovingPlatform = false;
            }
        }
        else
        {
            _isGrounded = false;
            
            _currentPlatform = null;
            _isOnMovingPlatform = false;
        }
        
        // These vectors are used to apply velocity perpendicular to the surface normal
        //Debug.DrawRay(transform.position, Vector3.down * 1.7f, Color.red);   
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1.7f, groundLayers))
        {
            _forwardOnSurface = Vector3.Cross(transform.right, rayHit.normal).normalized;
            _rightOnSurface = Vector3.Cross(_surfaceNormal, _forwardOnSurface);
        }
        else
        {
            // These are fallback vectors in case the Raycast doesn't hit anything
            _forwardOnSurface = transform.forward;
            _rightOnSurface = transform.right;
        }

        Vector3 highestHit = Vector3.down * 1.7f;
        
        // Forward-left raycast
        Vector3 origin1 = _playerRootPosition + (transform.forward - transform.right) * 0.13f; 
        Debug.DrawRay(origin1, Vector3.down * 1.7f, Color.yellow);
        if (Physics.Raycast(origin1, Vector3.down, out RaycastHit rayHit1, 1.7f, groundLayers))
        {
            // The first one starts as the highest hit by default
            surfaceDownhill2 = Vector3.Cross((Vector3.Cross( Vector3.up, rayHit1.normal)), rayHit1.normal).normalized;;
            _surfaceNormal = rayHit1.normal;
            highestHit = rayHit1.point;
        }
        
        // Forward-right raycast
        Vector3 origin2 = _playerRootPosition + (transform.forward + transform.right) * 0.13f; 
        Debug.DrawRay(origin2, Vector3.down * 1.7f, Color.yellow);
        if (Physics.Raycast(origin2, Vector3.down, out RaycastHit rayHit2, 1.7f, groundLayers))
        {
            if (rayHit2.point.y > highestHit.y)
            {
                surfaceDownhill2 = Vector3.Cross((Vector3.Cross( Vector3.up, rayHit2.normal)), rayHit2.normal).normalized;
                _surfaceNormal = rayHit2.normal;
                highestHit = rayHit2.point;
            }
        }
        
        // Backwards-left raycast
        Vector3 origin3 = _playerRootPosition + (-transform.forward - transform.right) * 0.13f; 
        Debug.DrawRay(origin3, Vector3.down * 1.7f, Color.yellow);
        if (Physics.Raycast(origin3, Vector3.down, out RaycastHit rayHit3, 1.7f, groundLayers))
        {
            if (rayHit3.point.y > highestHit.y)
            {
                surfaceDownhill2 = Vector3.Cross((Vector3.Cross( Vector3.up, rayHit3.normal)), rayHit3.normal).normalized;
                _surfaceNormal = rayHit3.normal;
                highestHit = rayHit3.point;
            }
        }
        
        // Backwards-right raycast
        Vector3 origin4 = _playerRootPosition + (-transform.forward + transform.right) * 0.13f; 
        Debug.DrawRay(origin4, Vector3.down * 1.7f, Color.yellow);
        if (Physics.Raycast(origin4, Vector3.down, out RaycastHit rayHit4, 1.7f, groundLayers))
        { 
            if (rayHit4.point.y > highestHit.y)
            {
                surfaceDownhill2 = Vector3.Cross((Vector3.Cross( Vector3.up, rayHit4.normal)), rayHit4.normal).normalized;
                _surfaceNormal = rayHit4.normal;
                highestHit = rayHit4.point;
            }
        }
        
        _slopeAngle = Vector3.Angle(Vector3.up, _surfaceNormal);
        _surfaceDownhill = Vector3.Normalize(surfaceDownhill1 + surfaceDownhill2);
        //_surfaceDownhill = Vector3.Normalize(surfaceDownhill1);
    }
    
    private void SlideDownhill()
    {
        // This Vector points downhill
        Debug.DrawRay(_surfaceContact, _surfaceDownhill, Color.blue);
        
        _velocity = _surfaceDownhill * slopeSlideSpeed;
        
        // Rotate the player towards the slideDirection
        Vector3 flatSurfaceDownhill = new Vector3(_surfaceDownhill.x, 0, _surfaceDownhill.z);
        
        Quaternion stepRotation = Quaternion.LookRotation(flatSurfaceDownhill, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, stepRotation, slopeAlignmentSpeed * Time.deltaTime);
    }

    private void StickToPlatforms()
    {
        // Moving platforms
        if (_isOnMovingPlatform)
        {
            // Get the Velocity of the platform and add it to the player
            _velocity += _currentPlatform.linearVelocity;
            
            // Get the rotation of the platform and rotate the player with it
            Vector3 newPosition = RotateVectorAroundTransform(transform.position, _currentPlatform.rotationRoot, Vector3.up, _currentPlatform.angularVelocityY);
         
            Vector3 movementVector = newPosition - transform.position;
            Debug.DrawRay(transform.position, movementVector*10, Color.magenta);
            _velocity += movementVector;
            transform.Rotate(transform.up, _currentPlatform.angularVelocityY);
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

    private bool IsNearLowLedge()
    {
        Vector3 raycastOrigin = _playerRootPosition + transform.TransformVector(localRaycastOrigin);
        float raycastLength = localRaycastOrigin.y - lowLedgeStart;
        
        //Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.magenta);
        if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hitLow, raycastLength, groundLayers))
        {
            _ledgeTargetPosition = hitLow.point;
            Vector3 ledgeSurfaceNormal = hitLow.normal;
            if (Vector3.Dot(ledgeSurfaceNormal, transform.up) > 0.5f)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    
    private bool IsNearHighLedge()
    {
        Vector3 raycastOrigin = _playerRootPosition + transform.TransformVector(localRaycastOrigin);
        float raycastLength = localRaycastOrigin.y - highLedgeStart;
        
        //Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.red);
        if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, groundLayers))
        {
            _ledgeTargetPosition = hitHigh.point;
            Vector3 surfaceNormal = hitHigh.normal;
            if (Vector3.Dot(surfaceNormal, transform.up) > 0.5f
                && !_isGrounded)
            {
                return true;
            }
        }
        return false;
    }

    private void ClimbLowerLedge()
    {
        cc.enabled = false;
        transform.position = _ledgeTargetPosition + transform.forward/4 + Vector3.up;
        cc.enabled = true;
        _velocity = Vector3.zero;
    }

    private void AttachToLedge()
    {
        _isHanginOnLedge = true;
        print("Attach to ledge pls");
    }
}

