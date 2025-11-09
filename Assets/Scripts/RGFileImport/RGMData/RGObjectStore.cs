using System;
using System.Collections.Generic;
using RGFileImport;
using UnityEngine;

public class RGObjectStore
{
    public class MasterSlavesStruct
    {
        public RGScriptedObject master;
        public List<RGScriptedObject> slaves;
        public MasterSlavesStruct()
        {
            master = null;
            slaves = new List<RGScriptedObject>();
        }
        public void AddMaster(RGScriptedObject o)
        {
            master = o;
            for(int i=0;i<slaves.Count;i++)
            {
                slaves[i].FamilySetParent(master);
            }
        }
        public void AddSlave(RGScriptedObject o)
        {
            slaves.Add(o);
            Debug.Log($"FAMILY: adding slave {o.scriptName}");
            if(master != null)
            {
                Debug.Log($"FAMILY: {o.scriptName}: master NOT NULL");
                for(int i=0;i<slaves.Count;i++)
                {
                    Debug.Log($"FAMILY: {o.scriptName}: setting parent for {i}:{slaves[i].scriptName}");
                    slaves[i].FamilySetParent(master);
                }
            }

        }

    }
    // level objects
    public static Dictionary<uint, RGScriptedObject> scriptedObjects;
    // named objects
    public static Dictionary<string , RGScriptedObject> namedScriptedObjects;
    // special objects
    static RGScriptedObject player;
    static RGScriptedObject camera;
    // TODO: camera belongs here
    // groups
    public static Dictionary<uint, List<RGScriptedObject>> scriptedGroups;
    // master/slaves
    public static Dictionary<uint, MasterSlavesStruct> scriptedSlaves;

    public void Clear()
    {
        // TODO: clear these decently
        if(scriptedObjects != null)
            scriptedObjects = new Dictionary<uint, RGScriptedObject>();
        if(namedScriptedObjects != null)
            namedScriptedObjects = new Dictionary<string, RGScriptedObject>();
        if(scriptedGroups != null)
            scriptedGroups = new Dictionary<uint, List<RGScriptedObject>>();
        if(scriptedSlaves != null)
            scriptedSlaves = new Dictionary<uint, MasterSlavesStruct>();
    }

    public static int DoObjectTask(uint objectId, string subjectName, int taskId, bool isMultiTask, int[] parameters)
    {
        RGScriptedObject subject = null;
        if(subjectName == "object_me")
        {
            subject = scriptedObjects[objectId];
        }
        else if(subjectName == "object_player")
        {
            subject = GetPlayer();
        }
        else if(subjectName == "object_camera")
        {
            subject = GetCamera();
        }
        else
        {
            subject = namedScriptedObjects[subjectName];
        }
        return subject.functions[taskId](objectId, isMultiTask, parameters);
    }
// level objects
    public static void AddObject(uint id, string objectName, RGScriptedObject o)
    {
        Debug.Log($"Adding item {id} with name {objectName}");
        if(scriptedObjects == null)
            scriptedObjects = new Dictionary<uint, RGScriptedObject>();
        scriptedObjects.Add(id, o);

        if(objectName != null)
        {
            if(namedScriptedObjects == null)
                namedScriptedObjects = new Dictionary<string, RGScriptedObject>();
            namedScriptedObjects.Add(objectName, o);
        }
    }

// special objects
    public static void AddPlayer(RGFileImport.RGRGMFile filergm,RGFileImport.RGRGMFile.RGMMPOBItem cyrus_data)
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("PlayerObject");

        playerObjects[0].AddComponent<RGScriptedObject>();
        player = playerObjects[0].GetComponent<RGScriptedObject>();
        player.Instanciate(cyrus_data, filergm, "OBSERVAT");
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

    }
    public static RGScriptedObject GetPlayer()
    {
        return player;
    }
    public static void SetCamera(RGScriptedObject newCamera)
    {
        camera = newCamera;
    }
    public static RGScriptedObject GetCamera()
    {
        return camera;
    }


// groups
    public static void AddToGroup(uint groupId, RGScriptedObject o)
    {
        if(scriptedGroups == null)
            scriptedGroups = new Dictionary<uint, List<RGScriptedObject>>();

        Debug.Log($"FAMILY: added to group {groupId}");
        if( groupId!= 0)
        {
            List<RGScriptedObject> groupMembers;
            if(scriptedGroups.TryGetValue(groupId, out groupMembers))
            {
                groupMembers.Add(o);
            }
            else
            {
                scriptedGroups.Add(groupId, new List<RGScriptedObject>());
                scriptedGroups[groupId].Add(o);
            }
        }
    }
    public static bool DoGroupSync(uint groupId, uint sync_point)
    {
        for(int i=0;i<scriptedGroups[groupId].Count;i++)
        {
            if(!scriptedGroups[groupId][i].IsSyncPointSet(sync_point))
            {
                return false;
            }
        }
        // none out of sync, set to 0
        for(int i=0;i<scriptedGroups[groupId].Count;i++)
        {
            scriptedGroups[groupId][i].clearSyncPoint();
        }
        return true;
    }
// master/slaves
    public static void AddMaster(uint masterId, RGScriptedObject o)
    {
        if(scriptedSlaves == null)
            scriptedSlaves = new Dictionary<uint, MasterSlavesStruct>();
        if(masterId != 0)
        {
            MasterSlavesStruct masterSlaves;
            if(scriptedSlaves.TryGetValue(masterId, out masterSlaves))
            {
                masterSlaves.AddMaster(o);
                Debug.Log($"added master {o.scriptName} to existing master {masterId}");
            }
            else
            {
                masterSlaves = new MasterSlavesStruct();
                masterSlaves.AddMaster(o);
                scriptedSlaves.Add(masterId, masterSlaves);
                Debug.Log($"added master {o.scriptName} to new master {masterId}");
            }
        }
    }

    public static void AddSlave(uint slaveId, RGScriptedObject o)
    {
        if(scriptedSlaves == null)
            scriptedSlaves = new Dictionary<uint, MasterSlavesStruct>();
        if(slaveId != 0)
        {
            MasterSlavesStruct masterSlaves;
            if(scriptedSlaves.TryGetValue(slaveId, out masterSlaves))
            {
                masterSlaves.AddSlave(o);
                Debug.Log($"added slave {o.scriptName} to existing master {slaveId}");
            }
            else
            {
                masterSlaves = new MasterSlavesStruct();
                masterSlaves.AddSlave(o);
                scriptedSlaves.Add(slaveId, masterSlaves);
                Debug.Log($"added slave {o.scriptName} to new master {slaveId}");
            }
        }
    }
}
