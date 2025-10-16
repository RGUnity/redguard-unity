using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string RGM;
    [SerializeField] private string WLD;
    [SerializeField] private string COL;
    [SerializeField] private GameObject sceneRoot;
    
    private GameObject _sceneSubRoot;
    private List<GameObject> loadedObjects;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Maybe generate an RGM dropdown?
        // List<RGINIStore.worldData> worldList = RGINIStore.GetWorldList();
        // for (int i = 0; i < worldList.Count; i++)
        // {
        //     print(worldList[i].RGM);
        // };
        
        SpawnArea(RGM, WLD, COL);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnArea(string RGM, string WLD, string COL)
    {
        // objectRootGenerated is simply a new GameObject that makes deleting objects easier
        Destroy(_sceneSubRoot);
        _sceneSubRoot = new GameObject();
        _sceneSubRoot.transform.SetParent(sceneRoot.transform);
        _sceneSubRoot.name = RGM;

        // Create all objects of that area and parent them under the root
        loadedObjects = ModelLoader.LoadArea(RGM, COL, WLD);
        
        foreach (var obj in loadedObjects)
        {
            obj.transform.SetParent(_sceneSubRoot.transform);
        }

        SwitchTextureFilterMode(FilterMode.Point);
        EnableAnimations(true);
        
        print("Loaded area: " + RGM);
        RGMeshStore.DumpDict();
        RG3DStore.DumpDict();
        RGRGMStore.DumpDict();
        RGTexStore.DumpDict();
    }
    
    public void SwitchTextureFilterMode(FilterMode mode)
    {
        foreach (var mat in RGTexStore.MaterialDict)
        {
            mat.Value.mainTexture.filterMode = mode;
        }
    }
    
    public void EnableAnimations(bool enableAnimations)
    {
        foreach (var obj in loadedObjects)
        {
            if (obj.TryGetComponent(out RGScriptedObject rgso))
            {
                if (rgso.type == RGScriptedObject.ScriptedObjectType.scriptedobject_animated)
                {
                    rgso.allowAnimation = enableAnimations;
                }
            }
        }
    }
}
