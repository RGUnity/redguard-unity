using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;
using Unity.Profiling;

public static class RGRGMStore
{

static readonly ProfilerMarker s_load_RGM = new ProfilerMarker("GetRGM");
static readonly ProfilerMarker s_load_MPSF = new ProfilerMarker("LoadMPSF");
static readonly ProfilerMarker s_load_MPSO = new ProfilerMarker("LoadMPSO");
static readonly ProfilerMarker s_load_MPRP = new ProfilerMarker("LoadMPRP");
static readonly ProfilerMarker s_load_MPOB = new ProfilerMarker("LoadMPOB");
static readonly ProfilerMarker s_load_WDNM = new ProfilerMarker("LoadWDNM");



    //const float RGM_MPOB_SCALE = 0.000195f;
    const float RGM_MPOB_SCALE = 1/5120.0f;
    const float RGM_X_OFS = 0.0f;//29.8f;
    const float RGM_Y_OFS = 0.0f; //-22.8f;
    const float RGM_Z_OFS = 0.0f;

    static Dictionary<string, RGFileImport.RGRGMFile> RGMDict;

    static RGRGMStore()
    {
        RGMDict = new Dictionary<string, RGFileImport.RGRGMFile>();
    }
    public static void DumpDict()
    {
        Debug.Log($"RGMS:");
        List<string> keys = new List<string>(RGMDict.Keys);
        for(int i=0;i<keys.Count;i++)
            Debug.Log($"{i:D3}: {keys[i]}");
    }


    public static RGFileImport.RGRGMFile GetRGM(string RGMname)
    {
using(s_load_RGM.Auto()){
        string rgm_key = $"{RGMname}";
        RGFileImport.RGRGMFile o;
        if(RGMDict.TryGetValue(rgm_key, out o))
        {
            return o;
        }
        else
        {
            string path = new string(Game.pathManager.GetMapsFolder() + RGMname + ".RGM");
            RGMDict.Add(rgm_key, new RGFileImport.RGRGMFile());
            RGMDict[rgm_key].LoadFile(path);
        }
        return RGMDict[rgm_key];
}
    }

    public struct RGRGMData
    {
        public uint id;
        public string name;
        public string name2;
        public Vector3 position;
        public Vector3 rotation;
        public RGRGMData(uint id, string n, string n2, Vector3 p, Vector3 r)
        {
            this.id = id;
            name = n;
            name2 = n2;
            position = p;
            rotation = r;
        }
    }
    public struct RGRGMRopeData
    {
        public uint id;
        public string ropeModel;
        public string staticModel;
        public Vector3 position;
        public int count;
        public RGRGMRopeData(uint id, string n, string n2, Vector3 p, int c)
        {
            this.id = id;
            ropeModel = n;
            staticModel = n2;
            position = p;
            count = c;
        }
    }

