using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ToolBox.Serialization;
using UnityEngine;

public class SavedNPCLoader : MonoBehaviour
{
    public void LoadNPCs()
    {
        if (DataSerializer.TryLoad("NPCDataList", out List<NPCData> loadedNPCDataList))
        {
            foreach (var _npcData in loadedNPCDataList)
            {
                if (GameSaveManager.sceneNPCDict.ContainsKey(_npcData.id))
                {
                    ApplyNPCData(GameSaveManager.sceneNPCDict[_npcData.id], _npcData);
                }
                else
                {
                    print("No GameObject found for " + _npcData.id + ". Creating new instance.");
                    ApplyNPCData(SpawnNPC(_npcData), _npcData);
                }
            }
            print(loadedNPCDataList.Count + " NPCs loaded from Savefile, scene now contains " + GameSaveManager.sceneNPCDict.Count);
        }
    }

    private void ApplyNPCData(GameObject NPC, NPCData _npcData)
    {

        // Load values
        NPC.GetComponent<NPC>().actionState = _npcData.actionState;
        NPC.GetComponent<NPC>().health = _npcData.health;

        // Load transform data
        NPC.transform.position = _npcData.position;
        NPC.transform.rotation = _npcData.rotation;
        //print("Set " + GameSaveManager.sceneNPCDict[_npcData.id] + "to loaded position" + _npcData.position);
    }

    private GameObject SpawnNPC(NPCData _npcData)
    {
        var newInstance = Instantiate(_npcData.config.prefab, _npcData.position, _npcData.rotation);
        newInstance.GetComponent<NPC>().id = _npcData.id;
        newInstance.GetComponent<NPC>().RegisterNPC();
        return newInstance;
    }
}
