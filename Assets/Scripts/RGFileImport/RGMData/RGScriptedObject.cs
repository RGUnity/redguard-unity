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
        task_waitingtasks,
        task_rotating,
        task_moving,
        task_waiting,
        task_syncing,
    }
	// TODO: these are duplicated from RGRGMStore
	const float RGM_MPOB_SCALE = 1/5120.0f;
	
	string objectName;
	public string scriptName;

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
    public bool DEBUGSCRIPTING=true;
	public bool allowScripting;
    public ScriptData script;
// tasks
    class TaskData
    {
        public TaskType type;
        public float timer;
        public float duration;
        // rotating
        public Vector3 rotationTarget;
        public Vector3 rotationStart;
        // moving
        public Vector3 positionTarget;
        public Vector3 positionStart;
        // waiting
            // nothing here ;)
        // syncing
        public uint syncPoint;
        public TaskData()
        {
            type = TaskType.task_idle;
        }
    }

    TaskData mainTask = new TaskData();
    List<TaskData> multiTasks = new List<TaskData>();
    static int FUNC_CNT = 367;
    Func<bool, int[], int>[] functions;

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

        Vector3 position = Vector3.zero;
 		position.x = (float)(MPOB.posX)*RGM_MPOB_SCALE;
		position.y = -(float)(MPOB.posY)*RGM_MPOB_SCALE;
		position.z = -(float)(0xFFFFFF-MPOB.posZ)*RGM_MPOB_SCALE;
        Vector3 rotation = RGRGMStore.eulers_from_MPOB_data(MPOB);
		gameObject.transform.position = position;
		gameObject.transform.Rotate(rotation);

        allowAnimation = false;

        allowScripting = false;
        SetupFunctions();

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
		
        attributes = new byte[256];
        Array.Copy(filergm.RAAT.attributes, RAHDData.index*256, attributes, 0, 256);

        // DO THIS AFTER SETTING POSITION AND ROTATION
        if(attributes[25] != 0) // attr_master
            RGFamilyStore.AddMaster(attributes[25], this);
        if(attributes[26] != 0) // attr_slave
            RGFamilyStore.AddSlave(attributes[26], this);
        if(attributes[29] != 0) // att_group
            RGFamilyStore.AddToGroup(attributes[29], this);


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

    public void FamilySetParent(RGScriptedObject parent)
    {
        this.transform.SetParent(parent.transform);
    }
    void UpdateTasks()
    {
        if(!allowScripting)
            return;

        UpdateTask(mainTask);
        if(mainTask.type == TaskType.task_idle)
            script.tickScript();
        for(int i=multiTasks.Count-1;i>=0;i--)
        {
            UpdateTask(multiTasks[i]);
            if(multiTasks[i].type == TaskType.task_idle)
                multiTasks.RemoveAt(i);
        }
    }
    void UpdateTask(TaskData taskData)
    {
        float frameTime = Time.deltaTime;
        switch(taskData.type)
        {
            case TaskType.task_idle:
                break;
            case TaskType.task_waitingtasks:
                if(multiTasks.Count == 0)
                    taskData.type = TaskType.task_idle;
                break;
            case TaskType.task_rotating:
                taskData.timer += frameTime;
                Vector3 newRotation;
                if(taskData.timer < taskData.duration)
                {
                    newRotation = Vector3.Lerp(taskData.rotationStart, taskData.rotationTarget,taskData.timer/taskData.duration);
                }
                else
                {
                    newRotation = taskData.rotationTarget;
                    taskData.type = TaskType.task_idle;
                }
                gameObject.transform.localEulerAngles = newRotation;
                break;
            case TaskType.task_moving:
                taskData.timer += frameTime;
                Vector3 newPosition;
                if(taskData.timer < taskData.duration)
                {
                    newPosition = Vector3.Lerp(taskData.positionStart, taskData.positionTarget,taskData.timer/taskData.duration);
                }
                else
                {
                    newPosition = taskData.positionTarget;
                    taskData.type = TaskType.task_idle;
                }
                gameObject.transform.localPosition = newPosition;
                break;
            case TaskType.task_waiting:
                taskData.timer += frameTime;
                if(taskData.timer > taskData.duration)
                {
                    taskData.type = TaskType.task_idle;
                }
                break;
            case TaskType.task_syncing:
                // VAT ARE YOU SYNCING ABOUT?
                // ModelLoader sets this to 0 when all members are done
                if(taskData.syncPoint == 0)
                {
                    taskData.type = TaskType.task_idle;
                }
                break;
            default:
                break;
        }
    }
    void SetupFunctions()
    {
        functions = new Func<bool, int[], int>[FUNC_CNT];
        functions[17] = WaitOnTasks;
        functions[44] = RotateByAxis;
        functions[45] = RotateToAxis;
        functions[53] = MoveByAxis;
        functions[60] = Wait;
        functions[271] = SyncWithGroup;

        // overwrite all non-implemented functions with a NIMPL error
        for(int i=0;i<FUNC_CNT;i++)
        {
            if(functions[i] == null)
                functions[i] = soupdeffcn_nimpl.getNIMPL(i);
        }
        
    }
