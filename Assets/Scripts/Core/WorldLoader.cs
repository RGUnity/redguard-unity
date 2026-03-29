using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public static class WorldLoader
{
    struct WorldLoadRequest
    {
        public bool loadRequested;
        public FFIWorldStore.WorldData worldData;
        public int playerSpawnLocation;
        public int playerSpawnOrientation;
    }

    static bool SFXLoaded;
    static bool RTXLoaded;
    static bool playerLoaded;
    static List<GameObject> loadedObjects;

    static WorldLoadRequest loadRequest;

    static WorldLoader()
    {
        SFXLoaded = false;
        RTXLoaded = false;
        playerLoaded = false;
        loadedObjects = new List<GameObject>();
    }
    // TODO: this is a(n adapted) copy from RGScriptedObject
    const float DA2DG = (180.0f/1024.0f); // negative angles?

    public static void RequestLoadWorld(int worldId, int playerSpawnLocation, int playerSpawnOrientation)
    {
        loadRequest.loadRequested = true;
        loadRequest.worldData = FFIWorldStore.GetWorldList()[worldId];
        loadRequest.playerSpawnLocation = playerSpawnLocation;
        loadRequest.playerSpawnOrientation = playerSpawnOrientation;

        Material loadScreenMat = FFIGxaLoader.GetMaterial_GXA(loadRequest.worldData.loadScreen, 0);
        if (loadScreenMat != null)
        {
            Game.uiManager.ShowLoadingScreen(loadScreenMat.mainTexture);
        }

    }
    static void UnloadLoadedWorld()
    {
        foreach(GameObject obj in loadedObjects)
        {
            UnityEngine.Object.Destroy(obj);
        }
        Resources.UnloadUnusedAssets();
        RGObjectStore.Clear();
    }

    public static bool LoadWorldIfRequested()
    {
        if(loadRequest.loadRequested)
        {
            loadRequest.loadRequested = false;

            UnloadLoadedWorld();
            LoadWorld(loadRequest.worldData, 
                      loadRequest.playerSpawnLocation,
                      loadRequest.playerSpawnOrientation);

            Debug.Log($"LOADED WORLD: {loadRequest.worldData.RGM}, {loadRequest.playerSpawnLocation}, {loadRequest.playerSpawnOrientation}");
        Game.uiManager.HideLoadingScreen();
            return true;
        }
        return false;
    }
    public static void LoadWorld(FFIWorldStore.WorldData worldData, int playerSpawnLocation, int playerSpawnOrientation)
    {
        string RGM;
        string COL;
        string WLD;
        string SFX;
        string RTX;

        RGM = worldData.RGM;
        COL = worldData.COL;
        WLD = worldData.WLD;
        // this one is not in any INI files?
        SFX = "MAIN";
        // TODO: this should come from SYSTEM.INI
        RTX = "ENGLISH";

        // soundLoading
        if(!SFXLoaded)
        {
            FFISoundStore.LoadSFX(SFX);
            SFXLoaded = true;
        }

        if(!RTXLoaded)
        {
            FFISoundStore.LoadRTX(RTX);
            RTXLoaded = true;
        }


        // load in objects
        loadedObjects = FFIModelLoader.LoadArea(RGM, COL, WLD);

        // load the player
        LoadPlayer(RGM, playerSpawnLocation, playerSpawnOrientation);

        for(int i = 0; i < WorldObjects[RGM].Count;i++)
        {
            
            if (FFIModelLoader.ScriptedObjects.TryGetValue(WorldObjects[RGM][i], out RGScriptedObject scriptedObject))
            {
                scriptedObject.EnableScripting();
            }
        }

    }
    static void LoadPlayer(string RGM, int playerSpawnLocation, int playerSpawnOrientation)
    {
        // only load player once, this is a bit of a hack, we should just clear it decently
        if(!playerLoaded)
        {
            // TODO: we need to account for gremlin too
            RGFileImport.RGRGMFile filergm = FFIModelLoader.CurrentRgmData;

            // Create scripted object
            RGFileImport.RGRGMFile.RGMMPOBItem cyrus_data = new RGFileImport.RGRGMFile.RGMMPOBItem();
            cyrus_data.id = 0x1337;
            cyrus_data.type = RGFileImport.RGRGMFile.ObjectType.object_3d;
            cyrus_data.scriptName = "CYRUS";
    /*
            // this doesnt work, we need the gameObject
            cyrus_data.posX = (int)9490432;
            cyrus_data.posY = (int)-98304;
            cyrus_data.posZ = (int)10362880;
    */

            RGObjectStore.AddPlayer(filergm, cyrus_data);
            RGObjectStore.GetPlayer().EnableScripting();
            playerLoaded = true;
        }

        Vector3 playerSpawnPos = Vector3.zero;
        if (RGObjectStore.mapMarkerList != null && playerSpawnLocation >= 0 && playerSpawnLocation < RGObjectStore.mapMarkerList.Count)
        {
            playerSpawnPos = RGObjectStore.mapMarkerList[playerSpawnLocation];
        }
        Quaternion playerSpawnRot = Quaternion.AngleAxis(((float)playerSpawnOrientation)*DA2DG, Vector3.up);

        RGObjectStore.GetPlayerMain().SetPositionAndRotation(playerSpawnPos, playerSpawnRot);
    }


    // list of all game objects per map
    // uncommenting these will enable the script for the object
    static Dictionary<string, List<uint>> WorldObjects = new Dictionary<string, List<uint>> {
        {"START" , new List<uint>{
            // 0x8384DD60, // SHARK
            // 0x8384DDA8, // WATERSND
            // 0x8384DEE8, // SHARK
            // 0x8384E028, // SPIRATE3
            // 0x8384E168, // SPIRATE4
            // 0x8384E2A8, // SPIRATE5
            // 0x8384E3E8, // SVANDAR
            // 0x8384E530, // VANDBOAT
            // 0x8384E578, // SHARK
            // 0x8384E5C0, // SHARK
            // 0x8384E608, // SHARK
            // 0x8384E650, // SHARK
            // 0x8384E698, // SHARK
            // 0x8384E6E0, // WATERSND
            // 0x8384E820, // SHARK
            // 0x8384EA48, // SCENEST
            // 0x8384EB88, // BOAT1
            // 0x8384ECC8, // SBRENNAN
            // 0x8384EE08, // SPIRATE1
            // 0x8384EF40, // SPIRATE2
            // 0x8384F080, // SBOAT
            // 0x8384F0C8, // WATERSND
            // 0x8384F110, // SHARK
            // 0x8384F158, // WATERSND
            // 0x8384F1A0, // WATERSND
            // 0x8384F1E8, // SHARK
            // 0x8384F230, // SHARK
            // 0x8384F278, // SHARK
            // 0x8384F2C0, // SHARK
            // 0x8384F308, // SHARK
            // 0x0, // SHARK
            }
        },
        {"DRINT" , new List<uint>{
            // 0x8384EAC0, // TORCH
            // 0x8384F698, // DRLIGHT1
            // 0x8384F848, // DR_LASER
            // 0x8384FA88, // A
            // 0x8384FBC0, // DR_FLR1
            // 0x838502A8, // A
            0x838504F0, // EXTDR01
            0x838506B8, // XGOLD05
            0x838507F8, // XHEALTH
            0x83850840, // XGOLD05
            0x83850980, // XHEALTH
            // 0x83850AC0, // LAVABUB1
            // 0x83850D08, // DR_TRAK3
            0x83850D50, // XGOLD05
            // 0x83850D98, // LAVABUB1
            // 0x83850DE0, // LAVABUB1
            // 0x83850E28, // LAVABUB1
            // 0x83850E70, // LAVABUB1
            // 0x83851268, // LAVABUB1
            0x838512B0, // EXTDR03
            0x83851328, // XHEALTH
            // 0x83851370, // TORCH
            // 0x838516B8, // DRLIGHT1
            // 0x83851700, // LAVABUB1
            // 0x83851A68, // LAVABUB1
            // 0x83851AB0, // GUARDTST
            // 0x83851CE8, // TORCH
            // 0x838521E0, // FIRESND
            // 0x838526A8, // DR_LVRD2
            // 0x838527E8, // DR_DORD2
            // 0x83852830, // DRLIGHT1
            // 0x838528A8, // TORCH
            0x838529E8, // XHEALTH
            0x83852C08, // XIRONSK
            0x83852D60, // XSTRENTH
            0x83852DA8, // XSTRENTH
            0x83852DF0, // XGOLD05
            0x83852E38, // XGOLD05
            0x83852E80, // XGOLD05
            0x83852EC8, // XGOLD05
            0x83852F10, // XHEALTH
            // 0x83852F78, // LAVABUB1
            // 0x83853008, // FIRESND
            // 0x83853598, // TORCH
            // 0x838537C8, // DR_DORF1
            // 0x83853908, // DR_BLC
            0x83853950, // XHEALTH
            0x83853998, // XHEALTH
            0x838539E0, // XSTRENTH
            0x83853A28, // XHEALTH
            0x83853A70, // XHEALTH
            0x83853BB0, // XHEALTH
            // 0x83853CF0, // DRBOT03
            // 0x83853D38, // GUARDTS2
            // 0x84352090, // GUARDTST
            // 0x84352108, // DRLIGHT1
            // 0x84352150, // TORCH
            // 0x843523E0, // DRLIGHT1
            // 0x84352568, // DR_DORE
            0x843525B0, // XGOLD05
            0x84352878, // XHEALTH
            // 0x843528C0, // LAVABUB1
            // 0x84352908, // LAVABUB1
            // 0x84352950, // LAVABUB1
            // 0x84352998, // LAVABUB1
            // 0x843529E0, // LAVABUB1
            // 0x84352A28, // LAVABUB1
            // 0x84352A70, // LAVABUB1
            // 0x84352CD0, // LAVABUB1
            // 0x84352F10, // DR_DORC
            // 0x84352F58, // LAVABUB1
            // 0x84352FA0, // LAVABUB1
            // 0x84352FE8, // LAVABUB1
            // 0x84353030, // LAVABUB1
            // 0x84353078, // LAVABUB1
            // 0x843530C0, // LAVABUB1
            0x84353458, // XHEALTH
            // 0x843535E8, // DR_ARM2
            // 0x84353630, // TORCH
            // 0x843536C0, // DRLIGHT1
            // 0x84353830, // TORCH
            // 0x84353978, // WATRFALL
            // 0x84353AB8, // WATRFALL
            // 0x84353BF8, // GUARDTS2
            // 0x84353D38, // DR_DORD1
            // 0x84353EC8, // DR_TRIG2
            // 0x84354008, // DR_TRIG1
            // 0x84354148, // DR_WATER
            // 0x84354380, // DR_SWD2
            // 0x843544C0, // DR_SWD1
            // 0x843546B8, // DR_ARM1
            // 0x84354918, // GUARDTS2
            // 0x84354B50, // DR_DORB2
            // 0x84354C98, // DR_DORB1
            // 0x843550E8, // TORCH
            0x84355130, // XHEALTH
            0x84355340, // XHEALTH
            0x84355388, // XGOLD05
            0x843553D0, // XGOLD05
            0x84355418, // XHEALTH
            0x843556A8, // XHEALTH
            // 0x843558E8, // GOLMDEAD
            // 0x84355AC0, // XDRGEAR
            // 0x84355CB8, // GOLEM
            // 0x84355DF8, // DR_BLC
            // 0x84356018, // DR_BLC
            0x84356060, // XMONEY
            0x843560A8, // XHEALTH
            // 0x84356158, // TORCH
            // 0x84356620, // FIRESND
            // 0x84356788, // DR_LVRDC
            // 0x843567D0, // LAVABUB1
            // 0x84356818, // LAVABUB1
            // 0x84356860, // LAVABUB1
            // 0x843568A8, // LAVABUB1
            // 0x843568F0, // LAVABUB1
            // 0x843569D8, // LAVABUB1
            // 0x84356A20, // LAVABUB1
            // 0x84356A68, // LAVABUB1
            // 0x84356AB0, // LAVABUB1
            // 0x84356AF8, // LAVABUB1
            // 0x84356B40, // LAVABUB1
            // 0x84356BF0, // LAVABUB1
            // 0x84356C38, // FIRESND
            // 0x84356C80, // TORCH
            // 0x84356CC8, // LAVABUB1
            // 0x84356D10, // LAVABUB1
            // 0x84356D58, // LAVABUB1
            // 0x84356DA0, // LAVABUB1
            // 0x84356DE8, // LAVABUB1
            // 0x84356E30, // LAVABUB1
            // 0x843570B8, // LAVABUB1
            0x84357100, // XGOLD05
            // 0x84357148, // LAVABUB1
            // 0x84357190, // LAVABUB1
            // 0x843571D8, // LAVABUB1
            // 0x84357220, // LAVABUB1
            // 0x84357360, // LAVABUB1
            // 0x84357458, // DR_LVRD1
            // 0x843574A0, // FIRESND
            // 0x843575C0, // TORCH
            // 0x84357608, // LAVABUB1
            // 0x84357650, // LAVABUB1
            // 0x843576F8, // LAVABUB1
            // 0x84357830, // DR_BLC
            0x84357878, // XHEALTH
            0x843578C0, // XHEALTH
            0x84357908, // XGOLD05
            // 0x84357A48, // DR_BLC
            // 0x84357DE0, // DR_BLC
            // 0x84357E58, // LAVABUB1
            // 0x84357EA0, // LAVABUB1
            // 0x84357EE8, // LAVABUB1
            // 0x84357F30, // LAVABUB1
            // 0x84357F78, // LAVABUB1
            // 0x84357FC0, // LAVABUB1
            // 0x84358008, // LAVABUB1
            // 0x84358098, // LAVABUB1
            // 0x84358148, // TORCH
            // 0x843581D8, // FIRESND
            0x84358220, // XIRONSK
            // 0x84358268, // LAVABUB1
            // 0x843582B0, // LAVABUB1
            // 0x843584F8, // LAVABUB1
            // 0x84358540, // DRBOT02
            0x84358588, // XGOLD05
            0x843585D0, // XGOLD05
            0x84358618, // XGOLD05
            // 0x84358660, // TORCH
            // 0x843589F0, // DRLIGHT1
            0x84358C08, // XHEALTH
            // 0x84358FD8, // DR_DORB
            // 0x84359020, // LAVABUB1
            // 0x84359330, // LAVABUB1
            0x84359378, // XHEALTH
            // 0x84359500, // GUARDTS2
            // 0x84359590, // TORCH
            // 0x84359CB0, // FIRESND
            0x8435A018, // XHEALTH
            // 0x8435A258, // DR_CLOK5
            // 0x8435A490, // DR_CLOK4
            // 0x8435A788, // DR_CLOK6
            // 0x8435A9C0, // DR_CLOK3
            // 0x8435AD68, // DR_CLOK2
            // 0x8435ADB0, // LAVABUB1
            // 0x8435ADF8, // LAVABUB1
            // 0x8435AE40, // LAVABUB1
            // 0x8435AE88, // LAVABUB1
            // 0x8435AED0, // LAVABUB1
            // 0x8435AF18, // LAVABUB1
            // 0x8435B038, // LAVABUB1
            // 0x8435B080, // LAVABUB1
            // 0x8435B0F8, // LAVABUB1
            // 0x8435B188, // DRLIGHT1
            0x8435B1D0, // XHEALTH
            0x8435B278, // XHEALTH
            // 0x8435B3B8, // GUARDTS2
            // 0x8435B6A8, // DR_STSND
            // 0x8435B8E8, // DR_FLWR2
            // 0x8435BB28, // DR_FLWR1
            // 0x8435BD60, // DR_FLWRA
            // 0x835E23C8, // DR_FLWRC
            // 0x835E2410, // DR_FLWR2
            // 0x835E2550, // DR_FLWR1
            // 0x835E2690, // DR_FLWRB
            // 0x835E2720, // DR_FLWRD
            0x835E28B8, // XSTRENTH
            // 0x835E2900, // LAVABUB1
            // 0x835E2948, // LAVABUB1
            // 0x835E2990, // LAVABUB1
            // 0x835E29D8, // LAVABUB1
            // 0x835E2B40, // LAVABUB1
            // 0x835E2B88, // LAVABUB1
            // 0x835E2BD0, // LAVABUB1
            // 0x835E2C18, // LAVABUB1
            // 0x835E2C60, // LAVABUB1
            // 0x835E2CA8, // TORCH
            // 0x835E2E78, // DRLIGHT1
            // 0x835E2F08, // FIRESND
            // 0x835E2F50, // TORCH
            // 0x835E2F98, // DR_FLWR2
            // 0x835E30D8, // DR_FLWR1
            // 0x835E3218, // DR_FLWR3
            // 0x835E34D0, // DR_FLWR6
            0x835E3518, // XMONEY
            0x835E3560, // XHEALTH
            // 0x835E35A8, // LAVABUB1
            0x835E3638, // XHEALTH
            0x835E37F0, // XMONEY
            // 0x835E3B10, // DR_LOAD4
            // 0x835E3C50, // DR_LOAD1
            // 0x835E3EB8, // DR_WALK3
            // 0x835E40F8, // DR_LOAD3
            // 0x835E4310, // DR_LOAD2
            // 0x835E4558, // A
            // 0x835E46C8, // DR_WALK2
            // 0x835E43F0, // DR_BRSND
            // 0x835E47E0, // LAVABUB1
            // 0x835E4828, // LAVABUB1
            // 0x835E4870, // LAVABUB1
            // 0x835E48B8, // LAVABUB1
            // 0x835E4900, // LAVABUB1
            // 0x835E4948, // LAVABUB1
            // 0x835E4C50, // DRBOT02
            // 0x835E4D90, // DR_ARRO2
            // 0x835E4DD8, // DR_WALK1
            // 0x835E4E20, // GUARDTS2
            // 0x835E52A0, // GUARDTST
            0x835E53E0, // XHEALTH
            // 0x835E5458, // DR_ARRO1
            // 0x835E54A0, // LAVABUB1
            // 0x835E54E8, // LAVABUB1
            // 0x835E5530, // LAVABUB1
            // 0x835E5578, // LAVABUB1
            // 0x835E5790, // LAVABUB1
            // 0x835E5A80, // DR_WIND
            // 0x835E5D90, // DR_BRG06
            // 0x835E5FC0, // DR_BRG04
            // 0x835E61F8, // DR_BRG03
            // 0x835E6330, // DR_BRG02
            // 0x835E63A8, // DRBOT02
            // 0x835E6558, // DRLIGHT1
            0x835E6738, // XHEALTH
            // 0x835E6780, // LAVABUB1
            // 0x835E67C8, // LAVABUB1
            // 0x835E6810, // LAVABUB1
            // 0x835E6858, // LAVABUB1
            // 0x835E68A0, // LAVABUB1
            // 0x835E68E8, // LAVABUB1
            // 0x835E6930, // LAVABUB1
            // 0x835E6A50, // DR_WIND
            // 0x835E6B10, // DRBOT02
            // 0x835E6D50, // GUARDTS2
            // 0x835E6D98, // LAVABUB1
            // 0x835E6DE0, // LAVABUB1
            // 0x835E6E28, // LAVABUB1
            0x835E6E70, // XMONEY
            0x835E6EB8, // XSTRENTH
            // 0x835E6F00, // LAVABUB1
            // 0x835E6F48, // LAVABUB1
            // 0x835E6F90, // LAVABUB1
            // 0x835E6FD8, // LAVABUB1
            // 0x835E7068, // LAVABUB1
            // 0x835E70D0, // FIRESND
            // 0x835E74A0, // TORCH
            // 0x835E74E8, // TORCH
            // 0x835E7628, // DRLIGHT1
            // 0x835E7A60, // DRLIGHT1
            0x835E7B68, // XHEALTH
            // 0x835E7BB0, // LAVABUB1
            0x835E7BF8, // XHEALTH
            0x835E7C40, // XHEALTH
            0x835E8420, // XHEALTH
            0x835E86E0, // XHEALTH
            // 0x835E8820, // SCB_DOOR
            // 0x835E8968, // SCB_DOOR
            // 0x835E8AB0, // SCB_DOOR
            // 0x835E8BF0, // SCB_DOOR
            // 0x835E8E10, // SCB_DOOR
            // 0x835E9178, // SCB_CMR3
            // 0x835E93B0, // SCB_ELV1
            // 0x835E9648, // SCB_CHNR
            // 0x835E9788, // SCB_CHNL
            0x835E98C8, // EXTDR02
            // 0x835E9910, // SCB_CMR1
            // 0x835E9A50, // SCB_ELV1
            // 0x835E9BB0, // DR_DEAD1
            // 0x835E9CF0, // DR_DEAD1
            // 0x835E9EF8, // DR_DEAD1
            // 0x835E9F88, // SCB_LVRL
            // 0x8435C3D8, // SCB_HNDR
            // 0x8435C618, // SCB_HNDL
            // 0x8435C858, // SCB_PVTL
            // 0x8435CA90, // SCB_PVTR
            // 0x8435CCC8, // SCB_ARML
            // 0x8435CF00, // SCB_ARMR
            // 0x8435D118, // SCB_SHEL
            // 0x8435D350, // SCB_SHDL
            // 0x8435D588, // SCB_SHDR
            // 0x8435D7C8, // SCB_LVRR
            // 0x8435DA28, // SCB_TAIL
            // 0x8435DA70, // SCB_SPK
            // 0x8435DAB8, // SCB_SPK
            // 0x8435DB00, // SCB_SPK
            // 0x8435DB48, // SCB_SPK
            // 0x8435DB90, // SCB_SPK
            // 0x8435DBD8, // SCB_SPK
            // 0x8435DC20, // SCB_SPK
            // 0x8435DC68, // SCB_SPK
            // 0x8435DCB0, // SCB_SPK
            // 0x8435DCF8, // SCB_SPK
            // 0x8435DD40, // SCB_SPK
            // 0x8435DE80, // SCB_SPK
            // 0x8435E0C8, // SCB_PAD
            // 0x8435E2A8, // SCB_ELV2
            // 0x8435E2F0, // SCB_CMR2
            // 0x8435E338, // SCB_ELV1
            // 0x8435E478, // SCB_ELV2
            // 0x8435E5B8, // SCB_CHNM
            // 0x8435E7F0, // SCB_HEAD
            // 0x8435EA28, // SCB_LEGS
            // 0x8435ED98, // SCB_PNCR
            0x8435EDE0, // XHEALTH
            // 0x0, // XHEALTH
            }
        },
        {"PALACE" , new List<uint>{
            // 0x83850710, // GUARDPL1
            // 0x83850948, // TORCH
            0x838509C0, // EXTPLF
            // 0x83850C58, // TORCH
            // 0x83850DB8, // GUARDPL2
            // 0x83850E70, // FIRESND
            // 0x83850EB8, // TORCH
            // 0x83851090, // TORCH
            // 0x838512D0, // SCENE1
            // 0x83851410, // GUARDPL3
            // 0x83851640, // PI_DOR3B
            // 0x83851688, // PI_DOR3A
            // 0x83851738, // PI_DOR3B
            // 0x83851780, // TORCH
            // 0x838517E8, // FIRESND
            // 0x83851898, // TORCH
            // 0x838518E0, // TORCH
            // 0x83851948, // TORCH
            // 0x83851B60, // FIRESND
            // 0x83851CA0, // PI_TAP
            // 0x83851DE0, // PI_TAP
            // 0x83851F20, // GUARDTSP
            0x838520E8, // XBUCKET
            // 0x83852228, // PI_PIG00
            // 0x83852290, // TORCH
            // 0x838523D0, // FIRESND
            // 0x83852438, // TORCH
            // 0x838524A0, // TORCH
            // 0x838524E8, // FIRESND
            // 0x83853688, // FIRESND
            // 0x838536D0, // GUARDTSP
            // 0x83853738, // GUARDTSP
            // 0x83853958, // FIRESND
            // 0x83853A98, // TORCH
            // 0x83853CB8, // PI_DOR2B
            // 0x83853DF0, // GUARDTSP
            // 0x83853F30, // DRAM
            // 0x83853F78, // RICHTON
            // 0x84352100, // PI_DOR2B
            // 0x84352148, // PI_DOR2A
            // 0x843521B0, // TORCH
            // 0x843524E8, // FIRESND
            0x84352688, // XHEALTH
            // 0x843526D0, // TORCH
            // 0x84352780, // FIRESND
            // 0x84352830, // TORCH
            // 0x843529B0, // TORCH
            // 0x84352E38, // PI_PIG01
            // 0x84353378, // FIRESND
            // 0x843539F0, // PI_KITDR
            // 0x84353B10, // PI_POTW
            // 0x84353C80, // TORCH
            // 0x84353DE0, // TORCH
            // 0x84354018, // PI_POT1
            // 0x84354158, // PI_POT2
            // 0x84354398, // FIRESND
            // 0x843546B0, // PI_DORL1
            // 0x84354A78, // PI_FIRE
            // 0x84354AC0, // TORCH
            // 0x84354B48, // TORCH
            // 0x84354B90, // FIRESND
            // 0x84354BD8, // FIRESND
            // 0x84354C20, // TORCH
            // 0x84354CA8, // TORCH
            // 0x84354CF0, // FIRESND
            // 0x84355098, // FIRESND
            // 0x843550E0, // GUARDTSP
            0x84355250, // XHEALTH
            0x84355448, // EXTPAL3
            // 0x843556A8, // GUARDTSP
            // 0x84355838, // PI_LVRA
            // 0x84355A70, // PI_PLTA
            // 0x84355BD0, // PI_PILA
            // 0x84355C18, // TORCH
            // 0x84355CA8, // FIRESND
            // 0x84355D10, // FIRESND
            // 0x84356288, // TORCH
            // 0x843563D0, // PI_FLR
            0x84356418, // XHEALTH
            0x84356460, // XHEALTH
            // 0x843564A8, // GUARDTSP
            // 0x84356640, // GUARDTSP
            // 0x84356688, // FIRESND
            // 0x84356D80, // TORCH
            // 0x84356EC0, // GUARDTS3
            // 0x84357000, // PI_DORLX
            // 0x84357048, // PI_CHNDA
            // 0x84357090, // PI_CHNDA
            // 0x843571D0, // TORCH
            // 0x84357410, // PI_CHND1
            // 0x84357648, // PI_DORL3
            // 0x84357F30, // PI_DORL2
            // 0x84357F78, // GUARDTSP
            // 0x84357FC0, // TORCH
            // 0x84358008, // PI_CHNDA
            // 0x84358148, // PI_CHNDA
            // 0x84358190, // PI_CHNDB
            // 0x843581D8, // PI_CHNDB
            // 0x84358220, // PI_CHNDB
            // 0x84358360, // PI_CHNDB
            // 0x835E2480, // PI_CHND2
            // 0x835E2620, // PI_DORL5
            // 0x835E2760, // TORCH
            // 0x835E27A8, // PI_CHNDC
            // 0x835E27F0, // PI_CHNDC
            // 0x835E2838, // PI_CHNDC
            // 0x835E2978, // PI_CHNDC
            // 0x835E2E88, // PI_CHND3
            0x835E2ED0, // XHEALTH
            // 0x835E3308, // GUARDTSP
            // 0x835E3350, // GUARDTSP
            // 0x835E3490, // GUARDTSP
            // 0x835E3740, // PI_DOR4B
            // 0x835E3938, // PI_DOR4A
            // 0x835E3AC0, // PI_DOR4B
            0x835E3CF0, // XIRONSK
            0x835E3F08, // XSTRENTH
            0x835E4310, // XHEALTH
            0x835E43E8, // XHEALTH
            // 0x835E4570, // GUARDTS3
            // 0x835E46B0, // PI_DORL4
            // 0x835E46F8, // TORCH
            // 0x835E4780, // TORCH
            // 0x835E47C8, // FIRESND
            // 0x835E50A0, // FIRESND
            0x835E5130, // EXTPAL2
            // 0x835E5198, // TORCH
            // 0x835E5930, // FIRESND
            // 0x835E5A08, // GUARDTSP
            // 0x835E5A50, // PI_DORL4
            // 0x835E5A98, // PI_DORL4
            // 0x835E5F80, // TORCH
            // 0x835E6348, // GUARDTSP
            // 0x835E6728, // XPALKEY
            // 0x835E68B8, // XCHEST
            // 0x835E6900, // XCHEST
            // 0x835E6990, // XCHEST
            // 0x835E6A68, // XCHEST
            // 0x835E6AB0, // GUARDTS3
            // 0x835E6AF8, // GUARDTS3
            // 0x835E6D80, // GUARDTS3
            // 0x835E6DE8, // TORCH
            // 0x835E6E30, // FIRESND
            // 0x835E70D8, // TORCH
            // 0x835E7168, // XCHEST
            // 0x835E71F8, // XCHEST
            // 0x835E7240, // XCHEST
            // 0x835E72D0, // XCHEST
            // 0x835E7890, // XCHEST
            // 0x835E79D0, // PI_PLAN
            // 0x835E7E08, // PI_WANT
            // 0x835E7F48, // GUARDTS3
            0x835E8088, // EXTPAL4
            0x835E80D0, // EXTPAL1
            // 0x835E8178, // TORCH
            // 0x835E83B0, // FIRESND
            0x835E84F0, // EXTPAL5
            0x835E8630, // EXTPAL6
            0x835E8678, // EXTPAL7
            // 0x835E86E0, // TORCH
            // 0x835E87D8, // FIRESND
            // 0x835E8820, // FIRESND
            // 0x0, // TORCH
            }
        },
        {"ISLAND" , new List<uint>{
            // 0x828AC850, // X
            // 0x828AE4B8, // X
            // 0x828AEDD0, // X
            // 0x828B6E00, // X
            // 0x828B6F48, // SHARK
            // 0x828B6F90, // SHARK
            // 0x828B70C8, // SHARK
            // 0x828B7210, // A
            // 0x828B7258, // SHARK
            // 0x828B7398, // SHARK
            0x828B74D0, // DOLPHIN
            0x828B7518, // DOLPHIN
            // 0x828B7658, // SHARK
            // 0x828B7798, // WATERSND
            0x828B7978, // XGOLD05
            0x828B79C0, // SEAGULL3
            // 0x828B7BB8, // A
            0x828B7CF8, // DOLPHIN
            0x828B7E38, // SEAGULL
            0x82BD2DA0, // SEAGULL2
            // 0x82BD2DE8, // SHARK
            // 0x82BD2F10, // WATERSND
            // 0x82BD3050, // THUGIS1
            // 0x82BD3098, // RNDGOLD5
            // 0x82BD30E0, // WATERSND
            // 0x82BD3128, // SHARK
            // 0x82BD3170, // SHARK
            // 0x82BD31B8, // SHARK
            // 0x82BD3200, // SHARK
            // 0x82BD3248, // SHARK
            // 0x82BD3290, // SHARK
            // 0x82BD32D8, // SHARK
            // 0x82BD3320, // WATERSND
            // 0x82BD3368, // WATERSND
            // 0x82BD33B0, // SHARK
            // 0x82BD33F8, // SHARK
            // 0x82BD3440, // SHARK
            // 0x82BD3488, // WATERSND
            // 0x82BD34D0, // WATERSND
            // 0x82BD3518, // SHARK
            // 0x82BD3560, // SHARK
            // 0x82BD35A8, // SHARK
            // 0x82BD35F0, // WATERSND
            // 0x82BD3C20, // SHARK
            // 0x82BD3C68, // WATERSND
            0x82BD3CB0, // SEAGULL2
            // 0x82BD3CF8, // WATERSND
            // 0x82BD3D40, // SHARK
            // 0x82BD3D88, // WATERSND
            // 0x82BD3DD0, // WATERSND
            // 0x82BD3E18, // WATERSND
            // 0x82BD3E60, // SHARK
            // 0x82BD3EA8, // SHARK
            // 0x82BD3EF0, // WATERSND
            0x82BD4010, // SEAGULL
            0x82BD4058, // XGOLD05
            // 0x82BD40A0, // RNDGOLD5
            0x82BD40E8, // SEAGULL
            0x82BD4930, // SEAGULL3
            0x82BD4AF8, // XFLASK
            // 0x82BD4D30, // FLASKCH
            // 0x82BD5298, // FLASKCH2
            // 0x82BD52E0, // THUGIS2
            0x82BD5328, // XGOLD05
            0x82BD5370, // XGOLD05
            0x82BD54B0, // XGOLD05
            0x82BD56F0, // XSTRENTH
            0x82BD5930, // XHEALTH
            // 0x82BD60C0, // LOCKDOOR
            0x82BD6200, // SEAGULL2
            // 0x82BD6248, // YAELIISL
            // 0x82BD6388, // WATERSND
            // 0x82BD64C8, // YAELBOAT
            // 0x82BD6608, // SHARK
            // 0x82BD6650, // WINDSND
            // 0x82BD6698, // WATERSND
            // 0x82BD66E0, // WATERSND
            // 0x82BD6728, // WATERSND
            // 0x82BD6770, // SHARK
            // 0x82BD67B8, // WATERSND
            0x82BD6800, // SEAGULL3
            0x82BD6848, // XGOLD05
            0x82BD6890, // SEAGULL2
            0x82BD6C38, // SEAGULL
            // 0x82BD6DA0, // LOCKDOOR
            0x82BD7080, // XGOLD05
            // 0x82BD73A8, // LOCKDOOR
            // 0x82BD73F0, // SHARK
            // 0x82BD7438, // SHARK
            // 0x82BD7578, // WATERSND
            // 0x82BD75C0, // GUARDTS2
            // 0x82BD7608, // WINDSND
            // 0x82BD7650, // WATERSND
            // 0x82BD7698, // WATERSND
            // 0x82BD76E0, // WATERSND
            0x82BD7D70, // SEAGULL
            // 0x82BD7F90, // SHRINE06
            // 0x82BD80D0, // YAELCAM1
            // 0x82BD8208, // LH_FIRE
            // 0x82BD8348, // BEAM
            // 0x82BD84A0, // LH_LEVER
            0x82BD84E8, // SEAGULL
            0x82BD8610, // SEAGULL2
            // 0x82BD8840, // LH_SHUT2
            // 0x82BD8B30, // LH_SHUT1
            // 0x82BD8D68, // LH_DOOR
            // 0x82BD9058, // LH_PYRE
            0x82BD93B8, // SEAGULL3
            // 0x82BD9400, // WATERSND
            // 0x82BD9540, // WINDSND
            // 0x82BD9780, // DR_ARRO4
            // 0x829AC1A0, // DR_WALK4
            // 0x829AC6B0, // WINDSND
            // 0x829AC8C8, // WATERSND
            0x829AC910, // SEAGULL
            // 0x829AC9E8, // WATERSND
            // 0x829ACA30, // SHARK
            // 0x829ACA78, // SHARK
            // 0x829ACC90, // WATERSND
            // 0x829ACEA8, // DRSTUFF
            // 0x829AD108, // GUARDTS2
            0x829AD248, // ENTDR01
            // 0x829AD388, // DR_EDRAX
            // 0x829AD5B8, // DR_EDRA2
            // 0x829AD800, // DR_EDRA1
            // 0x829ADCA0, // GUARDTS2
            // 0x829AE310, // DR_EXSIG
            // 0x829AE358, // GUARDTS3
            // 0x829AE3A0, // GUARDTS2
            // 0x829AE5F8, // WATERSND
            0x829AE988, // XGOLD05
            // 0x829AE9D0, // WATERSND
            // 0x829AEA18, // WATERSND
            // 0x829AEA60, // WINDSND
            0x829AEAA8, // XGOLD05
            0x829AEB38, // XGOLD05
            // 0x829AEC80, // DRSTUFF
            // 0x829AEED8, // WINDSND
            // 0x829AF018, // GUARDTS2
            0x829AF158, // ENTDR03
            // 0x829AF5A0, // DR_XDOO2
            // 0x829AF5E8, // WINDSND
            // 0x829AF630, // WINDSND
            // 0x829AF8B0, // WATERSND
            // 0x829AF8F8, // GUARD02
            // 0x829AFAC8, // GUARD02
            // 0x829AFB10, // WATERSND
            // 0x829AFB58, // SHARK
            // 0x829AFE28, // SHARK
            // 0x829B01D0, // GUARDTS2
            0x829B03A0, // XGOLD05
            // 0x829B03E8, // WATERSND
            // 0x829B0528, // WATERSND
            0x829B0818, // ENTDR02
            // 0x829B0A48, // SHRINE04
            // 0x829B0EB8, // ARKAY
            // 0x829B1010, // OB_EXT03
            // 0x829B1058, // WATERSND
            // 0x829B10A0, // SHARK
            // 0x829B13A0, // WINDSND
            // 0x829B13E8, // GUARDTS2
            // 0x829B1430, // GUARDTS2
            // 0x829B1AD0, // WINDSND
            // 0x829B1D00, // OB_EXT06
            // 0x829B1E40, // OB_EXT07
            0x829B1F80, // SEAGULL3
            0x829B20C0, // ENTOBSRV
            // 0x829B2108, // GEARSND
            // 0x829B2150, // WATERSND
            // 0x829B2198, // WATERSND
            0x829B21E0, // SEAGULL3
            0x829B2228, // SEAGULL2
            0x829B2270, // SEAGULL
            // 0x829B22B8, // WATERSND
            // 0x829B2300, // SHARK
            // 0x829B2348, // WATERSND
            // 0x829B2488, // WATERSND
            // 0x829B24D0, // DRBOT01
            // 0x829B2518, // WINDSND
            // 0x829B2730, // DRBOT01
            // 0x829B2778, // WATERSND
            0x829B28B8, // SEAGULL3
            // 0x829B2AF0, // OB_EXT09
            // 0x829B2D30, // OB_EXT05
            // 0x829B2E78, // OB_EXT04
            // 0x829B2EC0, // WATERSND
            // 0x829B3000, // GEARSND
            // 0x829B33E8, // OB_EXT11
            // 0x829B3608, // OB_EXT08
            // 0x829B3918, // OB_EXT00
            // 0x829B3A70, // OB_EXT12
            // 0x829B3AB8, // WATERSND
            0x829B3B00, // SEAGULL
            // 0x829B3B48, // SHARK
            // 0x829B3EC0, // SHARK
            // 0x829B3F08, // WINDSND
            // 0x82BD9E18, // WINDSND
            // 0x82BD9E60, // GUARD02
            // 0x82BD9EA8, // GUARD02
            // 0x82BD9EF0, // WATERSND
            // 0x82BD9F38, // SHARK
            // 0x82BD9F80, // WATERSND
            // 0x82BD9FC8, // WATERSND
            // 0x82BDA1C0, // WINDSND
            // 0x82BDA320, // WINDSND
            // 0x82BDA680, // GUARD02
            // 0x82BDA9E0, // GUARD02
            // 0x82BDAA28, // WATERSND
            // 0x82BDAA70, // SHARK
            0x82BDAAB8, // SEAGULL
            // 0x82BDAB00, // SHARK
            // 0x82BDAB48, // SHARK
            // 0x82BDAB90, // SHARK
            // 0x82BDABD8, // WATERSND
            // 0x82BDAD18, // GUARDTS2
            // 0x82BDB090, // DR_SLIDE
            // 0x82BDB2D0, // DR_ST
            // 0x82BDB6B8, // DR_FIT_A
            // 0x82BDBAD0, // DR_TP7
            // 0x82BDBC88, // LAVABUB1
            // 0x82BDBEE8, // DR_FIT_B
            // 0x82BDC070, // DR_SLID2
            // 0x82BDC580, // DR_PIPE6
            0x82BDC5C8, // SEAGULL3
            // 0x82BDC610, // WATERSND
            // 0x82BDC658, // WATERSND
            // 0x82BDC6A0, // WATERSND
            // 0x82BDC6E8, // SHARK
            // 0x82BDC730, // WATERSND
            // 0x82BDC778, // WATERSND
            // 0x82BDC7C0, // GUARDTS2
            // 0x82BDC808, // LAVABUB1
            // 0x82BDC990, // LAVABUB1
            // 0x82BDCAF8, // DR_PIPE5
            // 0x82BDCD10, // LAVABUB1
            // 0x82BDCF48, // DR_SPIN6
            // 0x82BDD088, // DR_SPIN5
            // 0x82BDD2C0, // DR_TP5
            // 0x82BDD500, // DR_BP5
            // 0x82BDD740, // DR_POL5
            // 0x82BDD8B0, // DR_FIT05
            // 0x82BDDA08, // LAVABUB1
            // 0x82BDDB48, // DR_TP6
            // 0x82BDDC88, // DR_BP6
            // 0x82BDDDC8, // DR_POL6
            // 0x82BDE030, // DR_FIT06
            // 0x82BDE738, // DR_PIPE4
            // 0x82BDE780, // RNDGOLD5
            0x82BDE7C8, // SEAGULL
            0x82BDE810, // SEAGULL2
            0x82BDE858, // SEAGULL3
            // 0x82BDE998, // WATERSND
            // 0x82BDEAD8, // BRENNAN
            // 0x82BDDE98, // BOATSTAR
            0x82BDEC60, // SEAGULL
            // 0x82BDECA8, // WATERSND
            // 0x82BDECF0, // WATERSND
            // 0x82BDED80, // WATERSND
            // 0x82BDEE10, // WATERSND
            // 0x82BDEE58, // WATERSND
            // 0x82BDEEA0, // SHARK
            // 0x82BDEFE0, // WATERSND
            // 0x82BDF418, // DR_PIPE3
            // 0x82BDF460, // LAVABUB1
            // 0x82BDF4A8, // LAVABUB1
            // 0x82BDF4F0, // LAVABUB1
            // 0x82BDF538, // LAVABUB1
            // 0x82BDF798, // LAVABUB1
            // 0x82BDF968, // DR_BOIL
            // 0x82BDFB98, // DR_EXMC1
            // 0x82BDFF88, // DR_EXMB1
            // 0x82BE00C8, // DR_SPIN3
            // 0x82BE0208, // DR_TP3
            // 0x82BE0348, // DR_BP3
            // 0x82BE0488, // DR_POL3
            // 0x82BE06B8, // DR_FIT03
            // 0x82BE07F8, // DR_FIT01
            // 0x82BE0840, // LAVABUB1
            // 0x82BE0888, // LAVABUB1
            // 0x82BE0A58, // LAVABUB1
            // 0x82BE0B98, // DR_EXMC2
            // 0x82BE0D20, // DR_EXMB2
            // 0x82BE0E60, // DR_SPIN4
            // 0x82BE0FA0, // DR_TP4
            // 0x82BE10E0, // DR_BP4
            // 0x82BE1220, // DR_POL4
            // 0x82BE1458, // DR_FIT04
            // 0x82BE1698, // DR_FIT02
            // 0x82BE1830, // DR_PIPE2
            // 0x82BE1970, // WINDSND
            // 0x82BE2360, // SHRINE03
            // 0x82BE1A50, // XHUNDING
            // 0x82BE2580, // WATERSND
            // 0x82BE27B0, // AL_HIDE
            // 0x82BE2AA0, // TOBYDOOR
            0x82BE2D70, // SEAGULL3
            // 0x82BE2DB8, // WATERSND
            // 0x82BE3450, // WATERSND
            0x82BE35B8, // ENTMAGE3
            0x82BE3600, // SEAGULL
            0x82BE36D8, // SEAGULL2
            // 0x82BE3720, // LOCKDOOR
            // 0x82BE39A8, // LOCKDOOR
            // 0x82BE39F0, // WATERSND
            // 0x82BE3A38, // SHARK
            // 0x82BE3BC0, // WATERSND
            // 0x82BE3DA0, // DR_PIPE1
            // 0x82BE3DE8, // LAVABUB1
            // 0x82BE3E30, // LAVABUB1
            // 0x82BE4490, // LAVABUB1
            // 0x82BE44D8, // WATERSND
            0x82BE4520, // DOLPHIN
            0x82BE4568, // DOLPHIN
            // 0x82BE4778, // WATERSND
            // 0x82BE49F8, // BOAT2
            // 0x82BE4A40, // BRENDOOR
            0x82BE4C38, // SEAGULL
            0x82BE4D78, // SEAGULL2
            // 0x82BE5238, // TOBYBOAT
            // 0x82BE5428, // DOCKSND
            // 0x82BE5628, // LOCKDOOR
            0x82BE5768, // ENTTEMP1
            // 0x82BE58A0, // NTTORCH
            // 0x82BE5AF8, // SIGNZ
            // 0x82BE6010, // FIRESND
            0x82BE6150, // ENTMAGE1
            0x82BE6198, // ENTMAGE2
            // 0x82BE6200, // NTTORCH
            // 0x82BE64D8, // FIRESND
            // 0x82BE6828, // SIGNZ
            // 0x82BE6A60, // SIGNX
            0x82BE6BA0, // ENTSMUG1
            0x82BE6C18, // ENTCART1
            // 0x82BE6D80, // SIGNZ
            0x82BE6DC8, // SEAGULL
            0x82BE6E58, // SEAGULL3
            0x82BE6EA0, // SEAGULL
            // 0x82BE6EE8, // WATERSND
            // 0x82BE6F30, // SHARK
            // 0x82BE6F78, // WATERSND
            // 0x82BE6FC0, // WATERSND
            // 0x82BE7008, // GUARDTS2
            // 0x82BE7320, // WINDSND
            // 0x82BE7368, // WATERSND
            // 0x82BE73B0, // WATERSND
            // 0x82BE7698, // WATERSND
            // 0x82BE7968, // FLAG2
            0x82BE7AA8, // SEAGULL
            // 0x82BE7D78, // GUARD01
            // 0x82BE7FD8, // DOCKSND
            0x82BE8020, // ENTHARB3
            // 0x82BE8160, // LOCKDOOR
            0x82BE82A0, // ENTHARBL
            // 0x82BE82E8, // NTLIGHT
            // 0x82BE8420, // NTLIGHT
            // 0x82BE88C0, // SIONA
            // 0x82BE8BB0, // HARBWALK
            // 0x82BE8BF8, // LOCKDOOR
            // 0x82BE8FA0, // NTLIGHT
            // 0x82BE8FE8, // GUARD01
            0x82BE9030, // SEAGULL2
            0x82BE93F8, // SEAGULL
            0x82BE9538, // ENTJFFE1
            // 0x82BE9678, // THUGTN6
            // 0x82BE96C0, // MARIAH
            // 0x82BE9880, // SIGNX
            // 0x82BE98C8, // NTLIGHT
            // 0x82BE9A40, // NTLIGHT
            0x82BE9AD0, // SEAGULL
            0x82BE9B18, // SEAGULL2
            // 0x82BE9C38, // WATERSND
            // 0x82BE9C80, // WATERSND
            // 0x82BE9CC8, // SHARK
            // 0x82BE9D10, // WATERSND
            // 0x82BE9D58, // GUARDTS2
            // 0x82BE9DA0, // GUARDTS2
            // 0x82BE9F08, // GUARDTS2
            // 0x82BEA470, // GUARD01
            // 0x82BEA760, // WATERSND
            // 0x82BEA978, // BOATCAMX
            0x82BEA9C0, // ENTWARE1
            // 0x82BEAB00, // NTLIGHT
            // 0x82BEAC40, // TRITHICK
            // 0x82BEAE88, // CLOTH01
            // 0x82BEB0C0, // CLOTH03
            // 0x82BEAFB0, // CLOTH02
            // 0x82BEBA98, // CLOTH01
            // 0x82BEBAE0, // KOTARO
            // 0x82BEBB28, // LOCKDOOR
            0x82BEBB70, // ENTWARE1
            // 0x82BEBBB8, // GUARD01
            // 0x82BEBCF8, // NTLIGHT
            // 0x82BEB320, // KOTAROTB
            // 0x82BEC278, // SIGNZ
            0x82BEC3B8, // ENTHARB2
            0x82BEC400, // ENTHARB1
            // 0x82BEC810, // NTLIGHT
            // 0x82BEBF78, // BOATCAM1
            // 0x82BECB68, // LOCKDOOR
            0x82BECCA8, // ENTCATA1
            0x82BED038, // ENTBELL2
            // 0x82BED6B0, // BELL
            // 0x82BED7F0, // TS_RP
            // 0x82BEDA38, // TS_ROPEH
            // 0x82BEDCE0, // TS_BUCK
            // 0x82BEDE20, // ROLLOEXT
            // 0x82BEE058, // TS_WHEEL
            0x82BEE0A0, // ENTROLL1
            // 0x82BEE378, // LOCKDOOR
            0x82BEE3C0, // ENTJFFE2
            // 0x82BEE500, // LOCKDOOR
            // 0x82BEE548, // LAKENE
            // 0x82BEE668, // NTLIGHT
            // 0x82BEE7D0, // SIGNZ
            // 0x82BEE938, // LOCKDOOR
            // 0x82BEE980, // WATERSND
            // 0x82BEE9C8, // WATERSND
            0x82BEEA10, // SEAGULL2
            0x82BEEAE8, // SEAGULL2
            // 0x82BEEB30, // SHARK
            // 0x82BEEC08, // WATERSND
            // 0x82BEED70, // GUARDTS2
            // 0x82BEF160, // DRSTUFF
            // 0x82BEEEA8, // CAMPDIRG
            0x82BEF410, // XGOLD05
            0x82BEF548, // XGOLD05
            0x82BEF790, // XSAP
            0x82BF02E0, // XBOTTLE
            0x82BF0540, // XECTO
            0x82BF0778, // XMILK
            0x82BF13C0, // XBLOOD
            // 0x82BF1720, // GUARD02
            // 0x82BF0888, // CLOTH02
            // 0x82BF18E0, // CLOTH03
            // 0x82BF1928, // CLOTH03
            // 0x82BF1B58, // CLOTH01
            // 0x82BF1BA0, // THUGTN4
            // 0x82BF1BE8, // LOCKDOOR
            // 0x82BF1C30, // LOCKDOOR
            // 0x82BF1FB0, // NTLIGHT
            0x82BF20F0, // ENTSTOR1
            0x82BF2230, // ENTSILV1
            0x82BF2278, // ENTKRIS1
            // 0x82BF2308, // NTLIGHT
            // 0x82BF2808, // SIGNX
            0x82BF2850, // ENTBELL1
            0x82BF2988, // SEAGULL
            // 0x82BF30F8, // SNAKE
            // 0x82BF3290, // THUGTN5
            // 0x82BF3320, // LOCKDOOR
            // 0x82BF3368, // FLAG2
            // 0x82BF3560, // GUARD01
            // 0x82BF35A8, // LOCKDOOR
            // 0x82BF35F0, // LOCKDOOR
            // 0x82BF3898, // NTLIGHT
            0x82BF39D8, // ENTTAVE1
            // 0x82BF3A20, // CRENDAL
            // 0x82BF3A68, // NTLIGHT
            // 0x82BF3D08, // SIGNZ
            // 0x82BF3D50, // LOCKDOOR
            // 0x82BF3F20, // NTTORCH
            // 0x82BF3F68, // LOCKDOOR
            // 0x82BF4118, // NTLIGHT
            // 0x82BF41A8, // LOCKDOOR
            // 0x82BF41F0, // WATERSND
            // 0x82BF4238, // SHARK
            // 0x82BF4280, // WATERSND
            // 0x82BF42C8, // WATERSND
            // 0x82BF4310, // WATERSND
            0x82BF4358, // XGOLD05
            0x82BF43E8, // XGOLD05
            0x82BF4430, // XGOLD05
            0x82BF4478, // XGOLD05
            0x82BF44C0, // XGOLD05
            // 0x82BF48F8, // GUARDTS2
            // 0x82BF3B58, // DRSTUFF
            // 0x82BF4B58, // DRSTUFF
            0x82BF4BA0, // XGOLD05
            // 0x82BF4BE8, // GUARDTS2
            // 0x82BF4D08, // FLAG2
            // 0x82BF5490, // GUARD02
            0x82BF5C48, // XPOUCH
            // 0x82BF5ED0, // FIRESND
            // 0x82BF5F18, // LOCKDOOR
            // 0x82BF64C0, // NTTORCH
            // 0x82BF5610, // SHRINE05
            // 0x82BF6788, // AVIK
            // 0x82BF6848, // NTLIGHT
            // 0x82BF6B60, // GUARD01
            // 0x82BF73A8, // LOCKDOOR
            // 0x82BF7750, // TS_WAGON
            // 0x82BF7798, // GUARDTS3
            // 0x82BF77E0, // WATERSND
            // 0x82BF7828, // WATERSND
            // 0x82BF7870, // SHARK
            // 0x82BF78B8, // WATERSND
            // 0x82BF7948, // WATERSND
            // 0x82BF7B88, // DRSTUFF
            0x82BF7FD8, // XGOLD05
            // 0x82BF8020, // HEADGOLD
            // 0x82BF8428, // GUARD01
            0x82BF8470, // ENTGERR1
            // 0x82BF84B8, // NTLIGHT
            // 0x82BF8A20, // SIGNZ
            // 0x82BF8A68, // THUGTN7
            // 0x82BF8AB0, // LOCKDOOR
            // 0x82BF8AF8, // LOCKDOOR
            // 0x82BF90F8, // NTLIGHT
            // 0x82BF9308, // WATERSND
            // 0x82BF9500, // FLAG
            // 0x82BF9548, // FLAG
            // 0x82BF9590, // FLAG
            // 0x82BF95D8, // FLAG
            // 0x82BF9668, // WATERSND
            // 0x82BF96B0, // FLAG
            // 0x82BF8640, // X
            0x82BF9A30, // ENTJAIL2
            // 0x82BF9C48, // GUARDJL1
            0x82BF9C90, // ENTJAIL
            // 0x82BF9CD8, // LOCKDOOR
            // 0x82BF9FA8, // LOCKDOOR
            0x82BFA5B0, // ENTJAIL
            0x82BFA5F8, // XMONEY
            0x82BFA958, // XMONEY
            // 0x82BFA9A0, // WATERSND
            // 0x82BFA9E8, // WATERSND
            // 0x82BFAA30, // SHARK
            // 0x82BFAA78, // SHARK
            // 0x82BFAAC0, // SHARK
            // 0x82BFAB08, // SHARK
            // 0x82BFAED8, // WATERSND
            0x82BFAF20, // XGOLD05
            0x82BFB278, // XGOLD05
            // 0x82BFB2C0, // WATERSND
            // 0x82BFB308, // WATERSND
            // 0x82BFB4D8, // WATERSND
            0x82BFB948, // ENTCAVE
            // 0x82BFBC40, // WATRFALL
            // 0x82BFBC88, // WATERSND
            // 0x82BFBDC8, // WATERSND
            0x82BFBF08, // ENTPLF
            // 0x82BFBFD8, // GUARDTN1
            // 0x82BFC250, // A
            0x82BFC370, // ENTJAIL
            0x82BFC5F8, // XMONEY
            0x82BFC6D0, // XMONEY
            // 0x82BFC870, // GUARDTS2
            // 0x82BFC8B8, // TORCH
            // 0x82BFC100, // FIRESND
            // 0x82BFC148, // GUARDTS2
            // 0x82BFCAA0, // WATERSND
            // 0x82BFCAE8, // SHARK
            // 0x82BFCB30, // SHARK
            // 0x82BFCB78, // SHARK
            // 0x82BFCBC0, // WATERSND
            // 0x82BFCF40, // WATERSND
            // 0x82BFD2E0, // WATERSND
            // 0x82BFD328, // GUARD01
            // 0x82BFD9C0, // WATERSND
            // 0x82BFDAE0, // GUARDTS2
            // 0x82BFDC00, // GUARDTS2
            0x82BFDD68, // XHEALTH
            // 0x82BFDDD0, // GUARDTS2
            // 0x82BFDE18, // TORCH
            // 0x82BFE030, // FIRESND
            // 0x82BFC9F0, // JC_ROOF
            // 0x82BFE130, // JC_ROOF
            // 0x82BFE268, // JC_ROOF
            // 0x82BFE3A0, // JC_ROOF
            // 0x82BFE4D8, // JC_ROOF
            // 0x82BFE610, // JC_ROOF
            // 0x82BFE8E0, // JC_ROOF
            // 0x82BFE748, // JC_ROOF
            // 0x82BFEAA0, // JC_ROOF
            // 0x82BFEBB0, // A
            // 0x82BFED18, // JC_ROOF
            // 0x82BFEE38, // JC_ROOF
            // 0x82BFEF70, // JC_ROOF
            // 0x82BFF258, // JC_ROOF
            // 0x82BFF0A8, // A
            // 0x82BFF0F0, // WATERSND
            // 0x82BFF7D8, // SHARK
            // 0x82BFF8F8, // WATERSND
            // 0x82BFFDD8, // WATERSND
            // 0x82C005E8, // GUARD01
            0x82C00630, // XHEALTH
            0x82C00678, // XMONEY
            0x82C006C0, // XHEALTH
            // 0x82C00708, // GUARDTS2
            // 0x82C007B8, // GUARDTS2
            // 0x82C00800, // TORCH
            // 0x82C00A18, // FIRESND
            // 0x82C00B58, // J_CANOPY
            // 0x82C00BC0, // GUARDTST
            // 0x82C00C08, // TORCH
            // 0x82C00C50, // FIRESND
            // 0x82C00C98, // WATERSND
            // 0x82C00CE0, // SHARK
            // 0x82C00D28, // SHARK
            // 0x82C01690, // WATERSND
            // 0x82C01A18, // FALLDEAD
            // 0x82C01E88, // CAVECAM
            // 0x82C025E8, // GUARD01
            // 0x82C02768, // WATERSND
            // 0x82C033C0, // WATERSND
            0x82C03408, // XMONEY
            0x82C03450, // XHEALTH
            0x82C035B8, // XHEALTH
            // 0x82C03648, // GUARDTST
            0x82C03690, // XMONEY
            // 0x82C036D8, // J_CANOPY
            // 0x82C03720, // GUARDTS3
            // 0x82C03840, // GUARDTST
            // 0x82C039F0, // LOCKDOOR
            // 0x82C03A38, // WATERSND
            // 0x82C03A80, // SHARK
            // 0x82C03C50, // WATERSND
            // 0x82C05038, // WATERSND
            // 0x82C05080, // LOCKDOOR
            // 0x82C050C8, // LOCKDOOR
            0x82C05110, // XHEALTH
            // 0x82C05158, // LOCKDOOR
            // 0x82C051A0, // GUARDTS3
            // 0x82C051E8, // GUARDTST
            // 0x82C05230, // J_CANOPY
            // 0x82C05298, // TORCH
            // 0x82C054D8, // FIRESND
            // 0x82C05640, // J_CANOPY
            0x82C05688, // XHEALTH
            // 0x82C056D0, // SHARK
            // 0x82C05760, // SHARK
            // 0x82C057A8, // WATERSND
            // 0x82C06788, // WATERSND
            0x82C067D0, // XHEALTH
            // 0x82C06818, // LOCKDOOR
            // 0x82C06938, // GUARDTST
            0x82C06AA0, // XHEALTH
            0x82C06AE8, // XMONEY
            // 0x82C06B30, // GUARDTS2
            // 0x82C06BC0, // WATERSND
            // 0x82C06C08, // WATERSND
            // 0x82C06D48, // WATERSND
            // 0x82C06E88, // THUGIS6
            // 0x82C06ED0, // THUGIS5
            // 0x82C08270, // RNDGOLD5
            // 0x82C082B8, // WATERSND
            // 0x82C083F8, // SHARK
            // 0x82C08638, // NCBOAT
            // 0x82C08818, // BOATMAN
            // 0x82C088A8, // SHARK
            0x82C088F0, // DOLPHIN
            0x82C08938, // SEAGULL
            0x82C08980, // SEAGULL2
            // 0x82C089C8, // WATERSND
            0x82C08FC8, // SEAGULL3
            // 0x82C09108, // THUGIS4
            // 0x82C09248, // CHESTGLD
            // 0x82C09290, // CHESTTOP
            // 0x82C092D8, // A
            0x82C09320, // XSTRENTH
            0x82C096A0, // XHEALTH
            0x82C0AA80, // XGOLD05
            // 0x82C0AC78, // PALCDIRG
            // 0x82C0ACC0, // WATERSND
            // 0x82C0AD08, // WATERSND
            // 0x82C0ADE0, // WATERSND
            // 0x82C0AE28, // WATERSND
            // 0x82C0BA88, // WATERSND
            // 0x82C0C188, // SHARK
            // 0x82C0D358, // SHRINE01
            // 0x82C0D860, // LOCKDOOR
            // 0x82C0D8A8, // WATERSND
            // 0x82C0DA10, // SHARK
            // 0x82C0DA58, // SHARK
            0x82C0DAA0, // XSTRENTH
            0x82C0DAE8, // XHEALTH
            0x82C0DB30, // XSTRENTH
            0x82C0DB78, // XGOLD05
            0x82C0DBC0, // XGOLD05
            0x82C0DC08, // XGOLD05
            0x82C0DC50, // XHEALTH
            0x82C0DC98, // XHEALTH
            0x82C0DD70, // XGOLD05
            // 0x82C0DDB8, // SHARK
            // 0x82C0DE90, // SHARK
            // 0x82C0EF78, // WATERSND
            // 0x82C0F258, // SHRINE02
            // 0x82C0F118, // NCROPEI1
            // 0x82C0F2E8, // NCPLANKI
            // 0x82C0F330, // NCPLANKI
            // 0x82C0F378, // NCPLANKI
            // 0x82C0F3C0, // NCPLANKI
            // 0x82C0F6F0, // NCPLANKI
            // 0x82C0F738, // NCROPEI2
            // 0x82C0F780, // NCPLANKI
            // 0x82C0F7C8, // NCPLANKI
            // 0x82C0F810, // NCPLANKI
            // 0x82C0F8E8, // NCPLANKI
            // 0x82C0F930, // NCPLANKI
            // 0x82C0F978, // NCPLANKI
            // 0x82C0F9C0, // NCPLANKI
            // 0x82C0FA08, // NCPLANKI
            // 0x82C0FB60, // NCPLANKI
            // 0x82C0FBF0, // TENTACLE
            // 0x82C0FDC0, // WATERSND
            // 0x82C0FE08, // NCPLANKG
            // 0x82C0FE50, // NCPLANKG
            // 0x82C0FFB8, // NCPLANKG
            // 0x82C10000, // NCPLANKG
            // 0x82C10048, // NCPLANKG
            // 0x82C10090, // NCPLANKG
            // 0x82C100D8, // NCPLANKG
            // 0x82C102F0, // TENTACLE
            // 0x82C10430, // NCPLNKF1
            // 0x82C10570, // NCROPEF1
            // 0x82C106B0, // NCPLANKF
            // 0x82C106F8, // NCROPEF2
            // 0x82C10740, // NCPLNKF1
            // 0x82C10788, // NCPLANKF
            // 0x82C107D0, // NCPLNKF1
            // 0x82C10818, // NCPLANKF
            // 0x82C10980, // NCPLANKF
            // 0x82C109C8, // NCPLANKF
            // 0x82C10A10, // NCPLANKF
            // 0x82C10A58, // NCPLANKF
            // 0x82C10B30, // NCPLANKF
            // 0x82C10B78, // NCPLANKF
            // 0x82C10BC0, // NCPLANKF
            // 0x82C10C08, // NCPLNKF1
            // 0x82C10C50, // NCPLNKF1
            // 0x82C10C98, // NCPLANKF
            // 0x82C10CE0, // NCPLANKF
            // 0x82C10DB8, // NCPLANKF
            // 0x82C10EF8, // WATERSND
            // 0x82C11038, // NCROPEE2
            // 0x82C11080, // NCPLANKE
            // 0x82C110C8, // NCPLANKE
            // 0x82C11110, // NCPLANKE
            // 0x82C11158, // NCPLANKE
            // 0x82C11298, // NCPLANKE
            // 0x82C112E0, // NCPLNKE1
            // 0x82C11328, // NCPLNKE1
            // 0x82C11370, // NCPLNKE1
            // 0x82C113B8, // NCPLNKE1
            // 0x82C11400, // NCPLNKE1
            // 0x82C11660, // NCPLANKE
            // 0x82C116A8, // NCROPEE1
            // 0x82C11998, // NCPLANKE
            // 0x82C11AD8, // NCPLANKD
            // 0x82C11B20, // NCPLNKD1
            // 0x82C11B68, // NCPLNKD1
            // 0x82C11CA8, // NCPLNKD1
            // 0x82C11DE8, // NCROPED2
            // 0x82C11E30, // NCROPED1
            // 0x82C11E78, // NCPLNKD1
            // 0x82C11EC0, // NCPLANKD
            // 0x82C11F08, // NCPLANKD
            // 0x82C11F50, // NCPLANKD
            // 0x82C11F98, // NCPLANKD
            // 0x82C11FE0, // NCPLANKD
            // 0x82C12028, // NCPLANKD
            // 0x82C12070, // NCPLANKD
            // 0x82C12100, // WATERSND
            // 0x82C12268, // NCPLNKD1
            // 0x82C122B0, // NCPLANKD
            // 0x82C122F8, // NCPLANKD
            // 0x82C12340, // NCPLANKD
            // 0x82C12388, // NCPLANKD
            // 0x82C12558, // NCPLANKD
            // 0x82C125A0, // NCPLANKC
            // 0x82C12750, // NCPLANKC
            // 0x82C12798, // WATERSND
            // 0x82C127E0, // TENTACLE
            // 0x82C12920, // NCPLANKC
            // 0x82C12968, // NCROPEC2
            // 0x82C129B0, // NCPLANKC
            // 0x82C129F8, // NCPLANKC
            // 0x82C12A40, // NCPLANKC
            // 0x82C12A88, // NCPLANKC
            // 0x82C12AD0, // NCPLANKC
            // 0x82C12B18, // NCPLANKC
            // 0x82C12B60, // NCPLANKC
            // 0x82C12BA8, // NCPLANKC
            // 0x82C12CE8, // NCPLANKC
            // 0x82C12D30, // NCPLNKC1
            // 0x82C12D78, // NCPLNKC1
            // 0x82C12EB8, // NCPLNKC1
            // 0x82C12F00, // NCROPEC1
            // 0x82C131A8, // NCPLANKC
            // 0x82C131F0, // NCROPEB1
            // 0x82C13330, // WATERSND
            // 0x82C0F540, // X
            // 0x82C13628, // FIRESND
            // 0x82C13670, // NCROPEA1
            // 0x82C14928, // WATERSND
            0x82C14970, // XGOLD05
            // 0x82C149B8, // GUARDTS2
            // 0x82C14A00, // GUARDTS2
            // 0x82C14A48, // WATERSND
            // 0x82C14B20, // NCPLANKI
            // 0x82C14B68, // NCPLANKI
            // 0x82C14C88, // NCPLANKI
            // 0x82C14DC8, // TENTACLE
            // 0x82C15010, // SERPDEAD
            // 0x82C151E0, // SERPENT
            // 0x82C15228, // NCPLANKH
            // 0x82C15270, // NCPLANKH
            // 0x82C152B8, // NCPLANKH
            // 0x82C15300, // NCPLANKH
            // 0x82C15348, // NCPLANKH
            // 0x82C15488, // NCPLANKH
            // 0x82C154D0, // NCPLNKH1
            // 0x82C15518, // NCPLNKH1
            // 0x82C15560, // NCPLNKH1
            // 0x82C155A8, // NCPLNKH1
            // 0x82C155F0, // NCPLANKH
            // 0x82C15730, // NCPLANKH
            // 0x82C15808, // NCROPEH2
            // 0x82C15850, // NCPLANKH
            // 0x82C15898, // NCPLANKH
            // 0x82C158E0, // NCPLANKH
            // 0x82C15928, // NCPLANKH
            // 0x82C15A90, // NCPLANKH
            // 0x82C15AD8, // NCPLANKG
            // 0x82C15B20, // NCPLANKG
            // 0x82C15B68, // NCPLANKG
            // 0x82C15CA8, // NCPLANKG
            // 0x82C15CF0, // NCROPEG2
            // 0x82C15D38, // NCPLANKG
            // 0x82C15D80, // NCPLANKG
            // 0x82C15EC0, // NCPLANKG
            // 0x82C15F08, // NCPLNKG1
            // 0x82C15F50, // NCPLNKG1
            // 0x82C16090, // NCPLANKG
            // 0x82C161B0, // NCROPEG1
            // 0x82C161F8, // SHARK
            // 0x82C16240, // WATERSND
            // 0x82C16288, // NCPLANKE
            // 0x82C162D0, // NCPLANKE
            // 0x82C16318, // NCPLNKE1
            // 0x82C16360, // NCPLNKE1
            // 0x82C16438, // NCPLNKE1
            // 0x82C165E8, // NCPLANKE
            // 0x82C16630, // TENTACLE
            // 0x82C16678, // WATERSND
            // 0x82C166C0, // SHARK
            // 0x82C16798, // SHARK
            // 0x82C167E0, // NCPLANKC
            // 0x82C169B0, // NCPLANKC
            // 0x82C169F8, // NCPLANKB
            // 0x82C16A40, // NCPLANKB
            // 0x82C16A88, // NCPLANKB
            // 0x82C16AD0, // NCPLANKB
            // 0x82C16B18, // NCPLANKB
            // 0x82C16B60, // NCPLANKB
            // 0x82C16CA0, // NCPLANKB
            // 0x82C16CE8, // NCPLNKB1
            // 0x82C16D30, // NCPLNKB1
            // 0x82C16D78, // NCPLANKB
            // 0x82C16EB8, // NCPLANKB
            // 0x82C17038, // NCROPEB2
            // 0x82C17080, // NCPLANKB
            // 0x82C170C8, // NCPLANKB
            // 0x82C17110, // NCPLANKB
            // 0x82C17158, // NCPLANKB
            // 0x82C171A0, // NCPLANKB
            // 0x82C172C8, // NCPLANKB
            // 0x82C17310, // NCPLANKA
            // 0x82C17358, // NCPLANKA
            // 0x82C173A0, // NCPLANKA
            // 0x82C173E8, // NCPLANKA
            // 0x82C17430, // NCPLANKA
            // 0x82C17478, // NCPLANKA
            // 0x82C174C0, // NCPLANKA
            // 0x82C17550, // NCPLANKA
            // 0x82C17598, // X
            // 0x82C17648, // FIRESND
            // 0x82C17690, // NCPLANKA
            // 0x82C176D8, // NCPLANKA
            // 0x82C17720, // NCPLANKA
            // 0x82C17768, // NCPLANKA
            // 0x82C177B0, // NCPLANKA
            // 0x82C177F8, // NCPLANKA
            // 0x82C17840, // NCPLANKA
            // 0x82C17888, // NCPLANKA
            // 0x82C17A10, // NCPLANKA
            // 0x82C18E58, // NCROPEA2
            // 0x82C18F98, // GUARDTS2
            0x82C19300, // KNOCKERS
            // 0x82C19348, // GUARDTS3
            // 0x82C19390, // WATERSND
            // 0x82C19560, // SHARK
            // 0x82C19638, // NCROPEH1
            // 0x82C19758, // WATERSND
            // 0x82C198C0, // SHARK
            0x82C19908, // SEAGULL
            0x82C19950, // SEAGULL2
            0x82C19998, // SEAGULL3
            // 0x82C1A040, // WATERSND
            // 0x82C1A280, // GC_ROCK1
            // 0x82C1A3A0, // GC_ROCK2
            // 0x82C1A4E0, // SABAN
            // 0x82C1A778, // GC_DIRT
            // 0x82C1A8B8, // LOCKDOOR
            // 0x82C1A9F0, // HAYLSOUL
            // 0x82C1B1A0, // COYLE
            // 0x82C1C060, // BULL
            // 0x82C1C2D8, // GUARDTS2
            // 0x82C1C320, // SHARK
            // 0x82C1C368, // SHARK
            // 0x82C1C3F8, // SHARK
            // 0x82C1C6B8, // WATERSND
            // 0x82C1C700, // THUGIS3
            // 0x82C1DEF0, // RNDGOLD5
            // 0x82C1E010, // WATERSND
            // 0x82C1E1C0, // WATERSND
            // 0x82C1E208, // WATERSND
            0x82C1E250, // SEAGULL
            0x82C1E298, // SEAGULL2
            // 0x82C1E2E0, // RNDGOLD5
            0x82C1E328, // SEAGULL3
            0x82C1E370, // SEAGULL2
            0x82C1E810, // SEAGULL
            // 0x82C1E858, // SHARK
            0x82C1E8A0, // DOLPHIN
            0x82C1E978, // SEAGULL2
            // 0x82C1E9C0, // WATERSND
            // 0x82C1EA08, // WATERSND
            // 0x82C1EA50, // WATERSND
            // 0x82C1EA98, // WATERSND
            0x82C1EAE0, // SEAGULL3
            // 0x82C1EB28, // RNDGOLD5
            0x82C1EB70, // SEAGULL2
            0x82C1EBB8, // SEAGULL
            0x82C1ED68, // SEAGULL3
            // 0x82C1EDB0, // WATERSND
            // 0x82C1EDF8, // WATERSND
            // 0x82C1EE40, // SHARK
            0x82C1EE88, // SEAGULL
            0x82C1EF18, // SEAGULL3
            0x82C1EF60, // DOLPHIN
            // 0x82C1EFA8, // WATERSND
            // 0x82C1F038, // WATERSND
            // 0x82C1F080, // WATERSND
            // 0x82C1F0C8, // WATERSND
            // 0x82C1F110, // WATERSND
            0x82C1F158, // SEAGULL2
            0x82C1F1A0, // SEAGULL
             0x82C1F1E8, // SEAGULL3
            // 0x82C1F230, // WATERSND
            0x82C1F278, // DOLPHIN
            0x82C1F2C0, // DOLPHIN
            0x82C1F308, // DOLPHIN
            // 0x82C1F350, // SHARK
            // 0x82C1F398, // SHARK
            // 0x82C1F3E0, // SHARK
            // 0x82C1F428, // SHARK
            // 0x82C1F470, // SHARK
            // 0x82C1F4B8, // SHARK
            // 0x82C1F500, // SHARK
            // 0x82C1F548, // SHARK
            // 0x82C1F590, // SHARK
            // 0x82C1F5D8, // SHARK
            // 0x82C1F620, // SHARK
            // 0x82C1F668, // SHARK
            // 0x82C1F6B0, // SHARK
            // 0x82C1F6F8, // SHARK
            // 0x82C1F740, // SHARK
            // 0x82C1F788, // SHARK
            // 0x82C1F7D0, // SHARK
            // 0x82C1F818, // SHARK
            // 0x0, // SHARK
            }
        },
        {"BRENNANS" , new List<uint>{
            // 0x83847858, // X
            // 0x838478D0, // X
            // 0x83847918, // X
            // 0x83847960, // X
            // 0x8384A870, // X
            // 0x8384AAE8, // BRENDOR2
            // 0x8384AC70, // BRENNANF
            0x8384ADA8, // EXTBREN
            // 0x8384AF28, // TORCH
            // 0x8384B120, // BOAT1
            // 0x8384B168, // GUARDTS2
            // 0x8384B878, // GUARDTS2
            // 0x8384BB28, // CHESTGLD
            // 0x0, // CHESTTOP
            }
        },
        {"ROLLOS" , new List<uint>{
            // 0x829AE738, // X
            0x829B0C40, // EXTROLLO
            // 0x829B0E28, // ROLLO
            // 0x829B10E8, // ROLLOSL
            // 0x829B1130, // TV_BOTTL
            // 0x829B1178, // TV_BOTTL
            // 0x829B11C0, // TV_BOTTL
            // 0x829B1208, // TV_BOTTL
            // 0x829B1498, // TV_BOTTL
            // 0x829B16B0, // TORCH
            // 0x829B18F0, // ROLLOCH
            // 0x829B1BD8, // CANDLE
            // 0x829B1D18, // TV_BOTTL
            // 0x829B1E58, // ROLLOTB
            // 0x829B1FC8, // ROLLOTB
            // 0x829B2010, // TV_BOTTL
            // 0x829B2058, // TV_BOTTL
            // 0x829B2198, // ROLLOTB
            // 0x0, // ROLLOTB
            }
        },
        {"TAVERN" , new List<uint>{
            // 0x82BD37B0, // X
            // 0x82BD3828, // X
            // 0x82BD3900, // X
            // 0x82BD5EA8, // X
            // 0x82BD6018, // TV_BARBK
            // 0x82BD6258, // TV_BARON
            // 0x82BD6470, // TOBIAS
            // 0x82BD6590, // TV_BOTTL
            // 0x82BD65D8, // TV_BOTTL
            // 0x82BD6930, // TV_BOTTL
            // 0x82BD6978, // DREEKIUS
            // 0x82BD6CE8, // TV_BARON
            // 0x82BD6F00, // HURT
            // 0x82BD7040, // THUGTV1
            // 0x82BD7330, // THUGTV2
            // 0x82BD7558, // FIRESND
            // 0x82BD75A0, // TORCH
            // 0x82BD75E8, // TORCH
            // 0x82BD7800, // TORCH
            // 0x82BD7A38, // TV_HEAD
            0x82BD7B78, // EXTTAV
            // 0x82BD7D00, // THUGTV3
            // 0x82BD8040, // CANDLE
            // 0x82BD8258, // TV_TABLE
            // 0x0, // TV_BOTTL
            }
        },
        {"NECRTOWR" , new List<uint>{
            // 0x838479E0, // X
            // 0x83847B48, // X
            // 0x8384E530, // X
            0x8384E778, // EXTNCTW
            // 0x8384EB28, // NCSHELF
            // 0x8384EC68, // TORCH
            // 0x8384EDB0, // CANDLE
            // 0x8384EDF8, // CANDLE
            // 0x8384EE40, // CANDLE
            // 0x8384F760, // CANDLE
            // 0x8384FAA8, // NCTABLE
            // 0x8384FBE8, // VERMAI01
            // 0x8384FC30, // NCPITE1
            // 0x8384FC78, // NCPITE1
            // 0x8384FDB8, // NCPITE1
            // 0x8384FE00, // NCPITB1
            // 0x8384FE48, // NCPITB1
            // 0x8384FF88, // NCPITB1
            // 0x838500C8, // ISZARA
            // 0x83850330, // NCEXPLA
            // 0x83850458, // NCDOORT1
            // 0x838504A0, // CANDLE
            // 0x83850578, // CANDLE
            // 0x838505C0, // NCEXPLA
            // 0x83850B30, // NCEXPLA
            0x83850D68, // XECTO
            0x83851030, // XBLOOD
            // 0x838510D8, // NCDOORTX
            // 0x83851218, // VERMAI01
            // 0x83851358, // NC_VILED
            // 0x83851498, // NGASDEAD
            0x83851680, // XSPREAR
            0x838518C0, // XSTHAIR
            0x83851AF8, // XAMBER
            0x83851D18, // XSALLY
            0x83851F48, // XDHEART
            0x83852178, // XHORN
            0x838523B0, // XPIGSAC
            // 0x838523F8, // NC_BOUNC
            // 0x83852440, // NCPITE1
            // 0x83852488, // NCPITE1
            // 0x838524D0, // NCPITE1
            // 0x83852518, // NCPITB1
            // 0x83852560, // NCPITB1
            // 0x838526A0, // NCPITB1
            // 0x83852880, // SCENE6
            // 0x838529A0, // NGASTATW
            // 0x83852AD8, // NCPITE1
            // 0x83852D58, // NCPIT
            // 0x838531C0, // CANDLE
            // 0x838533B8, // CANDLE
            // 0x838535F8, // NCSHELF
            // 0x83853640, // NCEXPLB
            // 0x838536F0, // NCEXPLB
            // 0x838537A0, // NCEXPLB
            // 0x838537E8, // NCEXPLB
            // 0x83853A58, // NCEXPLA
            // 0x83853AA0, // NCEXPLA
            // 0x83853B68, // NCEXPLA
            // 0x83853C90, // NCEXPLA
            // 0x83853EC0, // NCCALD01
            0x84352138, // XLICH
            0x84352370, // XMILK
            0x843525A8, // XSAP
            0x84352A98, // XNECBOOK
            0x84352BF0, // XHEALTH
            0x84352D48, // XHEALTH
            // 0x84352F00, // NCELIX01
            // 0x0, // CANDLE
            }
        },
        {"SILVER2" , new List<uint>{
            // 0x83849D30, // X
            // 0x83849F48, // GUARDSV1
            0x8384A560, // EXTSILV1
            // 0x8384AC18, // TORCH
            // 0x8384AE38, // MG_CHEST
            // 0x8384AF78, // GUARDSV1
            0x8384B0B8, // EXTSILV2
            // 0x8384B8E8, // SILVERSS
            // 0x8384BB48, // MG_CHEST
            // 0x8384BE50, // SILVERC1
            // 0x0, // SILVERC2
            }
        },
        {"SMDEN" , new List<uint>{
            // 0x835E4970, // X
            0x835E6EE0, // EXTSMDEN
            // 0x835E71F8, // CANDLE
            // 0x835E7240, // TV_BOTTL
            // 0x835E7288, // TV_BOTTL
            // 0x835E75F8, // TV_BOTTL
            // 0x835E7A30, // URIKSTUF
            // 0x835E7C58, // URIKTB
            // 0x835E7CD0, // URIK
            // 0x835E8040, // URIKSTUF
            // 0x0, // URIKTB
            }
        },
        {"SILVER1" , new List<uint>{
            // 0x82BD37B0, // X
            // 0x82BD5CF0, // X
            // 0x82BD61B0, // TORCH
            // 0x82BD6720, // KRISANTB
            // 0x82BD6860, // KRISANTB
            0x82BD6920, // EXTSILVS
            // 0x82BD6AC8, // TORCH
            0x82BD6D08, // KRISAND
            // 0x82BD72D0, // KRISANTB
            // 0x82BD7318, // KRISANTB
            // 0x0, // KRISANTB
            }
        },
        {"VILE" , new List<uint>{
            // 0x8384DEA0, // VILECHAT
            // 0x8384DFE0, // VILECHAT
            // 0x8384E100, // WATERSND
            // 0x8384E240, // WATERSND
            // 0x8384E480, // VR_SIGL2
            // 0x8384E5C0, // VR_GARD2
            // 0x8384E7D8, // VR_DOOR
            // 0x8384E918, // LOCKDOOR
            // 0x8384EA58, // VR_SIGL1
            // 0x8384EBE0, // VR_GARD1
            // 0x8384EC28, // VILEDOG
            // 0x8384ED68, // VR_DOOR
            // 0x8384EEA0, // VILEKID
            // 0x8384F280, // A
            // 0x8384F3C0, // VILECHA1
            // 0x8384F408, // VILECHA3
            // 0x8384F590, // WATERSND
            // 0x0, // VILECHA2
            }
        },
        {"MGUILD" , new List<uint>{
            // 0x82BD38D0, // X
            // 0x82BD39D8, // X
            // 0x82BD9EB0, // X
            // 0x82BDA0C8, // MG_BARBK
            // 0x82BDA3D8, // CANDLE
            0x82BDA618, // XLICH
            // 0x82BDABA8, // MGBOOKS
            // 0x82BDACE0, // FALICIA
            0x82BDAFE8, // XBALL
            0x82BDB370, // XHORN
            // 0x82BDB4B0, // JAGANVIR
            // 0x82BDB6E0, // CAMTARG1
            // 0x82BDB728, // TORCH
            // 0x82BDB960, // MGBOOKS
            // 0x82BDBBA0, // MG_BOOK
            0x82BDBDE0, // XPIGSAC
            0x82BDC018, // XSALLY
            0x82BDC3E8, // XAMBER
            0x82BDC948, // XBLOODM
            // 0x82BDCC80, // MGBOOKS
            // 0x82BDCEB0, // MG_MIRR
            // 0x82BDCFF0, // SCENE4
            // 0x82BDD130, // MG_DOOR
            // 0x82BDDB60, // MG_DOR1A
            // 0x82BDE020, // MG_CHEST
            // 0x82BDE0C8, // MGJAGDR
            // 0x82BDE158, // TORCH
            // 0x82BDE1A0, // CANDLE
            // 0x82BDE400, // CANDLE
            // 0x82BDEA78, // MG_PEND1
            // 0x82BDED90, // MGUILDTB
            // 0x82BDF0A0, // MG_DOR1
            // 0x0, // MG_DOR2
            }
        },
        {"HARBTOWR" , new List<uint>{
            // 0x82BD3750, // X
            // 0x82BD37F8, // X
            // 0x82BD3840, // X
            // 0x82BD38B8, // X
            // 0x82BD39C0, // X
            // 0x82BD5ED0, // X
            // 0x82BD60E0, // TORCH
            0x82BD62C8, // XFLAG
            0x82BD6408, // EXTHTW3
            // 0x82BD67D0, // PRNELL
            // 0x82BD6A90, // CHESTGLD
            // 0x82BD6CC0, // CHESTTOP
            0x82BD6E00, // EXTHTW2
            0x82BD6F08, // EXTHTW1
            // 0x82BD7678, // TORCH
            // 0x0, // TORCH
            }
        },
        {"CATACOMB" , new List<uint>{
            // 0x83849570, // X
            // 0x83852710, // X
            // 0x838530F0, // DRAM
            // 0x83853CF8, // RICHTON
            // 0x843526D0, // SCENE2
            // 0x84353828, // GUARDSC2
            // 0x84353E38, // DRAM_SC9
            // 0x84353F78, // SCENE9
            // 0x843540B8, // GUARDSC9
            // 0x843546C8, // RICHTON9
            0x84354808, // EXTCATA1
            // 0x84354A00, // GUARDTS3
            // 0x84354D20, // TORCH
            // 0x84354E80, // TORCH
            // 0x84354FC8, // FIRESND
            // 0x84355010, // TORCH
            // 0x84355260, // FIRESND
            // 0x84355688, // CTLEVER2
            // 0x84355990, // RAT
            // 0x84355A88, // TORCH
            // 0x84355DE0, // TORCH
            // 0x84356000, // CTDOORDL
            // 0x84356140, // CTDOORDR
            // 0x84356280, // RAT
            // 0x843562C8, // GUARDTS2
            // 0x84356408, // TORCH
            // 0x843568D0, // TORCH
            // 0x84356A10, // TORCH
            // 0x84356C70, // TORCH
            // 0x84356DC0, // GUARDTS3
            // 0x84356EA0, // GUARDTS3
            // 0x84356FE0, // TORCH
            // 0x843572F0, // CTBLOCK1
            0x843574D8, // XMONEY
            0x84357718, // XSTRENTH
            0x84357858, // XHEALTH
            0x84357AB0, // XHEALTH
            // 0x84357AF8, // GUARDTS3
            // 0x84357B40, // GUARDTS3
            // 0x84357B88, // RAT
            // 0x84357BF0, // RAT
            // 0x84357D30, // FIRESND
            // 0x84357EA0, // TORCH
            // 0x84357EE8, // TORCH
            // 0x84358090, // FIRESND
            // 0x84358160, // TORCH
            // 0x84358498, // FIRESND
            // 0x843588F0, // CT_HEAD
            // 0x84358998, // GUARDTS2
            // 0x84358C18, // TORCH
            // 0x84358C60, // RAT
            // 0x84358CA8, // RAT
            // 0x84358CF0, // RAT
            // 0x84358D88, // RAT
            // 0x84358DD0, // FIRESND
            // 0x84358EC8, // TORCH
            // 0x84359170, // TORCH
            // 0x843591B8, // GUARDTS2
            // 0x84359200, // RAT
            // 0x84359340, // RAT
            // 0x84359478, // CTLOCK01
            // 0x84359718, // TORCH
            // 0x84359858, // CTDOOR5
            // 0x84359950, // TORCH
            // 0x84359BD0, // TORCH
            // 0x8435A1A0, // TORCH
            // 0x8435A2E0, // CTBLADE
            // 0x8435A358, // CTBLADE
            // 0x8435A3F8, // GUARDTS3
            // 0x8435A520, // TORCH
            // 0x8435A758, // TORCH
            // 0x8435A810, // FIRESND
            // 0x8435AA28, // TORCH
            // 0x8435AA70, // CTLEVER5
            // 0x8435AC60, // TORCH
            // 0x8435ACA8, // LAVABUB1
            // 0x8435ACF0, // LAVABUB1
            // 0x8435AD38, // LAVABUB1
            // 0x8435AF08, // LAVABUB1
            // 0x8435AF50, // TORCH
            // 0x8435B000, // FIRESND
            // 0x8435B318, // CT_HEAD
            // 0x8435B380, // FIRESND
            // 0x8435B3F8, // TORCH
            // 0x8435B460, // GUARDTS2
            // 0x8435B4A8, // FIRESND
            // 0x8435B4F0, // TORCH
            // 0x8435B558, // TORCH
            // 0x8435B740, // FIRESND
            0x8435B830, // XTORCHP
            // 0x8435B8E0, // TORCH
            // 0x8435B928, // GUARDTS3
            // 0x8435BCA0, // GUARDTS3
            // 0x8435BCE8, // GUARDTS3
            // 0x8435BE90, // TORCH
            // 0x8435BFD0, // GUARDTS3
            // 0x8435C3E8, // CTDOOR58
            // 0x8435C580, // CTBLADEZ
            // 0x8435C8B8, // TORCH
            // 0x8435CB88, // LAVABUB1
            // 0x8435CBD0, // GUARDTS3
            // 0x8435CC18, // GUARDTS3
            // 0x8435CE10, // LAVABUB1
            // 0x8435CE58, // GUARDTS3
            // 0x8435CF98, // GUARDTS3
            // 0x8435D0C0, // CTBLADE1
            // 0x8435D108, // CTBLADE1
            // 0x8435D368, // CTBLADE1
            // 0x8435D528, // CTRUNE15
            // 0x8435D570, // TORCH
            // 0x8435D648, // TORCH
            // 0x8435D6F8, // TORCH
            // 0x8435D7A8, // FIRESND
            // 0x8435D7F0, // TORCH
            // 0x8435D9A8, // FIRESND
            // 0x8435DA10, // GUARDTST
            // 0x8435DA88, // FIRESND
            // 0x8435DC00, // TORCH
            // 0x8435DC48, // TORCH
            // 0x8435DCB0, // FIRESND
            // 0x8435DD18, // TORCH
            // 0x8435DE38, // FIRESND
            // 0x8435DEA0, // RAT
            // 0x8435DEE8, // FIRESND
            // 0x8435DF30, // TORCH
            0x8435DFE0, // XTORCHP
            // 0x8435E120, // TORCH
            // 0x8435E300, // CTVRUN58
            // 0x8435E460, // CT_FIRE
            // 0x8435E578, // CT_SECLT
            // 0x8435E8E8, // TORCH
            // 0x8435EC00, // CT_SECRT
            // 0x8435ED90, // CTRUNE58
            // 0x8435EEB8, // TORCH
            // 0x8435EF20, // TORCH
            // 0x8435EF88, // FIRESND
            // 0x8435F1B0, // FIRESND
            // 0x8435F1F8, // GUARDTS3
            // 0x8435F358, // GUARDTS3
            // 0x8435F6C0, // TORCH
            // 0x8435F988, // TORCH
            // 0x8435FAC8, // LAVABUB1
            // 0x8435FC18, // CTLOCK03
            0x8435FC60, // XHEALTH
            // 0x8435FCA8, // LAVABUB1
            // 0x8435FDE8, // LAVABUB1
            // 0x8435FE30, // CTSKDOOR
            // 0x8435FEB8, // TORCH
            // 0x8435FF00, // FIRESND
            // 0x8435FF48, // FIRESND
            // 0x8435FF90, // TORCH
            // 0x84360018, // TORCH
            // 0x84360258, // FIRESND
            // 0x843602C0, // FIRESND
            // 0x84360338, // TORCH
            // 0x84360460, // GUARDTST
            // 0x84360558, // TORCH
            // 0x843605C0, // TORCH
            // 0x84360698, // FIRESND
            // 0x84360700, // FIRESND
            // 0x84360748, // TORCH
            // 0x843607B0, // FIRESND
            // 0x843607F8, // TORCH
            // 0x84360840, // RAT
            // 0x84360888, // GUARDTS3
            // 0x843608D0, // TORCH
            // 0x84360C68, // TORCH
            // 0x84360CB0, // GUARDTS3
            // 0x843610F8, // TORCH
            0x843612C0, // XWEIGHT1
            0x84361400, // XWEIGHT3
            0x84361540, // XWEIGHT2
            // 0x84361680, // CT_WEIT1
            // 0x843616C8, // CT_WEIT2
            // 0x843619A0, // CT_WEIT2
            // 0x843619E8, // CTBLADE
            // 0x84361A30, // FIRESND
            // 0x84361B50, // GUARDTS3
            // 0x84361C90, // TORCH
            // 0x84361FC8, // CTRUNE16
            // 0x84362030, // FIRESND
            // 0x843623A0, // FIRESND
            // 0x843623E8, // LAVABUB1
            // 0x84362690, // LAVABUB1
            // 0x843626D8, // LAVABUB1
            // 0x84362720, // LAVABUB1
            // 0x84362768, // LAVABUB1
            // 0x843627B0, // LAVABUB1
            // 0x84362970, // LAVABUB1
            // 0x84362A20, // RAT
            // 0x84362A68, // FIRESND
            // 0x84362C58, // TORCH
            // 0x84362D98, // TORCH
            // 0x84362FC8, // CTDOORRP
            // 0x84363340, // CTPADRP
            // 0x84363388, // FIRESND
            // 0x84363598, // TORCH
            // 0x843637B0, // A
            // 0x843638F0, // CTDOORBR
            // 0x84363A10, // CTDOORBL
            // 0x84363A58, // RAT
            // 0x84363BE0, // RAT
            // 0x84363D20, // CTDOORAL
            // 0x84363E60, // CTDOORAR
            // 0x84363FA0, // FIRESND
            // 0x84364018, // TORCH
            // 0x84364230, // A
            // 0x843642A8, // FIRESND
            // 0x84364310, // TORCH
            // 0x843643C0, // FIRESND
            // 0x84364500, // TORCH
            // 0x84364640, // CTRNDOR2
            // 0x84364718, // CTRNDOR1
            // 0x84364760, // TORCH
            // 0x843647A8, // TORCH
            // 0x84364830, // FIRESND
            // 0x84364AF0, // FIRESND
            // 0x84364D30, // CTPADA2
            // 0x84364FC8, // CTBLOCK2
            // 0x84365108, // CTPADA2
            // 0x84365340, // CTWBLADE
            // 0x84365560, // CTPADW1
            // 0x84365798, // CTPADW2
            // 0x843658D8, // CTPADW3
            // 0x84365A18, // CTPADW4
            // 0x84365B58, // CTPADW5
            // 0x84365C98, // CTPADW6
            // 0x84365E30, // CTPADW7
            // 0x84365E78, // CTBLADEZ
            // 0x84366070, // CTBLADEZ
            // 0x843660D8, // RAT
            // 0x843662F0, // TORCH
            // 0x84366430, // CTDOORCR
            // 0x84366680, // CTDOORCL
            // 0x84366808, // RAT
            // 0x84366A40, // CTPAD04
            // 0x84366C80, // CTPAD03
            // 0x84366EC0, // CTPAD02
            // 0x84367060, // CTPAD01
            // 0x84367168, // GUARDTST
            // 0x84367368, // TORCH
            // 0x843673B0, // RAT
            // 0x84367610, // RAT
            // 0x84367750, // CTDIALR3
            // 0x84367890, // CTDIALR2
            // 0x843679D0, // CTDIALR1
            // 0x84367D10, // CTDIAL01
            // 0x84367D58, // CTLEVER1
            // 0x84368020, // TORCH
            // 0x843680B0, // CTLEVR86
            // 0x843681C0, // TORCH
            // 0x84368440, // FIRESND
            0x84368620, // XSKEY
            // 0x84368860, // CTWTDOOR
            // 0x84368B98, // CTPADW8
            // 0x84368D90, // CTRUNE17
            // 0x84368ED0, // GUARDTS2
            0x84369110, // XIRONSK
            0x84369158, // XGOLD05
            0x843691A0, // XGOLD05
            0x843691E8, // XHEALTH
            0x84369328, // XHEALTH
            // 0x84369540, // CHESTGLD
            // 0x843696B0, // CHESTTOP
            // 0x843696F8, // TORCH
            // 0x84369740, // FIRESND
            // 0x84369788, // TORCH
            // 0x843697F0, // FIRESND
            // 0x84369838, // TORCH
            // 0x84369958, // FIRESND
            // 0x843699C0, // GUARDTS2
            // 0x84369A08, // RAT
            // 0x84369A50, // FIRESND
            // 0x84369A98, // TORCH
            // 0x84369BF0, // A
            // 0x84369C38, // FIRESND
            // 0x84369CE8, // TORCH
            // 0x84369D78, // TORCH
            // 0x84369DC0, // GUARDTS2
            // 0x84369E50, // A
            // 0x84369EE0, // A
            0x84369F80, // XTORCHP
            // 0x84369FC8, // RAT
            // 0x8436A0F0, // A
            // 0x843695D8, // FIRESND
            // 0x8436A2A8, // TORCH
            // 0x8436A2F0, // TORCH
            // 0x8436A418, // TORCH
            // 0x8436A860, // CTROT86
            // 0x8436AA90, // CTRSLV04
            // 0x8436AAD8, // STONESND
            // 0x8436ADA8, // CTRSLV04
            // 0x8436AE10, // GUARDTS3
            // 0x8436AEE0, // TORCH
            // 0x8436B0F0, // FIRESND
            // 0x8436B1C8, // TORCH
            // 0x8436B210, // FIRESND
            0x8436B258, // XTORCHP
            0x8436B428, // XHEALTH
            // 0x8436B470, // TORCH
            // 0x8436B4D8, // GUARDTST
            // 0x8436A938, // FIRESND
            // 0x8436B7B8, // TORCH
            // 0x8436B9B8, // CTDOOR04
            0x8436BA00, // XTORCHP
            // 0x8436BA48, // RAT
            // 0x8436BA90, // A
            // 0x8436BAD8, // A
            // 0x8436BBD0, // TORCH
            // 0x8436BDC0, // TORCH
            // 0x8436BFF8, // CTDOOR03
            // 0x8436C118, // CTPADA1
            0x8436C160, // XTORCHP
            0x8436C1A8, // XHEALTH
            // 0x8436C1F0, // GUARDTST
            // 0x8436C3A8, // TORCH
            // 0x8436C410, // TORCH
            // 0x8436BE68, // FIRESND
            // 0x8436BEB0, // TORCH
            0x8436C550, // XMONEY
            0x8436C598, // XHEALTH
            0x8436C5E0, // XHEALTH
            0x8436C628, // XHEALTH
            // 0x8436C670, // A
            // 0x8436C6B8, // A
            // 0x8436CAD8, // A
            // 0x8436CB20, // CTXRUN58
            // 0x8436CC38, // TORCH
            // 0x8436CDC0, // FIRESND
            // 0x8436D038, // CTDOOR86
            // 0x8436D1C0, // CTLEVR73
            // 0x8436D300, // CTRSLV03
            // 0x8436D348, // CTRSLV02
            // 0x8436D488, // CTRSLV04
            // 0x8436D4D0, // CTRSLV01
            // 0x8436D518, // CTRSLV03
            // 0x8436D560, // CTRSLV02
            // 0x8436D5A8, // STONESND
            // 0x8436D5F0, // STONESND
            // 0x8436D730, // CTRSLV01
            // 0x8436D778, // CTLEDGF2
            // 0x8436D8B8, // CTRSLV04
            // 0x8436DBD0, // TORCH
            // 0x8436DFB0, // TORCH
            // 0x8436E098, // TORCH
            // 0x8436E1B8, // FIRESND
            // 0x8436E3E0, // FIRESND
            0x8436E428, // XSTRENTH
            0x8436E470, // XIRONSK
            0x8436E4B8, // XMONEY
            0x8436E500, // XMONEY
            // 0x8436E548, // TORCH
            // 0x8436E590, // TORCH
            // 0x8436E730, // TORCH
            // 0x8436E8E0, // FIRESND
            // 0x8436E990, // FIRESND
            // 0x8436EAB0, // TORCH
            // 0x8436EBA8, // GUARDTST
            // 0x8436EC78, // TORCH
            // 0x8436ECC0, // TORCH
            // 0x8436ED08, // A
            // 0x8436EE28, // CTDOOR03
            // 0x8436EE70, // RAT
            // 0x8436EF50, // RAT
            // 0x8436EF98, // TORCH
            // 0x8436F0D8, // RAT
            // 0x8436F218, // GUARDCT1
            // 0x8436F260, // PRISONER
            // 0x8436F2C8, // A
            // 0x8436F310, // FIRESND
            // 0x8436F358, // FIRESND
            // 0x8436F3A0, // TORCH
            // 0x8436F450, // TORCH
            // 0x8436F4B8, // TORCH
            // 0x8436F5B0, // FIRESND
            // 0x835E2130, // RAT
            // 0x835E2348, // CT_BLOCK
            // 0x835E2390, // A
            // 0x835E2420, // TORCH
            // 0x835E2488, // FIRESND
            // 0x835E25F0, // TORCH
            // 0x835E2658, // TORCH
            // 0x835E2860, // FIRESND
            // 0x835E28A8, // RAT
            // 0x835E2920, // GUARDTST
            // 0x835E2AA8, // TORCH
            // 0x835E2BF8, // CTRAKDOR
            // 0x835E2C60, // TORCH
            // 0x835E2DB0, // FIRESND
            // 0x835E2E40, // RAT
            // 0x835E2E88, // CTRSLV04
            // 0x835E2ED0, // CTRSLV02
            // 0x835E2F18, // STONESND
            // 0x835E3208, // STONESND
            // 0x835E3440, // CTRING4
            // 0x835E3678, // CTRING3
            // 0x835E38B8, // CTRING2
            // 0x835E3A18, // CTRING1
            // 0x835E3C18, // CTRING2
            // 0x835E3C60, // CTRSLV03
            // 0x835E3CA8, // CTRSLV02
            // 0x835E3CF0, // CTRSLV04
            // 0x835E3D38, // STONESND
            // 0x835E3D80, // STONESND
            // 0x835E3DC8, // CTRSLV01
            // 0x835E3E78, // TORCH
            // 0x835E3A98, // FIRESND
            // 0x835E3AE0, // RAT
            0x835E3FD0, // XHEALTH
            0x835E4018, // XHEALTH
            0x835E4618, // XHEALTH
            // 0x835E4680, // TORCH
            // 0x835E4778, // FIRESND
            // 0x835E4850, // FIRESND
            // 0x835E4A20, // TORCH
            // 0x835E4AB8, // GUARDTST
            // 0x835E4B48, // FIRESND
            // 0x835E4BF8, // TORCH
            // 0x835E4D18, // FIRESND
            // 0x835E4D60, // CTRSLV02
            // 0x835E4DA8, // CTRSLV04
            // 0x835E4DF0, // CTRSLV03
            // 0x835E4E38, // CTRSLV04
            // 0x835E4E80, // CTRSLV01
            // 0x835E4EC8, // CTRSLV04
            // 0x835E5008, // CTRSLV02
            // 0x835E5178, // CTDOOR73
            // 0x835E51C0, // CTRSLV04
            // 0x835E5208, // CTRSLV04
            0x835E5250, // XSTRENTH
            0x835E5298, // XTORCHP
            // 0x835E53A8, // RAT
            // 0x835E5A80, // CT_STUFF
            // 0x835E5B10, // FIRESND
            // 0x835E5C50, // TORCH
            // 0x835E5D28, // TORCH
            // 0x835E5FD0, // TORCH
            // 0x835E62E0, // CT_GOLD
            // 0x835E6520, // SULGEM_G
            // 0x835E6660, // DRAGON
            // 0x835E6898, // SCENE8
            // 0x835E6AD0, // CC80DR4
            // 0x835E69C0, // CC80DR3
            // 0x835E6D80, // CT_GOLD
            // 0x835E7040, // TORCH
            // 0x835E6D30, // CC_BRAZE
            // 0x835E7178, // CT_GOLD
            // 0x835E7410, // TORCH
            // 0x835E7508, // TORCH
            // 0x835E75E0, // TORCH
            // 0x835E7810, // CT_GOLD
            // 0x835E7A20, // DEADNAFR
            // 0x835E7A88, // CT_GOLD
            // 0x835E7B80, // TORCH
            // 0x835E7C58, // TORCH
            // 0x835E7CC0, // CT_GOLD
            // 0x835E7E90, // TORCH
            // 0x835E80D8, // CC80DR1
            // 0x835E84B0, // CC80DR2
            0x0, // EXTCATA2
            }
        },
        {"CAVERNS" , new List<uint>{
            // 0x835E5118, // X
            // 0x835E5850, // X
            // 0x835E5898, // X
            // 0x835E7D68, // X
            // 0x835E7EA8, // CV_EXPL
            // 0x835E8418, // TROLL01
            // 0x835E8558, // CV_EXPL
            // 0x835E85A0, // CV_MUSH2
            // 0x835E85E8, // CV_EXPL
            // 0x835E8630, // CV_EXPL
            // 0x835E8678, // CV_EXPL
            // 0x835E86C0, // CV_EXPL
            // 0x835E8800, // CV_EXPL
            0x835E8948, // XHEALTH
            0x835E8C38, // XHEALTH
            0x835E8E20, // XTORCHP
            // 0x835E9048, // CV_FFLY1
            // 0x835E91A8, // TORCH
            // 0x835E9500, // FIRESND
            // 0x835E9720, // CV_KEY
            // 0x835E9768, // WATERSND
            // 0x835E97B0, // CV_MUSH2
            // 0x835E9968, // CV_EXPL
            // 0x835E9BA0, // CV_RIVR3
            // 0x835E9E20, // CV_RIVR2
            // 0x835E9E68, // GOBLIN03
            // 0x835E9ED0, // TORCH
            // 0x838492C0, // FIRESND
            // 0x83849408, // CV_DOR06
            // 0x83849640, // CV_RIVR2
            0x83849728, // XMONEY
            0x838497A0, // XHEALTH
            // 0x83849808, // TORCH
            // 0x83849958, // FIRESND
            // 0x83849D68, // WATERSND
            // 0x83849FA0, // CV_TILT1
            // 0x8384A2D8, // CV_TILT
            // 0x8384A410, // CV_FISH
            // 0x8384A6A0, // CV_FFLY1
            // 0x8384AA08, // CV_FFLY2
            // 0x8384ABC0, // WATERSND
            0x8384AD00, // XIRONSK
            0x8384AD48, // XMONEY
            0x8384AD90, // XHEALTH
            0x8384ADD8, // XHEALTH
            // 0x8384AE20, // FIRESND
            // 0x8384AE68, // FIRESND
            // 0x8384AF68, // TORCH
            // 0x8384AFB0, // TORCH
            // 0x8384B6A0, // TORCH
            // 0x8384B6E8, // WATERSND
            // 0x8384B790, // CV_FFLY1
            // 0x8384BBA8, // CV_TILT
            // 0x8384BBF0, // CV_FFLY2
            // 0x8384BC38, // CV_FFLY1
            // 0x8384C2F8, // CV_FFLY1
            // 0x8384C340, // CV_EXPL
            // 0x8384C388, // CV_EXPL
            // 0x8384C3D0, // CV_EXPL
            // 0x8384C460, // CV_EXPL
            // 0x8384C4A8, // TORCH
            // 0x8384C510, // FIRESND
            // 0x8384C558, // WATERSND
            // 0x8384C5A0, // CV_FFLY1
            // 0x8384C5E8, // CV_FFLY1
            // 0x8384C6C0, // CV_MUSH2
            // 0x8384C708, // CV_EXPL
            // 0x8384C750, // CV_EXPL
            // 0x8384C798, // CV_EXPL
            // 0x8384C7E0, // CV_EXPL
            // 0x8384C968, // CV_EXPL
            0x8384CAB0, // XSTRENTH
            // 0x8384CAF8, // TROLL01
            // 0x8384CC38, // CV_RIVR2
            // 0x8384CC80, // CV_EXPL
            // 0x8384CCC8, // CV_EXPL
            // 0x8384CD10, // CV_EXPL
            // 0x8384CD58, // CV_EXPL
            // 0x8384CDA0, // CV_EXPL
            // 0x8384D010, // CV_EXPL
            0x8384D198, // XMONEY
            // 0x8384D210, // CV_MUSH
            // 0x8384D258, // GOBLIN03
            // 0x8384D488, // GOBLIN03
            // 0x8384D6A8, // CV_SKUL2
            // 0x8384D720, // CV_SKUL1
            // 0x8384D788, // TORCH
            // 0x8384D980, // FIRESND
            // 0x8384DCB8, // CV_FFLY1
            // 0x8384DEF0, // CV_CAGE3
            // 0x8384E128, // CV_TELE3
            // 0x8384E368, // CV_CAGE5
            // 0x8384E438, // CV_CAGEC
            // 0x8384E560, // CV_CAGEC
            // 0x8384E6A0, // CV_CAGE4
            // 0x8384E9A8, // CV_TILT
            // 0x8384E9F0, // CV_FFLY1
            // 0x8384EA80, // CV_FFLY1
            // 0x8384EB18, // FIRESND
            // 0x8384ECB0, // TORCH
            // 0x8384EE20, // OGRE
            // 0x8384F058, // CV_PILB5
            // 0x8384F198, // CV_PILB6
            // 0x8384F2D8, // CV_PILB4
            // 0x8384F418, // CV_PILA6
            // 0x8384F558, // CV_PILA5
            // 0x8384F878, // CV_PILA4
            // 0x8384F8C0, // TORCH
            // 0x8384F908, // TORCH
            // 0x8384F970, // TORCH
            // 0x8384FBA0, // FIRESND
            // 0x8384FCE0, // CV_PILB3
            // 0x8384FF18, // CV_PILB1
            // 0x83850228, // CV_PILB2
            // 0x83850460, // CV_B24
            // 0x838506A0, // CV_A1
            // 0x838509B0, // CV_B21
            // 0x83850AF0, // CV_PILA1
            // 0x83850C30, // CV_PILA2
            // 0x83850D70, // CV_PILA3
            // 0x83850F90, // CV_A24
            // 0x838511C8, // CV_A23
            // 0x83851400, // CV_A21
            // 0x83851640, // CV_A22
            // 0x83851780, // CV_PILC4
            // 0x838518C0, // CV_PILC5
            // 0x83851BE0, // CV_PILC6
            // 0x83851C28, // TORCH
            // 0x83851C90, // TORCH
            // 0x83851DD0, // FIRESND
            // 0x83852010, // CV_C22
            // 0x83852150, // CV_PILC3
            // 0x83852290, // CV_PILC2
            // 0x838523D0, // CV_PILC1
            // 0x83852608, // CV_B23
            // 0x83852828, // CV_B22
            // 0x83852A60, // CV_C21
            // 0x83852C98, // CV_C23
            // 0x83852E10, // CV_C24
            // 0x83852FF0, // TROLL01
            // 0x83853168, // CV_RIVR1
            // 0x838531B0, // CV_EXPL
            // 0x838531F8, // CV_EXPL
            // 0x83853240, // CV_EXPL
            // 0x83853288, // CV_EXPL
            // 0x83853370, // CV_EXPL
            0x838533B8, // XTORCHP
            // 0x838535B8, // TROLL01
            // 0x83853600, // CV_TRAP
            // 0x838537B0, // CV_RIVR3
            // 0x838537F8, // CV_MUSH2
            // 0x838536E0, // CV_RIVR2
            // 0x83853A90, // TROLL01
            // 0x838538C8, // CV_WHEEL
            // 0x83853C38, // TORCH
            // 0x83853DC0, // FIRESND
            // 0x83853E28, // TORCH
            // 0x83853FB0, // FIRESND
            // 0x84352038, // CV_CAGEW
            // 0x84352178, // TORCH
            // 0x84352308, // CV_VOA
            // 0x84352410, // CV_TILT
            // 0x843524B8, // TORCH
            0x84352520, // XMONEY
            // 0x843525B0, // TORCH
            // 0x843527E0, // FIRESND
            // 0x84352828, // GOBLIN01
            0x84352870, // XMONEY
            0x843528B8, // XHEALTH
            0x84352BD8, // XHEALTH
            // 0x84352EE8, // CV_SHIP3
            // 0x84353120, // CV_SHIP2
            // 0x84353268, // CV_SHIP4
            // 0x843533B0, // CV_SHIP4
            // 0x843534F0, // CV_SHIP4
            // 0x84353568, // CV_SHIP4
            // 0x843536F0, // CV_TILT
            // 0x843538F0, // CV_STLB
            // 0x843539B0, // CV_TILT
            // 0x843539F8, // TORCH
            // 0x84353B60, // CV_MUSH2
            0x84353C80, // XTORCHP
            // 0x84353CC8, // WATERSND
            // 0x84353D30, // TORCH
            // 0x84353ED0, // FIRESND
            // 0x84353FE8, // CV_MUST
            // 0x84354108, // FIRESND
            // 0x843541C8, // CV_TRAP
            // 0x84354210, // CV_EXPL
            // 0x84354258, // CV_EXPL
            // 0x843542A0, // CV_EXPL
            // 0x84354318, // CV_EXPL
            // 0x843543C8, // TORCH
            0x84354410, // XHEALTH
            0x84354488, // XMONEY
            // 0x84354520, // FIRESND
            // 0x84354660, // TORCH
            // 0x843547B8, // CV_DOR02
            0x84354800, // XMONEY
            0x84354848, // XMONEY
            0x84354890, // XMONEY
            0x843548D8, // XHEALTH
            0x84354920, // XHEALTH
            0x84354968, // XHEALTH
            // 0x843549D0, // CV_MUSH
            // 0x84354A60, // FIRESND
            // 0x84354D20, // TORCH
            // 0x84354E78, // CV_SHIP1
            // 0x84354EC0, // CV_SHIP1
            // 0x84354F08, // CV_SHIP1
            // 0x84354F50, // CV_SHIP1
            // 0x84354F98, // CV_SHIP1
            // 0x84354FE0, // CV_SHIP1
            // 0x84355028, // CV_SHIP1
            // 0x84355070, // CV_SHIP1
            // 0x843550B8, // CV_SHIP1
            // 0x843552B0, // CV_SHIP1
            // 0x843552F8, // CV_SHIP2
            // 0x84355370, // CV_SHIP1
            // 0x843553B8, // CV_SHIP1
            // 0x84355400, // CV_SHIP1
            // 0x84355448, // CV_SHIP1
            // 0x84355490, // CV_SHIP1
            // 0x843554D8, // CV_SHIP1
            // 0x84355520, // CV_SHIP1
            // 0x84355568, // CV_SHIP4
            // 0x843555B0, // CV_SHIP4
            // 0x84355700, // CV_SHIP4
            // 0x84355748, // CV_SHIP1
            // 0x84355790, // CV_TILT
            // 0x843558D0, // WATERSND
            // 0x84355A60, // CV_STLA
            // 0x84355D38, // CV_TILT
            // 0x84356000, // CV_CHN1
            // 0x84356170, // CV_MUSH
            // 0x843562B0, // CV_CHN2
            // 0x84356408, // CV_GEAR1
            // 0x84356450, // CV_FFLY1
            // 0x84356498, // CV_FFLY1
            // 0x84356528, // TORCH
            // 0x84356570, // FIRESND
            // 0x843566F0, // CV_MUSH2
            // 0x84356738, // CV_MUSH
            0x84356780, // XTORCHP
            0x843567C8, // XMONEY
            // 0x84356908, // TROLL01
            // 0x84356950, // CV_GEAR2
            // 0x84356B78, // CV_SHIP2
            // 0x84356BC0, // CV_FFLY1
            // 0x84356C08, // CV_MUSH2
            // 0x84356CE0, // CV_MUSH2
            // 0x84356D28, // CV_EXPL
            // 0x84356D70, // CV_EXPL
            // 0x84356EC8, // CV_SHIP3
            // 0x84356F58, // CV_FFLY1
            // 0x84356FA0, // CV_MUSH2
            // 0x84356FE8, // CV_MUSH2
            // 0x84357030, // CV_MUSH2
            // 0x84357108, // CV_TRAP
            // 0x84357150, // CV_EXPL
            // 0x84357198, // CV_EXPL
            // 0x84357240, // CV_EXPL
            // 0x84357288, // CV_MUSH
            // 0x843572D0, // CV_MUSH
            // 0x843573F0, // CV_MUSH
            0x84357438, // XHEALTH
            // 0x843574C8, // CV_FFLY1
            // 0x84357530, // FIRESND
            // 0x84357670, // TORCH
            // 0x843578A8, // CV_LOCK1
            // 0x84357AE8, // CV_LOCK2
            // 0x84357C60, // CV_DOR01
            // 0x84357D70, // GOBLIN01
            0x84357FF8, // EXTCAVE
            // 0x84358040, // CV_FFLY2
            // 0x84358088, // CV_MUSH2
            // 0x843580D0, // TORCH
            // 0x84358118, // FIRESND
            // 0x84358338, // CV_MUSH
            // 0x84358640, // CV_BRDG1
            // 0x84358460, // CV_SHCAM
            0x843587C0, // XHEALTH
            0x84358900, // XHEALTH
            // 0x84358978, // CV_BARRL
            // 0x843589F0, // CV_TILT
            // 0x84358BA8, // CV_TRAP
            // 0x84358BF0, // CV_SHCM2
            // 0x84358D10, // WATERSND
            // 0x84358F08, // CV_FFLY1
            0x84358F50, // XHEALTH
            // 0x84358F98, // CV_MUSH2
            // 0x84359080, // CV_MUSH2
            // 0x843590C8, // TORCH
            // 0x843592E0, // FIRESND
            // 0x84359420, // TORCH
            // 0x843595C8, // X
            // 0x84359708, // CV_CRACK
            // 0x84359940, // CV_CRAK1
            // 0x84359B78, // CV_CRAK2
            // 0x84359EC8, // CV_LVR1
            // 0x8435A1E0, // CV_TELE2
            // 0x8435A578, // CV_GATE1
            // 0x8435A5C0, // GOBLIN01
            0x8435A698, // XHEALTH
            // 0x8435A730, // FIRESND
            // 0x8435A798, // TORCH
            // 0x8435A858, // TORCH
            0x8435A998, // XTORCHP
            // 0x8435A9E0, // CV_CRACX
            0x8435AA28, // XMONEY
            0x8435AA70, // XMONEY
            0x8435AAE8, // XHEALTH
            // 0x8435AC48, // TORCH
            // 0x8435AE90, // CV_BRAZR
            // 0x8435AED8, // CV_FFLY1
            // 0x8435AF20, // TORCH
            // 0x8435AD58, // FIRESND
            // 0x8435B0C8, // GOBLIN01
            0x8435B110, // XHEALTH
            // 0x8435B1E8, // CV_FFLY1
            0x8435B230, // XHEALTH
            // 0x8435B2A8, // CV_FFLY1
            // 0x8435B410, // CV_TRAP
            // 0x8435B458, // GOBLIN01
            0x8435B4A0, // XIRONSK
            0x8435B4E8, // XHEALTH
            0x8435B530, // XMONEY
            // 0x8435B748, // CV_TRAP
            // 0x8435B790, // GOBLIN02
            // 0x8435B7D8, // GOBLIN02
            // 0x8435B820, // CV_FFLY1
            // 0x8435B868, // CV_FFLY1
            // 0x8435BAD8, // CV_TRAP
            // 0x8435BB68, // WATERSND
            // 0x8435BFB8, // WATERSND
            // 0x8435C000, // WATERSND
            // 0x8435C048, // CV_FFLY1
            // 0x8435C090, // GOBLIN01
            // 0x8435C198, // CV_EXPL
            // 0x8435AFC8, // X
            // 0x8435B010, // X
            // 0x8435C208, // X
            // 0x8435C250, // X
            // 0x8435C2E8, // X
            // 0x8435C330, // CV_FFLY2
            // 0x8435C378, // CV_EXPL
            // 0x8435C3C0, // CV_EXPL
            // 0x8435C408, // CV_EXPL
            // 0x8435C6E0, // CV_EXPL
            0x8435C728, // XTORCHP
            0x8435C770, // XSTRENTH
            0x8435C7B8, // XHEALTH
            0x8435C800, // XHEALTH
            0x8435C848, // XMONEY
            0x8435C890, // XHEALTH
            // 0x8435C938, // CV_FFLY1
            // 0x8435C9E8, // TORCH
            // 0x8435CB50, // FIRESND
            // 0x8435CC00, // TORCH
            // 0x8435CC48, // FIRESND
            // 0x8435CDB8, // GOBLIN02
            // 0x8435CE00, // CV_SPEAR
            // 0x8435CE48, // CV_SPEAR
            // 0x8435CE90, // CV_SPEAR
            // 0x8435CED8, // CV_SPEAR
            // 0x8435CF50, // CV_FFLY1
            // 0x8435CF98, // TORCH
            // 0x8435D048, // FIRESND
            // 0x8435D090, // CV_SPEAR
            // 0x8435D0D8, // CV_SPEAR
            // 0x8435D150, // CV_SPEAR
            // 0x8435D198, // CV_TRAP
            // 0x8435D228, // TORCH
            // 0x8435D290, // FIRESND
            // 0x8435D2D8, // GOBLIN02
            // 0x8435D320, // GOBLIN02
            0x8435D368, // XMONEY
            0x8435D3B0, // XHEALTH
            0x8435D3F8, // XHEALTH
            // 0x8435D5E0, // CV_TRAP
            // 0x8435D628, // GOBLIN04
            0x8435D670, // XTORCHP
            0x8435D7B0, // XMONEY
            // 0x8435DA10, // CV_KITH
            // 0x8435DC60, // CV_PASS
            // 0x8435DF80, // GOBLIN01
            // 0x8435DB20, // CV_TELE1
            // 0x8435E278, // CV_TRAP
            // 0x8435E2E0, // TORCH
            // 0x8435E3A0, // FIRESND
            0x8435E3E8, // XIRONSK
            0x0, // XMONEY
            }
        },
        {"GERRICKS" , new List<uint>{
            // 0x82BD3750, // X
            // 0x82BD3798, // X
            // 0x82BD3810, // X
            // 0x82BD3858, // X
            // 0x82BD38A0, // X
            // 0x82BD62D0, // X
            // 0x82BD66C0, // XCOMPASS
            0x82BD6800, // PARROT
            0x82BD6940, // GERRICK
            // 0x82BD6D30, // XCHEST
            // 0x82BD6FA8, // G_GETOFF
            0x82BD72E0, // EXTGERR
            // 0x82BD77E8, // XCANDLE
            // 0x82BD7C00, // XTORCH
            // 0x82BD7D30, // XSHOVEL
            // 0x82BD7E70, // G_GETOFF
            // 0x82BD7FB0, // G_GETOFF
            // 0x82BD81E8, // XFEATHER
            // 0x82BD8330, // XALOE
            // 0x82BD8418, // G_GETOFF
            // 0x82BD8558, // G_GETOFF
            // 0x0, // G_GETOFF
            }
        },
        {"BELLTOWR" , new List<uint>{
            // 0x82BD3750, // X
            // 0x82BD3798, // X
            // 0x82BD37E0, // X
            // 0x82BD5E60, // X
            0x82BD61D0, // EXTBELL1
            // 0x82BD63E8, // BELLROPE
            0x82BD6520, // EXTBELL2
            // 0x82BD6660, // FAVIS
            0x82BD6970, // XEYEPIEC
            // 0x82BD7028, // BELLROP2
            // 0x0, // BELL
            }
        },
        {"TEMPLE" , new List<uint>{
            // 0x82BD3780, // X
            // 0x82BD3888, // X
            // 0x82BD9E50, // X
            0x82BD9F88, // EXTTEMP
            // 0x82BDA2C8, // TORCH
            // 0x82BDA340, // TORCH
            // 0x82BDA550, // TORCH
            // 0x82BDA6F8, // FIRESND
             0x82BDAA48, // NIDAL
            // 0x82BDAAC0, // TORCH
            // 0x82BDAC00, // TORCH
            // 0x82BDAE10, // T_ALTAR
            // 0x82BDAEA0, // FIRESND
            // 0x82BDAF30, // FIRESND
            // 0x82BDB068, // T_ALTAR
            // 0x82BDB1A8, // T_ALTAR
            // 0x82BDB220, // TORCH
            // 0x82BDB268, // TORCH
            // 0x82BDB2B0, // TORCH
            // 0x82BDB4E8, // T_ALTAR
            // 0x82BDB578, // FIRESND
            // 0x0, // T_ALTAR
            }
        },
        {"JFFERS" , new List<uint>{
            // 0x829AE5C8, // X
            // 0x829AE610, // X
            // 0x829AE798, // X
            // 0x829B0C78, // XBOOKS
            // 0x829B0DB8, // XBOOKR
            // 0x829B1910, // XBOOKF
             0x829B1C68, // JFFER
            // 0x829B1EA0, // JFFERTB
            // 0x829B2220, // TORCH
            0x829B27B0, // EXTJFFR2
            // 0x829B28F0, // XBOOKE
            // 0x829B2A30, // XBOOKD
            0x0, // EXTJFFR1
            }
        },
        {"HIDEINT" , new List<uint>{
            // 0x828770D0, // X
            // 0x82877268, // SHARK
            // 0x828772B0, // SHARK
            // 0x828772F8, // SHARK
            // 0x82877508, // SHARK
            // 0x82877648, // YAELI
            // 0x82877888, // HI_BOAT
            // 0x82877AB8, // HI_LINE2
            // 0x82877CE8, // HI_BUOY2
            // 0x82877E10, // BOAT3
            // 0x82878048, // BOAT3
            0x828786A0, // XHEALTH
            // 0x828793F8, // PIRATE3
            // 0x82879BD0, // PIRATE4
            // 0x82879EF0, // BOAT3
            // 0x8287A1D8, // VANDAR
            // 0x8287A318, // BASIL
            // 0x8287A360, // HI_DOOR3
            // 0x8287A3A8, // HI_DOOR3
            // 0x8287A4E0, // HI_DOOR3
            // 0x8287ABD8, // TORCH
            0x8287AD18, // XHEALTH
            // 0x8287AF60, // HI_DOOR4
            // 0x8287B0A8, // HI_HOOK
            // 0x8287B138, // HI_HOOK
            // 0x8287BB58, // HI_HOOK
            0x8287BC98, // XHEALTH
            // 0x8287BCE0, // HI_DOOR2
            // 0x8287BD28, // HI_DOOR2
            // 0x8287BE68, // HI_DOOR2
            // 0x8287BEB0, // HI_DOOR1
            // 0x8287C0B0, // HI_DOOR1
            // 0x8287C1F0, // PIRATE1
            // 0x8287CD08, // PIRATE2
            // 0x8287CD50, // HI_DOOR
            // 0x8287D1A0, // HI_DOOR1
            // 0x8287D7B8, // TORCH
            // 0x8287DD38, // HI_DOORX
            0x8287ED10, // XHEALTH
            0x8287EE38, // XIRONSK
            // 0x8287EF78, // TORCH
            // 0x8287F420, // CANDLE
            0x0, // XJOURNAL
            }
        },
        {"JAILINT" , new List<uint>{
            // 0x835E4830, // X
            // 0x835E4878, // X
            // 0x835E48C0, // X
            // 0x835E4908, // X
            // 0x835E4980, // X
            // 0x835E49F8, // X
            // 0x835E4A70, // X
            // 0x835E4AB8, // X
            // 0x835E4B00, // X
            // 0x835E4B48, // X
            // 0x835E4B90, // X
            // 0x835E4BD8, // X
            // 0x835E4CB0, // X
            // 0x835E71C8, // X
            0x835E7310, // XHEALTH
            0x835E7358, // XHEALTH
            0x835E7490, // XHEALTH
            // 0x835E7800, // TORCH
            // 0x835E7848, // JAILDOOR
            // 0x835E7890, // JAILDOOR
            // 0x835E78D8, // JAILDOOR
            // 0x835E7920, // JAILDOOR
            // 0x835E7968, // TORCH
            // 0x835E7A78, // TORCH
            // 0x835E7AC0, // TORCH
            // 0x835E7FB0, // TORCH
            // 0x835E8920, // TORCH
            // 0x835E8B58, // TORCH
            0x835E8C08, // EXTJAIL
            // 0x835E8D18, // TORCH
            // 0x835E9470, // TORCH
            // 0x835E94E8, // TORCH
            // 0x835E95F8, // TORCH
            0x835E9640, // XHEALTH
            0x835E9688, // XHEALTH
            0x835E97C0, // XHEALTH
            // 0x835E98F8, // JOTO
            // 0x835E9A38, // DRAM
            // 0x835E9C00, // GUARDJL2
            // 0x835E9D10, // TORCH
            // 0x835E9DF0, // TORCH
            // 0x835E9F00, // TORCH
            // 0x838490B8, // TORCH
            // 0x83849B60, // TORCH
            // 0x0, // LOCKDOOR
            }
        },
        {"EXTPALAC" , new List<uint>{
            // 0x82877468, // DRAM11B
            // 0x828775A8, // RICHT11
            // 0x828776E8, // GUARD11B
            // 0x82877828, // GUARD11A
            // 0x8287A708, // DRAM11A
            0x8287AAD0, // ENTPAL2
            // 0x8287B2E8, // EPLIFTLV
            // 0x8287B330, // GUARDNS3
            // 0x8287B470, // GUARDNS3
            0x8287B5B0, // ENTPAL5
            0x8287B6F0, // ENTPAL4
            0x8287BB90, // ENTPAL3
            0x8287BEC8, // ENTPAL6
            0x8287C8B0, // ENTPAL7
            0x8287D110, // ENTPAL1
            // 0x8287D158, // GUARDNS3
            // 0x8287D1A0, // GUARDNS3
            // 0x8287D7F0, // GUARDNS3
            // 0x8287DB08, // EP_LOCK1
            // 0x8287F038, // EPDOOR
            // 0x8287F270, // EP_LIFT
            // 0x8287F3B0, // WINDSND
            // 0x8287F4F0, // DRAMPAL
            // 0x8287F708, // GUARDS11
            // 0x8287F928, // PI_SCAF2
            // 0x8287FB60, // PI_SCAF3
            // 0x8287FCA0, // PI_SCAF1
            // 0x8287FDE0, // WINDSND
            // 0x828801F0, // RICHTONP
            // 0x82880330, // DRAMS11
            // 0x82880618, // SCENE11
            // 0x82880760, // BLIMP11
            // 0x828808A0, // GUARDNS3
            // 0x0, // LOCKDOOR
            }
        },
        {"NECRISLE" , new List<uint>{
            // 0x83847840, // X
            // 0x83847888, // X
            // 0x83847960, // X
            // 0x83847D38, // X
            // 0x8384E210, // X
            // 0x8384E258, // TENTACLE
            // 0x8384E2A0, // TENTACLE
            // 0x8384E2E8, // TENTACLE
            // 0x8384E330, // TENTACLE
            // 0x8384E378, // TENTACLE
            // 0x8384E3C0, // TENTACLE
            // 0x8384E408, // TENTACLE
            // 0x8384E720, // TENTACLE
            // 0x8384E768, // TENTACLE
            // 0x8384E8A8, // TENTACLE
            0x8384E9E8, // ENTNECRT
            // 0x8384EB28, // NGASTA
            // 0x8384EB70, // WATERSND
            // 0x8384EBB8, // TENTACLE
            // 0x8384EC00, // TENTACLE
            // 0x8384EC48, // TENTACLE
            // 0x8384EC90, // WATERSND
            // 0x8384ECD8, // WATERSND
            // 0x8384ED20, // TENTACLE
            // 0x8384EE60, // TENTACLE
            // 0x8384F090, // NCGATE03
            // 0x8384F270, // NCSCENE1
            // 0x8384F2B8, // BATC
            // 0x8384F300, // BATC
            // 0x8384F348, // BATC
            // 0x8384F390, // BATC
            // 0x8384F3D8, // BATC
            // 0x8384F518, // BATC
            // 0x8384F748, // NCROCK03
            // 0x8384F980, // NCROCK02
            // 0x8384FB98, // NCROCK01
            // 0x8384FCC0, // NCGATE04
            // 0x8384FD08, // TENTACLE
            // 0x8384FD50, // TENTACLE
            // 0x8384FD98, // TENTACLE
            // 0x8384FDE0, // WATERSND
            // 0x8384FF20, // TENTACLE
            // 0x8384FF68, // SKEL02
            // 0x838500A8, // WATERSND
            // 0x838500F0, // WINDSND
            // 0x83850138, // WATERSND
            // 0x83850180, // TENTACLE
            // 0x838502B8, // WATERSND
            // 0x83850300, // BAT
            // 0x83850348, // BAT
            // 0x838504E0, // BAT
            // 0x83850620, // BAT
            // 0x83850B80, // SKEL01
            // 0x83850DE0, // NCSTHEAD
            // 0x83850F90, // SKEL01
            // 0x83850FD8, // TENTACLE
            // 0x83851280, // WATERSND
            // 0x83851430, // ZOMBIE02
            // 0x83851478, // BAT
            // 0x83851798, // BAT
            0x838519D0, // XSTRENTH
            0x83851B18, // XHEALTH
            0x83851B60, // XHEALTH
            // 0x83851BA8, // SKEL02
            // 0x83851BF0, // BAT
            // 0x83851C38, // BAT
            // 0x83851D40, // BAT
            // 0x83851D88, // WATERSND
            0x83851EC8, // XHEALTH
            // 0x83852008, // ZOMBIE01
            // 0x838522A8, // NCWHEEL
            // 0x83852748, // NCEYE01
            // 0x83852790, // WATERSND
            // 0x838527D8, // TENTACLE
            // 0x83852868, // WATERSND
            // 0x838528B0, // ZOMBIE01
            // 0x83852940, // WINDSND
            // 0x83852BC8, // SKEL01
            // 0x83852C10, // BAT
            // 0x83852C58, // BAT
            // 0x83852CA0, // BAT
            // 0x83852CE8, // BAT
            // 0x83852D30, // BAT
            // 0x83852F60, // BAT
            // 0x83853428, // ZOMBIE03
            0x83853470, // XHEALTH
            // 0x838534B8, // BATC
            // 0x83853500, // BATC
            // 0x83853548, // BAT
            // 0x838536A8, // BAT
            // 0x838538C0, // FIRESND
            // 0x84352080, // TORCH
            // 0x843521E8, // TENTACLE
            // 0x84352980, // SKEL02
            // 0x84352B08, // WATERSND
            // 0x84352EF0, // NCWIND
            // 0x84352F38, // SKEL03
            // 0x84352F80, // ZOMBIE02
            // 0x843530A0, // BATC
            // 0x843530E8, // WATERSND
            // 0x84353130, // TENTACLE
            // 0x84353178, // WATERSND
            // 0x843531C0, // ZOMBIE03
            // 0x84353208, // BAT
            // 0x84353250, // BAT
            // 0x843537A8, // BAT
            0x843539E8, // XHEALTH
            // 0x84353BE0, // SKEL01
            // 0x84353C28, // BAT
            // 0x84353C70, // BAT
            // 0x84353FB8, // BAT
            // 0x84354000, // TENTACLE
            // 0x84354048, // SKEL01
            // 0x84354318, // SKEL01
            // 0x843543A8, // WINDSND
            // 0x843543F0, // BAT
            // 0x84354530, // BAT
            // 0x843546D0, // NCGATEX
            // 0x84354900, // NCGATE2
            // 0x84354C80, // NCGATE1
            // 0x84354DA0, // WINDSND
            // 0x84354E30, // WATERSND
            // 0x84354E78, // TENTACLE
            // 0x84354EC0, // TENTACLE
            // 0x84354F08, // TENTACLE
            // 0x84354F50, // TENTACLE
            // 0x84355268, // WATERSND
            // 0x843552D0, // FIRESND
            // 0x84355480, // TORCH
            // 0x843554C8, // BAT
            // 0x84355660, // BAT
            0x843556A8, // XHEALTH
            // 0x843556F0, // BAT
            // 0x84355738, // BAT
            // 0x843557D0, // BAT
            // 0x843558A8, // FIRESND
            // 0x843558F0, // WATERSND
            // 0x84355938, // TENTACLE
            // 0x84355980, // TENTACLE
            0x843559C8, // XSTRENTH
            0x84355A10, // XHEALTH
            // 0x84355A58, // TORCH
            // 0x84355C90, // FIRESND
            // 0x84356260, // NCBAR1
            // 0x843562A8, // DOORBAT
            // 0x843562F0, // DOORBAT
            // 0x84356338, // DOORBAT
            // 0x84356380, // DOORBAT
            // 0x84356520, // DOORBAT
            // 0x84356760, // NCLEVER1
            // 0x843568A8, // NCDOOR1
            // 0x84356920, // ZOMBIE01
            // 0x84356968, // BAT
            // 0x843569B0, // BAT
            // 0x843569F8, // BAT
            // 0x84356A40, // BAT
            // 0x84356A88, // BAT
            // 0x84356AD0, // BAT
            // 0x84356B18, // BAT
            // 0x84356B60, // BAT
            // 0x84356BA8, // BAT
            // 0x84356BF0, // BAT
            // 0x84356C38, // BAT
            // 0x84356DD0, // BAT
            // 0x84356EA8, // ZOMBIE02
            // 0x84356FC8, // TORCH
            // 0x84357010, // TENTACLE
            // 0x84357058, // WATERSND
            // 0x843570E8, // WATERSND
            // 0x84357130, // WATERSND
            // 0x84357208, // TENTACLE
            // 0x84357250, // BAT
            // 0x84357298, // BAT
            // 0x843572E0, // BAT
            // 0x84357628, // BAT
            // 0x84357790, // SKEL01
            // 0x843577D8, // TENTACLE
            // 0x84357820, // WATERSND
            // 0x84357868, // ZOMBIE01
            // 0x843578B0, // ZOMBIE03
            // 0x84357940, // WINDSND
            // 0x84357988, // ZOMBIE01
            // 0x84357C10, // WINDSND
            // 0x84357C58, // SKEL01
            // 0x84357E98, // SKEL01
            // 0x84358000, // WINDSND
            // 0x84358048, // WATERSND
            // 0x84358090, // TENTACLE
            // 0x843580D8, // TENTACLE
            // 0x84358120, // WATERSND
            // 0x843581F8, // ZOMBIE02
            // 0x843583F0, // SKEL03
            // 0x84358438, // SKEL01
            // 0x84358990, // SKEL01
            // 0x84358C60, // TENTACLE
            0x84358CA8, // XHEALTH
            // 0x84359200, // SKEL01
            0x84359248, // XSTRENTH
            0x84359290, // XHEALTH
            0x843592D8, // XHEALTH
            // 0x84359320, // BATC
            // 0x84359368, // SKEL02
            // 0x843593B0, // BAT
            // 0x843593F8, // BAT
            // 0x84359440, // BAT
            // 0x84359488, // BAT
            // 0x843594D0, // BAT
            // 0x84359620, // BAT
            // 0x84359668, // BATC
            // 0x84359788, // SKEL01
            // 0x843597D0, // TENTACLE
            // 0x84359818, // WATERSND
            // 0x843598A8, // WINDSND
            // 0x84359B78, // SKEL02
            // 0x84359BC0, // BATC
            // 0x84359C08, // SKEL02
            // 0x84359C50, // BAT
            // 0x84359C98, // BAT
            // 0x84359CE0, // BAT
            // 0x84359D28, // BAT
            // 0x84359D70, // BAT
            // 0x84359DB8, // BAT
            // 0x84359FB0, // BAT
            // 0x8435A040, // SKEL01
            // 0x8435A088, // WATERSND
            // 0x8435A2C8, // TENTACLE
            // 0x8435A670, // WINDSND
            // 0x8435AA60, // WINDSND
            // 0x8435AAA8, // TENTACLE
            // 0x8435AD30, // WATERSND
            // 0x8435AE08, // WATERSND
            // 0x8435AE50, // WATERSND
            // 0x8435AF90, // WATERSND
            // 0x8435B1C8, // NCBOAT
            // 0x8435B210, // BOATMAN
            // 0x8435B258, // TENTACLE
            // 0x8435B2A0, // WATERSND
            // 0x8435B2E8, // WATERSND
            // 0x8435B330, // WATERSND
            // 0x8435B378, // TENTACLE
            // 0x8435B3C0, // TENTACLE
            // 0x8435B408, // WATERSND
            // 0x8435B450, // TENTACLE
            // 0x8435B498, // TENTACLE
            // 0x8435B4E0, // TENTACLE
            // 0x0, // TENTACLE
            }
        },
        {"CARTOGR" , new List<uint>{
            // 0x838477D0, // X
            // 0x83849EA8, // X
            // 0x8384A480, // MAIKO
            // 0x8384A988, // MAIKOTB
            // 0x8384ABA0, // TORCH
            0x8384AC18, // EXTCART
            // 0x8384AEB8, // TORCH
            // 0x8384AFB8, // CG_ROLL
            // 0x8384B000, // CG_ROLL
            // 0x8384B048, // CG_ROLL
            // 0x8384B1B0, // CG_ROLL
            // 0x8384B360, // MAIKOTB
            // 0x0, // MAIKOTB
            }
        },
        {"OBSERVE" , new List<uint>{
            0x82BD3750, // X
            0x82BD9D00, // X
            0x82BD9E40, // EXTOBSRV
            0x82BDA0F0, // OB_CAM21
            0x82BDA330, // OB_DOR02
            0x82BDA470, // OB_DOR01
            0x82BDA6A0, // OB_ENG21
            0x82BDA9B0, // OB_STEPR
            0x82BDABD0, // OB_DOME3
            0x82BDAE08, // OB_DOME4
            0x82BDB040, // OB_DOME5
            0x82BDB358, // OB_DOME2
            0x82BDB598, // OB_PLT05
            0x82BDB7D0, // OB_CON01
            0x82BDB9F0, // OB_PLT06
            0x82BDBC58, // OB_PLT07
            0x82BDBD98, // ERASMO
            0x82BDBF70, // OB_GEAR
            0x82BDC140, // OB_ENG03
            0x82BDC378, // OB_PIPE
            0x82BDC5B0, // OB_ENG23
            0x82BDC9E8, // OB_ENG22
            0x82BDCC28, // OB_ENGC
            0x82BDCE60, // OB_STEP2
            0x82BDCF98, // OB_STEP1
            0x82BDD1B8, // A
            0x82BDD3F0, // OB_CON12
            0x82BDD628, // OB_TEL01
            0x82BDD868, // OB_TEL00
            0x82BDDB80, // OB_DOME1
            0x82BDDDB8, // OB_DOME7
            0x82BDE188, // OB_DOME6
            0x82BDE3C0, // OB_PLT03
            0x82BDE5F0, // OB_CON02
            0x82BDE730, // GEARSND
            0x82BDE968, // OB_PLT02
            0x82BDEAD8, // OB_PLT01
            0x82BDED18, // OB_PIPE
            0x82BDEE58, // OB_STJET
            0x82BDF098, // OB_PLT08
            0x82BDF408, // OB_PLT04
            0x82BDF628, // OB_ORY16
            0x82BDF860, // OB_ORY09
            0x82BDFA78, // OB_ORY07
            0x82BDFAC0, // OB_STJET
            0x82BDFC00, // OB_STJET
            0x82BDFE40, // OB_ENGA
            0x82BE0080, // OB_ORY11
            0x82BE02B8, // OB_ORY17
            0x82BE04D8, // OB_ORY15
            0x82BE0740, // OB_ORY14
            0x82BE08A0, // OB_ORY10
            0x82BE09B0, // GEARSND
            0x82BE0BF0, // OB_ORY03
            0x82BE1D10, // OB_ORY12
            0x82BE1F78, // OB_TEL04
            0x82BE2148, // OB_ORY13
            0x82BE2380, // OB_ENG12
            0x82BE25A8, // OB_ENG11
            0x82BE27C0, // OB_ENG13
            0x82BE2AD8, // OB_ENG01
            0x82BE2D08, // OB_PLT00
            0x82BE2F40, // OB_ORY08
            0x82BE3178, // OB_ORY06
            0x82BE33B8, // OB_ORY05
            0x82BE35F8, // OB_ORY04
            0x82BE3830, // OB_ORY02
            0x82BE3B28, // OB_ORY01
            0x82BE3D60, // OB_ENG04
            0x82BE42F8, // OB_ENG05
            0x82BE4538, // OB_TEL03
            0x82BE4698, // OB_CON11
            0x82BE4728, // GEARSND
            0x82BE4948, // OB_PIPE
            0x82BE4B68, // OB_TEL02
            0x82BE4DA0, // OB_CON04
            0x82BE4FD8, // OB_CON03
            0x82BE5218, // OB_LIFT
            0x82BE5668, // OB_CON05
            0x82BE5970, // OB_ENG18
            0x82BE5C58, // OB_CON08
            0x82BE5D98, // OB_LIFT
            0x82BE60A8, // OB_ENG25
            0x82BE6490, // OB_ENG17
            0x82BE66C8, // OB_CON10
            0x82BE6900, // OB_CON09
            0x82BE6B78, // OB_CON06
            0x82BE6BC0, // OB_ENGW
            0x82BE74E8, // OB_PIPE
            0x82BE7730, // OB_ERSL
            0x82BE7988, // OB_CON07
            0x82BE7CA0, // OB_ERS10
            0x82BE7FB8, // OB_ERS08
            0x0, // OB_ERS06
            }
        },
        {"HIDEOUT" , new List<uint>{
            // 0x83C79A30, // HI_BOAT1
            // 0x83C79A78, // MARINE
            // 0x83C79AC0, // MARINE
            // 0x83C79B08, // MARINE
            // 0x83C79CC8, // MARINE
            // 0x83C79F08, // HI_BOAT4
            // 0x83C7A138, // HI_PLNK2
            // 0x83C7A278, // HI_PLNK1
            // 0x83C7A400, // HI_BOAT3
            // 0x83C7A448, // MARINE
            // 0x83C7A490, // MARINE
            // 0x83C7A4D8, // MARINE
            // 0x83C7A8A0, // MARINE
            // 0x83C7AB08, // HI_VIEW1
            // 0x0, // HI_VIEW2
            }
        },
    };

}
