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
    public enum TaskType
    {
        task_idle,
        task_moving,
        task_waiting,
    }
	// TODO: these are duplicated from RGRGMStore
	const float RGM_MPOB_SCALE = 1/5120.0f;
	
	string objectName;
	string scriptName;
	Vector3 position;
	Vector3 rotation;

    // used for animations: 3DC files might not have the same vertex count
    // so we're stuck with this
    int currentMesh;
    Mesh[] meshes;
    int[] meshFrameCount;
    int meshTotalFrameCount;


    public ScriptedObjectType type;

	SkinnedMeshRenderer skinnedMeshRenderer;
    Light light;

// animations	
	public bool allowAnimation;
    public AnimData animations;
// scripting
	public bool allowScripting;
    public ScriptData script;
// tasks
    TaskType currentTask;
    int[] taskParameters;
    Vector3 moveTarget;
    Vector3 moveDelta;
    float waitTime;
    static int FUNC_CNT = 367;
    Func<int[], int>[] functions;

    public RGFileImport.RGRGMFile.RGMRAHDItem RAHDData;

    public void Instanciate3DObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
		animations = new AnimData(MPOB.scriptName);
        currentMesh = 0;

		if(animations.animationData.RAANItems.Count > 0)
		{
			Debug.Log($"ANIMATED {scriptName}");
            type = ScriptedObjectType.scriptedobject_animated;

			RGMeshStore.UnityData_3D data_3D = RGMeshStore.f3D2Mesh(animations.animationData.RAANItems[0].modelFile, name_col, RAHDData.textureId);

            meshes = new Mesh[animations.animationData.RAANItems.Count];
            meshFrameCount = new int[animations.animationData.RAANItems.Count];
            int totalFrames = 0;

            for(int j=0;j<animations.animationData.RAANItems.Count;j++)
            {
                string modelname_frame = animations.animationData.RAANItems[j].modelFile;
                RGMeshStore.UnityData_3D data_frame = RGMeshStore.f3D2Mesh(modelname_frame, name_col, RAHDData.textureId);
                meshes[j] = data_frame.mesh;
                meshFrameCount[j] = data_frame.framecount;
                totalFrames+=data_frame.framecount;
            }
		
			skinnedMeshRenderer.sharedMesh = meshes[0];
			skinnedMeshRenderer.SetMaterials(data_3D.materials);
		}
		else
		{
            type = ScriptedObjectType.scriptedobject_static;
            string modelname = MPOB.modelName.Split('.')[0];
			RGMeshStore.UnityData_3D data_3D = RGMeshStore.f3D2Mesh(modelname, name_col, RAHDData.textureId);
		
			skinnedMeshRenderer.sharedMesh = data_3D.mesh;
			skinnedMeshRenderer.SetMaterials(data_3D.materials);
		}

    }

    public void InstanciateLightObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
        type = ScriptedObjectType.scriptedobject_static;
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
        string modelname = MPOB.scriptName;;
        RGMeshStore.UnityData_3D data_3D = RGMeshStore.f3D2Mesh(modelname, name_col, RAHDData.textureId);
    
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
        RAHDData = filergm.RAHD.dict[scriptName];

		position.x = (float)(MPOB.posX)*RGM_MPOB_SCALE;
		position.y = -(float)(MPOB.posY)*RGM_MPOB_SCALE;
		position.z = -(float)(0xFFFFFF-MPOB.posZ)*RGM_MPOB_SCALE;
		rotation = RGRGMStore.eulers_from_MPOB_data(MPOB);
        allowAnimation = false;

        allowScripting = false;
        SetupFunctions();

        currentTask = TaskType.task_idle;;
        taskParameters = new int[8];
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

        script = new ScriptData(MPOB.scriptName, functions, MPOB.id);
		
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
    int normalizeFrame()
    {
        if(animations.nextKeyFrame >= 0)
        {
            int cnt_tot = 0;
            for(int i=0;i<meshFrameCount.Length;i++)
            {
                if(animations.currentKeyFrame<cnt_tot + meshFrameCount[i])
                {
                    currentMesh = i;
                    return cnt_tot;
                }
                cnt_tot += meshFrameCount[i];
            }
        }
        return 0;
    }
    void UpdateAnimations()
	{
		if (allowAnimation)
		{
            skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, 0.0f);
            skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, 0.0f);
            animations.runAnimation(Time.deltaTime);
/*
            if(animations.nextKeyFrame >= data_3D.framecount)
            {
                Debug.Log($"{scriptName}: Frame {animations.nextKeyFrame} requested, but 3DC only has {data_3D.framecount} frames.");
                return;
            }
            else
*/
            
                int frameofs = normalizeFrame();
                int currentKeyFrame = animations.currentKeyFrame - frameofs;
                int nextKeyFrame = animations.nextKeyFrame - frameofs;
                /*
                skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, 0.0f);
                skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, 100.0f);
                */

                // for animation blending, we need to track current and next frame
                float blend1 = (animations.frameTime/AnimData.FRAMETIME_VAL)*100.0f;
                float blend2 = 100.0f-blend1;
                skinnedMeshRenderer.SetBlendShapeWeight(currentKeyFrame, blend1);
                skinnedMeshRenderer.SetBlendShapeWeight(nextKeyFrame, blend2);
		}
	}
	void Update()
    {
        UpdateTasks();
        UpdateAnimations();
    }
    void UpdateTasks()
    {
        if(!allowScripting)
            return;
        float frameTime = Time.deltaTime;
        switch(currentTask)
        {
            case TaskType.task_idle:
                script.tickScript();
                break;
            case TaskType.task_moving:
                Vector3 moveFrame = moveDelta*frameTime;
                if(Vector3.Distance(position, moveTarget) > moveFrame.magnitude)
                {
                    position += moveFrame;
                    gameObject.transform.position = position;
                }
                else
                {
                    position = moveTarget;
                    gameObject.transform.position = position;
                    currentTask = TaskType.task_idle;
                }
                break;
            case TaskType.task_waiting:
                if(waitTime > 0)
                {
                    waitTime-= frameTime; // TODO: deltatime
                }
                else
                {
                    currentTask = TaskType.task_idle;
                }
                break;
            default:
                break;
        }
    }
    void SetupFunctions()
    {
        functions = new Func<int[], int>[FUNC_CNT];
        functions[53] = MoveByAxis;
        functions[60] = Wait;
    }
// SOUPDEF function implementations
// ASSUMTIONS:
// time is in seconds/9 < DEFINATELY WRONG
// axis 0 is global X
// axis 1 is global Y
// axis 2 is global Z
    static float TIME_VAL = 0.1f;
    /*task*/
    public int MoveByAxis(int[] i /*3*/)    
    {
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        Debug.Log("MoveByAxis");
        currentTask = TaskType.task_moving;
        Vector3 mt;
        switch(i[0])
        {
            case 0:
                mt = new Vector3(((float)i[1])*RGM_MPOB_SCALE, 0.0f, 0.0f);
                break;
            case 1:
                mt = new Vector3(0,0,0);
                break;
            case 2:
                mt = new Vector3(0,0,0);
                break;
            default:
                mt = new Vector3(0,0,0);
                break;
        }
        moveTarget = position + mt;
        float movespeed = ((float)i[2])*TIME_VAL;
        moveDelta = mt/movespeed;
        return 0;
    }
    public int Wait(int[] i /*1*/)    
    {
        // i[0]: time to wait
        Debug.Log("Wait");
        waitTime = i[0]*TIME_VAL;
        currentTask = TaskType.task_waiting;
        return 0;
    }

}
