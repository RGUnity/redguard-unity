using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using GLTFast.Export;
using SFB;

public class GLTFExporter : MonoBehaviour
{
    public async Task ExportGLTF(GameObject obj, string objectName)
    {
        var extensionList = new [] {
            new ExtensionFilter("glTF Binary", "glb"),
            new ExtensionFilter("glTF Separate (unpacked mesh & textures)", "gltf"),
        };
        var filePath = StandaloneFileBrowser.SaveFilePanel("Save File", "", objectName, extensionList);
        
        if (filePath != String.Empty)
        {
            // Define objects to export
            var objectsToExport = new GameObject[] {obj};

            var settings = new ExportSettings()
            {
                Format = GltfFormat.Json,
            };
            
            if (filePath.EndsWith("gltf"))
            {
                settings.Format = GltfFormat.Json;
            }
            else if (filePath.EndsWith("glb"))
            {
                settings.Format = GltfFormat.Binary;
            }
            
            var export = new GameObjectExport(settings);
            export.AddScene(objectsToExport);
        
            // Async glTF export
            var success = await export.SaveToFileAndDispose(filePath);
        
            if (success)
            {
                print("Exported " + filePath);
            }
            else
            {
                Debug.LogError("Something went wrong trying to export the model." + filePath);
            }
        }
    }
}
