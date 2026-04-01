using System;
using System.IO;
using UnityEngine;
using SFB;

public class GLTFExporter : MonoBehaviour
{
    [HideInInspector] public string modelName;
    [HideInInspector] public ModelFileType fileType;
    [HideInInspector] public string colName;
    [HideInInspector] public string rgmName;
    [HideInInspector] public string wldName;
    [HideInInspector] public bool isAreaExport;

    public void ExportGLTF(string objectName)
    {
        var extensionList = new[] {
            new ExtensionFilter("glTF Binary", "glb"),
        };
        var filePath = StandaloneFileBrowser.SaveFilePanel("Export GLB", "", objectName, extensionList);
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        bool success = ExportToPath(filePath);
        if (!success)
        {
            Debug.LogError("[Export] Failed to export " + objectName);
        }
    }

    public bool ExportToPath(string filePath)
    {
        try
        {
            byte[] glbBytes;
            if (isAreaExport)
            {
                glbBytes = ExportArea();
            }
            else
            {
                glbBytes = ExportModel();
            }

            if (glbBytes == null || glbBytes.Length == 0)
            {
                Debug.LogError("[Export] FFI returned empty GLB data. " + RgpreBindings.GetLastErrorMessage());
                return false;
            }

            File.WriteAllBytes(filePath, glbBytes);
            Debug.Log("[Export] Exported " + filePath + " (" + (glbBytes.Length / 1024) + " KB)");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("[Export] " + ex.Message);
            return false;
        }
    }

    public System.Threading.Tasks.Task<bool> ExportToPath(GameObject _, string filePath)
    {
        return System.Threading.Tasks.Task.FromResult(ExportToPath(filePath));
    }

    private byte[] ExportModel()
    {
        string artFolder = Game.pathManager.GetArtFolder();
        string assetsDir = Game.pathManager.GetRootFolder();
        string extension = fileType switch {
            ModelFileType.file3D => ".3D",
            ModelFileType.file3DC => ".3DC",
            ModelFileType.fileROB => ".ROB",
            _ => ".3D"
        };
        string modelPath = Path.Combine(artFolder, modelName + extension);
        if (!File.Exists(modelPath))
        {
            Debug.LogError("[Export] Model file not found: " + modelPath);
            return null;
        }

        IntPtr resultPtr = RgpreBindings.ConvertModelFromPath(modelPath, assetsDir);
        if (resultPtr == IntPtr.Zero)
        {
            Debug.LogError("[Export] FFI conversion failed: " + RgpreBindings.GetLastErrorMessage());
            return null;
        }

        return RgpreBindings.ExtractBytesAndFree(resultPtr);
    }

    private byte[] ExportArea()
    {
        string mapsFolder = Game.pathManager.GetMapsFolder();
        string assetsDir = Game.pathManager.GetRootFolder();

        if (!string.IsNullOrEmpty(wldName))
        {
            string wldPath = Path.Combine(mapsFolder, wldName + ".WLD");
            if (!File.Exists(wldPath))
            {
                Debug.LogError("[Export] WLD file not found: " + wldPath);
                return null;
            }

            IntPtr wldResult = RgpreBindings.ConvertWldFromPath(wldPath, assetsDir);
            if (wldResult == IntPtr.Zero)
            {
                Debug.LogError("[Export] FFI area conversion failed: " + RgpreBindings.GetLastErrorMessage());
                return null;
            }

            return RgpreBindings.ExtractBytesAndFree(wldResult);
        }

        string rgmPath = Path.Combine(mapsFolder, rgmName + ".RGM");
        if (!File.Exists(rgmPath))
        {
            Debug.LogError("[Export] RGM file not found: " + rgmPath);
            return null;
        }

        IntPtr rgmResult = RgpreBindings.ConvertRgmFromPath(rgmPath, assetsDir);
        if (rgmResult == IntPtr.Zero)
        {
            Debug.LogError("[Export] FFI area conversion failed: " + RgpreBindings.GetLastErrorMessage());
            return null;
        }

        return RgpreBindings.ExtractBytesAndFree(rgmResult);
    }
}
