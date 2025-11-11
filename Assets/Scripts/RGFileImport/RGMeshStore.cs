using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;
using Unity.Profiling;

public static class RGMeshStore
{

static readonly ProfilerMarker s_load_mesh = new ProfilerMarker("LoadMesh");
static readonly ProfilerMarker s_load_WLD2mesh = new ProfilerMarker("WLD2Mesh");
static readonly ProfilerMarker s_load_FLAT2mesh = new ProfilerMarker("FLAT2Mesh");
static readonly ProfilerMarker s_load_3D2mesh = new ProfilerMarker("f3D2Mesh");
static readonly ProfilerMarker s_load_3DStore = new ProfilerMarker("LoadMesh_3DStore");
static readonly ProfilerMarker s_load_WLD = new ProfilerMarker("LoadMesh_WLD");

static readonly ProfilerMarker pm_loadmesh_blendshapes = new ProfilerMarker("pm_loadmesh_blendshapes");
static readonly ProfilerMarker pm_loadmesh_submesh = new ProfilerMarker("pm_loadmesh_submesh");
static readonly ProfilerMarker pm_loadmesh_submesh_uvs = new ProfilerMarker("pm_loadmesh_submesh_uvs");
static readonly ProfilerMarker pm_loadmesh_framedata = new ProfilerMarker("pm_loadmesh_framedata");


//using (s_load_iFLAT.Auto()){

    const int WLD_TEXID_CNT = 64;
    public enum mesh_type
    {
        mesh_3d = 0,
        mesh_flat,
    }
    public struct UnityData_WLD
    {
        public Mesh mesh;
        public List<Material> materials;
    }
    public struct UnityData_3D
    {
        public Mesh mesh;
        public List<Material> materials;
        public int framecount;
        public Vector3[] vertices;
        public Vector3[] normals;
        public Vector3[][] frameDeltaVertices;
        public Vector3[][] frameDeltaNormals;
    }

    static Dictionary<string, UnityData_3D> Mesh3DDict; // key:    
                                                        // models: 3D/meshname/colname/texoverride
                                                        // flats:  FLAT/name/colname/texoverride

