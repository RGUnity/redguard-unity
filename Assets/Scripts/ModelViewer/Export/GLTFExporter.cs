using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
        string extension = fileType switch {
            ModelFileType.file3D => ".3D",
            ModelFileType.file3DC => ".3DC",
            ModelFileType.fileROB => ".ROB",
            _ => ".3D"
        };
        string modelPath = Path.Combine(artFolder, modelName + extension);
        byte[] modelBytes = File.ReadAllBytes(modelPath);

        IntPtr textureCache = CreateTextureCacheFromDisk();

        var modelPin = GCHandle.Alloc(modelBytes, GCHandleType.Pinned);
        try
        {
            IntPtr resultPtr;
            if (fileType == ModelFileType.fileROB)
            {
                resultPtr = RgpreBindings.ConvertRobToGlb(modelPin.AddrOfPinnedObject(), modelBytes.Length, textureCache);
            }
            else
            {
                resultPtr = RgpreBindings.ConvertModelToGlb(modelPin.AddrOfPinnedObject(), modelBytes.Length, textureCache);
            }

            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[Export] FFI conversion failed: " + RgpreBindings.GetLastErrorMessage());
                return null;
            }

            return RgpreBindings.ExtractBytesAndFree(resultPtr);
        }
        finally
        {
            modelPin.Free();
            if (textureCache != IntPtr.Zero)
            {
                RgpreBindings.FreeTextureCache(textureCache);
            }
        }
    }

    private byte[] ExportArea()
    {
        string artFolder = Game.pathManager.GetArtFolder();
        string mapsFolder = Game.pathManager.GetMapsFolder();
        var pinnedHandles = new List<GCHandle>();
        var allocatedPtrs = new List<IntPtr>();
        IntPtr textureCache = IntPtr.Zero;

        try
        {
            byte[] wldBytes = null;
            byte[] rgmBytes = File.ReadAllBytes(Path.Combine(mapsFolder, rgmName + ".RGM"));

            if (!string.IsNullOrEmpty(wldName))
            {
                wldBytes = File.ReadAllBytes(Path.Combine(mapsFolder, wldName + ".WLD"));
            }

            textureCache = CreateTextureCacheFromDisk();

            var modelNameList = new List<string>();
            var modelDataList = new List<byte[]>();
            var modelTypeList = new List<byte>();

            foreach (string path in Directory.GetFiles(artFolder, "*.3D"))
            {
                modelNameList.Add(Path.GetFileNameWithoutExtension(path).ToUpperInvariant());
                modelDataList.Add(File.ReadAllBytes(path));
                modelTypeList.Add(3);
            }
            foreach (string path in Directory.GetFiles(artFolder, "*.3DC"))
            {
                modelNameList.Add(Path.GetFileNameWithoutExtension(path).ToUpperInvariant());
                modelDataList.Add(File.ReadAllBytes(path));
                modelTypeList.Add(4);
            }
            foreach (string path in Directory.GetFiles(artFolder, "*.ROB"))
            {
                modelNameList.Add(Path.GetFileNameWithoutExtension(path).ToUpperInvariant());
                modelDataList.Add(File.ReadAllBytes(path));
                modelTypeList.Add(5);
            }

            IntPtr[] modelNames = new IntPtr[modelDataList.Count];
            IntPtr[] modelDatas = new IntPtr[modelDataList.Count];
            int[] modelLens = new int[modelDataList.Count];
            byte[] modelTypes = modelTypeList.ToArray();

            for (int i = 0; i < modelDataList.Count; i++)
            {
                IntPtr namePtr = Marshal.StringToHGlobalAnsi(modelNameList[i]);
                allocatedPtrs.Add(namePtr);
                modelNames[i] = namePtr;

                var pin = GCHandle.Alloc(modelDataList[i], GCHandleType.Pinned);
                pinnedHandles.Add(pin);
                modelDatas[i] = pin.AddrOfPinnedObject();
                modelLens[i] = modelDataList[i].Length;
            }

            IntPtr resultPtr;
            if (wldBytes != null)
            {
                var wldPin = GCHandle.Alloc(wldBytes, GCHandleType.Pinned);
                var rgmPin = GCHandle.Alloc(rgmBytes, GCHandleType.Pinned);
                pinnedHandles.Add(wldPin);
                pinnedHandles.Add(rgmPin);

                resultPtr = RgpreBindings.ConvertWldToGlb(
                    wldPin.AddrOfPinnedObject(), wldBytes.Length,
                    textureCache,
                    rgmPin.AddrOfPinnedObject(), rgmBytes.Length,
                    modelNames, modelDatas, modelLens, modelTypes, modelDataList.Count);
            }
            else
            {
                var rgmPin = GCHandle.Alloc(rgmBytes, GCHandleType.Pinned);
                pinnedHandles.Add(rgmPin);

                resultPtr = RgpreBindings.ConvertRgmToGlb(
                    rgmPin.AddrOfPinnedObject(), rgmBytes.Length,
                    textureCache,
                    modelNames, modelDatas, modelLens, modelTypes, modelDataList.Count);
            }

            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[Export] FFI area conversion failed: " + RgpreBindings.GetLastErrorMessage());
                return null;
            }

            return RgpreBindings.ExtractBytesAndFree(resultPtr);
        }
        finally
        {
            foreach (var ptr in allocatedPtrs)
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
            for (int i = pinnedHandles.Count - 1; i >= 0; i--)
            {
                if (pinnedHandles[i].IsAllocated)
                {
                    pinnedHandles[i].Free();
                }
            }
            if (textureCache != IntPtr.Zero)
            {
                RgpreBindings.FreeTextureCache(textureCache);
            }
        }
    }

    private IntPtr CreateTextureCacheFromDisk()
    {
        string artFolder = Game.pathManager.GetArtFolder();
        string paletteName = string.IsNullOrEmpty(colName) ? "ISLAND" : colName;
        string palettePath = Path.Combine(artFolder, paletteName + ".COL");

        if (!File.Exists(palettePath))
        {
            return IntPtr.Zero;
        }

        byte[] paletteBytes = File.ReadAllBytes(palettePath);
        var palettePin = GCHandle.Alloc(paletteBytes, GCHandleType.Pinned);

        try
        {
            string[] texbsiPaths = Directory.GetFiles(artFolder, "TEXBSI.*");
            var texbsiIds = new List<ushort>();
            var texbsiDataList = new List<byte[]>();
            var texbsiPins = new List<GCHandle>();

            foreach (string path in texbsiPaths)
            {
                string ext = Path.GetExtension(path).TrimStart('.');
                if (int.TryParse(ext, out int id) && id >= 0 && id <= ushort.MaxValue)
                {
                    texbsiIds.Add((ushort)id);
                    texbsiDataList.Add(File.ReadAllBytes(path));
                }
            }

            IntPtr[] texbsiDatas = new IntPtr[texbsiDataList.Count];
            int[] texbsiLens = new int[texbsiDataList.Count];
            for (int i = 0; i < texbsiDataList.Count; i++)
            {
                var pin = GCHandle.Alloc(texbsiDataList[i], GCHandleType.Pinned);
                texbsiPins.Add(pin);
                texbsiDatas[i] = pin.AddrOfPinnedObject();
                texbsiLens[i] = texbsiDataList[i].Length;
            }

            IntPtr cache = RgpreBindings.CreateTextureCache(
                palettePin.AddrOfPinnedObject(), paletteBytes.Length,
                texbsiIds.ToArray(), texbsiDatas, texbsiLens, texbsiDataList.Count);

            foreach (var pin in texbsiPins)
            {
                if (pin.IsAllocated)
                {
                    pin.Free();
                }
            }

            return cache;
        }
        finally
        {
            palettePin.Free();
        }
    }
}
