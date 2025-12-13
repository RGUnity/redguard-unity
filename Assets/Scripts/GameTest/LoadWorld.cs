using UnityEngine;
using System;
using System.Collections.Generic;

public class LoadWorld : MonoBehaviour
{
    void EnableScripting()
    {
// for ISLAND:
        try{
            
            ModelLoader.scriptedObjects[0x82BEC400].EnableScripting(); // ENTHARB1
            ModelLoader.scriptedObjects[0x82BEC3B8].EnableScripting(); // ENTHARB2
            ModelLoader.scriptedObjects[0x82BE8020].EnableScripting(); // ENTHARB3
        }
        catch(Exception ex){}
// for HARBTOWR:
        try{
            
            ModelLoader.scriptedObjects[0x82BD6408].EnableScripting(); // EXTHTW3
            ModelLoader.scriptedObjects[0x82BD6E00].EnableScripting(); // EXTHTW2
            ModelLoader.scriptedObjects[0x82BD6F08].EnableScripting(); // EXTHTW1
        }
        catch(Exception ex){}
// for SILVER1:
        try{
            
            ModelLoader.scriptedObjects[0x82BD6720].EnableScripting(); // KRISANTB
            ModelLoader.scriptedObjects[0x82BD6860].EnableScripting(); // KRISANTB
            ModelLoader.scriptedObjects[0x82BD6920].EnableScripting(); // EXTSILVS
            ModelLoader.scriptedObjects[0x82BD6D08].EnableScripting(); // KRISAND
            ModelLoader.scriptedObjects[0x82BD72D0].EnableScripting(); // KRISANTB
            ModelLoader.scriptedObjects[0x82BD7318].EnableScripting(); // KRISANTB
        }
        catch(Exception ex){}



    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
// OBS entry
//        WorldLoader.LoadWorld(5,0,0);
// OBS exit
//        WorldLoader.LoadWorld(1,6,1024);
// HARB3 entry
// SILVER1 entry
        WorldLoader.LoadWorld(18,0,1024);
        EnableScripting();

        RGRGMScriptStore.flags[203] = 1; // OB_Fixed
        ModelLoader.scriptedObjects[0x82BD9E40].EnableScripting(); // EXTOBSRV
//    	ModelLoader.scriptedObjects[0x82BDBD98].EnableScripting(true); // ERASMO
        if(false)
        {
    //		ModelLoader.scriptedObjects[0x82BD3750].EnableScripting(); // X
    //		ModelLoader.scriptedObjects[0x82BD9D00].EnableScripting(); // X
    		ModelLoader.scriptedObjects[0x82BD9E40].EnableScripting(); // EXTOBSRV
    //		ModelLoader.scriptedObjects[0x82BDA0F0].EnableScripting(); // OB_CAM21
    //		ModelLoader.scriptedObjects[0x82BDA6A0].EnableScripting(); // OB_ENG21
    //		ModelLoader.scriptedObjects[0x82BE4538].EnableScripting(); // OB_TEL03
    //		ModelLoader.scriptedObjects[0x82BE4728].EnableScripting(); // GEARSND
    //		ModelLoader.scriptedObjects[0x82BE4948].EnableScripting(); // OB_PIPE
    //		ModelLoader.scriptedObjects[0x82BE6BC0].EnableScripting(); // OB_ENGW
    //		ModelLoader.scriptedObjects[0x82BE74E8].EnableScripting(); // OB_PIPE
    //		ModelLoader.scriptedObjects[0x82BE7730].EnableScripting(); // OB_ERSL
    //		ModelLoader.scriptedObjects[0x82BE7CA0].EnableScripting(); // OB_ERS10
    //		ModelLoader.scriptedObjects[0x82BDBC58].EnableScripting(); // OB_PLT07
    //		ModelLoader.scriptedObjects[0x82BDBD98].EnableScripting(); // ERASMO
    //		ModelLoader.scriptedObjects[0x82BDBF70].EnableScripting(); // OB_GEAR
    //		ModelLoader.scriptedObjects[0x82BDC140].EnableScripting(); // OB_ENG03
    //		ModelLoader.scriptedObjects[0x82BDC378].EnableScripting(); // OB_PIPE
    //		ModelLoader.scriptedObjects[0x82BDE730].EnableScripting(); // GEARSND
    //		ModelLoader.scriptedObjects[0x82BDFAC0].EnableScripting(); // OB_STJET
    //		ModelLoader.scriptedObjects[0x82BDFC00].EnableScripting(); // OB_STJET
    //		ModelLoader.scriptedObjects[0x82BE09B0].EnableScripting(); // GEARSND
    //		ModelLoader.scriptedObjects[0x82BDED18].EnableScripting(); // OB_PIPE
    //		ModelLoader.scriptedObjects[0x82BDEE58].EnableScripting(); // OB_STJET
    //		ModelLoader.scriptedObjects[0x82BDD1B8].EnableScripting(); // A
    //		ModelLoader.scriptedObjects[0x0].EnableScripting(); // OB_ERS06
    		ModelLoader.scriptedObjects[0x82BDC5B0].EnableScripting(); // OB_ENG23
    		ModelLoader.scriptedObjects[0x82BDC9E8].EnableScripting(); // OB_ENG22
    //		ModelLoader.scriptedObjects[0x82BE2AD8].EnableScripting(); // OB_ENG01
            ModelLoader.scriptedObjects[0x82BDF408].EnableScripting(); // OB_PLT04
            ModelLoader.scriptedObjects[0x82BE3B28].EnableScripting(); // OB_ORY01
            ModelLoader.scriptedObjects[0x82BE0BF0].EnableScripting(); // OB_ORY03
            ModelLoader.scriptedObjects[0x82BDA330].EnableScripting(); // OB_DOR02
            ModelLoader.scriptedObjects[0x82BDA470].EnableScripting(); // OB_DOR01
            ModelLoader.scriptedObjects[0x82BDA9B0].EnableScripting(); // OB_STEPR
            ModelLoader.scriptedObjects[0x82BDABD0].EnableScripting(); // OB_DOME3
            ModelLoader.scriptedObjects[0x82BDAE08].EnableScripting(); // OB_DOME4
            ModelLoader.scriptedObjects[0x82BDB040].EnableScripting(); // OB_DOME5
            ModelLoader.scriptedObjects[0x82BDB358].EnableScripting(); // OB_DOME2
            ModelLoader.scriptedObjects[0x82BDB598].EnableScripting(); // OB_PLT05
            ModelLoader.scriptedObjects[0x82BDB7D0].EnableScripting(); // OB_CON01
            ModelLoader.scriptedObjects[0x82BDB9F0].EnableScripting(); // OB_PLT06
            ModelLoader.scriptedObjects[0x82BDCC28].EnableScripting(); // OB_ENGC
            ModelLoader.scriptedObjects[0x82BDCE60].EnableScripting(); // OB_STEP2
            ModelLoader.scriptedObjects[0x82BDCF98].EnableScripting(); // OB_STEP1
            ModelLoader.scriptedObjects[0x82BDD3F0].EnableScripting(); // OB_CON12
            ModelLoader.scriptedObjects[0x82BDD628].EnableScripting(); // OB_TEL01
            ModelLoader.scriptedObjects[0x82BDD868].EnableScripting(); // OB_TEL00
            ModelLoader.scriptedObjects[0x82BDDB80].EnableScripting(); // OB_DOME1
            ModelLoader.scriptedObjects[0x82BDDDB8].EnableScripting(); // OB_DOME7
            ModelLoader.scriptedObjects[0x82BDE188].EnableScripting(); // OB_DOME6
            ModelLoader.scriptedObjects[0x82BDE3C0].EnableScripting(); // OB_PLT03
            ModelLoader.scriptedObjects[0x82BDE5F0].EnableScripting(); // OB_CON02
            ModelLoader.scriptedObjects[0x82BDE968].EnableScripting(); // OB_PLT02
            ModelLoader.scriptedObjects[0x82BDEAD8].EnableScripting(); // OB_PLT01
            ModelLoader.scriptedObjects[0x82BDF098].EnableScripting(); // OB_PLT08
            ModelLoader.scriptedObjects[0x82BDF628].EnableScripting(); // OB_ORY16
            ModelLoader.scriptedObjects[0x82BDF860].EnableScripting(); // OB_ORY09
            ModelLoader.scriptedObjects[0x82BDFA78].EnableScripting(); // OB_ORY07
            ModelLoader.scriptedObjects[0x82BDFE40].EnableScripting(); // OB_ENGA
            ModelLoader.scriptedObjects[0x82BE0080].EnableScripting(); // OB_ORY11
            ModelLoader.scriptedObjects[0x82BE02B8].EnableScripting(); // OB_ORY17
            ModelLoader.scriptedObjects[0x82BE04D8].EnableScripting(); // OB_ORY15
            ModelLoader.scriptedObjects[0x82BE0740].EnableScripting(); // OB_ORY14
            ModelLoader.scriptedObjects[0x82BE08A0].EnableScripting(); // OB_ORY10
            ModelLoader.scriptedObjects[0x82BE1D10].EnableScripting(); // OB_ORY12
            ModelLoader.scriptedObjects[0x82BE1F78].EnableScripting(); // OB_TEL04
            ModelLoader.scriptedObjects[0x82BE2148].EnableScripting(); // OB_ORY13
            ModelLoader.scriptedObjects[0x82BE2380].EnableScripting(); // OB_ENG12
            ModelLoader.scriptedObjects[0x82BE25A8].EnableScripting(); // OB_ENG11
            ModelLoader.scriptedObjects[0x82BE27C0].EnableScripting(); // OB_ENG13
            ModelLoader.scriptedObjects[0x82BE2D08].EnableScripting(); // OB_PLT00
            ModelLoader.scriptedObjects[0x82BE2F40].EnableScripting(); // OB_ORY08
            ModelLoader.scriptedObjects[0x82BE3178].EnableScripting(); // OB_ORY06
            ModelLoader.scriptedObjects[0x82BE33B8].EnableScripting(); // OB_ORY05
            ModelLoader.scriptedObjects[0x82BE35F8].EnableScripting(); // OB_ORY04
            ModelLoader.scriptedObjects[0x82BE3830].EnableScripting(); // OB_ORY02
            ModelLoader.scriptedObjects[0x82BE3D60].EnableScripting(); // OB_ENG04
            ModelLoader.scriptedObjects[0x82BE42F8].EnableScripting(); // OB_ENG05
            ModelLoader.scriptedObjects[0x82BE4698].EnableScripting(); // OB_CON11
            ModelLoader.scriptedObjects[0x82BE4B68].EnableScripting(); // OB_TEL02
            ModelLoader.scriptedObjects[0x82BE4DA0].EnableScripting(); // OB_CON04
            ModelLoader.scriptedObjects[0x82BE4FD8].EnableScripting(); // OB_CON03
            ModelLoader.scriptedObjects[0x82BE5218].EnableScripting(); // OB_LIFT
            ModelLoader.scriptedObjects[0x82BE5668].EnableScripting(); // OB_CON05
            ModelLoader.scriptedObjects[0x82BE5970].EnableScripting(); // OB_ENG18
            ModelLoader.scriptedObjects[0x82BE5C58].EnableScripting(); // OB_CON08
            ModelLoader.scriptedObjects[0x82BE5D98].EnableScripting(); // OB_LIFT
            ModelLoader.scriptedObjects[0x82BE60A8].EnableScripting(); // OB_ENG25
            ModelLoader.scriptedObjects[0x82BE6490].EnableScripting(); // OB_ENG17
            ModelLoader.scriptedObjects[0x82BE66C8].EnableScripting(); // OB_CON10
            ModelLoader.scriptedObjects[0x82BE6900].EnableScripting(); // OB_CON09
            ModelLoader.scriptedObjects[0x82BE6B78].EnableScripting(); // OB_CON06
            ModelLoader.scriptedObjects[0x82BE7988].EnableScripting(); // OB_CON07
            ModelLoader.scriptedObjects[0x82BE7FB8].EnableScripting(); // OB_ERS08

        }
     }
    // Update is called once per frame
    bool worldChanged;
    void Update()
    {
        if(worldChanged)
            EnableScripting();
        worldChanged = WorldLoader.LoadWorldIfRequested();
        if(Input.GetKeyUp("space"))
        {
            Game.uiManager.ShowInteractionText("HEYOO");
            Game.uiManager.AddDialogueOption("OPT1");
            Game.uiManager.AddDialogueOption("OPT2");
            Game.uiManager.AddDialogueOption("OPT3");
            Game.uiManager.AddDialogueOption("OPT4");
        }
    }
}
