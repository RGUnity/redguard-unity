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
        task_rotating,
        task_moving,
        task_waiting,
        task_syncing,
    }
	// TODO: these are duplicated from RGRGMStore
	const float RGM_MPOB_SCALE = 1/5120.0f;
	
	string objectName;
	public string scriptName;
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

    public byte[] attributes;

// animations	
	public bool allowAnimation;
    public AnimData animations;
// scripting
	public bool allowScripting;
    public ScriptData script;
// tasks
    TaskType currentTask;
    float taskTimer;
    float taskDuration;
    // rotating
    Vector3 rotationCenter;
    Vector3 rotationTarget;
    Vector3 rotationStart;
    // moving
    Vector3 positionTarget;
    Vector3 positionStart;
    // waiting
        // nothing here ;)
    // syncing
    public uint sync_point;

    static int FUNC_CNT = 367;
    Func<int[], int>[] functions;
    // 

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

        attributes = new byte[256];
        Array.Copy(filergm.RAAT.attributes, RAHDData.index*256, attributes, 0, 256);

        if(attributes[26] != 0)
            RGFamilyStore.AddSlave(attributes[26], this);
        if(attributes[29] != 0)
            RGFamilyStore.AddToGroup(attributes[29], this);

		position.x = (float)(MPOB.posX)*RGM_MPOB_SCALE;
		position.y = -(float)(MPOB.posY)*RGM_MPOB_SCALE;
		position.z = -(float)(0xFFFFFF-MPOB.posZ)*RGM_MPOB_SCALE;
		rotation = RGRGMStore.eulers_from_MPOB_data(MPOB);
        allowAnimation = false;

        allowScripting = false;
        SetupFunctions();

        currentTask = TaskType.task_idle;;
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

    public void PropagatePositionDelta(Vector3 positionDelta)
    {
        position += positionDelta;
        gameObject.transform.position = position;

        // propagate to slaves
        if(attributes[25] != 0)
        {
            List<RGScriptedObject> slaves = RGFamilyStore.GetSlaves(attributes[25]);
            for(int i=0;i<slaves.Count;i++)
            {
                slaves[i].PropagatePositionDelta(positionDelta);
            }
        }
 
    }

    public void PropagateRotation2(Vector3 masterPosition, Vector3 rotationDelta, Vector3 positionDelta)
    {
        position += positionDelta;
        Vector3 dir = position - masterPosition;
        dir = Quaternion.Euler(rotationDelta) * dir;
		gameObject.transform.Rotate(rotationDelta);

        Vector3 oldPos = position;
        positionDelta += (dir+ masterPosition)-position;
        position += positionDelta;
        gameObject.transform.position = position;

        // propagate to slaves
        if(attributes[25] != 0)
        {
            List<RGScriptedObject> slaves = RGFamilyStore.GetSlaves(attributes[25]);
            for(int i=0;i<slaves.Count;i++)
            {
                slaves[i].PropagateRotation(oldPos, rotationDelta, positionDelta);
            }
        }
    }
    public void PropagateRotation(Vector3 masterPosition, Vector3 rotationDelta, Vector3 positionDelta2)
    {
        //position += positionDelta2;
        Vector3 dir = position - masterPosition;
        dir = Quaternion.Euler(rotationDelta) * dir;
        Vector3 positionDelta = (dir+ masterPosition)-position;
		gameObject.transform.Rotate(rotationDelta);
        PropagatePositionDelta(positionDelta);
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
            case TaskType.task_rotating:
                taskTimer += frameTime;
                Vector3 newRotation;
                if(taskTimer < taskDuration)
                {
                    newRotation = Vector3.Lerp(rotationStart, rotationTarget,taskTimer/taskDuration);
                }
                else
                {
                    newRotation = rotationTarget;
                    currentTask = TaskType.task_idle;
                }
                // propagate to slaves
                if(attributes[25] != 0)
                {
                    Vector3 rotationDelta = newRotation-rotation;
                    List<RGScriptedObject> slaves = RGFamilyStore.GetSlaves(attributes[25]);
                    for(int i=0;i<slaves.Count;i++)
                    {
                        slaves[i].PropagateRotation(position, rotationDelta, Vector3.zero);
                    }
                }
                rotation = newRotation;
                gameObject.transform.eulerAngles = rotation;
                break;
            case TaskType.task_moving:
                taskTimer += frameTime;
                Vector3 newPosition;
                if(taskTimer < taskDuration)
                {
                    newPosition = Vector3.Lerp(positionStart, positionTarget,taskTimer/taskDuration);
                }
                else
                {
                    newPosition = positionTarget;
                    currentTask = TaskType.task_idle;
                }
                // propagate to slaves
                if(attributes[25] != 0)
                {
                    Vector3 positionDelta = newPosition-position;
                    List<RGScriptedObject> slaves = RGFamilyStore.GetSlaves(attributes[25]);
                    for(int i=0;i<slaves.Count;i++)
                    {
                        slaves[i].PropagatePositionDelta(positionDelta);
                    }
                }
                position = newPosition;
                gameObject.transform.position = position;
                break;
            case TaskType.task_waiting:
                taskTimer += frameTime;
                if(taskTimer > taskDuration)
                {
                    currentTask = TaskType.task_idle;
                }
                break;
            case TaskType.task_syncing:
                // VAT ARE YOU SYNCING ABOUT?
                // ModelLoader sets this to 0 when all members are done
                if(sync_point == 0)
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
        functions[44] = RotateByAxis;
        functions[45] = RotateToAxis;
        functions[53] = MoveByAxis;
        functions[60] = Wait;
        functions[271] = SyncWithGroup;
    }
