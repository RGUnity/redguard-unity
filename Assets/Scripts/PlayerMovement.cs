using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float gravity = -15f;
    [SerializeField] private float groundMagnet = -4f;
    
    [Header("Basic Movement")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float velocitySmoothing = 12;
    [SerializeField] private float turnSpeed = 3f;
    
    [Header("Jumping")]
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float longJumpDistance = 6f;
    [SerializeField] private float shortJumpDistance = 4f;
    [SerializeField] private float longJumpThreshold = 3.5f;
    [SerializeField] private float shortJumpThreshold = 0.5f;
    
    [Header("Slope Sliding")]
    [SerializeField] private float slopeSlideSpeed = 5f;
    [SerializeField] private float slopeAlignmentSpeed = 3f;
    
    [Header("Moving Platforms")]
    [SerializeField] private string _movingPlatformTag;
    [SerializeField] private string _rotatingPlatformTag;
    
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
    
    private Vector3 _platformPosition;
    private bool _isOnMovingPlatform; 
    private bool _isOnRotatingPlatform; 
    private Vector3 _pointMovement;
    private float _pointRotation;
    
    // Other variables
    private InputManager _input;
    private Vector3 smoothVector;
    private float speed;


    private void Start()
    {
        _input = LocalScene.inputManager;
    }

    void FixedUpdate()
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
                if (_input.move.y == 0
                   || _input.move.x != 0)
                {
                    // Strafe left or right
                    _velocity = _rightOnSurface * (_input.move.x * walkSpeed);
                }
                else
                {
                    // Walk forward or backwards
                    _velocity = _forwardOnSurface * (_input.move.y * walkSpeed);
                }
            }
            // Movement when moveModifier is NOT pressed / is false
            else
            {
                // Run Forward
                if (_input.move.y > 0)
                {
                    _velocity = _forwardOnSurface * (_input.move.y * runSpeed);
                }
                // Walk backwards
                else
                {
                    _velocity = _forwardOnSurface * (_input.move.y * walkSpeed);
                }
                
                // Turn the player
                float yRotation = _input.move.x * turnSpeed/2 * Time.deltaTime * 60;
                transform.Rotate(0, yRotation, 0);
            }
            
            // Apply smoothing with MoveTowards
            smoothVector = Vector3.MoveTowards(smoothVector, _velocity, Time.deltaTime * velocitySmoothing);
            _velocity.z = smoothVector.z;
            _velocity.x = smoothVector.x;
            
            speed = _velocity.magnitude;
            
            if (_input.jump)
            {
                if (_input.move.y > 0
                    && speed > longJumpThreshold)
                {
                    // Long forward jump 
                    _velocity = _forwardOnSurface * longJumpDistance;
                    _velocity.y = jumpHeight;
                    _isGrounded = false;
                }
                else if(_input.move.y > 0
                        && speed > shortJumpThreshold)
                {
                    // Short forward jump
                    _velocity = _forwardOnSurface * shortJumpDistance;
                    _velocity.y = jumpHeight;
                    _isGrounded = false;
                }
                else
                {
                    // Standing jump
                    _velocity = Vector3.zero;
                    smoothVector = Vector3.zero;
                    _velocity.y = jumpHeight;
                    _isGrounded = false;
                }
            }
        }

        // Y Speed
        if (!_isGrounded)
        {
            // Apply gravity
            _velocity.y += gravity * Time.deltaTime; 
        }
        else
        {
            // Apply ground magnet
            _velocity.y += groundMagnet;
        }
        
        // Move the controller
        characterController.Move(_velocity * Time.deltaTime);

        // Stick to moving and rotating platforms
        StickToPlatforms();
    }

    private void GroundCheck()
    {
        if (Physics.SphereCast(transform.position, 0.35f, Vector3.down, out RaycastHit hit, 0.7f, groundLayers))
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
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
            transform.position += _pointMovement;
        }
        
        // Rotating platforms
        if (_isOnRotatingPlatform)
        {
            transform.RotateAround(_platformPosition, Vector3.up, _pointRotation );
        }
    }
    
    
    private void OnControllerColliderHit(ControllerColliderHit hit )
    {
        if (hit.collider.CompareTag(_movingPlatformTag))
        {
            _isOnMovingPlatform = true;
            _isOnRotatingPlatform = false;
            
            if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_MovingPlatform pf))
            {
                _pointMovement = pf.moveStep;
            }
        }
        else if (hit.collider.CompareTag(_rotatingPlatformTag))
        {
            _isOnMovingPlatform = false;
            _isOnRotatingPlatform = true;
            
            if (hit.rigidbody != null)
            {
                
                if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_RotatingPlatform pf))
                {
                    _pointRotation = pf.yStepRotation;
                }
                _platformPosition = hit.rigidbody.transform.position;
            }
        }
        else
        {
            _isOnMovingPlatform = false;
            _isOnRotatingPlatform = false;
        }
    }
}

