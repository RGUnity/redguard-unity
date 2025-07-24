using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMain: MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private PlayerMovementConfig config;

    // General CC properties
    private PlayerMovementStates _currentMovementState = PlayerMovementStates.Walking;
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _feetVisiblyGrounded;
    private Vector3 _playerRootPosition;

    // Surface information
    private float _surfaceAngleSphere;
    private float _surfaceAngleRay;
    private Vector3 _surfaceDownhill;
    private Vector3 _forwardOnSurface;
    private Vector3 _rightOnSurface;

    private bool _isOnScriptedObject;
    private float _pointRotation;

    // Other variables
    private InputManager _input;
    private Vector3 _smoothVelocity;
    private float _speed;
    private RGScriptedObject _currentScriptedGround;
    private Vector3 _ledgeTargetPosition;
    private bool _isClimbingUpLedge;
    private Vector3 _ledgeWallNormal;

    private void Start()
    {
        _input = LocalScene.inputManager;
    }

    private void FixedUpdate()
    {
        // This shows where the feet of the player are currently in the 3D space.
        float playerRootHeight = transform.position.y - cc.height / 2 + cc.center.y;
        _playerRootPosition = new Vector3(transform.position.x, playerRootHeight, transform.position.z);

        GroundCheck();
        HandleStepOffset();

        // Airborne State Entry and Exit
        if (!_isGrounded)
        {
            _currentMovementState = PlayerMovementStates.Airborne;
        }
        
        if (_input.jump
            && _currentMovementState == PlayerMovementStates.Walking)
        {
            _input.jump = false;
            _currentMovementState = PlayerMovementStates.Airborne;
            Jump();
        }
        else if (_input.jump)
        {
            _input.jump = false;
        }
        
        if (_isGrounded 
            && _currentMovementState == PlayerMovementStates.Airborne)
        {
            _currentMovementState = PlayerMovementStates.Walking;
        }
        
        // Slide state Entry and Exit
        if (_isGrounded
            && _feetVisiblyGrounded
            && _surfaceAngleSphere > cc.slopeLimit
            && _surfaceAngleRay > cc.slopeLimit)
        {
            _currentMovementState = PlayerMovementStates.Sliding;
        }
        else if (_isGrounded
                 && _feetVisiblyGrounded
                 && _surfaceAngleSphere <= cc.slopeLimit
                 && _surfaceAngleRay <= cc.slopeLimit)
        {
            _currentMovementState = PlayerMovementStates.Walking;
        }
        
        // Climbing State Entry and Exit
        if (_currentMovementState == PlayerMovementStates.Airborne)
        {
            if (IsNearHighLedge())
            {
                _velocity = Vector3.zero;
                _currentMovementState = PlayerMovementStates.Climbing;
            }
        }
        
        if (_currentMovementState == PlayerMovementStates.Climbing 
            && _input.dropDown)
        {
            _currentMovementState = PlayerMovementStates.Walking;
            _velocity = Vector3.zero;
            _smoothVelocity = Vector3.zero;
            // Todo: Disable the ledge detection for a short time to avoid the player getting stuck
            // OR shorten the ledge detection ray.
        }
        else if (_currentMovementState == PlayerMovementStates.Climbing 
                 && _input.climbUp)
        {
            PullUpLedge();
            // Todo: Check if the player has enough space to climb up
        }
        
        // Switch to desired state
        switch (_currentMovementState)
        {
            case PlayerMovementStates.Walking:
                WalkMode();
                break;
            case PlayerMovementStates.Airborne:
                AirborneMode();
                break;
            case PlayerMovementStates.Climbing:
                ClimbMode();
                break;
            case PlayerMovementStates.Sliding:
                SlideMode();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Apply the velocity to the CC
        cc.Move(_velocity);

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        // This ray represents the direction velocity
        //Debug.DrawRay(transform.position - Vector3.up, cc.velocity.normalized, Color.green);

        // This is mainly a readonly variable to validate the resulting movement speed
        _speed = cc.velocity.magnitude;
    }
    
    private void WalkMode()
    {
        // A debug ray pointing in Cyrus forward direction
        //Debug.DrawRay(transform.position + Vector3.down, _forwardOnSurface, Color.yellow);
        
        // Movement when moveModifier is pressed / is true
        if (_input.moveModifier)
        {
            if (_input.move.y != 0)
            {
                // Walk forward or backwards
                _velocity = _forwardOnSurface * (_input.move.y * config.walkSpeed / 60);

                // Turn the player
                float yRotation = _input.move.x * config.turnSpeed * Time.deltaTime * 60;
                transform.Rotate(0, yRotation, 0);
            }
            else if (_input.move.x != 0
                     && _input.move.y == 0)
            {
                // Strafe left or right
                _velocity = _rightOnSurface * (_input.move.x * config.walkSpeed / 60);
            }
            else
            {
                _velocity = Vector3.zero;
            }
        }
        // Movement when moveModifier is NOT pressed / is false
        else
        {
            // Run Forward
            if (_input.move.y > 0)
            {
                _velocity = _forwardOnSurface * (_input.move.y * config.runSpeed / 60);
            }
            // Walk backwards
            else if (_input.move.y < 0)
            {
                _velocity = _forwardOnSurface * (_input.move.y * config.walkSpeed / 60);
            }
            else
            {
                _velocity = Vector3.zero;
            }

            // Turn the player
            float yRotation = _input.move.x * config.turnSpeed * Time.deltaTime * 60;
            transform.Rotate(0, yRotation, 0);
        }
        
        // Apply ground magnet
        _velocity.y += config.groundMagnet;
        
        // Apply smoothing with MoveTowards
        _smoothVelocity = Vector3.MoveTowards(_smoothVelocity, _velocity, (1 / config.velocitySmoothing));
        _velocity.z = _smoothVelocity.z;
        _velocity.x = _smoothVelocity.x;
        
        // Stay on moving and rotating platforms
        StickToPlatforms();
        
        // Step dropping
        if (_isGrounded
            && !_feetVisiblyGrounded
            && _velocity.magnitude < 1
            && cc.velocity.y < 0.01)
        {
            ForceDropDown();
        }
    }

    private void AirborneMode()
    {
        // Apply gravity
        _velocity.y += config.gravity / 1000;
    }
    
    private void ClimbMode()
    {
        // Cast a sphere from the player to the _ledgeTargetPosition
        if (Physics.SphereCast(transform.position, 0.1f, _ledgeTargetPosition - transform.position, out RaycastHit hit, 2f, config.groundLayers))
        {
            Vector3 newNormal = hit.normal;
            // Lerp it to reduce jitters
            _ledgeWallNormal = Vector3.Lerp(_ledgeWallNormal, newNormal, 0.2f);
            Debug.DrawRay(transform.position, _ledgeWallNormal, Color.magenta);
        }

        // Player should always be facing the wall
        transform.rotation = Quaternion.LookRotation(-_ledgeWallNormal, transform.up);
            
        // Calculate movement direction on ledge
        Vector3 wallRight = Vector3.Cross(transform.up, -_ledgeWallNormal);

        // Move on ledge
        if (_input.move.x > 0 && CanClimbRight())
        {
            _velocity = wallRight - _ledgeWallNormal;
        }
        else if (_input.move.x < 0 && CanClimbLeft())
        {
            _velocity = -wallRight - _ledgeWallNormal;
        }

        // Keep the value positive and also hammer it towards full numbers
        float moveDir = Mathf.Sign(Mathf.Abs(_input.move.x));
        _velocity *= moveDir * 0.0334f * config.ledgeStrafeSpeed;
    }

    private void SlideMode()
    {
        // Add velocity in the downhill direction
        _velocity = _surfaceDownhill * config.slopeSlideSpeed;

        // Rotate the player towards the slideDirection
        Vector3 flatSurfaceDownhill = new Vector3(_surfaceDownhill.x, 0, _surfaceDownhill.z);

        //Debug.DrawRay(transform.position + Vector3.down, flatSurfaceDownhill, Color.blue);
        Quaternion stepRotation = Quaternion.LookRotation(flatSurfaceDownhill, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, stepRotation, config.slopeAlignmentSpeed * Time.deltaTime);
    }
    

    // There is a bug in Unity's CC where it ignores the step limit and jumps upwards when walking near some edges.
    // Here is a forum thread that explains more and also offers a workaround, which is implemented below.
    // Link: https://forum.unity.com/threads/character-controller-unexpected-step-offset-behaviour.640828/

    private void HandleStepOffset()
    {
        float distance = cc.radius + cc.skinWidth + config.rayDistance;
        Vector3 targetDirection = _forwardOnSurface * _input.move.y + transform.right * _input.move.x;
        
        //Raycast at player's ground level in direction of movement
        bool bottomRaycast = Physics.Raycast(_playerRootPosition, targetDirection, out _, distance);
        
        //Raycast at player's ground level + StepOffset in direction of movement
        Vector3 bottomWithStepOffset = _playerRootPosition + new Vector3(0,config.stepOffset, 0);
        bool upperRaycast = Physics.Raycast(bottomWithStepOffset, targetDirection, out _, distance/2);
        
        if (bottomRaycast && upperRaycast)
        {
            // When both the lower and high ray hit something, we are probably in front of a wall.
            // In this case, set stepOffset to 0 to avoid the player glitching over it
            cc.stepOffset = 0;
        }
        else if (bottomRaycast && !upperRaycast)
        {
            // if only the lower ray hits something, we are probably in front of a step.
            // In this case, set stepOffset to the desired value
            cc.stepOffset = config.stepOffset;
        }
        else
        {
            // In all other cases, set stepOffset to 0 just to be sure
            cc.stepOffset = 0;
        }
    }

    private void Jump()
    {
        if (IsNearLowLedge())
        {
            PullUpLedge();
            return;
        }

        if (_input.move.y > 0
            && _speed > config.longJumpThreshold)
        {
            // Long forward jump
            _velocity = _forwardOnSurface * config.longJumpDistance / 10;
            _velocity.y = config.jumpHeight / 10;
            _isGrounded = false;
        }
        else if (_input.move.y > 0
                && _speed > config.shortJumpThreshold)
        {
            // Short forward jump
            _velocity = _forwardOnSurface * config.shortJumpDistance / 10;
            _velocity.y = config.jumpHeight / 10;
            _isGrounded = false;
        }
        else
        {
            // Standing jump
            _velocity = Vector3.zero;
            _smoothVelocity = Vector3.zero;
            _velocity.y = config.jumpHeight / 10;
            _isGrounded = false;
        }
    }

    private void GroundCheck()
    {
        Vector3 surfaceDownhillSphere = default;
        Vector3 surfaceDownhillRay = default;

        if (Physics.SphereCast(transform.position, 0.34f, Vector3.down, out RaycastHit sphereHit, 0.7f, config.groundLayers))
        {
            _isGrounded = true;
            _surfaceAngleSphere = Vector3.Angle(Vector3.up, sphereHit.normal);
            surfaceDownhillSphere = Vector3.Cross((Vector3.Cross(Vector3.up, sphereHit.normal)), sphereHit.normal).normalized;

            if (sphereHit.transform.TryGetComponent(out RGScriptedObject platform))
            {
                _currentScriptedGround = platform;
                _isOnScriptedObject = true;
                _currentScriptedGround.playerStanding = true;
            }
            else
            {
                if(_currentScriptedGround != null)
                    _currentScriptedGround.playerStanding = false;
                _currentScriptedGround = null;
                _isOnScriptedObject = false;
            }
        }
        else
        {
            _isGrounded = false;

            if(_currentScriptedGround != null)
                _currentScriptedGround.playerStanding = false;
            _currentScriptedGround = null;
            _isOnScriptedObject = false;
        }

        //Debug.DrawRay(transform.position, Vector3.down * 1.7f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1.7f, config.groundLayers))
        {
            _surfaceAngleRay = Vector3.Angle(Vector3.up, rayHit.normal);
            surfaceDownhillRay = Vector3.Cross((Vector3.Cross(Vector3.up, rayHit.normal)), rayHit.normal).normalized;
            _forwardOnSurface = Vector3.Cross(transform.right, rayHit.normal).normalized;
            _rightOnSurface = Vector3.Cross(rayHit.normal, _forwardOnSurface);

            _surfaceDownhill = Vector3.Normalize(surfaceDownhillSphere + surfaceDownhillRay);
        }
        else
        {
            // These are fallback vectors in case the Raycast doesn't hit anything
            _forwardOnSurface = transform.forward;
            _rightOnSurface = transform.right;

            _surfaceDownhill = surfaceDownhillSphere;
        }

        // Overhang check - returns true if the players feet are not visibly touching ground
        if (Physics.SphereCast(transform.position, 0.15f, Vector3.down, out RaycastHit sphereHit2, 1f, config.groundLayers))
        {
            _feetVisiblyGrounded = true;
        }
        else
        {
            _feetVisiblyGrounded = false;
        }
    }

    private void ForceDropDown()
    {
        // Add velocity in the downhill direction
        _surfaceDownhill += Vector3.down * 2;
        _velocity = _surfaceDownhill.normalized * (config.slopeSlideSpeed*2);
    }

    private void StickToPlatforms()
    {
        /*
        // Moving platforms
        if (_isOnScriptedObject)
        {
            // Get the Velocity of the platform and add it to the player
            _velocity += _currentScriptedGround.linearVelocity;

            // Get the rotation of the platform and rotate the player with it
            Vector3 newPosition = RotateVectorAroundTransform(transform.position, _currentScriptedGround.rotationRoot, Vector3.up, _currentScriptedGround.angularVelocityY);

            Vector3 movementVector = newPosition - transform.position;
            Debug.DrawRay(transform.position, movementVector * 10, Color.magenta);
            _velocity += movementVector;
            transform.Rotate(transform.up, _currentScriptedGround.angularVelocityY);
        }
        */
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

    // Cyrus can climb directly over low ledges
    private bool IsNearLowLedge()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 raycastOrigin = playerRootPosition + transform.TransformVector(config.centerLedgeRaycastOrigin);
        float raycastLength = config.centerLedgeRaycastOrigin.y - config.lowLedgeStart;

        //Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.magenta);
        if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hitLow, raycastLength, config.groundLayers))
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

    // Cyrus can jump and hang on to high ledges
    private bool IsNearHighLedge()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 raycastOrigin = playerRootPosition + transform.TransformVector(config.centerLedgeRaycastOrigin);
        float raycastLength = config.centerLedgeRaycastOrigin.y - config.highLedgeStart;

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.red);

        // This is the center raycast that starts ledge climbing
        if (Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, config.groundLayers))
        {
            _ledgeTargetPosition = hitHigh.point;
            Vector3 surfaceNormal = hitHigh.normal;
            float angleCheck = Vector3.Dot(surfaceNormal, transform.up);
            // A perfectly even  ground has angleCheck = 1
            if (angleCheck > 0.9f
                && !_isGrounded)
            {
                return true;
            }
        }
        return false;
    }

    private void PullUpLedge()
    {
        cc.enabled = false;
        transform.position = _ledgeTargetPosition + transform.forward / 4 + Vector3.up;
        cc.enabled = true;
        _velocity = Vector3.zero;
    }
    
    // Check if there is space to climb to the left
    private bool CanClimbLeft()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 leftRaycastOrigin = playerRootPosition + transform.TransformVector(config.leftLedgeRaycastOrigin);
        float raycastLength = config.centerLedgeRaycastOrigin.y - config.highLedgeStart;
        Debug.DrawRay(leftRaycastOrigin, Vector3.down * raycastLength, Color.red);
        
        if (Physics.Raycast(leftRaycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, config.groundLayers))
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
    
    // Check if there is space to climb to the right
    private bool CanClimbRight()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 rightRaycastOrigin = playerRootPosition + transform.TransformVector(config.rightLedgeRaycastOrigin);
        float raycastLength = config.centerLedgeRaycastOrigin.y - config.highLedgeStart;
        Debug.DrawRay(rightRaycastOrigin, Vector3.down * raycastLength, Color.red);
        
        if (Physics.Raycast(rightRaycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, config.groundLayers))
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
}