    static RGMeshStore()
    {
        Mesh3DDict = new Dictionary<string, UnityData_3D>();
    }
    public static void DumpDict()
    {
        Debug.Log($"MESHES:");
        List<string> keys = new List<string>(Mesh3DDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
    }
    public static UnityData_3D LoadMesh(mesh_type type, string meshname, string name_col, int texture_override = 0)
    {
using(s_load_mesh.Auto()){
        UnityData_3D o;
        string typestr = new string("");
        switch(type)
        {
            case mesh_type.mesh_3d:
                typestr = "3D";
                break;
            case mesh_type.mesh_flat:
                typestr = "FLAT";
                break;
            default:
                typestr = "KAPUT";
                break;
        }
        string key = new string($"{typestr}/{meshname}/{name_col}/{texture_override:D3}");
        if(Mesh3DDict.TryGetValue(key, out o))
        {
            return o;
        }
        else
        {
            switch(type)
            {
                case mesh_type.mesh_3d:
                    o = f3D2Mesh(meshname, name_col, texture_override);
                    break;
                case mesh_type.mesh_flat:
                    o = FLAT2Mesh(meshname, name_col);
                    break;
                default:
                    throw new Exception($"Unknown mesh type: {type}");
            }

            Mesh3DDict.Add(key, o);
            return Mesh3DDict[key];
        }
}
    }
    
    public static UnityData_3D LoadMesh3D(mesh_type type, string meshname, string name_col)
    {
        using(s_load_mesh.Auto()){
            UnityData_3D o;
            string typestr = new string("");
            switch(type)
            {
                case mesh_type.mesh_3d:
                    typestr = "3D";
                    break;
                case mesh_type.mesh_flat:
                    typestr = "FLAT";
                    break;
                default:
                    typestr = "KAPUT";
                    break;
            }
            string key = new string($"{typestr}/{meshname}/{name_col}/{0:D3}");
            if(Mesh3DDict.TryGetValue(key, out o))
            {
                return o;
            }
            else
            {
                switch(type)
                {
                    case mesh_type.mesh_3d:
                        o = f3D2Mesh_3D(meshname, name_col);
                        break;
                    case mesh_type.mesh_flat:
                        o = FLAT2Mesh(meshname, name_col);
                        break;
                    default:
                        throw new Exception($"Unknown mesh type: {type}");
                }

                Mesh3DDict.Add(key, o);
                return Mesh3DDict[key];
            }
        }
    }
    
    public static UnityData_WLD WLD2Mesh(string filename_wld, string name_col)
    {

using(s_load_WLD2mesh.Auto()){
        RGFileImport.RGWLDFile file_wld = new RGFileImport.RGWLDFile();
        file_wld.LoadFile(filename_wld);
        Mesh mesh_wld = LoadMesh_WLD(file_wld);

        List<Material> materials = new List<Material>();
        for(int i=0;i<WLD_TEXID_CNT;i++)
        {
            materials.Add(RGTexStore.GetMaterial(name_col,file_wld.sec[0].texbsi_file,i,"DEFAULT"));
        }

        UnityData_WLD data = new UnityData_WLD();
        data.mesh = mesh_wld;
        data.materials = materials;

        return data;
}
    }
    public static UnityData_3D FLAT2Mesh(string meshname, string palettename)
    {
using(s_load_FLAT2mesh.Auto()){
        RG3DStore.Mesh3D_intermediate mesh_i = RG3DStore.LoadMeshIntermediateFlat(meshname);
        return LoadMesh_3DStore(mesh_i, palettename, "FLATS", 0); // TODO: check if any FLATs have overrides
}
    }

    public static UnityData_3D f3D2Mesh(string meshname, string palettename, int texture_override = 0)
    {
        using(s_load_3D2mesh.Auto())
        {
                RG3DStore.Mesh3D_intermediate mesh_i = RG3DStore.LoadMeshIntermediate3DC(meshname);
                return LoadMesh_3DStore(mesh_i, palettename, "DEFAULT", texture_override);
        }
    }
    
    public static UnityData_3D f3D2Mesh_3D(string meshname, string palettename, int texture_override = 0)
    {
        using(s_load_3D2mesh.Auto())
        {
            RG3DStore.Mesh3D_intermediate mesh_i = RG3DStore.LoadMeshIntermediate3D(meshname);
            return LoadMesh_3DStore(mesh_i, palettename, "DEFAULT", texture_override);
        }
    }
    
    private static UnityData_3D LoadMesh_3DStore(RG3DStore.Mesh3D_intermediate mesh_i, string palettename, string shadername, int texture_override)
    {
using(s_load_3DStore.Auto()){
        List<Material> materials = new List<Material>();
        Mesh mesh_3d = new Mesh();
        mesh_3d.subMeshCount = mesh_i.subMeshCount;
        mesh_3d.vertices = mesh_i.vertices.ToArray();
        mesh_3d.normals = mesh_i.normals.ToArray();
pm_loadmesh_blendshapes.Begin();
        for(int j=0;j<mesh_i.framecount;j++)
        {
            mesh_3d.AddBlendShapeFrame($"FRAME_{j}", 100.0f, mesh_i.frameDeltaVertices[j].ToArray(), mesh_i.frameDeltaNormals[j].ToArray(), null);
        }
pm_loadmesh_blendshapes.End();

        List<Vector2> nuvs = new List<Vector2>(mesh_i.uv);
        int i = 0;
pm_loadmesh_submesh.Begin();
        foreach(var submesh in mesh_i.submeshes)
        {
            string[] keys = submesh.Key.Split("/");
            int texbsi = Int32.Parse(keys[0]);
            int img = Int32.Parse(keys[1]);
            if(texture_override != 0 && texbsi >= 0)
                texbsi = texture_override;

            Material mat = RGTexStore.GetMaterial(palettename, texbsi, img, shadername);
            materials.Add(mat);

            float aspect = (float)mat.mainTexture.height/(float)mat.mainTexture.width;
            float h = (float)mat.mainTexture.height;
            float w = (float)mat.mainTexture.width;

pm_loadmesh_submesh_uvs.Begin();
            List<int> tri_lst = submesh.Value;
            for(int j=0;j<tri_lst.Count;j++)
            {
                float uv_x = nuvs[tri_lst[j]].x/16.0f;
                float uv_y = nuvs[tri_lst[j]].y/16.0f;

                nuvs[tri_lst[j]] = new Vector2(uv_x/w, uv_y/h);
             }
pm_loadmesh_submesh_uvs.End();
            mesh_3d.SetTriangles(tri_lst.ToArray(), i);
            i++;
        }
pm_loadmesh_submesh.End();
        mesh_3d.uv = nuvs.ToArray();

        UnityData_3D data = new UnityData_3D();
        data.framecount = mesh_i.framecount;
        data.vertices = mesh_i.vertices.ToArray();
        data.normals = mesh_i.normals.ToArray();

        data.frameDeltaVertices = new Vector3[data.framecount][];
        data.frameDeltaNormals = new Vector3[data.framecount][];
pm_loadmesh_framedata.Begin();
        for(int j=1;j<data.framecount;j++)
        {
            data.frameDeltaVertices[j] = mesh_i.frameDeltaVertices[j].ToArray();
            data.frameDeltaNormals[j] = mesh_i.frameDeltaNormals[j].ToArray();
        }
pm_loadmesh_framedata.End();
        data.mesh = mesh_3d;
        data.materials = materials;

        return data;
}
    }
    private static Mesh LoadMesh_WLD(RGFileImport.RGWLDFile file_wld)
    {
using(s_load_WLD.Auto()){
        Mesh mesh = new Mesh();

        file_wld.BuildMeshes();

        mesh.indexFormat = IndexFormat.UInt32;
        mesh.subMeshCount = WLD_TEXID_CNT;

        List<Vector3> vec_lst = new List<Vector3>();
        List<Vector2> uv_lst  = new List<Vector2>();
        List<int[]>   tri_lst = new List<int[]>();
        int tri_ofs = 0;
        for(int i=0;i<mesh.subMeshCount;i++)
        {
            vec_lst.AddRange(file_wld.meshes[i].vertices);
            uv_lst.AddRange(file_wld.meshes[i].uv);

            int[] tri_tmp = file_wld.meshes[i].triangles.ToArray();
            int j = 0;
            for(j=0;j<tri_tmp.Length;j++)
            {
                tri_tmp[j] += tri_ofs;
            }
            tri_ofs += j;
            tri_lst.Add(tri_tmp);
        }

        mesh.vertices = vec_lst.ToArray();
        mesh.uv = uv_lst.ToArray();
        for(int i=0;i<mesh.subMeshCount;i++)
            mesh.SetTriangles(tri_lst[i],i);
        mesh.RecalculateNormals();
        return mesh;
}
    }
}
