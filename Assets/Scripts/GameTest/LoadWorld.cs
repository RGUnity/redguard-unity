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
        cyrus_data.id = 0x1337;
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


        RGObjectStore.AddPlayer(filergm, cyrus_data);
        /*
        playerObject = new GameObject($"CYRUS");

        playerObject.AddComponent<RGScriptedObject>();
        playerObject.GetComponent<RGScriptedObject>().Instanciate(cyrus_data, filergm, "OBSERVAT");
        playerObject.GetComponent<RGScriptedObject>().allowAnimation = true;
        */

        }
    void Start()
    {
        ModelLoader.LoadArea("OBSERVE", "OBSERVAT", "");
        AddPlayer();
        RGRGMScriptStore.flags[203] = 1; // OB_Fixed
    }

    // Update is called once per frame
    void Update()
    {
        if(true)
        {
            /*
		ModelLoader.scriptedObjects[0x82BDF408].allowScripting = true; // OB_PLT04
		ModelLoader.scriptedObjects[0x82BDBC58].allowScripting = true; // OB_PLT07
		ModelLoader.scriptedObjects[0x82BDB598].allowScripting = true; // OB_PLT05
		ModelLoader.scriptedObjects[0x82BDB9F0].allowScripting = true; // OB_PLT06
		ModelLoader.scriptedObjects[0x82BDE3C0].allowScripting = true; // OB_PLT03
		ModelLoader.scriptedObjects[0x82BDE968].allowScripting = true; // OB_PLT02
		ModelLoader.scriptedObjects[0x82BDEAD8].allowScripting = true; // OB_PLT01
		ModelLoader.scriptedObjects[0x82BDF098].allowScripting = true; // OB_PLT08
		ModelLoader.scriptedObjects[0x82BE2D08].allowScripting = true; // OB_PLT00
            */
		ModelLoader.scriptedObjects[0x82BDA9B0].allowScripting = true; // OB_STEPR
		ModelLoader.scriptedObjects[0x82BDCE60].allowScripting = true; // OB_STEP2
		ModelLoader.scriptedObjects[0x82BDCF98].allowScripting = true; // OB_STEP1
        }
        if(true)
        {
//		ModelLoader.scriptedObjects[0x82BD3750].allowScripting = true; // X
//		ModelLoader.scriptedObjects[0x82BD9D00].allowScripting = true; // X
//		ModelLoader.scriptedObjects[0x82BD9E40].allowScripting = true; // EXTOBSRV
//		ModelLoader.scriptedObjects[0x82BDA0F0].allowScripting = true; // OB_CAM21
//		ModelLoader.scriptedObjects[0x82BDA6A0].allowScripting = true; // OB_ENG21
//		ModelLoader.scriptedObjects[0x82BE4538].allowScripting = true; // OB_TEL03
//		ModelLoader.scriptedObjects[0x82BE4728].allowScripting = true; // GEARSND
//		ModelLoader.scriptedObjects[0x82BE4948].allowScripting = true; // OB_PIPE
//		ModelLoader.scriptedObjects[0x82BE6BC0].allowScripting = true; // OB_ENGW
//		ModelLoader.scriptedObjects[0x82BE74E8].allowScripting = true; // OB_PIPE
//		ModelLoader.scriptedObjects[0x82BE7730].allowScripting = true; // OB_ERSL
//		ModelLoader.scriptedObjects[0x82BE7CA0].allowScripting = true; // OB_ERS10
//		ModelLoader.scriptedObjects[0x82BDBC58].allowScripting = true; // OB_PLT07
//		ModelLoader.scriptedObjects[0x82BDBD98].allowScripting = true; // ERASMO
		ModelLoader.scriptedObjects[0x82BDBF70].allowScripting = true; // OB_GEAR
//		ModelLoader.scriptedObjects[0x82BDC140].allowScripting = true; // OB_ENG03
//		ModelLoader.scriptedObjects[0x82BDC378].allowScripting = true; // OB_PIPE
//		ModelLoader.scriptedObjects[0x82BDE730].allowScripting = true; // GEARSND
		ModelLoader.scriptedObjects[0x82BDF408].allowScripting = true; // OB_PLT04
//		ModelLoader.scriptedObjects[0x82BDFAC0].allowScripting = true; // OB_STJET
//		ModelLoader.scriptedObjects[0x82BDFC00].allowScripting = true; // OB_STJET
//		ModelLoader.scriptedObjects[0x82BE09B0].allowScripting = true; // GEARSND
//		ModelLoader.scriptedObjects[0x82BDED18].allowScripting = true; // OB_PIPE
//		ModelLoader.scriptedObjects[0x82BDEE58].allowScripting = true; // OB_STJET
//		ModelLoader.scriptedObjects[0x82BDD1B8].allowScripting = true; // A
//		ModelLoader.scriptedObjects[0x0].allowScripting = true; // OB_ERS06
//		ModelLoader.scriptedObjects[0x82BDC5B0].allowScripting = true; // OB_ENG23
//		ModelLoader.scriptedObjects[0x82BDC9E8].allowScripting = true; // OB_ENG22
//		ModelLoader.scriptedObjects[0x82BE2AD8].allowScripting = true; // OB_ENG01
//		ModelLoader.scriptedObjects[0x82BE3B28].allowScripting = true; // OB_ORY01
//		ModelLoader.scriptedObjects[0x82BE0BF0].allowScripting = true; // OB_ORY03
		ModelLoader.scriptedObjects[0x82BDA330].allowScripting = true; // OB_DOR02
		ModelLoader.scriptedObjects[0x82BDA470].allowScripting = true; // OB_DOR01
		ModelLoader.scriptedObjects[0x82BDA9B0].allowScripting = true; // OB_STEPR
		ModelLoader.scriptedObjects[0x82BDABD0].allowScripting = true; // OB_DOME3
		ModelLoader.scriptedObjects[0x82BDAE08].allowScripting = true; // OB_DOME4
		ModelLoader.scriptedObjects[0x82BDB040].allowScripting = true; // OB_DOME5
		ModelLoader.scriptedObjects[0x82BDB358].allowScripting = true; // OB_DOME2
		ModelLoader.scriptedObjects[0x82BDB598].allowScripting = true; // OB_PLT05
		ModelLoader.scriptedObjects[0x82BDB7D0].allowScripting = true; // OB_CON01
		ModelLoader.scriptedObjects[0x82BDB9F0].allowScripting = true; // OB_PLT06
		ModelLoader.scriptedObjects[0x82BDCC28].allowScripting = true; // OB_ENGC
		ModelLoader.scriptedObjects[0x82BDCE60].allowScripting = true; // OB_STEP2
		ModelLoader.scriptedObjects[0x82BDCF98].allowScripting = true; // OB_STEP1
		ModelLoader.scriptedObjects[0x82BDD3F0].allowScripting = true; // OB_CON12
		ModelLoader.scriptedObjects[0x82BDD628].allowScripting = true; // OB_TEL01
		ModelLoader.scriptedObjects[0x82BDD868].allowScripting = true; // OB_TEL00
		ModelLoader.scriptedObjects[0x82BDDB80].allowScripting = true; // OB_DOME1
		ModelLoader.scriptedObjects[0x82BDDDB8].allowScripting = true; // OB_DOME7
		ModelLoader.scriptedObjects[0x82BDE188].allowScripting = true; // OB_DOME6
		ModelLoader.scriptedObjects[0x82BDE3C0].allowScripting = true; // OB_PLT03
		ModelLoader.scriptedObjects[0x82BDE5F0].allowScripting = true; // OB_CON02
		ModelLoader.scriptedObjects[0x82BDE968].allowScripting = true; // OB_PLT02
		ModelLoader.scriptedObjects[0x82BDEAD8].allowScripting = true; // OB_PLT01
		ModelLoader.scriptedObjects[0x82BDF098].allowScripting = true; // OB_PLT08
		ModelLoader.scriptedObjects[0x82BDF628].allowScripting = true; // OB_ORY16
		ModelLoader.scriptedObjects[0x82BDF860].allowScripting = true; // OB_ORY09
		ModelLoader.scriptedObjects[0x82BDFA78].allowScripting = true; // OB_ORY07
		ModelLoader.scriptedObjects[0x82BDFE40].allowScripting = true; // OB_ENGA
		ModelLoader.scriptedObjects[0x82BE0080].allowScripting = true; // OB_ORY11
		ModelLoader.scriptedObjects[0x82BE02B8].allowScripting = true; // OB_ORY17
		ModelLoader.scriptedObjects[0x82BE04D8].allowScripting = true; // OB_ORY15
		ModelLoader.scriptedObjects[0x82BE0740].allowScripting = true; // OB_ORY14
		ModelLoader.scriptedObjects[0x82BE08A0].allowScripting = true; // OB_ORY10
		ModelLoader.scriptedObjects[0x82BE1D10].allowScripting = true; // OB_ORY12
		ModelLoader.scriptedObjects[0x82BE1F78].allowScripting = true; // OB_TEL04
		ModelLoader.scriptedObjects[0x82BE2148].allowScripting = true; // OB_ORY13
		ModelLoader.scriptedObjects[0x82BE2380].allowScripting = true; // OB_ENG12
		ModelLoader.scriptedObjects[0x82BE25A8].allowScripting = true; // OB_ENG11
		ModelLoader.scriptedObjects[0x82BE27C0].allowScripting = true; // OB_ENG13
		ModelLoader.scriptedObjects[0x82BE2D08].allowScripting = true; // OB_PLT00
		ModelLoader.scriptedObjects[0x82BE2F40].allowScripting = true; // OB_ORY08
		ModelLoader.scriptedObjects[0x82BE3178].allowScripting = true; // OB_ORY06
		ModelLoader.scriptedObjects[0x82BE33B8].allowScripting = true; // OB_ORY05
		ModelLoader.scriptedObjects[0x82BE35F8].allowScripting = true; // OB_ORY04
		ModelLoader.scriptedObjects[0x82BE3830].allowScripting = true; // OB_ORY02
		ModelLoader.scriptedObjects[0x82BE3D60].allowScripting = true; // OB_ENG04
		ModelLoader.scriptedObjects[0x82BE42F8].allowScripting = true; // OB_ENG05
		ModelLoader.scriptedObjects[0x82BE4698].allowScripting = true; // OB_CON11
		ModelLoader.scriptedObjects[0x82BE4B68].allowScripting = true; // OB_TEL02
		ModelLoader.scriptedObjects[0x82BE4DA0].allowScripting = true; // OB_CON04
		ModelLoader.scriptedObjects[0x82BE4FD8].allowScripting = true; // OB_CON03
		ModelLoader.scriptedObjects[0x82BE5218].allowScripting = true; // OB_LIFT
		ModelLoader.scriptedObjects[0x82BE5668].allowScripting = true; // OB_CON05
		ModelLoader.scriptedObjects[0x82BE5970].allowScripting = true; // OB_ENG18
		ModelLoader.scriptedObjects[0x82BE5C58].allowScripting = true; // OB_CON08
		ModelLoader.scriptedObjects[0x82BE5D98].allowScripting = true; // OB_LIFT
		ModelLoader.scriptedObjects[0x82BE60A8].allowScripting = true; // OB_ENG25
		ModelLoader.scriptedObjects[0x82BE6490].allowScripting = true; // OB_ENG17
		ModelLoader.scriptedObjects[0x82BE66C8].allowScripting = true; // OB_CON10
		ModelLoader.scriptedObjects[0x82BE6900].allowScripting = true; // OB_CON09
		ModelLoader.scriptedObjects[0x82BE6B78].allowScripting = true; // OB_CON06
		ModelLoader.scriptedObjects[0x82BE7988].allowScripting = true; // OB_CON07
		ModelLoader.scriptedObjects[0x82BE7FB8].allowScripting = true; // OB_ERS08

        }
    }
}
