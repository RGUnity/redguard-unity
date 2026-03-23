using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public static class FFIModelLoader
{
    private static Shader defaultShader;

    public static GameObject Load3D(string modelName, string colName)
    {
        return LoadModel(modelName, ".3D", colName, "3D_" + modelName);
    }

    public static GameObject Load3DC(string modelName, string colName)
    {
        return LoadModel(modelName, ".3DC", colName, "3DC_" + modelName);
    }

    public static List<GameObject> LoadROB(string robName, string colName)
    {
        string artFolder = Game.pathManager.GetArtFolder();
        string robPath = Path.Combine(artFolder, robName + ".ROB");
        if (!File.Exists(robPath))
        {
            Debug.LogError("[FFI] ROB not found: " + robPath);
            return new List<GameObject>();
        }

        byte[] robBytes = File.ReadAllBytes(robPath);
        var robPin = GCHandle.Alloc(robBytes, GCHandleType.Pinned);
        try
        {
            IntPtr resultPtr = RgpreBindings.ParseRobData(robPin.AddrOfPinnedObject(), robBytes.Length);
            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[FFI] Failed to parse ROB " + robName + ": " + RgpreBindings.GetLastErrorMessage());
                return new List<GameObject>();
            }

            byte[] rgmdBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
            var segments = RgmdDeserializer.DeserializeRobWithMaterials(rgmdBytes, out _, out _);

            var objects = new List<GameObject>(segments.Count);
            foreach (var segment in segments)
            {
                List<Material> materials = CreateMaterials(segment.materials, colName);
                int frameCount = segment.mesh != null ? segment.mesh.blendShapeCount : 0;
                objects.Add(CreateGameObject(segment.name, segment.mesh, materials, frameCount));
            }

            return objects;
        }
        finally
        {
            if (robPin.IsAllocated)
            {
                robPin.Free();
            }
        }
    }

    private static GameObject LoadModel(string modelName, string extension, string colName, string displayName)
    {
        string artFolder = Game.pathManager.GetArtFolder();
        string modelPath = Path.Combine(artFolder, modelName + extension);
        if (!File.Exists(modelPath))
        {
            Debug.LogError("[FFI] Model not found: " + modelPath);
            return new GameObject(displayName);
        }

        byte[] modelBytes = File.ReadAllBytes(modelPath);
        var modelPin = GCHandle.Alloc(modelBytes, GCHandleType.Pinned);
        try
        {
            IntPtr resultPtr = RgpreBindings.ParseModelData(modelPin.AddrOfPinnedObject(), modelBytes.Length);
            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[FFI] Failed to parse " + modelName + extension + ": " + RgpreBindings.GetLastErrorMessage());
                return new GameObject(displayName);
            }

            byte[] rgmdBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
            var (mesh, materialInfos, frameCount) = RgmdDeserializer.DeserializeModel(rgmdBytes, displayName);
            if (mesh == null)
            {
                return new GameObject(displayName);
            }

            List<Material> materials = CreateMaterials(materialInfos, colName);
            return CreateGameObject(displayName, mesh, materials, frameCount);
        }
        finally
        {
            if (modelPin.IsAllocated)
            {
                modelPin.Free();
            }
        }
    }

    private static List<Material> CreateMaterials(RgmdDeserializer.SubmeshMaterialInfo[] materialInfos, string colName)
    {
        int materialCount = materialInfos != null ? materialInfos.Length : 0;
        var materials = new List<Material>(Math.Max(materialCount, 1));

        if (materialCount == 0)
        {
            var fallback = new Material(GetDefaultShader());
            fallback.color = Color.magenta;
            materials.Add(fallback);
            return materials;
        }

        for (int i = 0; i < materialInfos.Length; i++)
        {
            var info = materialInfos[i];
            var material = new Material(GetDefaultShader());

            if (info.isSolidColor)
            {
                material.color = FFITextureLoader.GetPaletteColor(info.colorIndex, colName);
            }
            else
            {
                Texture2D texture = FFITextureLoader.DecodeTexture(info.textureId, info.imageId, colName);
                if (texture != null)
                {
                    material.mainTexture = texture;
                }
                else
                {
                    material.color = Color.magenta;
                }
            }

            materials.Add(material);
        }

        return materials;
    }

    private static GameObject CreateGameObject(string name, Mesh mesh, List<Material> materials, int frameCount)
    {
        var obj = new GameObject(name);
        if (mesh == null)
        {
            return obj;
        }

        if (frameCount > 0)
        {
            SkinnedMeshRenderer smr = obj.AddComponent<SkinnedMeshRenderer>();
            smr.sharedMesh = mesh;
            smr.SetMaterials(materials);

            BlendShapeAnimator animator = obj.AddComponent<BlendShapeAnimator>();
            animator.Initialize(smr);
        }
        else
        {
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
            meshRenderer.SetMaterials(materials);
        }

        MeshCollider meshCollider = obj.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        return obj;
    }

    private static Shader GetDefaultShader()
    {
        if (defaultShader == null)
        {
            defaultShader = Shader.Find("Universal Render Pipeline/Lit");
            if (defaultShader == null)
            {
                defaultShader = Shader.Find("Standard");
            }
        }

        return defaultShader;
    }
}
