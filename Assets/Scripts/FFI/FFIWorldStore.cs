using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FFIWorldStore
{
    public struct WorldData
    {
        public string RGM;
        public string WLD;
        public string COL;
        public string skyBoxGXA;
        public string skyBoxBSI;
        public string sunImage;
        public string loadScreen;
    }

    private static Dictionary<int, WorldData> cachedWorldList;

    public static Dictionary<int, WorldData> GetWorldList()
    {
        if (cachedWorldList != null)
            return cachedWorldList;

        cachedWorldList = new Dictionary<int, WorldData>();
        string worldIniPath = Path.Combine(Game.pathManager.GetRootFolder(), "WORLD.INI");
        if (!File.Exists(worldIniPath))
        {
            Debug.LogWarning("[FFIWorldStore] WORLD.INI not found: " + worldIniPath);
            return cachedWorldList;
        }

        var worlds = new Dictionary<int, WorldData>();
        string[] lines = File.ReadAllLines(worldIniPath);

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.Length == 0 || line[0] == ';' || line[0] == '[')
                continue;

            int eqIndex = line.IndexOf('=');
            if (eqIndex < 0)
                continue;

            string key = line.Substring(0, eqIndex).Trim();
            string val = line.Substring(eqIndex + 1).Trim();

            // Parse indexed keys like world_map[1]
            int bracketOpen = key.IndexOf('[');
            if (bracketOpen < 0)
                continue;

            int bracketClose = key.IndexOf(']', bracketOpen);
            if (bracketClose < 0)
                continue;

            if (!int.TryParse(key.Substring(bracketOpen + 1, bracketClose - bracketOpen - 1), out int worldIndex))
                continue;

            string memberName = key.Substring(0, bracketOpen);

            if (!worlds.TryGetValue(worldIndex, out WorldData w))
            {
                w = new WorldData
                {
                    RGM = string.Empty,
                    WLD = string.Empty,
                    COL = string.Empty,
                    skyBoxGXA = string.Empty,
                    skyBoxBSI = string.Empty,
                    sunImage = string.Empty,
                    loadScreen = string.Empty
                };
            }

            switch (memberName)
            {
                case "world_map":
                    w.RGM = ToUpperStem(val);
                    break;
                case "world_world":
                    w.WLD = ToUpperStem(val);
                    break;
                case "world_palette":
                    w.COL = ToUpperStem(val);
                    break;
                case "world_sky":
                    w.skyBoxGXA = ToUpperStem(val);
                    break;
                case "world_skyfx":
                    w.skyBoxBSI = ToUpperStem(val);
                    break;
                case "world_sunimg":
                    w.sunImage = ToUpperStem(val);
                    break;
                case "world_flash_filename":
                    w.loadScreen = ToUpperStem(val);
                    break;
            }

            worlds[worldIndex] = w;
        }

        cachedWorldList = worlds;
        return cachedWorldList;
    }

    public static void ClearCache()
    {
        cachedWorldList = null;
    }

    private static string ToUpperStem(string path)
        => FFIPathUtils.NormalizeModelName(path);
}
