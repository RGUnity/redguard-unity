using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using RGFileImport;
using Debug = UnityEngine.Debug;

public static class FFIModelLoader
{
    private static Shader defaultShader;
    public static Dictionary<uint, RGScriptedObject> scriptedObjects = new Dictionary<uint, RGScriptedObject>();
    public static RGRGMFile CurrentRgmData { get; private set; } = new RGRGMFile();

    // Global caches — persist across LoadArea/LoadModel/TryGetMeshData calls
    private static readonly Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshCache
        = new Dictionary<string, (Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>(StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, List<(string name, Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)>> robCache
        = new Dictionary<string, List<(string, Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>>(StringComparer.OrdinalIgnoreCase);

    private static readonly Dictionary<string, Material> materialCache
        = new Dictionary<string, Material>();

    public static void ClearCache()
    {
        meshCache.Clear();
        robCache.Clear();
        materialCache.Clear();
        FFITextureLoader.ClearCache();
        FFIGxaLoader.ClearCache();
    }

    [Serializable]
    private class RgmMetadataRoot
    {
        public List<MpobMetadataEntry> mpob_objects;
    }

    [Serializable]
    private class MpobMetadataEntry
    {
        public uint id;
        public string script_name;
        public string model_name;
        public string object_type;
        public float[] position;
        public float[] angles;
        public int radius;
        public int intensity;
        public int red;
        public int green;
        public int blue;
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
        string assetsDir = Game.pathManager.GetRootFolder();
        string robPath = Path.Combine(artFolder, robName + ".ROB");
        if (!File.Exists(robPath))
        {
            Debug.LogError("[FFI] ROB not found: " + robPath);
            return new List<GameObject>();
        }

        if (!robCache.TryGetValue(robName, out var cachedSegments))
        {
            IntPtr resultPtr = RgpreBindings.ParseRobData(robPath);
            if (resultPtr == IntPtr.Zero)
            {
                Debug.LogError("[FFI] Failed to parse ROB " + robName + ": " + RgpreBindings.GetLastErrorMessage());
                return new List<GameObject>();
            }

            var segments = RgmdDeserializer.DeserializeRobWithMaterials(resultPtr, out _, out _);
            cachedSegments = new List<(string, Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>(segments.Count);
            foreach (var seg in segments)
            {
                int fc = seg.mesh != null ? seg.mesh.blendShapeCount : 0;
                cachedSegments.Add((seg.name, seg.mesh, seg.materials, fc));
            }
            robCache[robName] = cachedSegments;
        }

        var objects = new List<GameObject>(cachedSegments.Count);
        foreach (var seg in cachedSegments)
        {
            List<Material> materials = CreateMaterials(seg.materials, colName);
            objects.Add(CreateGameObject(seg.name, seg.mesh, materials, seg.frameCount));
        }

        return objects;
    }

    public static List<GameObject> LoadArea(string areaName, string paletteName, string wldName)
    {
        var swTotal = Stopwatch.StartNew();
        var objects = new List<GameObject>();
        scriptedObjects = new Dictionary<uint, RGScriptedObject>();
        CurrentRgmData = new RGRGMFile();
        string artFolder = Game.pathManager.GetArtFolder();
        string mapsFolder = Game.pathManager.GetMapsFolder();
        string assetsDir = Game.pathManager.GetRootFolder();

        string robPath = Path.Combine(artFolder, areaName + ".ROB");
        var meshDict = new Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)>(StringComparer.OrdinalIgnoreCase);

        if (File.Exists(robPath))
        {
            if (!robCache.TryGetValue(areaName, out var cachedSegments))
            {
                IntPtr resultPtr = RgpreBindings.ParseRobData(robPath);
                if (resultPtr != IntPtr.Zero)
                {
                    var segments = RgmdDeserializer.DeserializeRobWithMaterials(resultPtr, out _, out _);
                    cachedSegments = new List<(string, Mesh, RgmdDeserializer.SubmeshMaterialInfo[], int)>(segments.Count);
                    foreach (var seg in segments)
                    {
                        int fc = seg.mesh != null ? seg.mesh.blendShapeCount : 0;
                        cachedSegments.Add((seg.name, seg.mesh, seg.materials, fc));
                    }
                    robCache[areaName] = cachedSegments;
                }
            }

            if (cachedSegments != null)
            {
                foreach (var seg in cachedSegments)
                {
                    if (!meshDict.ContainsKey(seg.name))
                    {
                        meshDict[seg.name] = (seg.mesh, seg.materials, seg.frameCount);
                        meshCache[seg.name] = (seg.mesh, seg.materials, seg.frameCount);
                    }
                }
            }
        }

        string rgmPath = Path.Combine(mapsFolder, areaName + ".RGM");
        if (File.Exists(rgmPath))
        {
            LoadRgmSections(rgmPath, paletteName, meshDict, objects);
        }

        if (File.Exists(rgmPath))
        {
            IntPtr resultPtr = RgpreBindings.ParseRgmPlacements(rgmPath);
            if (resultPtr != IntPtr.Zero)
            {
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

                Debug.Log($"[FFI LoadArea] {rgpl.placements.Count} placements, {rgpl.lights.Count} lights. Placed: {placed}, Skipped (empty): {skippedEmpty}, Skipped (no mesh): {skippedNoMesh}");
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

        if (!string.IsNullOrEmpty(wldName))
        {
            string wldPath = Path.Combine(mapsFolder, wldName + ".WLD");
            if (File.Exists(wldPath))
            {
                IntPtr resultPtr = RgpreBindings.ParseWldTerrainData(wldPath);
                if (resultPtr != IntPtr.Zero)
                {
                    var (mesh, matInfos, _) = RgmdDeserializer.DeserializeModel(resultPtr, "Terrain");
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
        }
        swTotal.Stop();
        Debug.Log($"[FFI LoadArea] Total={swTotal.ElapsedMilliseconds}ms");

        return objects;
    }

    private static (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)? TryLoadModelFile(string modelName)
    {
        if (meshCache.TryGetValue(modelName, out var cached))
            return cached;

        string artFolder = Game.pathManager.GetArtFolder();

        foreach (string ext in new[] { ".3DC", ".3D" })
        {
            string path = Path.Combine(artFolder, modelName + ext);
            if (!File.Exists(path))
            {
                continue;
            }

            IntPtr resultPtr = RgpreBindings.ParseModelData(path);
            if (resultPtr == IntPtr.Zero)
            {
                continue;
            }

            var (mesh, matInfos, frameCount) = RgmdDeserializer.DeserializeModel(resultPtr, modelName);
            if (mesh != null)
            {
                var result = (mesh, matInfos, frameCount);
                meshCache[modelName] = result;
                return result;
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

            IntPtr resultPtr = RgpreBindings.ParseModelData(modelPath);
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

            meshCache[modelName] = (mesh, materialInfos, frameCount);
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
                matKey = colName + "_solid_" + info.colorIndex;
            else
                matKey = colName + "_tex_" + info.textureId + "_" + info.imageId;

            if (materialCache.TryGetValue(matKey, out Material cachedMat))
            {
                materials.Add(cachedMat);
                continue;
            }

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
                    // UVs from DLL are divided by 16.0 only (pixel-space).
                    // Scale by 1/textureDimensions to get normalized 0-1 UVs.
                    material.mainTextureScale = new Vector2(1f / texture.width, 1f / texture.height);
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
        {
            return false;
        }

        mesh = loaded.Value.mesh;
        frameCount = loaded.Value.frameCount;
        materials = CreateMaterials(loaded.Value.materials, colName);
        return true;
    }

    private static byte[] GetSection(string rgmPath, string tag)
    {
        IntPtr ptr = RgpreBindings.GetRgmSection(rgmPath, tag, 0);
        if (ptr == IntPtr.Zero) return null;
        return RgpreBindings.ExtractBytesAndFree(ptr);
    }

    private static void LoadRgmSections(
        string rgmPath,
        string paletteName,
        Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshDict,
        List<GameObject> objects)
    {
        CurrentRgmData = new RGRGMFile();

        // Load raw sections via FFI
        byte[] rahdBytes = GetSection(rgmPath, "RAHD");
        byte[] mpobBytes = GetSection(rgmPath, "MPOB");
        byte[] raatBytes = GetSection(rgmPath, "RAAT");
        byte[] ranmBytes = GetSection(rgmPath, "RANM");
        byte[] ralcBytes = GetSection(rgmPath, "RALC");
        byte[] raanBytes = GetSection(rgmPath, "RAAN");
        byte[] ragrBytes = GetSection(rgmPath, "RAGR");
        byte[] rastBytes = GetSection(rgmPath, "RAST");
        byte[] rasbBytes = GetSection(rgmPath, "RASB");
        byte[] ravaBytes = GetSection(rgmPath, "RAVA");
        byte[] rascBytes = GetSection(rgmPath, "RASC");
        byte[] rahkBytes = GetSection(rgmPath, "RAHK");

        // Populate raw byte sections
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

        // Parse RALC items (12 bytes each: 3x int32)
        if (ralcBytes != null && ralcBytes.Length >= 12)
        {
            int count = ralcBytes.Length / 12;
            CurrentRgmData.RALC.items = new List<RGRGMFile.RGMRALCItem>(count);
            for (int i = 0; i < count; i++)
            {
                int off = i * 12;
                CurrentRgmData.RALC.items.Add(new RGRGMFile.RGMRALCItem
                {
                    offsetX = BitConverter.ToInt32(ralcBytes, off),
                    offsetY = BitConverter.ToInt32(ralcBytes, off + 4),
                    offsetZ = BitConverter.ToInt32(ralcBytes, off + 8)
                });
            }
        }

        // Parse RAHD (structured — uses MemoryReader)
        if (rahdBytes != null && rahdBytes.Length > 4)
        {
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

        // Parse MPOB and build scripted objects (66 bytes per item)
        if (mpobBytes != null && mpobBytes.Length > 4)
        {
            var reader = new RGFileImport.MemoryReader(mpobBytes);
            uint numItems = (uint)reader.ReadInt32();
            for (int i = 0; i < (int)numItems; i++)
            {
                var mpob = new RGRGMFile.RGMMPOBItem();
                mpob.id = (uint)reader.ReadInt32();                     // 4 bytes
                mpob.type = (RGRGMFile.ObjectType)reader.ReadByte();    // 1 byte
                byte isActive = reader.ReadByte();                      // 1 byte

                char[] scriptChars = reader.ReadChars(9);               // 9 bytes
                mpob.scriptName = new string(scriptChars).Split('\0')[0];

                char[] modelChars = reader.ReadChars(9);                // 9 bytes
                string rawModel = new string(modelChars).Split('\0')[0];
                mpob.modelName = NormalizeModelName(rawModel);

                reader.ReadByte();                                      // isStatic 1 byte
                reader.ReadInt16();                                     // unknown1 2 bytes

                // positions are 24-bit + 1 padding byte each
                mpob.posX = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
                reader.ReadByte();
                mpob.posY = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);
                reader.ReadByte();
                mpob.posZ = reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16);

                mpob.anglex = reader.ReadInt32();                       // 4 bytes
                mpob.angley = reader.ReadInt32();                       // 4 bytes
                mpob.anglez = reader.ReadInt32();                       // 4 bytes

                short textureData = reader.ReadInt16();                 // 2 bytes
                // textureId = high 9 bits, imageId = low 7 bits
                mpob.intensity = reader.ReadInt16();                    // 2 bytes
                mpob.radius = reader.ReadInt16();                       // 2 bytes
                reader.ReadInt16();                                     // modelId 2 bytes
                reader.ReadInt16();                                     // worldId 2 bytes
                mpob.red = reader.ReadInt16();                          // 2 bytes
                mpob.green = reader.ReadInt16();                        // 2 bytes
                mpob.blue = reader.ReadInt16();                         // 2 bytes

                try
                {
                    string objectName = "B_" + i.ToString("D3") + "_" + (mpob.scriptName ?? "OBJ");
                    GameObject go = new GameObject(objectName);
                    go.AddComponent<RGScriptedObject>();
                    RGScriptedObject scripted = go.GetComponent<RGScriptedObject>();
                    scripted.Instanciate(mpob, CurrentRgmData, paletteName);
                    scriptedObjects[mpob.id] = scripted;
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

        // Initialize animation and script stores
        try { RGRGMAnimStore.ReadAnim(CurrentRgmData); } catch (Exception ex) { Debug.LogWarning("[FFI] AnimStore init: " + ex.Message); }
        try { RGRGMScriptStore.ReadScript(CurrentRgmData); } catch (Exception ex) { Debug.LogWarning("[FFI] ScriptStore init: " + ex.Message); }
    }

    private static void BuildRuntimeObjectsFromMetadata(
        byte[] metadataBytes,
        string paletteName,
        Dictionary<string, (Mesh mesh, RgmdDeserializer.SubmeshMaterialInfo[] materials, int frameCount)> meshDict,
        List<GameObject> worldObjects)
    {
        string metadataJson = Encoding.UTF8.GetString(metadataBytes);
        RgmMetadataRoot metadata = JsonUtility.FromJson<RgmMetadataRoot>(metadataJson);
        if (metadata == null || metadata.mpob_objects == null)
        {
            return;
        }

        CurrentRgmData = new RGRGMFile();
        CurrentRgmData.RAAT.attributes = new byte[Math.Max(metadata.mpob_objects.Count * 256, 256)];

        int index = 0;
        foreach (MpobMetadataEntry entry in metadata.mpob_objects)
        {
            var ra = new RGRGMFile.RGMRAHDItem
            {
                scriptName = entry.script_name ?? string.Empty,
                index = index,
                textureId = 0,
                RALCOffset = 0,
                RALCCount = 0,
                RANMOffset = 0,
                RANMLength = 0
            };
            CurrentRgmData.RAHD.dict[ra.scriptName] = ra;

            var mpob = new RGRGMFile.RGMMPOBItem
            {
                id = entry.id,
                scriptName = entry.script_name ?? string.Empty,
                modelName = NormalizeModelName(entry.model_name),
                type = ParseObjectType(entry.object_type),
                posX = ReadInt(entry.position, 0),
                posY = ReadInt(entry.position, 1),
                posZ = ReadInt(entry.position, 2),
                anglex = ReadInt(entry.angles, 0),
                angley = ReadInt(entry.angles, 1),
                anglez = ReadInt(entry.angles, 2),
                radius = entry.radius,
                intensity = entry.intensity,
                red = entry.red,
                green = entry.green,
                blue = entry.blue
            };

            try
            {
                string objectName = "B_" + index.ToString("D3") + "_" + (mpob.scriptName ?? "OBJ");
                GameObject go = new GameObject(objectName);
                go.AddComponent<RGScriptedObject>();
                RGScriptedObject scripted = go.GetComponent<RGScriptedObject>();
                scripted.Instanciate(mpob, CurrentRgmData, paletteName);
                scriptedObjects[mpob.id] = scripted;
                worldObjects.Add(go);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("[FFI] Failed to spawn scripted object " + mpob.scriptName + ": " + ex.Message);
            }

            if (!string.IsNullOrEmpty(mpob.modelName) && !meshDict.ContainsKey(mpob.modelName))
            {
                var loaded = TryLoadModelFile(mpob.modelName);
                if (loaded.HasValue)
                {
                    meshDict[mpob.modelName] = loaded.Value;
                }
            }

            index++;
        }
    }

    private static int ReadInt(float[] values, int index)
    {
        if (values == null || values.Length <= index)
        {
            return 0;
        }

        return Mathf.RoundToInt(values[index]);
    }

    private static string NormalizeModelName(string modelName)
    {
        if (string.IsNullOrEmpty(modelName))
        {
            return string.Empty;
        }

        string normalized = modelName.Replace('\\', '/');
        int slash = normalized.LastIndexOf('/');
        string file = slash >= 0 ? normalized.Substring(slash + 1) : normalized;
        int dot = file.LastIndexOf('.');
        string stem = dot > 0 ? file.Substring(0, dot) : file;
        return stem.ToUpperInvariant();
    }

    private static RGRGMFile.ObjectType ParseObjectType(string objectType)
    {
        return objectType switch
        {
            "object_lightobject" => RGRGMFile.ObjectType.object_lightobject,
            "object_light" => RGRGMFile.ObjectType.object_light,
            "object_sound" => RGRGMFile.ObjectType.object_sound,
            _ => RGRGMFile.ObjectType.object_3d
        };
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