// SOUPDEF function implementations
// ASSUMTIONS:
// time is in seconds/10 < probably about right?
// axis 0,1,2 are local X,Y,Z < dont quite know
    const float TIME_VAL = 0.1f;
    const float DA2DG = -(180.0f/1024.0f); // negative angles?
    /*task 17*/
    public int WaitOnTasks(bool multitask, int[] i /*0*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_WaitOnTasks({string.Join(",",i)})");
        // Wait for all multitasks to be completed
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_waitingtasks;

        if(multitask == false)
            mainTask = newTask;
        else
            multiTasks.Add(newTask);
        return 0;
    }

    /*task 44*/
    public int RotateByAxis(bool multitask, int[] i /*3*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_RotateByAxis({string.Join(",",i)})");
        // rotates X degrees around local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_rotating;
        newTask.duration = ((float)i[2])*TIME_VAL;;
        newTask.timer = 0;

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
        Vector3 localRotation = transform.localEulerAngles;
        newTask.rotationTarget = localRotation+rt;
        newTask.rotationStart = localRotation;
        if(multitask == false)
            mainTask = newTask;
        else
            multiTasks.Add(newTask);

        return 0;
    }
    /*task 45*/
    public int RotateToAxis(bool multitask, int[] i /*3*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_RotateToAxis({string.Join(",",i)})");
        // Rotate until rotation is X degrees around local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_rotating;
        newTask.duration = ((float)i[2])*TIME_VAL;;
        newTask.timer = 0;

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

        Vector3 localRotation = transform.localEulerAngles;
        rt.x = Mathf.DeltaAngle(localRotation.x, rt.x);
        rt.y = Mathf.DeltaAngle(localRotation.y, rt.y);
        rt.z = Mathf.DeltaAngle(localRotation.z, rt.z);
        newTask.rotationTarget = localRotation+rt;
        newTask.rotationStart = localRotation;
        if(multitask == false)
            mainTask = newTask;
        else
            multiTasks.Add(newTask);
        return 0;
    }
    /*task 53*/
    public int MoveByAxis(bool multitask, int[] i /*3*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_MoveByAxis({string.Join(",",i)})");
        // Moves along local axis
        // i[0]: axis (0/1/2)
        // i[1]: amount
        // i[2]: time to complete
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_moving;
        newTask.duration = ((float)i[2])*TIME_VAL;;
        newTask.timer = 0;

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
        newTask.positionTarget = transform.localPosition + mt;
        newTask.positionStart = transform.localPosition;
        if(multitask == false)
            mainTask = newTask;
        else
            multiTasks.Add(newTask);
        return 0;
    }
    /*task 60*/
    public int Wait(bool multitask, int[] i /*1*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_Wait({string.Join(",",i)})");
        // Wait some time
        // i[0]: time to wait
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_waiting;
        newTask.duration = ((float)i[0])*TIME_VAL;;
        newTask.timer = 0;

        if(multitask == false)
            mainTask = newTask;
        else
            multiTasks.Add(newTask);
        return 0;
    }
    /*task 271*/
    public int SyncWithGroup(bool multitask, int[] i /*1*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_SyncWithGroup({string.Join(",",i)})");
        // Sync with group to a sync point
        // i[0]: sync point to wait for
        mainTask.type = TaskType.task_syncing;
        mainTask.syncPoint = (uint)i[0];

        // unsets the sync point when the whole group is ready
        RGFamilyStore.DoGroupSync(attributes[29], mainTask.syncPoint);
        return 0;
    }
    public bool IsSyncPointSet(uint i)
    {
        if(mainTask.syncPoint == i)
            return true;
        else
            return false;
    }
    public void clearSyncPoint()
    {
        mainTask.syncPoint = 0;
    }
}
