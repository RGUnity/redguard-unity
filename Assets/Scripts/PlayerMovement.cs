using UnityEditor.UIElements;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private LayerMask groundLayers;
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 1.2f;
    [SerializeField] private float jumpSpeed = 7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundMagnet = -4f;
    
    [Header("Slope Sliding")]
    [SerializeField] private float slopeSlideSpeed = 5f;
    [SerializeField] private float slopeTurnSpeed = 3f;
    
    [Header("Moving Platforms")]
    [SerializeField] private string _movingPlatformTag;
    [SerializeField] private string _rotatingPlatformTag;
    
    // General CC properties
    private bool _isGrounded;
    private bool _isSliding; 
    private Vector3 _moveDirection;
    
    // Surface information
    private float _surfaceAngle;
    private Vector3 _surfaceNormal;
    private Vector3 _surfaceContact;
    private Vector3 _surfaceDownhill;
    private Vector3 _forwardOnSurface;
    
    private Vector3 _platformPosition;
    private bool _isOnMovingPlatform; 
    private bool _isOnRotatingPlatform; 
    private Vector3 _pointMovement;
    private float _pointRotation;
    
    
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
            _moveDirection = _forwardOnSurface;
            
            Debug.DrawRay(_surfaceContact, _forwardOnSurface, Color.yellow);
            _moveDirection *= Input.GetAxis("Vertical") * moveSpeed;
        
            // Rotate the player
            float yRotation = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime * 60;
            transform.Rotate(0, yRotation * rotateSpeed, 0);

            // Make the player jump
            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = jumpSpeed;
                _isGrounded = false;
            }
        }

        if (!_isGrounded)
        {
            // Apply gravity
            _moveDirection.y += gravity * Time.deltaTime; 
        }
        else
        {
            // Apply ground magnet
            _moveDirection.y += groundMagnet;
        }
        
        
        // Move the controller
        characterController.Move(_moveDirection * Time.deltaTime);

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
        }
    }
    
    private void SlideDownhill()
    {
        // This Vector points downhill
        Debug.DrawRay(_surfaceContact, _surfaceDownhill, Color.green);
        
        _moveDirection = _surfaceDownhill * slopeSlideSpeed;
        
        // Rotate the player towards the slideDirection
        Vector3 flatSurfaceDownhill = new Vector3(_surfaceDownhill.x, 0, _surfaceDownhill.z);
        
        Debug.DrawRay(_surfaceContact, flatSurfaceDownhill, Color.blue);
        Quaternion stepRotation = Quaternion.LookRotation(flatSurfaceDownhill, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, stepRotation, slopeTurnSpeed * Time.deltaTime);
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
        if (hit.collider.tag == _movingPlatformTag)
        {
            _isOnMovingPlatform = true;
            _isOnRotatingPlatform = false;
            
            if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_MovingPlatform pf))
            {
                _pointMovement = pf.moveStep;
            }
        }
        else if (hit.collider.tag == _rotatingPlatformTag)
        {
            _isOnMovingPlatform = false;
            _isOnRotatingPlatform = true;

            if (hit.rigidbody.transform.parent.TryGetComponent(out TEST_RotatingPlatform pf))
            {
                _pointRotation = pf.yStepRotation;
            }
            
            _platformPosition = hit.rigidbody.transform.position;
        }
        else
        {
            _isOnMovingPlatform = false;
            _isOnRotatingPlatform = false;
        }
    }
}

