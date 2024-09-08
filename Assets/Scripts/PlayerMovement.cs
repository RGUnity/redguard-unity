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

    [Header("Step Handling (Corner-Jumps Fix)")]
    [SerializeField] private float stepOffset = 0.9f;
    [SerializeField] private float rayDistance = 1f;

    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private float longJumpDistance = 6f;
    [SerializeField] private float shortJumpDistance = 4f;
    [SerializeField] private float longJumpThreshold = 2.8f;
    [SerializeField] private float shortJumpThreshold = 0.5f;

    [Header("Slope Sliding")]
    [SerializeField] private float slopeSlideSpeed = 5f;
    [SerializeField] private float slopeAlignmentSpeed = 3f;

    [FormerlySerializedAs("localRaycastOrigin")]
    [Header("Ledge Climbing")]
    [SerializeField] private Vector3 centerLedgeRaycastOrigin = new (0, 2.25f, 0.4f);
    [SerializeField] private Vector3 leftLedgeRaycastOrigin = new (-0.3f, 2.25f, 0.5f);
    [SerializeField] private Vector3 rightLedgeRaycastOrigin = new (0.3f, 2.25f, 0.5f);
    [SerializeField] private float highLedgeStart = 2.1f;
    [SerializeField] private float lowLedgeStart = 0.9f;

    // General CC properties
    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _feetVisiblyGrounded;
    private bool _allowInputs;
    private Vector3 _playerRootPosition;

    // Surface information
    private float _surfaceAngleSphere;
    private float _surfaceAngleRay;
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
    // private bool _inFrontOfLowLedge;
    // private bool _inFrontOfHighLedge;
    private Vector3 _ledgeTargetPosition;
    private bool _isClimbingUpLedge;
    private bool _isHangingOnLedge;
    private Vector3 _ledgeWallNormal;
    private float _defaultGravity;


    private void Start()
    {
        _input = LocalScene.inputManager;
        _defaultGravity = gravity;
    }

    private void FixedUpdate()
    {
        float playerRootHeight = transform.position.y - cc.height / 2 + cc.center.y;
        _playerRootPosition = new Vector3(transform.position.x, playerRootHeight, transform.position.z);

        GroundCheck();
        HandleStepOffset();

        // Slope sliding
        if (_isGrounded
            && _feetVisiblyGrounded
            && _surfaceAngleSphere > cc.slopeLimit
            && _surfaceAngleRay > cc.slopeLimit)
        {
            SlideDownhill();
            _allowInputs = false;
        }
        else
        {
            _allowInputs = true;
        }

        // Step dropping
        if (_isGrounded
            && _allowInputs
            && !_feetVisiblyGrounded
            && _velocity.magnitude < 1
            && cc.velocity.y < 0)
        {
            ForceDropDown();
            _allowInputs = false;
        }

        // Movement when grounded
        if (_isGrounded
            && _allowInputs
            && !_isHangingOnLedge)
        {
            Debug.DrawRay(transform.position + Vector3.down, _forwardOnSurface, Color.yellow);

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

            // Apply ground magnet
            _velocity.y += groundMagnet;

            // Apply smoothing with MoveTowards
            _smoothVelocity = Vector3.MoveTowards(_smoothVelocity, _velocity, (1 / velocitySmoothing));
            _velocity.z = _smoothVelocity.z;
            _velocity.x = _smoothVelocity.x;

            // Stay on moving and rotating platforms
            StickToPlatforms();

            // Jump
            if (_input.jump)
            {
                Jump();
                _input.jump = false;
            }
        }

        // Gravity
        if (!_isGrounded
            && !_isHangingOnLedge)
        {
            // Apply gravity when jumping up
            _velocity.y += gravity / 1000;
        }

        // High ledge detection
        if (!_isGrounded)
        {
            if (IsNearHighLedge())
            {
                _isHangingOnLedge = true;
                AttachToLedge();
            }
            else
            {
                _isHangingOnLedge = false;
                DetachFromLedge();
            }
        }

        // Movement when hanging on ledge
        if (!_isGrounded
            && _allowInputs
            && _isHangingOnLedge)
        {
            // Cast a ray from the player to the _ledgeTargetPosition
            if (Physics.Raycast(transform.position, _ledgeTargetPosition - transform.position, out RaycastHit hit, 2f, groundLayers))
            {
                _ledgeWallNormal = hit.normal;
                Debug.DrawRay(transform.position, _ledgeWallNormal, Color.magenta);
            }

            // Player should always be facing the wall
            transform.rotation = Quaternion.LookRotation(-_ledgeWallNormal, transform.up);

            print("Can climb left: " + CanClimbLeft() + ". Can climb right: " + CanClimbRight());
            
            // Move right on Ledge
            if (_input.move.x > 0
                && CanClimbRight())
            {
                Vector3 rightOnWall = Vector3.Cross(transform.up, -_ledgeWallNormal);
                _velocity = rightOnWall * (_input.move.x * 1f);
                _velocity = _velocity.normalized * 0.05f;
                _velocity += -_ledgeWallNormal.normalized * 0.05f;
            }
            // Move left on Ledge
            else if (_input.move.x < 0
                     && CanClimbLeft())
            {
                Vector3 rightOnWall = Vector3.Cross(transform.up, -_ledgeWallNormal);
                _velocity = rightOnWall * (_input.move.x * 1f);
                _velocity = _velocity.normalized * 0.05f;
                _velocity += -_ledgeWallNormal.normalized * 0.05f;
            }
            else
            {
                // do fuck all
            }
        }

        // Apply the velocity to the CC
        cc.Move(_velocity);

        Debug.DrawRay(transform.position - Vector3.up, cc.velocity.normalized, Color.green);

        // I think this is in meters per second?
        _speed = cc.velocity.magnitude;
    }

    private void WalkMode()
    {
        if (_input.move.y != 0)
        {
            // Walk forward or backwards
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed / 60);

            // Turn the player
            float yRotation = _input.move.x * turnSpeed * Time.deltaTime * 60;
            transform.Rotate(0, yRotation, 0);
        }
        else if (_input.move.x != 0
                 && _input.move.y == 0)
        {
            // Strafe left or right
            _velocity = _rightOnSurface * (_input.move.x * walkSpeed / 60);
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
            _velocity = _forwardOnSurface * (_input.move.y * runSpeed / 60);
        }
        // Walk backwards
        else if (_input.move.y < 0)
        {
            _velocity = _forwardOnSurface * (_input.move.y * walkSpeed / 60);
        }
        else
        {
            _velocity = Vector3.zero;
        }

        // Turn the player
        float yRotation = _input.move.x * turnSpeed * Time.deltaTime * 60;
        transform.Rotate(0, yRotation, 0);
    }

    // There is a bug in Unity's CC where it ignores the step limit and jumps upwards when walking near some edges.
    // Here is a forum thread that explains more and also offers a workaround, which is implemented below.
    // Link: https://forum.unity.com/threads/character-controller-unexpected-step-offset-behaviour.640828/

    private void HandleStepOffset()
    {
        float distance = cc.radius + cc.skinWidth + rayDistance;
        //Position of player's ground level + StepOffset
        Vector3 bottomWithStepOffset = _playerRootPosition + new Vector3(0, stepOffset, 0);
        //Raycast at player's ground level in direction of movement
        bool bottomRaycast = Physics.Raycast(_playerRootPosition, _forwardOnSurface, out _, distance);
        //Raycast at player's ground level + StepOffset in direction of movement
        bool bottomWithStepOffsetRaycast = Physics.Raycast(bottomWithStepOffset, _forwardOnSurface, out _, distance);
        if (bottomRaycast && bottomWithStepOffsetRaycast)
        {
            //Wall in move direction
            //Block stepping over object
            cc.stepOffset = 0;
        }
        else if (bottomRaycast && !bottomWithStepOffsetRaycast)
        {
            //Step in move direction
            //Allow stepping over object
            cc.stepOffset = stepOffset;
        }
        else
        {
            //Nothing in move direction
            //Block stepping over object
            cc.stepOffset = 0;
        }
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
            _velocity = _forwardOnSurface * longJumpDistance / 10;
            _velocity.y = jumpHeight / 10;
            _isGrounded = false;
        }
        else if (_input.move.y > 0
                && _speed > shortJumpThreshold)
        {
            // Short forward jump
            _velocity = _forwardOnSurface * shortJumpDistance / 10;
            _velocity.y = jumpHeight / 10;
            _isGrounded = false;
        }
        else
        {
            // Standing jump
            _velocity = Vector3.zero;
            _smoothVelocity = Vector3.zero;
            _velocity.y = jumpHeight / 10;
            _isGrounded = false;
        }
    }

    private void GroundCheck()
    {
        Vector3 surfaceDownhillSphere = default;
        Vector3 surfaceDownhillRay = default;

        if (Physics.SphereCast(transform.position, 0.34f, Vector3.down, out RaycastHit sphereHit, 0.7f, groundLayers))
        {
            _isGrounded = true;
            _surfaceAngleSphere = Vector3.Angle(Vector3.up, sphereHit.normal);
            surfaceDownhillSphere = Vector3.Cross((Vector3.Cross(Vector3.up, sphereHit.normal)), sphereHit.normal).normalized;

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

        Debug.DrawRay(transform.position, Vector3.down * 1.7f, Color.red);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit, 1.7f, groundLayers))
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
        if (Physics.SphereCast(transform.position, 0.15f, Vector3.down, out RaycastHit sphereHit2, 1.84f, groundLayers))
        {
            _feetVisiblyGrounded = true;
        }
        else
        {
            _feetVisiblyGrounded = false;
        }
    }

    private void SlideDownhill()
    {
        // Add velocity in the downhill direction
        _velocity = _surfaceDownhill * slopeSlideSpeed;

        // Rotate the player towards the slideDirection
        Vector3 flatSurfaceDownhill = new Vector3(_surfaceDownhill.x, 0, _surfaceDownhill.z);

        //Debug.DrawRay(transform.position + Vector3.down, flatSurfaceDownhill, Color.blue);
        Quaternion stepRotation = Quaternion.LookRotation(flatSurfaceDownhill, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, stepRotation, slopeAlignmentSpeed * Time.deltaTime);
    }

    private void ForceDropDown()
    {
        // Add velocity in the downhill direction
        _surfaceDownhill += Vector3.down * 2;
        _velocity = _surfaceDownhill.normalized * (slopeSlideSpeed);
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
            Debug.DrawRay(transform.position, movementVector * 10, Color.magenta);
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

    // Cyrus can climb directly over low ledges
    private bool IsNearLowLedge()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 raycastOrigin = playerRootPosition + transform.TransformVector(centerLedgeRaycastOrigin);
        float raycastLength = centerLedgeRaycastOrigin.y - lowLedgeStart;

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.magenta);
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

    // Cyrus can jump and hang on to high ledges
    private bool IsNearHighLedge()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 raycastOrigin = playerRootPosition + transform.TransformVector(centerLedgeRaycastOrigin);
        float raycastLength = centerLedgeRaycastOrigin.y - highLedgeStart;

        Debug.DrawRay(raycastOrigin, Vector3.down * raycastLength, Color.red);

        // This is the center raycast that starts ledge climbing
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
        transform.position = _ledgeTargetPosition + transform.forward / 4 + Vector3.up;
        cc.enabled = true;
        _velocity = Vector3.zero;
    }

    private void AttachToLedge()
    {
        gravity = 0;
        _velocity = Vector3.zero;
    }

    // Check if there is space to climb to the left
    private bool CanClimbLeft()
    {
        Vector3 playerRootPosition = new Vector3(transform.position.x, _playerRootPosition.y, transform.position.z);
        Vector3 leftRaycastOrigin = playerRootPosition + transform.TransformVector(leftLedgeRaycastOrigin);
        float raycastLength = centerLedgeRaycastOrigin.y - highLedgeStart;
        Debug.DrawRay(leftRaycastOrigin, Vector3.down * raycastLength, Color.red);
        
        if (Physics.Raycast(leftRaycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, groundLayers))
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
        Vector3 rightRaycastOrigin = playerRootPosition + transform.TransformVector(rightLedgeRaycastOrigin);
        float raycastLength = centerLedgeRaycastOrigin.y - highLedgeStart;
        Debug.DrawRay(rightRaycastOrigin, Vector3.down * raycastLength, Color.red);
        
        if (Physics.Raycast(rightRaycastOrigin, Vector3.down, out RaycastHit hitHigh, 0.1f, groundLayers))
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

    private void DetachFromLedge()
    {
        gravity = _defaultGravity;
    }

    //todo: Add ledge sensors on the left and right to see where ledges end
}
