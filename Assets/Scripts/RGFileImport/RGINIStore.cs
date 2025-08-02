using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RGINIStore
{
    static RGINIFile iniData;

    static RGINIStore()
    {
        iniData = new RGINIFile();
        iniData.LoadFile($"{Game.pathManager.GetRootFolder()}/WORLD.INI");
    }

    public struct worldData
    {
        public string RGM; // doubles as name
        public string WLD;
        public string COL;
    }
    public static List<worldData> GetWorldList()
    {
        List<worldData> o = new List<worldData>();
        foreach(int key in iniData.worldData.worlds.Keys)
        {
            worldData w = new worldData();
            RGINIFile.INIWorld world = iniData.worldData.worlds[key];
            if(!string.IsNullOrEmpty(world.map))
            {
                // example input: MAPS\start.rgm
                // remove MAPS/ and .rgm:
                w.RGM = world.map.Substring(5).ToUpper();
                w.RGM = w.RGM.Substring(0, w.RGM.IndexOf("."));
                
            }
            else
                w.RGM = "";
            if(!string.IsNullOrEmpty(world.world))
            {
                // example input: MAPS\hideout.WLD
                // remove MAPS/ and .WLD:
                w.WLD = world.world.Substring(5).ToUpper();
                w.WLD = w.WLD.Substring(0, w.WLD.IndexOf("."));
            }
            else
                w.WLD = "";
            if(!string.IsNullOrEmpty(world.palette))
            {
                // example input: 3DART\sunset.COL
                // remove 3DART/ and .COL:
                w.COL = world.palette.Substring(6).ToUpper();
                w.COL = w.COL.Substring(0, w.COL.IndexOf("."));
            }
            else
                w.COL = "";
            o.Add(w);
        }
        return o;
    }
}
