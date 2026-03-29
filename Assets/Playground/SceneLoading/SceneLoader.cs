using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject sceneParent;
    [SerializeField] private string RGM;
    [SerializeField] private string WLD;
    [SerializeField] private string COL;

    private GameObject _sceneSubRoot;
    private List<GameObject> loadedObjects;

    void Start()
    {
        SpawnArea(RGM, WLD, COL);
        _sceneSubRoot.transform.localScale = Vector3.one * 0.3f;
    }

    public void SpawnArea(string RGM, string WLD, string COL)
    {
        Destroy(_sceneSubRoot);
        _sceneSubRoot = new GameObject();
        _sceneSubRoot.transform.SetParent(sceneParent.transform);
        _sceneSubRoot.name = RGM;

        FFIModelLoader.ClearCache();
        loadedObjects = FFIModelLoader.LoadArea(RGM, COL, WLD);

        foreach (var obj in loadedObjects)
        {
            obj.transform.SetParent(_sceneSubRoot.transform);
        }

        SwitchTextureFilterMode(FilterMode.Point);
        EnableAnimations(true);

        print("Loaded area: " + RGM);
    }

    public void SwitchTextureFilterMode(FilterMode mode)
    {
        if (_sceneSubRoot == null) return;
        foreach (var renderer in _sceneSubRoot.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in renderer.materials)
            {
                if (mat.mainTexture != null)
                    mat.mainTexture.filterMode = mode;
            }
        }
    }

    public void EnableAnimations(bool enableAnimations)
    {
        if (loadedObjects == null) return;
        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out BlendShapeAnimator animator))
            {
                animator.enabled = enableAnimations;
            }
        }
    }
}
