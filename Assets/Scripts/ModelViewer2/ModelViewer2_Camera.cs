using UnityEngine;
using UnityEngine.EventSystems;

public class ModelViewer2_Camera : MonoBehaviour
{
    [SerializeField] private ModelViewer2_GUI gui;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform rotationRootX;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float zoomSpeed = 5;
    
    private bool clickStartedInViewport = false;
    private float currentRotX = 160;
    private float currentRotY = 15;
    
    
    // Did you know? Update is called once per frame!
    private void Update()
    {
        bool mouseIsOverUI = eventSystem.IsPointerOverGameObject(gui.gameObject);
        
        // Mouse Rotation
        if (Input.GetMouseButton(0) && !mouseIsOverUI)
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
            Vector3 pos = _camera.localPosition;
            float multiplier = _camera.localPosition.z/100 * zoomSpeed *-1;
            pos.z += Input.mouseScrollDelta.y * multiplier;
        
            // Clamp the zoom range
            pos.z = Mathf.Clamp(pos.z, -5000, -1);
        
            // Move the camera by the new position
            _camera.localPosition = pos;
        }
    }
    
    public void FrameObject(GameObject target)
    {
        var bounds = GetMaxBounds(target);

        // Move camera root to the center
        transform.position = bounds.center;

        // Set camera distance
        float distance = bounds.size.magnitude;
        _camera.localPosition = new Vector3(0, 0, distance) * -1;
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
