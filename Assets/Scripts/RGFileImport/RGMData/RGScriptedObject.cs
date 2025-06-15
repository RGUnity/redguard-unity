using System;
using System.Collections.Generic;
using RGFileImport;
using UnityEngine;

public class RGScriptedObject : MonoBehaviour
{
    public enum ScriptedObjectType
    {
        scriptedobject_static       = 0,
        scriptedobject_animated     = 1,
        scriptedobject_light        = 2,
        scriptedobject_sound        = 3
    }
	// TODO: these are duplicated from RGRGMStore
	const float RGM_MPOB_SCALE = 1/5120.0f;
	
	string scriptName;
	Vector3 position;
	Vector3 rotation;

	RG2Mesh.UnityData_3D data_3D;
	bool animationRunning;

    ScriptedObjectType type;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
    Light light;
	
	public RGRGMAnimStore.RGMAnim animationData;

    public void Instanciate3DObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();
			
		animationData = new RGRGMAnimStore.RGMAnim(filergm.RAHD.dict[MPOB.scriptName], filergm.RAAN, filergm.RAGR);

		if(animationData.RAANItems.Count > 0)
		{
            type = ScriptedObjectType.scriptedobject_animated;
			string modelname_frame = animationData.RAANItems[0].modelFile;
			Debug.Log($"ANIMATED {scriptName}: \"{modelname_frame}\"");
			data_3D = RG2Mesh.f3D2Mesh(modelname_frame, name_col);

            List<Vector3[]> framevertices = new List<Vector3[]>();
            List<Vector3[]> framenormals = new List<Vector3[]>();
            for(int i=0;i<data_3D.framecount;i++)
            {
                framevertices.Add(data_3D.framevertices[i]);
                framenormals.Add(data_3D.framenormals[i]);
            }


            for(int j=1;j<animationData.RAANItems.Count;j++)
            {
                modelname_frame = animationData.RAANItems[j].modelFile;
                RG2Mesh.UnityData_3D data_frame = RG2Mesh.f3D2Mesh(modelname_frame, name_col);
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

		}
		else
		{
            type = ScriptedObjectType.scriptedobject_static;
            string modelname = MPOB.modelName.Split('.')[0];
			data_3D = RG2Mesh.f3D2Mesh(modelname, name_col);
		
			meshFilter.mesh = data_3D.mesh;
			meshRenderer.SetMaterials(data_3D.materials);
		}

    }

    public void InstanciateLightObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
        type = ScriptedObjectType.scriptedobject_static;
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();
			
        string modelname = MPOB.scriptName;;
        data_3D = RG2Mesh.f3D2Mesh(modelname, name_col);
    
        meshFilter.mesh = data_3D.mesh;
        meshRenderer.SetMaterials(data_3D.materials);

		light = gameObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.intensity = (float)(MPOB.intensity);
        light.range = (float)(MPOB.radius)/20.0f;
    }
    public void InstanciateLight(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm)
    {
        type = ScriptedObjectType.scriptedobject_light;
		light = gameObject.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = (float)(MPOB.radius)/20.0f;
        light.color = new Color((float)(MPOB.red)/255.0f,(float)(MPOB.green)/255.0f,(float)(MPOB.blue)/255.0f);
    }

	public void Instanciate(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
	{
		animationRunning = false;
        scriptName = MPOB.scriptName;

		position.x = (float)(MPOB.posX)*RGM_MPOB_SCALE;
		position.y = -(float)(MPOB.posY)*RGM_MPOB_SCALE;
		position.z = -(float)(0xFFFFFF-MPOB.posZ)*RGM_MPOB_SCALE;
		rotation = RGRGMStore.eulers_from_MPOB_data(MPOB);

        switch(MPOB.type)
        {
            case RGFileImport.RGRGMFile.ObjectType.object_3d:
                Instanciate3DObject(MPOB, filergm, name_col);
                break;
            case RGFileImport.RGRGMFile.ObjectType.object_lightobject:
                InstanciateLightObject(MPOB, filergm, name_col);
                break;
            case RGFileImport.RGRGMFile.ObjectType.object_light:
                InstanciateLight(MPOB, filergm);
                break;
            default:
                Debug.Log($"unhandled type: {MPOB.type} for object {MPOB.scriptName} with model {MPOB.modelName}");
                break;
        }

		
		gameObject.transform.position = position;
		gameObject.transform.Rotate(rotation);
	}
    public void SetAnim(int i)
    {
        if(type == ScriptedObjectType.scriptedobject_animated)
            if(animationData.PushAnimation((RGRGMAnimStore.AnimGroup)i,0) == 0)
                animationRunning = true;
    }
	float FRAMETIME_VAL = 0.2f;
	float FRAMETIME = 0.2f;
	void Update()
	{
        if(animationRunning)
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