    public static List<RGRGMData> LoadMPSF(string filename)
    {
using(s_load_MPSF.Auto()){
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPSF.items.Count;i++)
        {
            try{
                float posx =  (float)(filergm.MPSF.items[i].posX)*RGM_MPOB_SCALE;
                float posy = -(float)(filergm.MPSF.items[i].posY)*RGM_MPOB_SCALE;
                float posz = -(float)(0xFFFFFF-filergm.MPSF.items[i].posZ)*RGM_MPOB_SCALE;

                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_Z_OFS;

                string name = $"{filergm.MPSF.items[i].textureId}/{filergm.MPSF.items[i].imageId}";
                uint id = filergm.MPSF.items[i].id;
                data_out.Add(new RGRGMData(id, name, "", new Vector3(posx, posy, posz), new Vector3(0.0f, 0.0f,0.0f)));
            }
            catch(Exception ex)
            {
                string name = $"F_{i}_MPSF/{filergm.MPSF.items[i].textureId}/{filergm.MPSF.items[i].imageId}";
                Debug.Log($"Error loading MPSF item from {filename}: {name}: {ex}");
            }
        }
        return data_out;
}
    }

    public static List<RGRGMData> LoadMPSO(string filename)
    {
using(s_load_MPSO.Auto()){
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPSO.items.Count;i++)
        {
            try{
                float posx =  (float)(filergm.MPSO.items[i].posX)*RGM_MPOB_SCALE;
                float posy = -(float)(filergm.MPSO.items[i].posY)*RGM_MPOB_SCALE;
                float posz = -(float)(0xFFFFFF-filergm.MPSO.items[i].posZ)*RGM_MPOB_SCALE;

                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_Z_OFS;

                Vector3 eulers = eulers_from_MPSO_data(filergm.MPSO.items[i]);
                uint id = filergm.MPSO.items[i].id;

                data_out.Add(new RGRGMData(id, filergm.MPSO.items[i].name, "", new Vector3(posx, posy, posz), eulers));
            }
            catch(Exception ex)
            {
                Debug.Log($"Error loading MPSO item from {filename}: {filergm.MPSO.items[i].name}: {ex}");
            }
        }
        return data_out;
}
    }
    public static List<RGRGMRopeData> LoadMPRP(string filename)
    {
using(s_load_MPRP.Auto()){
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMRopeData> data_out = new List<RGRGMRopeData>();
        for(int i=0;i<filergm.MPRP.items.Count;i++)
        {
            try{
                float posx =  (float)(filergm.MPRP.items[i].posX)*RGM_MPOB_SCALE;
                float posy = -(float)(filergm.MPRP.items[i].posY)*RGM_MPOB_SCALE;
                float posz = -(float)(0xFFFFFF-filergm.MPRP.items[i].posZ)*RGM_MPOB_SCALE;

                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_Z_OFS;

                Vector3 eulers = new Vector3(0.0f,0.0f,0.0f);

                uint id = filergm.MPRP.items[i].id;
                string staticModel = filergm.MPRP.items[i].staticModel.Length>0?filergm.MPRP.items[i].staticModel:null;
                data_out.Add(new RGRGMRopeData(id, filergm.MPRP.items[i].ropeModel,staticModel, new Vector3(posx, posy, posz), filergm.MPRP.items[i].length));
            }
            catch(Exception ex)
            {
                Debug.Log($"Error loading MPRP item from {filename}: {filergm.MPRP.items[i].ropeModel}: {ex}");
            }
        }
        return data_out;
}
    }

    public static List<RGRGMData> LoadMPOB(string filename)
    {
using(s_load_MPOB.Auto()){
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPOB.items.Count;i++)
        {
            if(filergm.MPOB.items[i].isStatic == 1)
            {
                try{
                    float posx =  (float)(filergm.MPOB.items[i].posX)*RGM_MPOB_SCALE;
                    float posy = -(float)(filergm.MPOB.items[i].posY)*RGM_MPOB_SCALE;
                    float posz = -(float)(0xFFFFFF-filergm.MPOB.items[i].posZ)*RGM_MPOB_SCALE;

                    posx += RGM_X_OFS;
                    posy += RGM_Y_OFS;
                    posz += RGM_Z_OFS;

                    Vector3 eulers = eulers_from_MPOB_data(filergm.MPOB.items[i]);

                    string modelname = filergm.MPOB.items[i].modelName.Split('.')[0];
                    uint id = filergm.MPSO.items[i].id;

                    data_out.Add(new RGRGMData(id, filergm.MPOB.items[i].scriptName, modelname, new Vector3(posx, posy, posz), eulers));
                }
                catch(Exception ex)
                {
                    Debug.Log($"Error loading MPOB item from {filename}: {filergm.MPOB.items[i].scriptName}: {ex}");
                }
            }
        }
        return data_out;
}
    }
    public static List<RGRGMData> LoadWDNM(string filename)
    {
using(s_load_WDNM.Auto()){
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        uint ID_GEN = 0xFFFF0000;
        for(int i=0;i<filergm.WDNM.items.Count;i++)
        {
            ID_GEN++;
            int base_posx = filergm.WDNM.items[i].posX;
            int base_posy = filergm.WDNM.items[i].posY;
            int base_posz = filergm.WDNM.items[i].posZ;
            float fposx =  (float)((uint)base_posx%(uint)0x9FD800)*RGM_MPOB_SCALE;
            float fposy = -(float)(base_posy)*RGM_MPOB_SCALE;
            float fposz = -(float)((uint)(0xFFFFFF-base_posz)%(uint)0xFFDC00)*RGM_MPOB_SCALE;
            data_out.Add(new RGRGMData(ID_GEN,"LANTERN1", $"ITEM {i}", new Vector3(fposx, fposy, fposz), new Vector3(0.0f, 0.0f, 0.0f)));
            for(int j=0;j<filergm.WDNM.items[i].walkNodes.Length;j++)
            {
                ID_GEN++;
                try{
                    int sub_posx = filergm.WDNM.items[i].walkNodes[j].posX*256;
                    int sub_posy = filergm.WDNM.items[i].walkNodes[j].posY*256;
                    int sub_posz = filergm.WDNM.items[i].walkNodes[j].posZ*256;

                    float posx =  (float)((uint)sub_posx%(uint)0x10000A0)*RGM_MPOB_SCALE;
                    float posy = -(float)(sub_posy)*RGM_MPOB_SCALE;
                    float posz = -(float)((uint)(0xFFFFFF-sub_posz)%(uint)0xFFDC00)*RGM_MPOB_SCALE;

                    posx += RGM_X_OFS;
                    posy += RGM_Y_OFS;
                    posz += RGM_Z_OFS;

                    data_out.Add(new RGRGMData(ID_GEN,"LANTERN1", $"{i}_{j}", new Vector3(posx, posy, posz), new Vector3(0.0f, 0.0f, 0.0f)));

                }
                catch(Exception ex)
                {
                    Debug.Log($"Error loading WDNM item from {filename}: {i}: {ex}");
                }
            }
        }
        return data_out;
}
    }


    static Vector3 eulers_from_MPSO_data(RGFileImport.RGRGMFile.RGMMPSOItem i)
    {
        Matrix4x4 m = new Matrix4x4();
        m[0,0] = (new RGFileImport.Q4_28(i.rotation_matrix[0])).ToFloat();
        m[0,1] = (new RGFileImport.Q4_28(i.rotation_matrix[1])).ToFloat();
        m[0,2] = (new RGFileImport.Q4_28(i.rotation_matrix[2])).ToFloat();
        m[1,0] = (new RGFileImport.Q4_28(i.rotation_matrix[3])).ToFloat();
        m[1,1] = (new RGFileImport.Q4_28(i.rotation_matrix[4])).ToFloat();
        m[1,2] = (new RGFileImport.Q4_28(i.rotation_matrix[5])).ToFloat();
        m[2,0] = (new RGFileImport.Q4_28(i.rotation_matrix[6])).ToFloat();
        m[2,1] = (new RGFileImport.Q4_28(i.rotation_matrix[7])).ToFloat();
        m[2,2] = (new RGFileImport.Q4_28(i.rotation_matrix[8])).ToFloat();
        m[3,3] = 1;
        Vector3 eulers = Vector3.Scale(m.rotation.eulerAngles, RG3DStore.MESH_ROT_FLIP);
        return eulers;
    }
    public static Vector3 eulers_from_MPOB_data(RGFileImport.RGRGMFile.RGMMPOBItem i)
    {
        const float DA2DG = (180.0f/1024.0f);
        Vector3 eulers = new Vector3 (i.anglex%2048, i.angley%2048, i.anglez%2048);
        eulers *= DA2DG;
        eulers = Vector3.Scale(eulers, RG3DStore.MESH_ROT_FLIP*-1.0f);
        return eulers;
    }


}
 
