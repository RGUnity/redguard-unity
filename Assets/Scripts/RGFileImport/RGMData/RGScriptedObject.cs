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
		
			meshFilter.mesh = data_3D.mesh;
			meshRenderer.SetMaterials(data_3D.materials);
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
	
	void Update()
	{
		
	}
}
