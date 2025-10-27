using System;
using UnityEngine;

public class PlayerMeshLoader : MonoBehaviour
{
    [SerializeField] private GameObject meshParent;
    [SerializeField] private GameObject placeholder;
    private GameObject playerMesh;

    public void Start()
    {
        placeholder.SetActive(false);
        Spawn3DC("CYRSA001", "ISLAND");
        EnableAnimations(true);
    }


    public void Spawn3DC(string f3Dname, string colname)
    {
        // Create the object and parent it under the root
        playerMesh = ModelLoader.Load3DC(f3Dname, colname);
       
        playerMesh.transform.SetParent(meshParent.transform);
        playerMesh.transform.localPosition = Vector3.zero;
        playerMesh.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        playerMesh.transform.localScale = Vector3.one * 0.3f;
        
        print("Loaded object: " + f3Dname);

        //SwitchTextureFilterMode(FilterMode.Point);
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
        if (playerMesh.TryGetComponent(out RGScriptedObject rgso))
        {
            if (rgso.type == RGScriptedObject.ScriptedObjectType.scriptedobject_animated)
            {
                rgso.allowAnimation = enableAnimations;
            }
        }
    }
}
