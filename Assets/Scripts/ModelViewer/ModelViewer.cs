using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelViewer : MonoBehaviour
{
    [SerializeField] public ModelViewer_Settings settings;
    [SerializeField] private ModelViewer_GUI gui;
    [SerializeField] private ModelViewer_Camera mvCam;
    [SerializeField] public GLTFExporter glTFExporter;
    [SerializeField] private GameObject objectRoot;
    [SerializeField] private GameObject cameraRoot;
    [SerializeField] private string pathOverride;

    public string loadedFileName = "";
    public string minimalLoadedFileName = "";
    public string loadedWLD = "";
    public string loadedCOL = "";
    public GameObject _objectRootGenerated;
    public List<GameObject> loadedObjects = new();

    private Light cameraLight;
    private Light sceneLight;
    private GameObject sceneLightGizmo;

    // Double-click-to-isolate
    private Vector2 mouseDownPosition;
    private const float clickDistanceThreshold = 5f;
    private float lastClickTime;
    private const float doubleClickTime = 0.25f;

    private void Start()
    {
        // if a path override is set, use that
        if (pathOverride.Length > 0)
        {
            Game.pathManager.SetPath(pathOverride);
            print("Redguard Path Override found in Scene. Value: " + Game.pathManager.GetRootFolder());
        }
        
        // Initialize GUI
        gui.Initialize();
        settings.Initialize();
        SetupLights();
    }

    private void Update()
    {
        // Input Event: Toggle UI
        if (Keyboard.current.tabKey.wasReleasedThisFrame)
        {
            settings.ToggleUI();
        }

        // Input Event: Focus object
        if (Keyboard.current.fKey.wasReleasedThisFrame)
        {
            mvCam.FrameObject(_objectRootGenerated);
        }

        // Track mouse down position for drag-vs-click detection
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            mouseDownPosition = Mouse.current.position.ReadValue();
        }

        // Double-click-to-isolate (only if mouse didn't drag)
        if (Mouse.current.leftButton.wasReleasedThisFrame && !gui.IsMouseOverUI
            && !Keyboard.current.leftAltKey.isPressed
            && loadedObjects != null && loadedObjects.Count > 1)
        {
            Vector2 mouseUpPos = Mouse.current.position.ReadValue();
            float dragDistance = Vector2.Distance(mouseDownPosition, mouseUpPos);

            if (dragDistance < clickDistanceThreshold)
            {
                float timeSinceLastClick = Time.unscaledTime - lastClickTime;
                lastClickTime = Time.unscaledTime;

                if (timeSinceLastClick <= doubleClickTime)
                {
                    Ray ray = Camera.main.ScreenPointToRay(mouseUpPos);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        // Find which loadedObject owns the hit collider
                        GameObject hitRoot = hit.collider.gameObject;
                        foreach (var obj in loadedObjects)
                        {
                            if (hitRoot == obj || hitRoot.transform.IsChildOf(obj.transform))
                            {
                                gui.SelectObjectInDropdown(obj.name);
                                hitRoot = null;
                                break;
                            }
                        }
                        if (hitRoot != null)
                            gui.SelectObjectInDropdown(gui.objectDropdownResetText);
                    }
                    else
                    {
                        gui.SelectObjectInDropdown(gui.objectDropdownResetText);
                    }
                }
            }
        }

        // Scene light drag (middle mouse button)
        if (settings.useSceneLight && sceneLight != null
            && Mouse.current.middleButton.isPressed && !gui.IsMouseOverUI)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            Transform camTransform = mvCam.GetCameraTransform();
            float speed = 0.02f;
            Vector3 move = camTransform.right * mouseDelta.x * speed
                         + camTransform.up * mouseDelta.y * speed;
            sceneLight.transform.position += move;
        }
    }

    private void SetupLights()
    {
        // Camera light — parented to camera
        var camLightGO = new GameObject("CameraLight");
        camLightGO.transform.SetParent(mvCam.GetCameraTransform(), false);
        camLightGO.transform.localPosition = Vector3.zero;
        camLightGO.transform.localRotation = Quaternion.Euler(30f, 0f, 0f);
        cameraLight = camLightGO.AddComponent<Light>();
        cameraLight.type = LightType.Directional;
        cameraLight.intensity = 0.3f;
        cameraLight.color = Color.white;
        cameraLight.shadows = LightShadows.None;
        cameraLight.gameObject.SetActive(settings.useCameraLight);

        var sceneLightGO = new GameObject("SceneLight");
        sceneLightGO.transform.SetParent(objectRoot.transform, false);
        sceneLightGO.transform.localPosition = new Vector3(-3f, 3f, -2f);
        sceneLight = sceneLightGO.AddComponent<Light>();
        sceneLight.type = LightType.Point;
        sceneLight.intensity = settings.sceneLightIntensity * 100f;
        sceneLight.range = 500f;
        sceneLight.color = Color.white;
        sceneLight.shadows = LightShadows.None;

        // Gizmo sphere
        sceneLightGizmo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sceneLightGizmo.name = "SceneLightGizmo";
        sceneLightGizmo.transform.SetParent(sceneLightGO.transform, false);
        sceneLightGizmo.transform.localPosition = Vector3.zero;
        sceneLightGizmo.transform.localScale = Vector3.one * 0.3f;
        Destroy(sceneLightGizmo.GetComponent<Collider>());
        var gizmoRenderer = sceneLightGizmo.GetComponent<Renderer>();
        var gizmoMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        gizmoMat.SetColor("_EmissionColor", Color.yellow * 2f);
        gizmoMat.EnableKeyword("_EMISSION");
        gizmoMat.color = Color.yellow;
        gizmoRenderer.material = gizmoMat;

        sceneLightGO.SetActive(settings.useSceneLight);
    }

    public void SpawnModel(string f3Dname, ModelFileType fileType, string colname)
    {
        // If the same model is already loaded, stop
        if (f3Dname == minimalLoadedFileName)
        {
            return;
        }
        
        PrepareLoad();
        FFIModelLoader.ClearCache();
        
        loadedObjects = new List<GameObject>();
        switch (fileType)
        {
            case ModelFileType.file3D:
                loadedObjects.Add(FFIModelLoader.Load3D(f3Dname, colname));
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".3D";
                break;
            case ModelFileType.file3DC:
                loadedObjects.Add(FFIModelLoader.Load3DC(f3Dname, colname));
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".3DC";
                break;
            case ModelFileType.fileROB:
                loadedObjects = FFIModelLoader.LoadROB(f3Dname, colname);
                loadedFileName = "/Redguard/fxart/" + f3Dname + ".ROB";
                break;
        }

        minimalLoadedFileName = f3Dname;
        glTFExporter.modelName = f3Dname;
        glTFExporter.fileType = fileType;
        glTFExporter.colName = colname;
        glTFExporter.isAreaExport = false;
        glTFExporter.rgmName = "";
        glTFExporter.wldName = "";
        
        SpreadObjects(loadedObjects);
        
        FinalizeLoad();
        print("Loaded object: " + loadedFileName);
    }
    
    public void SpawnArea(string RGM, string WLD, string COL, string prettyAreaName)
    {
        // If the same area is already loaded, stop
        if (RGM == minimalLoadedFileName)
        {
            return;
        }
        
        PrepareLoad();
        FFIModelLoader.ClearCache();
        
        // Load Area objects
        loadedObjects = FFIModelLoader.LoadArea(RGM, COL, WLD);
        minimalLoadedFileName = RGM;
        loadedWLD = WLD;
        loadedCOL = COL;
        glTFExporter.rgmName = RGM;
        glTFExporter.wldName = WLD;
        glTFExporter.colName = COL;
        glTFExporter.isAreaExport = true;
        glTFExporter.modelName = "";

        if (WLD.Equals(string.Empty))
        {
            loadedFileName = prettyAreaName + " (" + RGM + ".RGM, " + COL + ".COL)";
        }
        else
        {
            loadedFileName = prettyAreaName + " (" + RGM + ".RGM, " + COL + ".COL, " + WLD + ".WLD)";
        }
        
        FinalizeLoad();
        print("Loaded area: " + loadedFileName);
    }
    
    private void PrepareLoad()
    {
        DeleteLoadedObject();
    }

    private void FinalizeLoad()
    {
        _objectRootGenerated.name = loadedFileName;
        ParentListObjects(loadedObjects, _objectRootGenerated);

        mvCam.FrameObject(_objectRootGenerated);
        
        gui.UpdateModelDependentUI();

        ApplyTextureFilterSetting();
        ApplyAnimationSetting();
        ApplyScriptObjectVisibility();
        RepositionSceneLight();
    }
    
    // Spread the loaded objects in negative X direction
    private void SpreadObjects(List<GameObject> objects)
    {
        float occupiedDistance = 0;
        
        foreach (var obj in objects)
        {
            var component = obj.GetComponent<Renderer>();
            float objectWidth = component.bounds.size.x;
            float originOffset =  obj.transform.position.x + component.bounds.center.x;
            
            float xPosition = (occupiedDistance + objectWidth / 2 + originOffset) * -1;
            obj.transform.position = new Vector3(xPosition, 0, 0);
            
            float spacing = 0.5f;
            occupiedDistance += objectWidth + spacing;
        }
    }

    private void ParentListObjects(List<GameObject> objects, GameObject parent)
    {
        foreach (var obj in objects)
        {
            obj.transform.SetParent(parent.transform);
        }
    }

    private void DeleteLoadedObject()
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        if (_objectRootGenerated)
        {
            Destroy(_objectRootGenerated);
            Resources.UnloadUnusedAssets();
        }
        _objectRootGenerated = new GameObject();
        _objectRootGenerated.transform.SetParent(objectRoot.transform);
    }

    public void IsolateObject(string selection)
    {
        // show the selected object
        foreach (var obj in loadedObjects)
        {
            if (obj.name == selection)
            {
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    public void ResetIsolation()
    {
        // Show all objects
        foreach (var obj in loadedObjects)
        {
            obj.gameObject.SetActive(true);
        }
    }
    
    public void ApplyTextureFilterSetting()
    {
        if (loadedObjects == null || loadedObjects.Count == 0)
        {
            return;
        }

        foreach (var renderer in _objectRootGenerated.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in renderer.materials)
            {
                if (mat == null || mat.mainTexture == null)
                {
                    continue;
                }

                mat.mainTexture.filterMode = settings.useTextureFiltering ? FilterMode.Bilinear : FilterMode.Point;
            }
        }
    }

    public void ApplyAnimationSetting()
    {
        if (loadedObjects == null)
        {
            return;
        }

        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out RGScriptedObject rgso))
            {
                if (rgso.type == RGScriptedObject.ScriptedObjectType.scriptedobject_animated)
                {
                    if(settings.playAnimations == true)
                        rgso.SetAnim((int)RGRGMAnimStore.AnimGroup.anim_panic, 0);
                    else
                        rgso.ClearAnim();
                }
            }

            if (obj.TryGetComponent(out BlendShapeAnimator bsa))
            {
                if (settings.playAnimations)
                    bsa.Play();
                else
                    bsa.Stop();
            }
        }
    }

    public void SetCameraLight(bool enable)
    {
        if (cameraLight != null)
            cameraLight.gameObject.SetActive(enable);
    }

    public void SetSceneLight(bool enable)
    {
        if (sceneLight != null)
        {
            sceneLight.gameObject.SetActive(enable);
            if (sceneLightGizmo != null)
                sceneLightGizmo.SetActive(enable);
        }
    }

    public void SetSceneLightIntensity(float intensity)
    {
        if (sceneLight != null)
            sceneLight.intensity = intensity * 100f;
    }

    public void ApplyScriptObjectVisibility()
    {
        if (loadedObjects == null) return;
        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out RGScriptedObject rgso))
            {
                obj.SetActive(settings.showScriptObjects);
            }
        }
    }

    public void ReloadWithPalette(string paletteName)
    {
        if (string.IsNullOrEmpty(minimalLoadedFileName)) return;

        string fileName = minimalLoadedFileName;

        // Clear caches so it rebuilds with the new palette
        FFIModelLoader.ClearCache();

        // Force reload by clearing the name check
        string savedLoadedFileName = loadedFileName;
        minimalLoadedFileName = "";

        // Determine if this was an area or a model
        if (savedLoadedFileName.EndsWith(".3D"))
            SpawnModel(fileName, ModelFileType.file3D, paletteName);
        else if (savedLoadedFileName.EndsWith(".3DC"))
            SpawnModel(fileName, ModelFileType.file3DC, paletteName);
        else if (savedLoadedFileName.EndsWith(".ROB"))
            SpawnModel(fileName, ModelFileType.fileROB, paletteName);
        else
            ReloadAreaWithPalette(fileName, paletteName);
    }

    private void ReloadAreaWithPalette(string rgmName, string paletteName)
    {
        SpawnArea(rgmName, loadedWLD, paletteName, rgmName);
    }

    private void RepositionSceneLight()
    {
        if (sceneLight == null || _objectRootGenerated == null) return;
        Bounds bounds = GetObjectBounds();
        float size = bounds.extents.magnitude;
        sceneLight.transform.position = bounds.center
            + Vector3.left * size * 1.0f
            + Vector3.up * size * 0.8f
            + Vector3.back * size * 0.6f;
        sceneLight.range = size * 4f;
    }

    private Bounds GetObjectBounds()
    {
        if (_objectRootGenerated == null)
            return new Bounds(Vector3.zero, Vector3.one);

        var renderers = _objectRootGenerated.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(_objectRootGenerated.transform.position, Vector3.one);

        var bounds = renderers[0].bounds;
        foreach (var r in renderers)
            bounds.Encapsulate(r.bounds);
        return bounds;
    }
}
