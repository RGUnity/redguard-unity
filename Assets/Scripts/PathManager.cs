using System;
using System.IO;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static bool useGlidePaths = false;
    
    public static string GetRootFolder()
    {
        return Game.configData.redguardPath;
    }

    public static string GetArtFolder()
    {
        if (Game.configData.useGlidePaths)
        {
            return Game.configData.redguardPath + "/fxart";
        }
        else
        {
            return Game.configData.redguardPath + "/3dart";
        }
    }

    public static string GetMapsFolder()
    {
        return Game.configData.redguardPath + "/maps";
    }

    public static bool CheckPaths()
    {
        var rootPath = GetRootFolder();
        if (Directory.Exists(rootPath))
        {
            print("configData.redguardPath does exist. Tested with path: " + rootPath);
            
            if (Directory.Exists(rootPath + "/3dart"))
            {
                useGlidePaths = false;
                print("Using Non-Glide Paths");
                return true;
            }

            if (Directory.Exists(rootPath + "/fxart"))
            {
                useGlidePaths = true;
                print("Using Glide Paths");
                return true;
            }
        }
        print("configData.redguardPath is invalid. Tested with path: " + rootPath);
        return false;
    }
}
