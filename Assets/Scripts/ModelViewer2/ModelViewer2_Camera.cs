using UnityEngine;
using UnityEngine.EventSystems;

public class ModelViewer2_Camera : MonoBehaviour
{
    [SerializeField] private ModelViewer2_GUI gui;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform rotationRootX;
    [SerializeField] private Transform cameraRootZ;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] public bool useFlyMode;
    
    private bool dragStartedInViewport;
    private float currentRotX = 160;
    private float currentRotY = 15;

    // Fly Camera settings
    private float sensitivity = 8;
    private float speed_slow = 25;
    private float speed_reg = 50;
    private float speed_fast = 100;
    private float speed_cur;
    
    // Did you know? Update is called once per frame!
    private void Update()
    {
        // IsPointerOverGameObject was removed at some point so now i need to find another way...
        // bool mouseIsOverUI = eventSystem.IsPointerOverGameObject(gui.gameObject);
        
        // Temporary fix to prevent errors
        bool mouseIsOverUI = false;

        if (Input.GetMouseButtonDown(0) && !mouseIsOverUI)
        {
            dragStartedInViewport = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            dragStartedInViewport = false;
        }

        if (useFlyMode)
        {
            if(Input.GetMouseButton(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                // movement
                Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
                if(Input.GetKey(KeyCode.LeftShift))
                    speed_cur = speed_fast;
                else if(Input.GetKey(KeyCode.LeftControl))
                    speed_cur = speed_slow;
                else
                    speed_cur = speed_reg;
                _camera.Translate(input * (speed_cur * Time.deltaTime));

                // rotation
                Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
                _camera.Rotate(mouseInput * (sensitivity * Time.deltaTime * 50));
                Vector3 euler = _camera.rotation.eulerAngles;
                _camera.rotation = Quaternion.Euler(euler.x, euler.y, 0);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (_camera.localPosition != Vector3.zero)
            {
                _camera.localPosition = Vector3.zero;
            }

            if (_camera.localRotation != Quaternion.identity)
            {
                _camera.localRotation =  Quaternion.identity;
            }
            
            // Mouse Rotation
            if (Input.GetMouseButton(0) && dragStartedInViewport)
            {
                // Calcuclate X axis
                currentRotX += Input.GetAxis("Mouse Y") * rotationSpeed;
                currentRotX = Mathf.Clamp(currentRotX, 90f, 270f);
            
                // Calculate Y axis
                currentRotY += Input.GetAxis("Mouse X") * rotationSpeed;
            }
            
            
            // Apply values to both objects
            rotationRootX.localRotation = Quaternion.Euler(currentRotX, 0, 0);
            transform.rotation = Quaternion.Euler(0, currentRotY, 0);

            // Mouse Wheel Zoom
            if (!mouseIsOverUI)
            {
                // Calculate Zoom value
                Vector3 pos = cameraRootZ.localPosition;
                float multiplier = cameraRootZ.localPosition.z/100 * zoomSpeed *-1;
                pos.z += Input.mouseScrollDelta.y * multiplier;
        
                // Clamp the zoom range
                pos.z = Mathf.Clamp(pos.z, -100000, -1);
        
                // Move the camera by the new position
                cameraRootZ.localPosition = pos;
            }
        }
    }
    
    public void FrameObject(GameObject target)
    {
        var bounds = GetMaxBounds(target);

        // Move camera root to the center
        transform.position = bounds.center;

        // Set camera distance
        float distance = bounds.size.magnitude;
        cameraRootZ.localPosition = new Vector3(0, 0, distance) * -1;
    }
    // Get bounding box of all spawned objects combined
    Bounds GetMaxBounds(GameObject g) {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers) {
            b.Encapsulate(r.bounds);
        }
        return b;
    }
}
