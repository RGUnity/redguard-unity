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

        if (filePath == String.Empty)
        {
            return;
        }

        await ExportToPath(obj, filePath);
    }

    /// <summary>
    /// Export a GameObject hierarchy to a GLB/GLTF file at the given path (no file dialog).
    /// </summary>
    public async Task<bool> ExportToPath(GameObject obj, string filePath)
    {
        var objectsToExport = new GameObject[] { obj };

        var settings = new ExportSettings()
        {
            Format = GltfFormat.Json,
        };

        if (filePath.EndsWith("glb"))
        {
            settings.Format = GltfFormat.Binary;
        }

        var export = new GameObjectExport(settings);
        export.AddScene(objectsToExport);

        var success = await export.SaveToFileAndDispose(filePath);

        if (success)
        {
            Debug.Log("Exported " + filePath);
        }
        else
        {
            Debug.LogError("Something went wrong trying to export the model." + filePath);
        }

        return success;
    }
}
