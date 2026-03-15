using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ModelViewer_Camera : MonoBehaviour
{
    [SerializeField] public ModelViewer_Settings settings;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform rotationRootX;
    [SerializeField] private Transform cameraRootZ;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private float zoomSpeed = 5;
    [SerializeField] public bool useFlyMode;

    private bool leftDragStartedInViewport;
    private bool rightDragStartedInViewport;
    private float currentRotX = 160;
    private float currentRotY = 15;

    // Fly Camera settings
    private float speed_slow = 5;
    private float speed_reg = 15;
    private float speed_fast = 50;
    private float speed_cur;
    private float flyPitch;
    private float flyYaw;
    private int flyRotationSkip;

    private void Update()
    {
        var mouse = Mouse.current;
        var kb = Keyboard.current;
        if (mouse == null || kb == null) return;

        // Normalize scroll: New Input System gives ~120 per notch, legacy gave ~1
        float scrollInput = mouse.scroll.ReadValue().y / 120f;

        // Track if mouse click started on viewport (not UI)
        if (mouse.leftButton.wasPressedThisFrame && !gui.IsMouseOverUI)
            leftDragStartedInViewport = true;
        if (mouse.leftButton.wasReleasedThisFrame)
            leftDragStartedInViewport = false;
        if (mouse.rightButton.wasPressedThisFrame && !gui.IsMouseOverUI)
            rightDragStartedInViewport = true;
        if (mouse.rightButton.wasReleasedThisFrame)
            rightDragStartedInViewport = false;

        if (useFlyMode && !gui.IsMouseOverUI)
        {
            // movement
            float h = 0f;
            if (kb.aKey.isPressed) h -= 1f;
            if (kb.dKey.isPressed) h += 1f;
            float v = 0f;
            if (kb.wKey.isPressed) v += 1f;
            if (kb.sKey.isPressed) v -= 1f;
            Vector3 input = new Vector3(h, 0f, v);
            if(kb.leftShiftKey.isPressed)
                speed_cur = speed_fast;
            else if(kb.leftCtrlKey.isPressed)
                speed_cur = speed_slow;
            else
                speed_cur = speed_reg;
            if (input != Vector3.zero)
                _camera.Translate(input * (speed_cur * Time.deltaTime));
            if (scrollInput != 0f)
                _camera.Translate(Vector3.up * scrollInput * speed_cur * 2f);

            // rotation — only while dragging
            // On fresh click, sync flyPitch/flyYaw from current camera rotation
            if ((mouse.leftButton.wasPressedThisFrame && !gui.IsMouseOverUI) || (mouse.rightButton.wasPressedThisFrame && !gui.IsMouseOverUI))
            {
                Vector3 currentEuler = _camera.rotation.eulerAngles;
                flyYaw = currentEuler.y;
                flyPitch = currentEuler.x;
                if (flyPitch > 180f) flyPitch -= 360f;
                flyRotationSkip = 3;
            }

            bool isDragging = (mouse.leftButton.isPressed && leftDragStartedInViewport) || (mouse.rightButton.isPressed && rightDragStartedInViewport);
            if (isDragging)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                if (flyRotationSkip > 0)
                {
                    flyRotationSkip--;
                }
                else
                {
                    Vector2 mouseDelta = mouse.delta.ReadValue();
                    flyYaw += mouseDelta.x * 0.1f;
                    flyPitch -= mouseDelta.y * 0.1f;
                }
                _camera.rotation = Quaternion.Euler(flyPitch, flyYaw, 0f);
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (!useFlyMode)
        {
            ResetFlyModeTransforms();

            // Mouse Rotation
            if (mouse.leftButton.isPressed && leftDragStartedInViewport)
            {
                Vector2 mouseDelta = mouse.delta.ReadValue();
                currentRotX += mouseDelta.y * rotationSpeed * 0.1f;
                currentRotX = Mathf.Clamp(currentRotX, 90f, 270f);
                currentRotY += mouseDelta.x * rotationSpeed * 0.1f;
            }

            // Apply values to both objects
            rotationRootX.localRotation = Quaternion.Euler(currentRotX, 0, 0);
            transform.rotation = Quaternion.Euler(0, currentRotY, 0);

            // Mouse Wheel Zoom
            if (!gui.IsMouseOverUI)
            {
                Vector3 pos = cameraRootZ.localPosition;
                float multiplier = cameraRootZ.localPosition.z/100 * zoomSpeed * -50;
                pos.z += scrollInput * multiplier;
                pos.z = Mathf.Clamp(pos.z, -100000, -1);
                cameraRootZ.localPosition = pos;
            }
        }
    }

    private void ResetFlyModeTransforms()
    {
        if (_camera.localPosition != Vector3.zero)
            _camera.localPosition = Vector3.zero;
        if (_camera.localRotation != Quaternion.identity)
            _camera.localRotation = Quaternion.identity;
        flyPitch = 0f;
        flyYaw = 0f;
    }

    public void FrameObject(GameObject target)
    {
        if (!target)
        {
            Debug.LogWarning("FrameObject target is null, cancelling FrameObject()");
            return;
        }

        ResetFlyModeTransforms();

        var bounds = GetMaxBounds(target);
        transform.position = bounds.center;
        float distance = bounds.size.magnitude;
        cameraRootZ.localPosition = new Vector3(0, 0, distance) * -1;
    }

    public Transform GetCameraTransform()
    {
        return _camera;
    }

    public float GetCameraDistance()
    {
        return -cameraRootZ.localPosition.z;
    }

    Bounds GetMaxBounds(GameObject g) {
        var renderers = g.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(g.transform.position, Vector3.zero);
        var b = renderers[0].bounds;
        foreach (Renderer r in renderers) {
            if (r.transform.position != Vector3.zero)
            {
                b.Encapsulate(r.bounds);
            }
        }
        return b;
    }
}
