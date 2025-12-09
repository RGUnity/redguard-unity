using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public static class WorldLoader
{
    struct WorldLoadRequest
    {
        public bool loadRequested;
        public int worldId;
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
    // TODO: this is a copy from RGScriptedObject
    const float DA2DG = -(180.0f/1024.0f); // negative angles?

    public static void RequestLoadWorld(int worldId, int playerSpawnLocation, int playerSpawnOrientation)
    {
        loadRequest.loadRequested = true;
        loadRequest.worldId = worldId;
        loadRequest.playerSpawnLocation = playerSpawnLocation;
        loadRequest.playerSpawnOrientation = playerSpawnOrientation;
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

    public static void LoadWorldIfRequested()
    {
        if(loadRequest.loadRequested)
        {
            loadRequest.loadRequested = false;

            UnloadLoadedWorld();
            LoadWorld(loadRequest.worldId, 
                      loadRequest.playerSpawnLocation,
                      loadRequest.playerSpawnOrientation);
            Debug.Log($"LOADED WORLD: {loadRequest.worldId},{loadRequest.playerSpawnLocation},{loadRequest.playerSpawnOrientation}");
        }
    }
    public static void LoadWorld(int worldId, int playerSpawnLocation, int playerSpawnOrientation)
    {
        string RGM;
        string COL;
        string WLD;
        string SFX;
        string RTX;

        // get the INI data for this world
        RGINIStore.worldData worldData = RGINIStore.GetWorldList()[worldId];

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
            RGSoundStore.LoadSFX(SFX);
        }

        if(!RTXLoaded)
        {
            RGSoundStore.LoadRTX(RTX);
        }


        // load in objects
        loadedObjects = ModelLoader.LoadArea(RGM, COL, WLD);

        // load the player
        LoadPlayer(RGM, playerSpawnLocation, playerSpawnOrientation);

    }
    static void LoadPlayer(string RGM, int playerSpawnLocation, int playerSpawnOrientation)
    {
        // only load player once, this is a bit of a hack, we should just clear it decently
        if(!playerLoaded)
        {
            // TODO: we need to account for gremlin too
            RGFileImport.RGRGMFile filergm = RGRGMStore.GetRGM(RGM);

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
            playerLoaded = true;
        }

        Vector3 playerSpawnPos = RGObjectStore.mapMarkerList[playerSpawnLocation];
        Quaternion playerSpawnRot = Quaternion.AngleAxis(((float)playerSpawnOrientation)/DA2DG, Vector3.up);

        RGObjectStore.GetPlayerMain().SetPositionAndRotation(playerSpawnPos, playerSpawnRot);
    }
}
