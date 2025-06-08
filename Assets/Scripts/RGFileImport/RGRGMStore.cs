using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RGRGMStore
{
    const float RGM_MPSO_SCALE = 0.05f; // about 1/20
    const float RGM_MPOB_SCALE = 0.000195f;
    const float RGM_Z_OFS = -3275.0f;
    const float RGM_X_OFS = 29.8f;
    const float RGM_Y_OFS = -22.8f;
    const float RGM_MPOB_Y_OFS = -3275.0f;

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

    public static List<RGRGMData> LoadMPSO(string filename)
    {
        RGFileImport.RGRGMFile filergm = new RGFileImport.RGRGMFile();
        filergm.LoadFile(filename);
        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPSO.items.Count;i++)
        {
            try{
                float posx =  (float)(filergm.MPSO.items[i].posx)*RGM_MPSO_SCALE;
                float posy = -((float)(0xFFFF-filergm.MPSO.items[i].posz))*RGM_MPSO_SCALE;
                float posz = ((float)(0xFFFF-filergm.MPSO.items[i].posy))*RGM_MPSO_SCALE;
                posx += RGM_X_OFS;
                posy += RGM_Y_OFS;
                posz += RGM_Z_OFS;

                Vector3 eulers = eulers_from_MPSO_data(filergm.MPSO.items[i]);

                data_out.Add(new RGRGMData(filergm.MPSO.items[i].name, "", new Vector3(posx, posz, posy), eulers));
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
        RGFileImport.RGRGMFile filergm = new RGFileImport.RGRGMFile();
        filergm.LoadFile(filename);
        List<RGRGMData> data_out = new List<RGRGMData>();
        for(int i=0;i<filergm.MPOB.items.Count;i++)
        {
            if(filergm.MPOB.items[i].isStatic == 1)
            {
                try{
                    float posx =  ((float)filergm.MPOB.items[i].posx)*RGM_MPOB_SCALE;
                    float posy = ((float)(filergm.MPOB.items[i].posz-0xFFFFFF))*RGM_MPOB_SCALE;
                    float posz = -((float)filergm.MPOB.items[i].posy)*RGM_MPOB_SCALE;
                    posx += RGM_X_OFS;
                    posy += RGM_Y_OFS;

                    Vector3 eulers = eulers_from_MPOB_data(filergm.MPOB.items[i]);

                    string modelname = filergm.MPOB.items[i].name2.Split('.')[0];

                    data_out.Add(new RGRGMData(filergm.MPOB.items[i].name, modelname, new Vector3(posx, posz, posy), eulers));
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
 
