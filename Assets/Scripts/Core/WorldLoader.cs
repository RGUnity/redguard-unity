using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;

public static class WorldLoader
{
    // TODO: this is a copy from RGScriptedObject
    const float DA2DG = -(180.0f/1024.0f); // negative angles?

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

        // load in sounds
        RGSoundStore.LoadSFX(SFX);
        RGSoundStore.LoadRTX(RTX);

        // load in objects
        ModelLoader.LoadArea(RGM, COL, WLD);

        // load the player
        LoadPlayer(RGM, playerSpawnLocation, playerSpawnOrientation);

    }
    static void LoadPlayer(string RGM, int playerSpawnLocation, int playerSpawnOrientation)
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

        Vector3 playerSpawnPos = RGObjectStore.mapMarkerList[playerSpawnLocation];
        Quaternion playerSpawnRot = Quaternion.AngleAxis(((float)playerSpawnOrientation)/DA2DG, Vector3.up);

        RGObjectStore.GetPlayerMain().SetPositionAndRotation(playerSpawnPos, playerSpawnRot);
        /*
        RGObjectStore.GetPlayerObject().transform.Position = playerSpawnPos;
        RGObjectStore.GetPlayerObject().transform.localRotation = playerSpawnRot;
        */
        Debug.Log($"POS/ROT: {playerSpawnPos}/{playerSpawnRot}");
    }
}
