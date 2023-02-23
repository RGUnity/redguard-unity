using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using UnityEngine;

public class LoadSavedNPCs : MonoBehaviour
{
    public void LoadNPCs()
    {
        if (DataSerializer.TryLoad("NPCDataList", out List<NPCData> loadedNPCDataList))
        {
            print("loadedNPCDataList count is " + loadedNPCDataList.Count);
            foreach (var _npcData in loadedNPCDataList)
            {
                if (GameSaveManager.sceneNPCDict.ContainsKey(_npcData.id))
                {
                    //print("[GameSaveManager.sceneNPCDict] contains key " + _npcData.id);
                    
                    // This is broken when the sceneNPCDict is static??? it just loses the object reference
                    GameObject myNPC = GameSaveManager.sceneNPCDict[_npcData.id];
                    
                    // Load values
                    myNPC.GetComponent<NPC>().actionState = _npcData.actionState;
                    myNPC.GetComponent<NPC>().health = _npcData.health;

                    // Load transform data
                    myNPC.transform.position = _npcData.position;
                    myNPC.transform.rotation = _npcData.rotation;
                    //print("Set " + GameSaveManager.sceneNPCDict[_npcData.id] + "to loaded position" + _npcData.position);
                }
                else
                {
                    print("shit");
                }
            }
        }
    }
}
