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

	RGMeshStore.UnityData_3D data_3D;
	public bool allowAnimation;

    public ScriptedObjectType type;

	SkinnedMeshRenderer skinnedMeshRenderer;
    Light light;
	
    public AnimData animations;

    public void Instanciate3DObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
		animations = new AnimData(MPOB.scriptName);

		if(animations.animationData.RAANItems.Count > 0)
		{
            type = ScriptedObjectType.scriptedobject_animated;
			string modelname_frame = animations.animationData.RAANItems[0].modelFile;
			Debug.Log($"ANIMATED {scriptName}: \"{modelname_frame}\"");
			data_3D = RGMeshStore.f3D2Mesh(modelname_frame, name_col);

            List<Vector3[]> framevertices = new List<Vector3[]>();
            List<Vector3[]> framenormals = new List<Vector3[]>();
            for(int i=0;i<data_3D.framecount;i++)
            {
                framevertices.Add(data_3D.vertices);
                framenormals.Add(data_3D.normals);
            }


            for(int j=1;j<animations.animationData.RAANItems.Count;j++)
            {
                modelname_frame = animations.animationData.RAANItems[j].modelFile;
                RGMeshStore.UnityData_3D data_frame = RGMeshStore.f3D2Mesh(modelname_frame, name_col);
                for(int i=0;i<data_frame.framecount;i++)
                {
                    framevertices.Add(data_frame.vertices);
                    framenormals.Add(data_frame.normals);
                }
            }
            data_3D.mesh.vertices = data_3D.vertices;
            data_3D.mesh.normals = data_3D.normals;
		
			skinnedMeshRenderer.sharedMesh = data_3D.mesh;
			skinnedMeshRenderer.SetMaterials(data_3D.materials);
/*
            for(int i=0;i<animations.validAnims.Count;i++)
                Debug.Log($"ANIMS: {i}:{animations.validAnims[i]}");
*/

		}
		else
		{
            type = ScriptedObjectType.scriptedobject_static;
            string modelname = MPOB.modelName.Split('.')[0];
			data_3D = RGMeshStore.f3D2Mesh(modelname, name_col);
		
			skinnedMeshRenderer.sharedMesh = data_3D.mesh;
			skinnedMeshRenderer.SetMaterials(data_3D.materials);
		}

    }

    public void InstanciateLightObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
        type = ScriptedObjectType.scriptedobject_static;
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
        string modelname = MPOB.scriptName;;
        data_3D = RGMeshStore.f3D2Mesh(modelname, name_col);
    
        skinnedMeshRenderer.sharedMesh = data_3D.mesh;
        skinnedMeshRenderer.SetMaterials(data_3D.materials);

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
    public void SetAnim(int animId, int firstFrame)
    {
        if(type == ScriptedObjectType.scriptedobject_animated)
        {
            if(animations.PushAnimation((RGRGMAnimStore.AnimGroup)animId,firstFrame) != 0)
                Debug.Log($"{scriptName}: animation {(RGRGMAnimStore.AnimGroup)animId} requested but doesnt exist");
        }
    }

	void Update()
	{
		if (allowAnimation)
		{
            skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, 0.0f);
            skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, 0.0f);
            animations.runAnimation(Time.deltaTime);

            if(animations.nextKeyFrame >= data_3D.framecount)
            {
                Debug.Log($"{scriptName}: Frame {animations.nextKeyFrame} requested, but 3DC only has {data_3D.framecount} frames.");
                return;
            }
            else if(animations.nextKeyFrame >= 0)
            {

                /*
                skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, 0.0f);
                skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, 100.0f);
                */

                // for animation blending, we need to track current and next frame
                float blend1 = (animations.frameTime/AnimData.FRAMETIME_VAL)*100.0f;
                float blend2 = 100.0f-blend1;
                skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, blend1);
                skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, blend2);
            }
		}
	}
}
