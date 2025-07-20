using System;
using System.Collections.Generic;
using RGFileImport;
using UnityEngine;

public class RGFamilyStore
{
    public static Dictionary<uint, List<RGScriptedObject>> scriptedGroups;
    public static Dictionary<uint, List<RGScriptedObject>> scriptedSlaves;
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
        Debug.Log($"FAMILY: syncing group {groupId} to {sync_point}");
        for(int i=0;i<scriptedGroups[groupId].Count;i++)
        {
            if(scriptedGroups[groupId][i].sync_point != sync_point)
                return false;
        }
        Debug.Log($"FAMILY: done syncing group {groupId} to {sync_point}");
        // none out of sync, set to 0
        for(int i=0;i<scriptedGroups[groupId].Count;i++)
        {
            scriptedGroups[groupId][i].sync_point = 0;
        }
        return true;
    }
// master/slaves
    public static void AddSlave(uint masterId, RGScriptedObject o)
    {
        if(scriptedSlaves == null)
            scriptedSlaves = new Dictionary<uint, List<RGScriptedObject>>();

        if( masterId!= 0)
        {
            List<RGScriptedObject> slaves;
            if(scriptedSlaves.TryGetValue(masterId, out slaves))
            {
                slaves.Add(o);
                Debug.Log($"SLAVE: added {o.scriptName} to existing master {masterId}");
            }
            else
            {
                scriptedSlaves.Add(masterId, new List<RGScriptedObject>());
                scriptedSlaves[masterId].Add(o);
                Debug.Log($"SLAVE: added {o.scriptName} to new master {masterId}");
            }
        }
    }
    public static List<RGScriptedObject> GetSlaves(uint masterId)
    {
            List<RGScriptedObject> slaves;
            scriptedSlaves.TryGetValue(masterId, out slaves);
            return slaves;
    }
}
