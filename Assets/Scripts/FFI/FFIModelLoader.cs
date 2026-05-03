using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using RGFileImport;
using Debug = UnityEngine.Debug;

public static class FFIModelLoader
{
    private static Shader defaultShader;
    public static Dictionary<uint, RGScriptedObject> ScriptedObjects = new Dictionary<uint, RGScriptedObject>();
    public static RGRGMFile CurrentRgmData { get; private set; } = new RGRGMFile();
    public static IntPtr CurrentWorldHandle { get; private set; } = IntPtr.Zero;
    public static string CurrentWorldContextKey { get; private set; } = string.Empty;

    private static int ReadSignedInt24(MemoryReader reader)
    {
        int value = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
        if ((value & 0x00800000) != 0)
        {
            value |= unchecked((int)0xFF000000);
        }

        return value;
    }

    private static int ReadUnsignedInt24(MemoryReader reader)
        => reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

    private static int ReadLegacyShiftedInt24(MemoryReader reader)
    {
        int b0 = reader.ReadByte();
        int b1 = reader.ReadByte();
        int b2 = reader.ReadByte();
        return (b0 << 8) | (b1 << 16) | (b2 << 24);
    }

    // Global caches — persist across LoadArea/LoadModel/TryGetMeshData calls
    private static readonly Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshCache
        = new Dictionary<string, (Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>(StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, List<(string name, Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)>> robCache
        = new Dictionary<string, List<(string, Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>>(StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, Material> materialCache
        = new Dictionary<string, Material>(StringComparer.OrdinalIgnoreCase);

    public static void CloseWorldContext()
    {
        if (CurrentWorldHandle != IntPtr.Zero)
        {
            RgpreBindings.CloseWorld(CurrentWorldHandle);
            CurrentWorldHandle = IntPtr.Zero;
        }

        CurrentWorldContextKey = string.Empty;
    }

    public static bool OpenWorldContext(int worldId)
    {
        CloseWorldContext();
        CurrentWorldHandle = RgpreBindings.OpenWorld(Game.pathManager.GetRootFolder(), worldId);
        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return false;
        }

        CurrentWorldContextKey = "world:" + worldId;
        return true;
    }

    public static bool OpenExplicitWorldContext(string rgmName, string wldName, string paletteName)
    {
        CloseWorldContext();

        string mapsFolder = Game.pathManager.GetMapsFolder();
        string rgmPath = string.IsNullOrEmpty(rgmName) ? string.Empty : Path.Combine(mapsFolder, rgmName + ".RGM");
        string wldPath = string.IsNullOrEmpty(wldName) ? string.Empty : Path.Combine(mapsFolder, wldName + ".WLD");

        CurrentWorldHandle = RgpreBindings.OpenWorldExplicit(
            Game.pathManager.GetRootFolder(),
            rgmPath,
            wldPath,
            paletteName);
        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return false;
        }

        CurrentWorldContextKey = $"explicit:{rgmName}:{wldName}:{paletteName}";
        return true;
    }

    public static bool OpenPaletteContext(string paletteName)
    {
        CloseWorldContext();
        CurrentWorldHandle = RgpreBindings.OpenWorldExplicit(
            Game.pathManager.GetRootFolder(),
            string.Empty,
            string.Empty,
            paletteName);
        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return false;
        }

        CurrentWorldContextKey = "palette:" + paletteName;
        return true;
    }

    private static string ContextualKey(string id)
        => string.IsNullOrEmpty(CurrentWorldContextKey) ? id : CurrentWorldContextKey + "::" + id;

    public static void ClearCache()
    {
        foreach (var entry in meshCache.Values)
        {
            if (entry.mesh != null)
                UnityEngine.Object.Destroy(entry.mesh);
        }
        meshCache.Clear();

        foreach (var segments in robCache.Values)
        {
            foreach (var seg in segments)
            {
                if (seg.mesh != null)
                    UnityEngine.Object.Destroy(seg.mesh);
            }
        }
        robCache.Clear();

        foreach (var mat in materialCache.Values)
        {
            if (mat != null)
                UnityEngine.Object.Destroy(mat);
        }
        materialCache.Clear();

        FFITextureLoader.ClearCache();
        FFIGxaLoader.ClearCache();
    }

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

        var cachedSegments = EnsureRobCached(robName, robPath);
        if (cachedSegments == null)
        {
            Debug.LogError("[FFI] Failed to parse ROB " + robName + ": " + RgpreBindings.GetLastErrorMessage());
            return new List<GameObject>();
        }

        var objects = new List<GameObject>(cachedSegments.Count);
        foreach (var seg in cachedSegments)
        {
            List<Material> materials = CreateMaterials(seg.materials, colName);
            objects.Add(CreateGameObject(seg.name, seg.mesh, materials, seg.frameCount));
        }

        return objects;
    }

