using System;
using System.Collections.Generic;
using RGFileImport;
using UnityEngine;

public class RGScriptedObject : MonoBehaviour
{
	// TODO: these are duplicated from RGRGMStore
	const float RGM_MPOB_SCALE = 1/5120.0f;
	
	string scriptName;
	Vector3 position;
	Vector3 rotation;

	RG2Mesh.UnityData_3D data_3D;
	bool isAnimated;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	
	public RGRGMAnimStore.RGMAnim animationData;
	public void instanciateRGScriptedObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
	{
		isAnimated = false;
        scriptName = MPOB.scriptName;

		position.x = (float)(MPOB.posX)*RGM_MPOB_SCALE;
		position.y = -(float)(MPOB.posY)*RGM_MPOB_SCALE;
		position.z = -(float)(0xFFFFFF-MPOB.posZ)*RGM_MPOB_SCALE;
		rotation = RGRGMStore.eulers_from_MPOB_data(MPOB);
		
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();
			
		animationData = new RGRGMAnimStore.RGMAnim(filergm.RAHD.dict[MPOB.scriptName], filergm.RAAN, filergm.RAGR);

		if(animationData.RAANItems.Count > 0)
		{
			isAnimated = true;
			string modelname = animationData.RAANItems[0].modelFile;
			Debug.Log($"ANIMATED {scriptName}: \"{modelname}\"");
			data_3D = RG2Mesh.f3D2Mesh(modelname, name_col);

            List<Vector3[]> framevertices = new List<Vector3[]>();
            List<Vector3[]> framenormals = new List<Vector3[]>();
            for(int i=0;i<data_3D.framecount;i++)
            {
                framevertices.Add(data_3D.framevertices[i]);
                framenormals.Add(data_3D.framenormals[i]);
            }


            for(int j=1;j<animationData.RAANItems.Count;j++)
            {
                modelname = animationData.RAANItems[j].modelFile;
                RG2Mesh.UnityData_3D data_frame = RG2Mesh.f3D2Mesh(modelname, name_col);
                for(int i=0;i<data_frame.framecount;i++)
                {
                    framevertices.Add(data_frame.framevertices[i]);
                    framenormals.Add(data_frame.framenormals[i]);
                }
            }
            data_3D.framevertices = framevertices.ToArray();
            data_3D.framenormals = framenormals.ToArray();
            data_3D.mesh.vertices = data_3D.framevertices[0];
            data_3D.mesh.normals = data_3D.framenormals[0];
		
			meshFilter.mesh = data_3D.mesh;
			meshRenderer.SetMaterials(data_3D.materials);

            // TODO: temp to false to prevent infinite loops
            isAnimated = false;
		}
		else
		{
			isAnimated = false;
			string modelname = MPOB.modelName.Split('.')[0];
			data_3D = RG2Mesh.f3D2Mesh(modelname, name_col);
		
			meshFilter.mesh = data_3D.mesh;
			meshRenderer.SetMaterials(data_3D.materials);
		}
		gameObject.transform.position = position;
		gameObject.transform.Rotate(rotation);
	}
    public void SetAnim(int i)
    {
        if(animationData.PushAnimation((RGRGMAnimStore.AnimGroup)i,0) == 0)
            isAnimated = true;
    }
	float FRAMETIME_VAL = 0.2f;
	float FRAMETIME = 0.2f;
	void Update()
	{
        if(isAnimated)
        {
            FRAMETIME-= Time.deltaTime;
            if(FRAMETIME<0.0f)
            {
                int nextframe = animationData.NextFrame();
                if(nextframe >= 0)
                {
                    meshFilter.mesh.SetVertices(data_3D.framevertices[nextframe]);
                    meshFilter.mesh.SetNormals(data_3D.framenormals[nextframe]);
                }
                FRAMETIME = FRAMETIME_VAL;
            }
        }
	}
}
