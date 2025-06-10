using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RGRGMStore
{
    const float RGM_MPOB_SCALE = 0.000195f;
    const float RGM_X_OFS = 0.0f;//29.8f;
    const float RGM_Y_OFS = 0.0f; //-22.8f;
    const float RGM_Z_OFS = 0.0f;

    static Dictionary<string, RGFileImport.RGRGMFile> RGMDict;

    public static string path_to_game = "./game_3dfx";
    static string maps_path = path_to_game + "/maps/";

    static RGRGMStore()
    {
        RGMDict = new Dictionary<string, RGFileImport.RGRGMFile>();
    }
    public static RGFileImport.RGRGMFile GetRGM(string RGMname)
    {
        string rgm_key = $"{RGMname}";
        RGFileImport.RGRGMFile o;
        if(RGMDict.TryGetValue(rgm_key, out o))
        {
            return o;
        }
        else
        {
            string path = new string(maps_path + RGMname + ".RGM");
            RGMDict.Add(rgm_key, new RGFileImport.RGRGMFile());
            RGMDict[rgm_key].LoadFile(path);
        }
        return RGMDict[rgm_key];
    }

    public struct RGRGMData
    {
        public string name;
        public string name2;
        public Vector3 position;
        public Vector3 rotation;
        public RGRGMData(string n, string n2, Vector3 p, Vector3 r)
        {
            name = n;
            name2 = n2;
            position = p;
            rotation = r;
        }
    }
    public static List<RGRGMData> LoadMPSF(string filename)
    {
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

                string name = $"MPSF/{filergm.MPSF.items[i].textureId}/{filergm.MPSF.items[i].imageId}";
                data_out.Add(new RGRGMData(name, "", new Vector3(posx, posy, posz), new Vector3(0.0f, 0.0f,0.0f)));
            }
            catch(Exception ex)
            {
                string name = $"F_{i}_MPSF/{filergm.MPSF.items[i].textureId}/{filergm.MPSF.items[i].imageId}";
                Debug.Log($"Error loading MPSF item from {filename}: {name}: {ex}");
            }
        }
        return data_out;
    }

    public static List<RGRGMData> LoadMPSO(string filename)
    {
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPSO.items.Count;i++)
        {
            try{
                float posx =  (float)(filergm.MPSO.items[i].posx)*RGM_MPOB_SCALE;
                float posy = -(float)(filergm.MPSO.items[i].posy)*RGM_MPOB_SCALE;
                float posz = -(float)(0xFFFFFF-filergm.MPSO.items[i].posz)*RGM_MPOB_SCALE;

                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_Z_OFS;

                Vector3 eulers = eulers_from_MPSO_data(filergm.MPSO.items[i]);

                data_out.Add(new RGRGMData(filergm.MPSO.items[i].name, "", new Vector3(posx, posy, posz), eulers));
            }
            catch(Exception ex)
            {
                Debug.Log($"Error loading MPSO item from {filename}: {filergm.MPSO.items[i].name}: {ex}");
            }
        }
        return data_out;
    }
    public static List<RGRGMData> LoadMPOB(string filename)
    {
        RGFileImport.RGRGMFile filergm = GetRGM(filename);

        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPOB.items.Count;i++)
        {
            if(filergm.MPOB.items[i].isStatic == 1)
            {
                try{
                    float posx =  (float)(filergm.MPOB.items[i].posx)*RGM_MPOB_SCALE;
                    float posy = -(float)(filergm.MPOB.items[i].posy)*RGM_MPOB_SCALE;
                    float posz = -(float)(0xFFFFFF-filergm.MPOB.items[i].posz)*RGM_MPOB_SCALE;

                    posx += RGM_X_OFS;
                    posy += RGM_Y_OFS;
                    posz += RGM_Z_OFS;

                    Vector3 eulers = eulers_from_MPOB_data(filergm.MPOB.items[i]);

                    string modelname = filergm.MPOB.items[i].name2.Split('.')[0];

                    data_out.Add(new RGRGMData(filergm.MPOB.items[i].name, modelname, new Vector3(posx, posy, posz), eulers));
                }
                catch(Exception ex)
                {
                    Debug.Log($"Error loading MPOB item from {filename}: {filergm.MPOB.items[i].name}: {ex}");
                }
            }
        }
        return data_out;
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
    static Vector3 eulers_from_MPOB_data(RGFileImport.RGRGMFile.RGMMPOBItem i)
    {
        const float DA2DG = (180.0f/1024.0f);
        Vector3 eulers = new Vector3 (i.anglex%2048, i.angley%2048, i.anglez%2048);
        eulers *= DA2DG;
        eulers = Vector3.Scale(eulers, RG3DStore.MESH_ROT_FLIP*-1.0f);
        return eulers;
    }


}
 