    /// <summary>
    /// Parses and caches ROB segments. Returns null only if the native parse fails.
    /// </summary>
    private static List<(string name, Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> EnsureRobCached(string robName, string robPath)
    {
        string cacheKey = ContextualKey(robName);
        if (robCache.TryGetValue(cacheKey, out var cachedSegments))
        {
            return cachedSegments;
        }

        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return null;
        }

        IntPtr resultPtr = RgpreBindings.ParseRobDataWorld(CurrentWorldHandle, robPath);
        if (resultPtr == IntPtr.Zero)
        {
            return null;
        }

        var segments = RgmdDeserializer.DeserializeRobWithMaterials(resultPtr, out _, out _);
        cachedSegments = new List<(string, Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>(segments.Count);
        foreach (var seg in segments)
        {
            int fc = seg.mesh != null ? seg.mesh.blendShapeCount : 0;
            cachedSegments.Add((seg.name, seg.mesh, seg.materials, fc));
        }
        robCache[cacheKey] = cachedSegments;
        return cachedSegments;
    }

    public static List<GameObject> LoadArea(string areaName, string paletteName, string wldName)
    {
        var swTotal = Stopwatch.StartNew();
        var objects = new List<GameObject>();
        ScriptedObjects = new Dictionary<uint, RGScriptedObject>();
        CurrentRgmData = new RGRGMFile();
        string artFolder = Game.pathManager.GetArtFolder();
        string mapsFolder = Game.pathManager.GetMapsFolder();

        string robPath = Path.Combine(artFolder, areaName + ".ROB");
        var meshDict = new Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(robPath))
        {
            var cachedSegments = EnsureRobCached(areaName, robPath);
            if (cachedSegments != null)
            {
                foreach (var seg in cachedSegments)
                {
                    if (!meshDict.ContainsKey(seg.name))
                    {
                        meshDict[seg.name] = (seg.mesh, seg.materials, seg.frameCount);
                        meshCache[ContextualKey(seg.name)] = (seg.mesh, seg.materials, seg.frameCount);
                    }
                }
            }
        }

        string rgmPath = Path.Combine(mapsFolder, areaName + ".RGM");
        if (File.Exists(rgmPath))
        {
            LoadRgmSections(paletteName, meshDict, objects);
        }

        if (CurrentWorldHandle != IntPtr.Zero)
        {
            PlaceRgplObjects(paletteName, meshDict, objects);
        }

        LoadTerrain(paletteName, objects);

        swTotal.Stop();
        Debug.Log($"[FFI LoadArea] Total={swTotal.ElapsedMilliseconds}ms");

        return objects;
    }

    private static void PlaceRgplObjects(
        string paletteName,
        Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshDict,
        List<GameObject> objects)
    {
        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return;
        }

        IntPtr resultPtr = RgpreBindings.GetWorldPlacements(CurrentWorldHandle);
        if (resultPtr == IntPtr.Zero)
        {
            return;
        }

