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

    public static List<GameObject> LoadArea(string areaName, string paletteName, string wldName)
    {
        var objects = new List<GameObject>();
        string artFolder = Game.pathManager.GetArtFolder();
        string mapsFolder = Game.pathManager.GetMapsFolder();

        string robPath = Path.Combine(artFolder, areaName + ".ROB");
        var meshDict = new Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(robPath))
        {
            byte[] robBytes = File.ReadAllBytes(robPath);
            var robPin = GCHandle.Alloc(robBytes, GCHandleType.Pinned);
            try
            {
                IntPtr resultPtr = RgpreBindings.ParseRobData(robPin.AddrOfPinnedObject(), robBytes.Length);
                if (resultPtr != IntPtr.Zero)
                {
                    byte[] rgmdBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
                    var segments = RgmdDeserializer.DeserializeRobWithMaterials(rgmdBytes, out _, out _);
                    foreach (var seg in segments)
                    {
                        if (!meshDict.ContainsKey(seg.name))
                        {
                            int frameCount = seg.mesh != null ? seg.mesh.blendShapeCount : 0;
                            meshDict[seg.name] = (seg.mesh, seg.materials, frameCount);
                        }
                    }
                }
            }
            finally
            {
                robPin.Free();
            }
        }

        string rgmPath = Path.Combine(mapsFolder, areaName + ".RGM");
        if (File.Exists(rgmPath))
        {
            byte[] rgmBytes = File.ReadAllBytes(rgmPath);
            var rgmPin = GCHandle.Alloc(rgmBytes, GCHandleType.Pinned);
            try
            {
                IntPtr resultPtr = RgpreBindings.ParseRgmPlacements(rgmPin.AddrOfPinnedObject(), rgmBytes.Length);
                if (resultPtr != IntPtr.Zero)
                {
                    byte[] rgplBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
                    var rgpl = RgplDeserializer.Deserialize(rgplBytes);

                    int skippedEmpty = 0;
                    int skippedNoMesh = 0;
                    int placed = 0;
                    var missingModels = new HashSet<string>();

                    foreach (var placement in rgpl.placements)
                    {
                        string modelName = placement.modelName;

                        // Flat sprite — create a textured quad
                        if (placement.textureId > 0 && string.IsNullOrEmpty(modelName))
                        {
                            GameObject quad = CreateFlatSprite(placement, paletteName);
                            if (quad != null)
                            {
                                objects.Add(quad);
                                placed++;
                            }
                            else
                            {
                                skippedNoMesh++;
                            }
                            continue;
                        }

                        if (string.IsNullOrEmpty(modelName))
                        {
                            skippedEmpty++;
                            continue;
                        }

                        Mesh mesh = null;
                        RgmdDeserializer.SubmeshMaterialInfo[] matInfos = null;
                        int frameCount = 0;

                        if (meshDict.TryGetValue(modelName, out var cached))
                        {
                            mesh = cached.mesh;
                            matInfos = cached.materials;
                            frameCount = cached.frameCount;
                        }
                        else
                        {
                            var loaded = TryLoadModelFile(modelName);
                            if (loaded.HasValue)
                            {
                                mesh = loaded.Value.mesh;
                                matInfos = loaded.Value.materials;
                                frameCount = loaded.Value.frameCount;
                                meshDict[modelName] = loaded.Value;
                            }
                        }

                        if (mesh == null)
                        {
                            skippedNoMesh++;
                            missingModels.Add(modelName);
                            continue;
                        }

                        List<Material> materials = CreateMaterials(matInfos, paletteName);
                        GameObject obj = CreateGameObject(modelName, mesh, materials, frameCount);
                        ApplyMatrix(obj.transform, placement.transform);
                        objects.Add(obj);
                        placed++;
                    }

                    Debug.Log($"[FFI LoadArea] RGPL: {rgpl.placements.Count} placements, {rgpl.lights.Count} lights. Placed: {placed}, Skipped (empty name): {skippedEmpty}, Skipped (no mesh): {skippedNoMesh}");
                    if (missingModels.Count > 0)
                        Debug.Log($"[FFI LoadArea] Missing models: {string.Join(", ", missingModels)}");

                    foreach (var light in rgpl.lights)
                    {
                        GameObject lightObj = new GameObject("Light_" + light.name);
                        Light lightComp = lightObj.AddComponent<Light>();
                        lightComp.type = LightType.Point;
                        lightComp.color = light.color;
                        lightComp.range = light.range;
                        lightObj.transform.position = light.position;
                        objects.Add(lightObj);
                    }
                }
            }
            finally
            {
                rgmPin.Free();
            }
        }

        if (!string.IsNullOrEmpty(wldName))
        {
            string wldPath = Path.Combine(mapsFolder, wldName + ".WLD");
            if (File.Exists(wldPath))
            {
                byte[] wldBytes = File.ReadAllBytes(wldPath);
                var wldPin = GCHandle.Alloc(wldBytes, GCHandleType.Pinned);
                try
                {
                    IntPtr resultPtr = RgpreBindings.ParseWldTerrainData(wldPin.AddrOfPinnedObject(), wldBytes.Length);
                    if (resultPtr != IntPtr.Zero)
                    {
                        byte[] rgmdBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
                        var (mesh, matInfos, _) = RgmdDeserializer.DeserializeModel(rgmdBytes, "Terrain");
                        if (mesh != null)
                        {
                            List<Material> materials = CreateMaterials(matInfos, paletteName);
                            GameObject terrainObj = new GameObject("Terrain");
                            terrainObj.isStatic = true;

                            MeshRenderer renderer = terrainObj.AddComponent<MeshRenderer>();
                            MeshFilter filter = terrainObj.AddComponent<MeshFilter>();
                            filter.sharedMesh = mesh;
                            renderer.SetMaterials(materials);

                            MeshCollider collider = terrainObj.AddComponent<MeshCollider>();
                            collider.sharedMesh = mesh;

                            objects.Add(terrainObj);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("[FFI] Failed to parse WLD terrain: " + RgpreBindings.GetLastErrorMessage());
                    }
                }
                finally
                {
                    wldPin.Free();
                }
            }
        }

        return objects;
    }

    private static (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)? TryLoadModelFile(string modelName)
    {
        string artFolder = Game.pathManager.GetArtFolder();

        foreach (string ext in new[] { ".3DC", ".3D" })
        {
            string path = Path.Combine(artFolder, modelName + ext);
            if (!File.Exists(path))
            {
                continue;
            }

            byte[] bytes = File.ReadAllBytes(path);
            var pin = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                IntPtr resultPtr = RgpreBindings.ParseModelData(pin.AddrOfPinnedObject(), bytes.Length);
                if (resultPtr == IntPtr.Zero)
                {
                    continue;
                }

                byte[] rgmdBytes = RgpreBindings.ExtractBytesAndFree(resultPtr);
                var (mesh, matInfos, frameCount) = RgmdDeserializer.DeserializeModel(rgmdBytes, modelName);
                if (mesh != null)
                {
                    return (mesh, matInfos, frameCount);
                }
            }
            finally
            {
                pin.Free();
            }
        }

        return null;
    }

    private static void ApplyMatrix(Transform transform, Matrix4x4 matrix)
    {
        transform.position = new Vector3(matrix.m03, matrix.m13, matrix.m23);

        Vector3 scale = new Vector3(
            new Vector3(matrix.m00, matrix.m10, matrix.m20).magnitude,
            new Vector3(matrix.m01, matrix.m11, matrix.m21).magnitude,
            new Vector3(matrix.m02, matrix.m12, matrix.m22).magnitude
        );
        transform.localScale = scale;

        if (scale.x > 0f && scale.y > 0f && scale.z > 0f)
        {
            Matrix4x4 rotMatrix = Matrix4x4.identity;
            rotMatrix.SetColumn(0, matrix.GetColumn(0) / scale.x);
            rotMatrix.SetColumn(1, matrix.GetColumn(1) / scale.y);
            rotMatrix.SetColumn(2, matrix.GetColumn(2) / scale.z);
            transform.rotation = rotMatrix.rotation;
        }
    }

    private static Mesh flatQuadMesh;

    private static GameObject CreateFlatSprite(RgplDeserializer.Placement placement, string paletteName)
    {
        Texture2D tex = FFITextureLoader.DecodeTexture(placement.textureId, placement.imageId, paletteName);
        if (tex == null) return null;

        if (flatQuadMesh == null)
        {
            flatQuadMesh = new Mesh();
            flatQuadMesh.name = "FlatQuad";
            flatQuadMesh.vertices = new Vector3[]
            {
                new Vector3(-0.5f, 0f, 0f),
                new Vector3( 0.5f, 0f, 0f),
                new Vector3( 0.5f, 1f, 0f),
                new Vector3(-0.5f, 1f, 0f)
            };
            flatQuadMesh.uv = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };
            flatQuadMesh.triangles = new int[] { 0, 2, 1, 0, 3, 2 };
            flatQuadMesh.normals = new Vector3[]
            {
                Vector3.back, Vector3.back, Vector3.back, Vector3.back
            };
        }

        string label = !string.IsNullOrEmpty(placement.sourceId) ? placement.sourceId : "Flat_" + placement.textureId + "_" + placement.imageId;
        GameObject obj = new GameObject(label);

        MeshFilter mf = obj.AddComponent<MeshFilter>();
        mf.sharedMesh = flatQuadMesh;

        MeshRenderer mr = obj.AddComponent<MeshRenderer>();
        Material mat = new Material(GetDefaultShader());
        mat.mainTexture = tex;
        mr.sharedMaterial = mat;

        ApplyMatrix(obj.transform, placement.transform);

        // Scale quad to match texture aspect ratio
        float aspect = (float)tex.width / tex.height;
        obj.transform.localScale = new Vector3(
            obj.transform.localScale.x * aspect,
            obj.transform.localScale.y,
            obj.transform.localScale.z
        );

        return obj;
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
