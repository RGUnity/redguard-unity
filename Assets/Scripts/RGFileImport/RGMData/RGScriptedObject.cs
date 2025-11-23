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
	
	public string objectName;
	public uint objectId;
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
    Collider collider;

    public bool playerStanding;
    public RGScriptedObject offsetTarget;
    Vector3 offsetDelta;

    public byte[] attributes;

// animations	
	public bool allowAnimation;
    public AnimData animations;
// scripting
    public bool DEBUGSCRIPTING=true;
	bool allowScripting;
    public ScriptData script;

    public List<Vector3> locations;
// tasks
    class TaskData
    {
        public TaskType type;
        public float timer;
        public float duration;
        // rotating
        public Quaternion rotationTarget;
        public Quaternion rotationStart;
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
    public Func<uint, bool, int[], int>[] functions;

    public RGFileImport.RGRGMFile.RGMRAHDItem RAHDData;

    ~RGScriptedObject()
    {
         // remove this object from the object store to prevent leaks
         // Note that meshes will only get cleaned up after calling Resources.UnloadUnusedAssets
         RGObjectStore.RemoveObject(objectId, objectName);
    }

    public void AddLight()
    {
        GameObject lightGO = new GameObject();
        Vector3 parentPosition;
        Quaternion parentRotation;
        this.transform.GetPositionAndRotation(out parentPosition, out parentRotation);
        lightGO.transform.SetPositionAndRotation(parentPosition, parentRotation);

        lightGO.transform.SetParent(this.transform);
		light = lightGO.AddComponent<Light>();
    }

    public void Instanciate3DObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
		animations = new AnimData(MPOB.scriptName);
        currentMesh = 0;

		if(animations.animationData.RAANItems.Count > 0)
		{
			Debug.Log($"ANIMATED {scriptName}");
            type = ScriptedObjectType.scriptedobject_animated;

			RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d,
                                                                    animations.animationData.RAANItems[0].modelFile,
                                                                    name_col,
                                                                    RAHDData.textureId);

            meshes = new Mesh[animations.animationData.RAANItems.Count];
            meshFrameCount = new int[animations.animationData.RAANItems.Count];
            int totalFrames = 0;

            for(int j=0;j<animations.animationData.RAANItems.Count;j++)
            {
                string modelname_frame = animations.animationData.RAANItems[j].modelFile;
                RGMeshStore.UnityData_3D data_frame = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d,
                                                                           modelname_frame,
                                                                           name_col,
                                                                           RAHDData.textureId);
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
			RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d,
                                                                    modelname,
                                                                    name_col,
                                                                    RAHDData.textureId);
		
			skinnedMeshRenderer.sharedMesh = data_3D.mesh;
			skinnedMeshRenderer.SetMaterials(data_3D.materials);
		}

    }

    public void InstanciateLightObject(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm, string name_col)
    {
        type = ScriptedObjectType.scriptedobject_static;
		skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			
        string modelname = MPOB.scriptName;
        RGMeshStore.UnityData_3D data_3D = RGMeshStore.LoadMesh(RGMeshStore.mesh_type.mesh_3d,
                                                                modelname,
                                                                name_col,
                                                                RAHDData.textureId);
    
        skinnedMeshRenderer.sharedMesh = data_3D.mesh;
        skinnedMeshRenderer.SetMaterials(data_3D.materials);

        AddLight();
        light.type = LightType.Point;
        light.intensity = (float)(MPOB.intensity);
        light.range = (float)(MPOB.radius)/20.0f;

    }
    public void InstanciateLight(RGFileImport.RGRGMFile.RGMMPOBItem MPOB, RGFileImport.RGRGMFile filergm)
    {
        type = ScriptedObjectType.scriptedobject_light;
        AddLight();
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
		transform.position = position;
		transform.Rotate(rotation);


        locations = new List<Vector3>();
        locations.Add(position);
        int RALC_offset = RAHDData.RALCOffset/12;
        for(int i=0;i<RAHDData.RALCCount;i++)
        {
            RGRGMFile.RGMRALCItem RALCData = filergm.RALC.items[RALC_offset+i];
            Vector3 loc = position;
            loc.x += (float)(RALCData.offsetX)*RGM_MPOB_SCALE;
            loc.y += -(float)(RALCData.offsetY)*RGM_MPOB_SCALE;
            loc.z += -(float)(0xFFFFFF-RALCData.offsetZ)*RGM_MPOB_SCALE;
            locations.Add(loc);
        }

        allowAnimation = false;

        allowScripting = false;
        SetupFunctions();

        playerStanding = false;
        offsetTarget = null;

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

        script = new ScriptData(MPOB.scriptName, MPOB.id);
		
        attributes = new byte[256];
        Array.Copy(filergm.RAAT.attributes, RAHDData.index*256, attributes, 0, 256);

        if(skinnedMeshRenderer.sharedMesh != null)
        {
            gameObject.AddComponent<MeshCollider>();
            gameObject.GetComponent<MeshCollider>().sharedMesh = skinnedMeshRenderer.sharedMesh;
        }
         
        objectName = null;
        objectId = MPOB.id;
        if(RAHDData.RANMLength > 0)
        {
            MemoryReader RANMReader = new MemoryReader(filergm.RANM.data);
            RANMReader.Seek((uint)RAHDData.RANMOffset, 0);
            char[] curc = RANMReader.ReadChars(RAHDData.RANMLength-1);
            objectName = new string(curc);
        }
        

        // DO THIS AFTER SETTING POSITION AND ROTATION
        EnableScripting();

	}
    public void EnableScripting()
    {
        // administration for RGObjectstore

        /*
        if(attributes[24] != 0) // attr_player_align
            gameObject.tag = "PLAYER_ALIGN";
        */
        if(attributes[25] != 0) // attr_master
            RGObjectStore.AddMaster(attributes[25], this);
        if(attributes[26] != 0) // attr_slave
            RGObjectStore.AddSlave(attributes[26], this);
        if(attributes[29] != 0) // att_group
            RGObjectStore.AddToGroup(attributes[29], this);

         RGObjectStore.AddObject(objectId, objectName, this);
    }
    public void ClearAnim()
    {
        animations.running = false;
        allowAnimation = false;
    }
    public void SetAnim(int animId, int firstFrame)
    {
        if(type == ScriptedObjectType.scriptedobject_animated)
        {
            if(animations.PushAnimation((RGRGMAnimStore.AnimGroup)animId,firstFrame) == 0)
            {
                // this resets the blend frames
                // TODO: smooth reset to frame 0 and then go from there
                // TODO 2: is there a reset frame defined?
                for(int i=0;i<skinnedMeshRenderer.sharedMesh.blendShapeCount;i++)
                    skinnedMeshRenderer.SetBlendShapeWeight(i, 0.0f);
                allowAnimation = true;
                UpdateAnimationsOffset();
            }
            else
                Debug.Log($"{scriptName}: animation {(RGRGMAnimStore.AnimGroup)animId} requested but doesnt exist");
        }
    }
    void UpdateAnimationsOffset()
    {
        int nextframe = animations.peekNextFrame();

        if(nextframe < 0 || nextframe > meshFrameCount[currentMesh])
        {
            int cnt_tot = 0;
            for(int i=0;i<meshFrameCount.Length;i++)
            {
                if(nextframe < (cnt_tot + meshFrameCount[i]))
                {
                    animations.offsetKeyFrame = cnt_tot;
                    currentMesh = i;
                    skinnedMeshRenderer.sharedMesh = meshes[currentMesh];
                    Debug.Log($"need model: {i} OFS: {cnt_tot}");
                    animations.runAnimation(Time.deltaTime, true);
                    break;
                }
                cnt_tot += meshFrameCount[i];
            }
        }
    }
    void UpdateAnimations()
	{
		if (allowAnimation)
		{
            skinnedMeshRenderer.SetBlendShapeWeight(animations.getCurrentFrame(), 0.0f);
            skinnedMeshRenderer.SetBlendShapeWeight(animations.getNextFrame(), 0.0f);
            animations.runAnimation(Time.deltaTime);
       
            /*
            skinnedMeshRenderer.SetBlendShapeWeight(animations.currentKeyFrame, 0.0f);
            skinnedMeshRenderer.SetBlendShapeWeight(animations.nextKeyFrame, 100.0f);
            */

            // for animation blending, we need to track current and next frame
            float blend1 = (animations.frameTime/AnimData.FRAMETIME_VAL)*100.0f;
            float blend2 = 100.0f-blend1;
            skinnedMeshRenderer.SetBlendShapeWeight(animations.getCurrentFrame(), blend1);
            skinnedMeshRenderer.SetBlendShapeWeight(animations.getNextFrame(), blend2);
		}
	}
	void Update()
    {
        UpdateTasks();
        UpdateAnimations();
        if(offsetTarget != null)
        {
            Vector3 offsetDeltaTransformed = offsetTarget.transform.position
                                            -(offsetTarget.transform.rotation*offsetDelta);
            transform.position = offsetDeltaTransformed;
        }
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
        {
            try {
            script.tickScript();
            }
            catch(Exception ex)
            {
                Exception ex2 = ex; 
                while(ex2.InnerException != null)
                {
                    Console.WriteLine($"ex: {ex2.Message}");
                    ex2 = ex2.InnerException;
                }
                throw new Exception($"Script for object {scriptName} failed with error:\n{ex.Message}\nStackTrace:\n${ex.StackTrace}");

            }
        }
        for(int i=multiTasks.Count-1;i>=0;i--)
        {
            UpdateTask(multiTasks[i]);
            if(multiTasks[i].type == TaskType.task_idle)
                multiTasks.RemoveAt(i);
        }
    }
    // LERP a quaternion without forcing it the short way around
    public static Quaternion LerpAbsolute(Quaternion p, Quaternion q, float t)
    {
        Quaternion r = Quaternion.identity;
        r.x = p.x * (1f - t) + q.x * (t);
        r.y = p.y * (1f - t) + q.y * (t);
        r.z = p.z * (1f - t) + q.z * (t);
        r.w = p.w * (1f - t) + q.w * (t);
        return r;
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
                Quaternion newRotation;
                if(taskData.timer < taskData.duration)
                {
                    newRotation = LerpAbsolute(taskData.rotationStart, taskData.rotationTarget,taskData.timer/taskData.duration);
                }
                else
                {
                    newRotation = taskData.rotationTarget;
                    taskData.type = TaskType.task_idle;
                }
                transform.localRotation = newRotation;
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
                transform.localPosition = newPosition;
                break;
            case TaskType.task_waiting:
                taskData.timer += frameTime;
                if(taskData.timer > taskData.duration)
                {
                    taskData.type = TaskType.task_idle;
                }
                break;
            case TaskType.task_syncing:
                // RGObjectStore sets this to 0 when all members are done
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
        functions = new Func<uint, bool, int[], int>[FUNC_CNT];
        functions[17] = WaitOnTasks;
        functions[44] = RotateByAxis;
        functions[45] = RotateToAxis;
        functions[53] = MoveByAxis;
        functions[56] = MoveToLocation;
        functions[60] = Wait;
        functions[62] = Light;
        functions[64] = LightIntensity;
        functions[65] = LightOff;
        functions[66] = LightOffset;
        functions[156] = Offset;
        functions[224] = PlayerStand;
        functions[271] = SyncWithGroup;

        // overwrite all non-implemented functions with a NIMPL error
        for(int i=0;i<FUNC_CNT;i++)
        {
            if(functions[i] == null)
                functions[i] = soupdeffcn_nimpl.getNIMPL(i);
        }
        
    }
    void AddTask(bool multitask, TaskData data)
    {
        if(multitask == false)
            mainTask = data;
        else
            multiTasks.Add(data);
    }
