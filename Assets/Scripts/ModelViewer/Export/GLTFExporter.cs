using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using GLTFast.Export;

public class GLTFExporter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public async Task ExportGLTF(GameObject obj, string exportDir)
    {
        // Create Subfolder for Object
        var exportDirWithSubfolder = exportDir + obj.name;
        var fullGLTFPath = exportDirWithSubfolder + "/" + obj.name + ".gltf";

        // If missing, create the target folder
        if (!Directory.Exists(exportDirWithSubfolder))
        {
            Directory.CreateDirectory(exportDirWithSubfolder);
        }
        
        // Define objects to export
        var objectsToExport = new GameObject[] {obj};
        
        var export = new GameObjectExport();
        export.AddScene(objectsToExport);
        
        // Async glTF export
        var success = await export.SaveToFileAndDispose(fullGLTFPath);

        if (success)
        {
            print("Exported " + fullGLTFPath);
        }
        else
        {
            Debug.LogError("Something went wrong trying to export the model." + fullGLTFPath);
        }
    }
}
