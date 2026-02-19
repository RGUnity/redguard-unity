using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;
using Assets.Scripts.RGFileImport.RGGFXImport;

public static class RGINIStore
{
    static RGINIFile iniWorldData;
    static RGINIFile iniMenuData;

    static RGINIStore()
    {
        iniWorldData = new RGINIFile();
        iniWorldData.LoadFile($"{Game.pathManager.GetRootFolder()}/WORLD.INI");
        iniMenuData = new RGINIFile();
        iniMenuData.LoadFile($"{Game.pathManager.GetRootFolder()}/MENU.INI");
    }

    // WORLD.INI
    public struct worldData
    {
        public string RGM; // doubles as name
        public string WLD;
        public string COL;
        public string skyBoxGXA;
        public string skyBoxBSI;
        public string sunImage;
        public string loadScreen;
    }
    public static Dictionary<int, worldData> GetWorldList()
    {
        Dictionary<int, worldData> o = new Dictionary<int, worldData>();
        foreach(int key in iniWorldData.worldData.worlds.Keys)
        {
            worldData w = new worldData();
            RGINIFile.INIWorld world = iniWorldData.worldData.worlds[key];
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
            if(!string.IsNullOrEmpty(world.sky))
            {
                // example input: system\sunset.GXA
                // remove system/ and .GXA:
                w.skyBoxGXA = world.sky.Substring(7).ToUpper();
                w.skyBoxGXA = w.skyBoxGXA.Substring(0, w.skyBoxGXA.IndexOf("."));
            }
            else
                w.skyBoxGXA = "";
            if(!string.IsNullOrEmpty(world.skyfx))
            {
                // example input: sky899.bsi
                // remove .BSI:
                w.skyBoxBSI = world.skyfx.Substring(0, world.skyfx.IndexOf(".")).ToUpper();
            }
            else
                w.skyBoxBSI = "";
            if(!string.IsNullOrEmpty(world.sunimg))
            {
                // example input: sun001.bsi
                // remove .BSI:
                w.skyBoxBSI = world.sunimg.Substring(0, world.sunimg.IndexOf(".")).ToUpper();
            }
            else
                w.sunImage = "";
            if(!string.IsNullOrEmpty(world.flash_filename))
            {
                // example input: system\island.gxa
                // remove system/ and .gxa
                w.loadScreen = world.flash_filename.Substring(7).ToUpper();
                w.loadScreen = w.loadScreen.Substring(0, w.loadScreen.IndexOf("."));
            }
            else
                w.loadScreen = "";

            o.Add(key, w);
        }
        return o;
    }
    // MENU.INI
    public enum textJustify
    {
        textjustify_left,
        textjustify_right,
    }
    public struct menuItemData
    {
        public int textX;
        public int textY;
        public string text;
        public textJustify justify;

    }
    public struct menuData
    {
        public List<menuItemData> itemData;
    }

}