// SOUPDEF function implementations
// ASSUMTIONS:
// time is in seconds/10 < probably about right?
// axis 0,1,2 are local X,Y,Z < dont quite know
    const float TIME_VAL = 0.1f;
    const float DA2DG = -(180.0f/1024.0f); // negative angles?
    /*task 17*/
    public int WaitOnTasks(uint caller, bool multitask, int[] i /*0*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_WaitOnTasks({string.Join(",",i)})");
        // Wait for all multitasks to be completed
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_waitingtasks;

        AddTask(multitask, newTask);
        return 0;
    }

    public Vector3 RGAxisToVector3(int RGAxis)
    {
        Vector3 axis;
        switch(RGAxis)
        {
            case 0:
                axis = Vector3.right;
                break;
            case 1:
                axis = Vector3.up;
                break;
            case 2:
                axis = Vector3.forward;
                break;
            default: 
                axis = Vector3.forward;
                break;
        }
        return Vector3.Scale(axis, new Vector3(1.0f, 1.0f,-1.0f));
    }
    /*task 44*/
    public int RotateByAxis(uint caller, bool multitask, int[] i /*3*/)    
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

        Vector3 axis = RGAxisToVector3(i[0]);
        Quaternion rotationDelta = Quaternion.AngleAxis(((float)i[1])*DA2DG, axis);
        Quaternion localRotation = transform.localRotation;

        newTask.rotationTarget = rotationDelta*localRotation;
        newTask.rotationStart = localRotation;
//        Debug.Log($"lr: {localRotation}\nrt: {rt}\ntar:{newTask.rotationTarget}");
        AddTask(multitask, newTask);

        return 0;
    }
    /*task 45*/
    public int RotateToAxis(uint caller, bool multitask, int[] i /*3*/)    
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

        Vector3 axis = RGAxisToVector3(i[0]);
        Quaternion rotationDelta = Quaternion.AngleAxis(((float)i[1])/DA2DG, axis);
        Quaternion localRotation = transform.localRotation;

        newTask.rotationTarget = rotationDelta;
        newTask.rotationStart = localRotation;
