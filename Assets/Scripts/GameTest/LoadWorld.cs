using UnityEngine;

public class LoadWorld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameObject playerObject;
    void AddPlayer()
    {
        RGFileImport.RGRGMFile filergm = RGRGMStore.GetRGM("OBSERVE");

    // Create scripted objects
        RGFileImport.RGRGMFile.RGMMPOBItem cyrus_data = new RGFileImport.RGRGMFile.RGMMPOBItem();
//        cyrus_data.id;
        cyrus_data.type = RGFileImport.RGRGMFile.ObjectType.object_3d;
//        cyrus_data.isActive;
        cyrus_data.scriptName = "CYRUS";
        /*
        cyrus_data.modelName;
        cyrus_data.isStatic;
        cyrus_data.unknown1;
        */
        cyrus_data.posX = (int)11296768;
        cyrus_data.posY = (int)-884224;
        cyrus_data.posZ = (int)6236416;
        /*
        cyrus_data.anglex;
        cyrus_data.angley;
        cyrus_data.anglez;
        cyrus_data.textureId;
        cyrus_data.imageId;
        cyrus_data.intensity;
        cyrus_data.radius;
        cyrus_data.modelId;
        cyrus_data.worldId;
        cyrus_data.red; 
        cyrus_data.green;
        cyrus_data.blue;
        */


        playerObject = new GameObject($"CYRUS");

        playerObject.AddComponent<RGScriptedObject>();
        playerObject.GetComponent<RGScriptedObject>().Instanciate(cyrus_data, filergm, "OBSERVAT");
        playerObject.GetComponent<RGScriptedObject>().allowAnimation = true;

        }
    void Start()
    {
        
        ModelLoader.RedguardPath = "./game_3dfx";
        ModelLoader.LoadArea("OBSERVE", "OBSERVAT", "");
        AddPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            ModelLoader.scriptedObjects[0x82BE25A8].allowScripting = true;
        }
        else if(Input.GetKeyDown("w"))
        {
            int[] parameters = new int[5];
            parameters[0] = 0;
            parameters[1] = -196608;
            parameters[2] = 40;
            ModelLoader.scriptedObjects[0x82BE25A8].MoveByAxis(parameters);
        }
        else if(Input.GetKeyDown("a"))
        {
            int[] parameters = new int[5];
            parameters[0] = 0;
            parameters[1] = 393216;
            parameters[2] = 72;
            ModelLoader.scriptedObjects[0x82BE25A8].MoveByAxis(parameters);
        }
        else if(Input.GetKeyDown("d"))
        {
            int[] parameters = new int[5];
            parameters[0] = 0;
            parameters[1] = -393216;
            parameters[2] = 72;
            ModelLoader.scriptedObjects[0x82BE25A8].MoveByAxis(parameters);
        }
        
    }
}
