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
            // OB_ENG11 = master
            // OB_ENG13 = slaved
            /*
            ModelLoader.scriptedObjects[0x82BE25A8].allowScripting = true;
            // OB_ENG04 + OB_ENG04
            ModelLoader.scriptedObjects[0x82BE3D60].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE42F8].allowScripting = true;
            // OB_ENGA + OB_ENGC
            ModelLoader.scriptedObjects[0x82BDFE40].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BDCC28].allowScripting = true;
            */
            // OB_ORY01-17
            ModelLoader.scriptedObjects[0x82BDF628].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BDF860].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BDFA78].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE0080].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE02B8].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE04D8].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE0740].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE08A0].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE0BF0].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE1D10].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE2148].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE2F40].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE3178].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE33B8].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE35F8].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE3830].allowScripting = true;
            ModelLoader.scriptedObjects[0x82BE3B28].allowScripting = true;
            /*
            // duplicates
            // 14: big blue
            ModelLoader.scriptedObjects[0x82BE0740].allowScripting = true;
            // 15: small blue
            ModelLoader.scriptedObjects[0x82BE04D8].allowScripting = true;
            // 17: inside big red
            ModelLoader.scriptedObjects[0x82BE02B8].allowScripting = true;
            */
        }
    }
}