//        Debug.Log($"lr: {localRotation}\nrt: {rt}\ntar:{newTask.rotationTarget}");
        AddTask(multitask, newTask);

        return 0;
    }
    /*task 53*/
    public int MoveByAxis(uint caller, bool multitask, int[] i /*3*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_MoveByAxis({string.Join(",",i)})");
        // Moves along global axis
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
                mt = new Vector3(0,0,(float)(i[1])*RGM_MPOB_SCALE);
                break;
            default:
                mt = new Vector3(0,0,0);
                break;
        }
        Debug.Log($"OUT: {mt}");
        newTask.positionTarget = transform.localPosition + mt;
        newTask.positionStart = transform.localPosition;
        AddTask(multitask, newTask);
        return 0;
    }
    /*task 56*/
    public int MoveToLocation(uint caller, bool multitask, int[] i /*2*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_MoveToLocation({string.Join(",",i)})");
        // Moves to a location
        // i[0]: location ID
        // i[1]: time to complete
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_moving;
        newTask.duration = ((float)i[1])*TIME_VAL;
        newTask.timer = 0;

        newTask.positionTarget = locations[i[0]];
        newTask.positionStart = transform.localPosition;
        AddTask(multitask, newTask);
        return 0;

    }
 
    /*task 60*/
    public int Wait(uint caller, bool multitask, int[] i /*1*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_Wait({string.Join(",",i)})");
        // Wait some time
        // i[0]: time to wait
        TaskData newTask = new TaskData();
        newTask.type = TaskType.task_waiting;
        newTask.duration = ((float)i[0])*TIME_VAL;;
        newTask.timer = 0;

        AddTask(multitask, newTask);
        return 0;
    }
    /*function 62*/
    public int Light(uint caller, bool multitask, int[] i /*2*/)
    {
        // Turns on a light
        // i[0]: light radius
        // i[1]: light intensity

        if(light == null)
            AddLight();
        light.enabled = true;
        light.type = LightType.Point;
        light.intensity = (float)(i[1]);
        light.range = (float)(i[0])/20.0f;
        return 0;
    }
    /*function 64*/
    public int LightIntensity(uint caller, bool multitask, int[] i /*1*/)
    {
        // Sets the light's intensity to something
        // i[0]: light intensity

        if(light == null)
            return 0;
        light.intensity = (float)(i[0]);
        return 0;
    }

    /*function 65*/
    public int LightOff(uint caller, bool multitask, int[] i /*0*/)
    {
        // Turns off a light

        if(light != null)
            light.enabled = false;
        return 0;
    }
    /*function 66*/
    public int LightOffset(uint caller, bool multitask, int[] i /*3*/)
    {
        // Offsets the light along local axis
        // i[0]: offset x
        // i[1]: offset y
        // i[2]: offset z
         
        Debug.Log($"{multitask}_{scriptName}_LightOffset({string.Join(",",i)})");
        if(light != null)
        {

            Vector3 ofs = new Vector3(((float)i[0])*RGM_MPOB_SCALE,
                                       -(float)((float)i[1]*RGM_MPOB_SCALE),
                                       0.0f);
                                       
            if(i[2] != 0)
                ofs.z = -(float)((float)(0xFFFFFF-i[2])*RGM_MPOB_SCALE);
            light.gameObject.transform.localPosition = ofs;
        }
        return 0;
    }
    /*task 156*/
    public int Offset(uint caller, bool multitask, int[] i /*3*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_Offset({string.Join(",",i)})");
        // Offsets the caller's location to this object's location
        // continuously until UnOffset is called
        // i[0]: offset x
        // i[1]: offset y
        // i[2]: offset z
        RGScriptedObject offsetObject = RGObjectStore.scriptedObjects[caller];
        Vector3 offsetPos = new Vector3(((float)i[0])*RGM_MPOB_SCALE,
                                        -(float)((float)i[1]*RGM_MPOB_SCALE),
                                        0.0f);
        if(i[2] != 0)
            offsetPos.z = -(float)((float)(0xFFFFFF-i[2])*RGM_MPOB_SCALE);

        offsetObject.offsetTarget = this;
        offsetObject.offsetDelta = offsetPos;
        return 0;
    }
 
    /*task 224*/
    public int PlayerStand(uint caller, bool multitask, int[] i /*0*/)    
    {
        // returns true if the player is standing on the object
        if(playerStanding)
            return 1;
        else
            return 0;
    }
 

    /*task 271*/
    public int SyncWithGroup(uint caller, bool multitask, int[] i /*1*/)    
    {
        Debug.Log($"{multitask}_{scriptName}_SyncWithGroup({string.Join(",",i)})");
        // Sync with group to a sync point
        // i[0]: sync point to wait for
        mainTask.type = TaskType.task_syncing;
        mainTask.syncPoint = (uint)i[0];

        // unsets the sync point when the whole group is ready
        RGObjectStore.DoGroupSync(attributes[29], mainTask.syncPoint);
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