// SOUPDEF function implementations
// ASSUMTIONS:
// time is in seconds/10 < probably about right?
// axis 0,1,2 are local X,Y,Z < dont quite know
    const float TIME_VAL = 0.1f;
    const float DA2DG = -(180.0f/1024.0f); // negative angles?
    /*task 44*/
    public int RotateByAxis(int[] i /*3*/)    
    {
        // rotates X degrees around local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        Debug.Log($"{scriptName}: RotateByAxis");
        currentTask = TaskType.task_rotating;
        rotationCenter = position;
        Vector3 rt;
        switch(i[0])
        {
            case 0:
                rt = new Vector3(((float)i[1])*DA2DG, 0.0f,0.0f);
                break;
            case 1:
                rt = new Vector3(0.0f, ((float)i[1])*DA2DG,0.0f);
                break;
            case 2:
                rt = new Vector3(0.0f, 0.0f,((float)i[1])*DA2DG);
                break;
            default:
                rt = new Vector3(0,0,0);
                break;
        }
        rotationTarget = rotation + rt;
        rotationStart = rotation;
        taskDuration = ((float)i[2])*TIME_VAL;;
        taskTimer = 0;
        return 0;
    }
    /*task 45*/
    public int RotateToAxis(int[] i /*3*/)    
    {
        // Rotate until rotation is X degrees around local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        Debug.Log($"{scriptName}: RotateToAxis");
        currentTask = TaskType.task_rotating;
        rotationCenter = position;
        Vector3 rt;
        switch(i[0])
        {
            case 0:
                rt = new Vector3(((float)i[1])*DA2DG, 0.0f,0.0f);
                break;
            case 1:
                rt = new Vector3(0.0f, ((float)i[1])*DA2DG,0.0f);
                break;
            case 2:
                rt = new Vector3(0.0f, 0.0f,((float)i[1])*DA2DG);
                break;
            default:
                rt = new Vector3(0,0,0);
                break;
        }

        rt.x = Mathf.DeltaAngle(rotation.x, rt.x);
        rt.y = Mathf.DeltaAngle(rotation.y, rt.y);
        rt.z = Mathf.DeltaAngle(rotation.z, rt.z);
        rotationTarget = rotation+rt;
        rotationStart = rotation;
        taskDuration = ((float)i[2])*TIME_VAL;;
        taskTimer = 0;
        return 0;
    }
    /*task 53*/
    public int MoveByAxis(int[] i /*3*/)    
    {
        // Moves along local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        Debug.Log($"{scriptName}: MoveByAxis");
        currentTask = TaskType.task_moving;
        Vector3 mt;
        switch(i[0])
        {
            case 0:
                mt = new Vector3(((float)i[1])*RGM_MPOB_SCALE, 0.0f, 0.0f);
                break;
            case 1:
                mt = new Vector3(0,-(float)((float)i[1]*RGM_MPOB_SCALE),0);
                break;
            case 2:
                mt = new Vector3(0,0,-(float)((float)(0xFFFFFF-i[1])*RGM_MPOB_SCALE));
                break;
            default:
                mt = new Vector3(0,0,0);
                break;
        }
        mt = gameObject.transform.TransformDirection(mt);
        
        positionTarget = position + mt;
        positionStart = position;
        taskDuration = ((float)i[2])*TIME_VAL;;
        taskTimer = 0;
        return 0;
    }
    /*task 60*/
    public int Wait(int[] i /*1*/)    
    {
        // Wait some time
        // i[0]: time to wait
        Debug.Log($"{scriptName}: Wait");
        currentTask = TaskType.task_waiting;

        taskDuration = ((float)i[0])*TIME_VAL;;
        taskTimer = 0;
        return 0;
    }
    /*task 271*/
    public int SyncWithGroup(int[] i /*1*/)    
    {
        // Sync with group to a sync point << ASSUMPTIONS HO
        // i[0]: sync point to wait for
        Debug.Log($"{scriptName}: SyncWithGroup");
        currentTask = TaskType.task_syncing;
        sync_point = (uint)i[0];

        RGFamilyStore.DoGroupSync(attributes[29], sync_point);
        // check if the whole group has the sync point
        return 0;
    }


}