        var rgpl = RgplDeserializer.Deserialize(resultPtr);

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
                GameObject quad = CreateFlatSprite(placement);
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
            string objectName = !string.IsNullOrEmpty(placement.sourceId) ? placement.sourceId : modelName;
            GameObject obj = CreateGameObject(objectName, mesh, materials, frameCount);
            ApplyMatrix(obj.transform, placement.transform);
            objects.Add(obj);
            placed++;
        }

        Debug.Log($"[FFI LoadArea] {rgpl.placements.Count} placements, {rgpl.lights.Count} lights. Placed: {placed}, Skipped (empty): {skippedEmpty}, Skipped (no mesh): {skippedNoMesh}");
        if (missingModels.Count > 0)
            Debug.Log($"[FFI LoadArea] Missing models: {string.Join(", ", missingModels)}");

        CreateLights(rgpl.lights, objects);
    }

    private static void CreateLights(List<RgplDeserializer.LightData> lights, List<GameObject> objects)
    {
        foreach (var light in lights)
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

    private static void LoadTerrain(string paletteName, List<GameObject> objects)
    {
        if (CurrentWorldHandle == IntPtr.Zero)
        {
            return;
        }

        IntPtr resultPtr = RgpreBindings.GetWorldTerrain(CurrentWorldHandle);
        if (resultPtr == IntPtr.Zero)
        {
            Debug.LogWarning("[FFI] Failed to parse WLD terrain: " + RgpreBindings.GetLastErrorMessage());
            return;
        }

        var (mesh, matInfos, _) = RgmdDeserializer.DeserializeModel(resultPtr, "Terrain");
        if (mesh == null)
        {
            return;
        }

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

    private static (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)? TryLoadModelFile(string modelName)
    {
        string cacheKey = ContextualKey(modelName);
        if (meshCache.TryGetValue(cacheKey, out var cached))
            return cached;

        string artFolder = Game.pathManager.GetArtFolder();

        foreach (string ext in new[] { ".3DC", ".3D" })
        {
            string path = Path.Combine(artFolder, modelName + ext);
            if (!File.Exists(path))
            {
                continue;
            }

            if (CurrentWorldHandle == IntPtr.Zero)
            {
                return null;
            }

            IntPtr resultPtr = RgpreBindings.ParseModelDataWorld(CurrentWorldHandle, path);
            if (resultPtr == IntPtr.Zero)
            {
                continue;
            }

            var (mesh, matInfos, frameCount) = RgmdDeserializer.DeserializeModel(resultPtr, modelName);
            if (mesh != null)
            {
                var result = (mesh, matInfos, frameCount);
                meshCache[cacheKey] = result;
                return result;
            }
        }

        return null;
    }

    private static void ApplyMatrix(Transform transform, Matrix4x4 matrix)
    {
        // RGPL/MPS placement matrices arrive in the exported scene convention, where X is
        // reflected relative to Unity. Convert with M' = S * M * S, S = diag(-1, 1, 1).
        // For a column-major 4x4 that means negating the row-0 XOR col-0 terms:
        //   m10, m20, m01, m02, m03
        matrix.m10 = -matrix.m10;
        matrix.m20 = -matrix.m20;
        matrix.m01 = -matrix.m01;
        matrix.m02 = -matrix.m02;
        matrix.m03 = -matrix.m03;

        transform.position = new Vector3(matrix.m03, matrix.m13, matrix.m23);

        Vector3 basisX = new Vector3(matrix.m00, matrix.m10, matrix.m20);
        Vector3 basisY = new Vector3(matrix.m01, matrix.m11, matrix.m21);
        Vector3 basisZ = new Vector3(matrix.m02, matrix.m12, matrix.m22);

        Vector3 scale = new Vector3(basisX.magnitude, basisY.magnitude, basisZ.magnitude);

        if (scale.x > 0f && scale.y > 0f && scale.z > 0f)
        {
            Vector3 rotX = basisX / scale.x;
            Vector3 rotY = basisY / scale.y;
            Vector3 rotZ = basisZ / scale.z;

            if (Vector3.Dot(Vector3.Cross(rotX, rotY), rotZ) < 0f)
            {
                scale.x = -scale.x;
                rotX = -rotX;
            }

            transform.localScale = scale;

            Matrix4x4 rotMatrix = Matrix4x4.identity;
            rotMatrix.SetColumn(0, new Vector4(rotX.x, rotX.y, rotX.z, 0f));
            rotMatrix.SetColumn(1, new Vector4(rotY.x, rotY.y, rotY.z, 0f));
            rotMatrix.SetColumn(2, new Vector4(rotZ.x, rotZ.y, rotZ.z, 0f));
            transform.rotation = rotMatrix.rotation;
            return;
        }

        transform.localScale = scale;
    }

    private static Mesh flatQuadMesh;

    private static GameObject CreateFlatSprite(RgplDeserializer.Placement placement)
    {
        Texture2D tex = FFITextureLoader.DecodeTexture(placement.textureId, placement.imageId);
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
        var loaded = TryLoadModelFile(modelName);
        if (!loaded.HasValue || loaded.Value.mesh == null)
        {
            string artFolder = Game.pathManager.GetArtFolder();
            string modelPath = Path.Combine(artFolder, modelName + extension);
            if (!File.Exists(modelPath))
            {
                Debug.LogError("[FFI] Model not found: " + modelPath);
                return new GameObject(displayName);
            }

            if (CurrentWorldHandle == IntPtr.Zero)
            {
                Debug.LogError("[FFI] No active world context for model load: " + modelName);
                return new GameObject(displayName);
            }

            IntPtr resultPtr = RgpreBindings.ParseModelDataWorld(CurrentWorldHandle, modelPath);
            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[FFI] Failed to parse " + modelName + extension + ": " + RgpreBindings.GetLastErrorMessage());
                return new GameObject(displayName);
            }

            var (mesh, materialInfos, frameCount) = RgmdDeserializer.DeserializeModel(resultPtr, displayName);
            if (mesh == null)
            {
                return new GameObject(displayName);
            }

            meshCache[ContextualKey(modelName)] = (mesh, materialInfos, frameCount);
            List<Material> mats = CreateMaterials(materialInfos, colName);
            return CreateGameObject(displayName, mesh, mats, frameCount);
        }

        List<Material> materials = CreateMaterials(loaded.Value.materials, colName);
        return CreateGameObject(displayName, loaded.Value.mesh, materials, loaded.Value.frameCount);
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
            string matKey;
            if (info.isSolidColor)
                matKey = "solid_" + info.solidColor.r + "_" + info.solidColor.g + "_" + info.solidColor.b;
            else
                matKey = CurrentWorldContextKey + "_tex_" + info.textureId + "_" + info.imageId;

            if (materialCache.TryGetValue(matKey, out Material cachedMat))
            {
                materials.Add(cachedMat);
                continue;
            }

            var material = new Material(GetDefaultShader());

            if (info.isSolidColor)
            {
                material.color = info.solidColor;
            }
            else
            {
                List<Texture2D> frames = FFITextureLoader.DecodeTextureAllFrames(info.textureId, info.imageId);
                if (frames != null && frames.Count > 0)
                {
                    material.mainTexture = frames[0];

                    for (int f = 0; f < frames.Count; f++)
                    {
                        material.SetTexture("FRAME_" + f, frames[f]);
                    }
                }
                else
                {
                    material.color = Color.magenta;
                }
            }

            materialCache[matKey] = material;
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

    public static bool TryGetMeshData(string modelName, string colName, out Mesh mesh, out List<Material> materials, out int frameCount)
    {
        mesh = null;
        materials = null;
        frameCount = 0;

        var loaded = TryLoadModelFile(modelName);
        if (!loaded.HasValue || loaded.Value.mesh == null)
        {
            return false;
        }

        mesh = loaded.Value.mesh;
        frameCount = loaded.Value.frameCount;
        materials = CreateMaterials(loaded.Value.materials, colName);
        return true;
    }

    public static bool TryGetFlatData(ushort textureId, byte imageId, out Mesh mesh, out List<Material> materials)
    {
        mesh = flatQuadMesh;
        materials = null;

        Texture2D tex = FFITextureLoader.DecodeTexture(textureId, imageId);
        if (tex == null)
        {
            return false;
        }

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
            mesh = flatQuadMesh;
        }

        Material material = new Material(GetDefaultShader());
        material.mainTexture = tex;
        materials = new List<Material> { material };
        return true;
    }

    private static byte[] GetSection(string tag)
    {
        if (CurrentWorldHandle == IntPtr.Zero)
            return null;

        IntPtr ptr = RgpreBindings.GetRgmSectionWorld(CurrentWorldHandle, tag, 0);
        if (ptr == IntPtr.Zero) return null;
        return RgpreBindings.ExtractBytesAndFree(ptr);
    }

    private static void LoadRgmSections(
        string paletteName,
        Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshDict,
        List<GameObject> objects)
    {
        CurrentRgmData = new RGRGMFile();

        PopulateRawSections();
        ParseRAHD(GetSection("RAHD"));
        ParseMPMK(GetSection("MPMK"));

        // Initialize animation and script stores
        try
        {
            RGRGMAnimStore.ReadAnim(CurrentRgmData);
            Debug.Log($"[FFI] AnimStore loaded {RGRGMAnimStore.Anims?.Count ?? 0} entries");
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[FFI] AnimStore init failed: " + ex.Message + "\n" + ex.StackTrace);
        }

        try
        {
            RGRGMScriptStore.ReadScript(CurrentRgmData);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[FFI] ScriptStore init: " + ex.Message);
        }

        ParseMPOB(GetSection("MPOB"), paletteName, meshDict, objects);
    }

    private static void ParseMPMK(byte[] mpmkBytes)
    {
        CurrentRgmData.MPMK.items.Clear();
        RGObjectStore.mapMarkerList = new List<Vector3>();

        if (mpmkBytes == null || mpmkBytes.Length < 4)
            return;

        var reader = new MemoryReader(mpmkBytes);
        uint numItems = (uint)reader.ReadInt32();
        for (int i = 0; i < (int)numItems; i++)
        {
            int posX = ReadLegacyShiftedInt24(reader);
            reader.ReadByte();
            int posY = ReadLegacyShiftedInt24(reader);
            reader.ReadByte();
            int posZ = ReadLegacyShiftedInt24(reader);
            reader.ReadByte();

            CurrentRgmData.MPMK.items.Add(new RGRGMFile.RGMMPMKItem
            {
                posX = posX,
                posY = posY,
                posZ = posZ,
            });

            // X: un-negate (raw formula had x_sign=-1 matching Rust; we negate back)
            // Y: keep negated (Rust convention y_sign=-1, Unity Y matches)
            // Z: keep as-is
            Vector3 markerPos = new Vector3(
                (float)posX * (1.0f / 5120.0f),
                -(float)posY * (1.0f / 5120.0f),
                -(float)(0xFFFFFF - posZ) * (1.0f / 5120.0f));
            RGObjectStore.AddMapMarker(markerPos);
        }
    }

    private static void PopulateRawSections()
    {
        byte[] raatBytes = GetSection("RAAT");
        byte[] ranmBytes = GetSection("RANM");
        byte[] ralcBytes = GetSection("RALC");
        byte[] raanBytes = GetSection("RAAN");
        byte[] ragrBytes = GetSection("RAGR");
        byte[] rastBytes = GetSection("RAST");
        byte[] rasbBytes = GetSection("RASB");
        byte[] ravaBytes = GetSection("RAVA");
        byte[] rascBytes = GetSection("RASC");
        byte[] rahkBytes = GetSection("RAHK");

        if (raatBytes != null) CurrentRgmData.RAAT.attributes = raatBytes;
        if (raanBytes != null) CurrentRgmData.RAAN.data = raanBytes;
        if (ragrBytes != null) CurrentRgmData.RAGR.data = ragrBytes;
        if (rahkBytes != null) CurrentRgmData.RAHK.data = rahkBytes;
        if (rascBytes != null) CurrentRgmData.RASC.scripts = rascBytes;

        if (ranmBytes != null)
        {
            CurrentRgmData.RANM.data = new char[ranmBytes.Length];
            for (int i = 0; i < ranmBytes.Length; i++)
                CurrentRgmData.RANM.data[i] = (char)ranmBytes[i];
        }

        if (rastBytes != null)
        {
            CurrentRgmData.RAST.text = new char[rastBytes.Length];
            for (int i = 0; i < rastBytes.Length; i++)
                CurrentRgmData.RAST.text[i] = (char)rastBytes[i];
        }

        if (rasbBytes != null && rasbBytes.Length >= 4)
        {
            CurrentRgmData.RASB.offsets = new int[rasbBytes.Length / 4];
            for (int i = 0; i < CurrentRgmData.RASB.offsets.Length; i++)
                CurrentRgmData.RASB.offsets[i] = BitConverter.ToInt32(rasbBytes, i * 4);
        }

        if (ravaBytes != null && ravaBytes.Length >= 4)
        {
            CurrentRgmData.RAVA.data = new int[ravaBytes.Length / 4];
            for (int i = 0; i < CurrentRgmData.RAVA.data.Length; i++)
                CurrentRgmData.RAVA.data[i] = BitConverter.ToInt32(ravaBytes, i * 4);
        }

        const int RalcItemSize = 12; // 3x int32
        if (ralcBytes != null && ralcBytes.Length >= RalcItemSize)
        {
            int count = ralcBytes.Length / RalcItemSize;
            CurrentRgmData.RALC.items = new List<RGRGMFile.RGMRALCItem>(count);
            for (int i = 0; i < count; i++)
            {
                int off = i * RalcItemSize;
                CurrentRgmData.RALC.items.Add(new RGRGMFile.RGMRALCItem
                {
                    offsetX = BitConverter.ToInt32(ralcBytes, off),
                    offsetY = BitConverter.ToInt32(ralcBytes, off + 4),
                    offsetZ = BitConverter.ToInt32(ralcBytes, off + 8)
                });
            }
        }
    }

    private static void ParseRAHD(byte[] rahdBytes)
    {
        if (rahdBytes == null || rahdBytes.Length <= 4)
        {
            return;
        }

        var reader = new RGFileImport.MemoryReader(rahdBytes);
        uint numItems = (uint)reader.ReadInt32();
        for (int i = 0; i < (int)numItems; i++)
        {
            var item = new RGRGMFile.RGMRAHDItem();
            item.index = i;
            item.unknown0 = reader.ReadInt32();
            item.unknown1 = reader.ReadInt32();
            char[] nameChars = reader.ReadChars(9);
            item.scriptName = new string(nameChars).Split('\0')[0];
            item.MPOBCount = reader.ReadInt32();
            item.unknown2 = reader.ReadInt32();
            item.RANMLength = reader.ReadInt32();
            item.RANMOffset = reader.ReadInt32();
            item.RAATOffset = reader.ReadInt32();
            item.RAANCount = reader.ReadInt32();
            item.RAANLength = reader.ReadInt32();
            item.RAANOffset = reader.ReadInt32();
            item.RAGRMaxGroup = reader.ReadInt32();
            item.RAGROffset = reader.ReadInt32();
            item.unknown3 = reader.ReadInt32();
            item.unknown4 = reader.ReadInt32();
            item.unknown5 = reader.ReadInt32();
            item.RASBCount = reader.ReadInt32();
            item.RASBLength = reader.ReadInt32();
            item.RASBOffset = reader.ReadInt32();
            item.RASCLength = reader.ReadInt32();
            item.RASCOffset = reader.ReadInt32();
            item.RASCThreadStart = reader.ReadInt32();
            item.RAHKLength = reader.ReadInt32();
            item.RAHKOffset = reader.ReadInt32();
            item.RALCCount = reader.ReadInt32();
            item.RALCLength = reader.ReadInt32();
            item.RALCOffset = reader.ReadInt32();
            item.RAEXLength = reader.ReadInt32();
            item.RAEXOffset = reader.ReadInt32();
            item.RAVACount = reader.ReadInt32();
            item.RAVALength = reader.ReadInt32();
            item.RAVAOffset = reader.ReadInt32();
            item.unknown6 = reader.ReadInt32();
            item.frameCount = reader.ReadInt32();
            item.MPSZNormalId = reader.ReadInt32();
            item.MPSZCombatId = reader.ReadInt32();
            item.MPSZDeadId = reader.ReadInt32();
            item.unknown7 = reader.ReadInt16();
            item.unknown8 = reader.ReadInt16();
            item.unknown9 = reader.ReadInt16();
            item.textureId = reader.ReadInt16();
            item.RAVCOffset = reader.ReadInt32();

            CurrentRgmData.RAHD.dict[item.scriptName] = item;
        }
    }

    private static void ParseMPOB(
        byte[] mpobBytes,
        string paletteName,
        Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshDict,
        List<GameObject> objects)
    {
        if (mpobBytes == null || mpobBytes.Length <= 4)
        {
            return;
        }

        var reader = new RGFileImport.MemoryReader(mpobBytes);
        uint numItems = (uint)reader.ReadInt32();
        for (int i = 0; i < (int)numItems; i++)
        {
            var mpob = new RGRGMFile.RGMMPOBItem();
            mpob.id = (uint)reader.ReadInt32();                     // 4 bytes
            mpob.type = (RGRGMFile.ObjectType)reader.ReadByte();    // 1 byte
            mpob.isActive = reader.ReadByte();                      // isActive 1 byte

            char[] scriptChars = reader.ReadChars(9);               // 9 bytes
            mpob.scriptName = new string(scriptChars).Split('\0')[0];

            char[] modelChars = reader.ReadChars(9);                // 9 bytes
            string rawModel = new string(modelChars).Split('\0')[0];
            mpob.modelName = NormalizeModelName(rawModel);

            mpob.isStatic = reader.ReadByte();                      // isStatic 1 byte
            mpob.unknown1 = reader.ReadInt16();                     // unknown1 2 bytes

            // positions are 24-bit + 1 padding byte each
            mpob.posX = ReadSignedInt24(reader);
            reader.ReadByte();
            mpob.posY = ReadSignedInt24(reader);
            reader.ReadByte();
            mpob.posZ = ReadUnsignedInt24(reader);

            mpob.anglex = reader.ReadInt32();                       // 4 bytes
            mpob.angley = reader.ReadInt32();                       // 4 bytes
            mpob.anglez = reader.ReadInt32();                       // 4 bytes

            ushort textureData = reader.ReadUInt16();               // 2 bytes
            mpob.textureId = (byte)(textureData >> 7);
            mpob.imageId = (byte)(textureData & 0x7F);
            mpob.intensity = reader.ReadInt16();                    // 2 bytes
            mpob.radius = reader.ReadInt16();                       // 2 bytes
            mpob.modelId = reader.ReadInt16();                      // modelId 2 bytes
            mpob.worldId = reader.ReadInt16();                      // worldId 2 bytes
            mpob.red = reader.ReadInt16();                          // 2 bytes
            mpob.green = reader.ReadInt16();                        // 2 bytes
            mpob.blue = reader.ReadInt16();                         // 2 bytes

            try
            {
                string objectName = "B_" + i.ToString("D3") + "_" + (mpob.scriptName ?? "OBJ");
                GameObject go = new GameObject(objectName);
                RGScriptedObject scripted = go.AddComponent<RGScriptedObject>();
                scripted.Instanciate(mpob, CurrentRgmData, paletteName);
                ScriptedObjects[mpob.id] = scripted;
                objects.Add(go);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[FFI] Failed to spawn scripted object " + mpob.scriptName + ": " + ex.Message);
            }

            if (!string.IsNullOrEmpty(mpob.modelName) && !meshDict.ContainsKey(mpob.modelName))
            {
                var loaded = TryLoadModelFile(mpob.modelName);
                if (loaded.HasValue)
                    meshDict[mpob.modelName] = loaded.Value;
            }
        }
    }

    private static string NormalizeModelName(string modelName)
        => FFIPathUtils.NormalizeModelName(modelName);

    private static Shader GetDefaultShader()
    {
        if (defaultShader == null)
        {
            defaultShader = Shader.Find("Universal Render Pipeline/Simple Lit");
            if (defaultShader == null)
            {
                defaultShader = Shader.Find("Universal Render Pipeline/Lit");
            }
            if (defaultShader == null)
            {
                defaultShader = Shader.Find("Standard");
            }
        }

        return defaultShader;
    }
}
