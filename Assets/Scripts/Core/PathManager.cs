using System.IO;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public string GetRootFolder()
    {
        return Game.Config.redguardPath;
    }

    public void SetPath(string path)
    {
        Game.Config.redguardPath = path;
    }
    public string GetArtFolder()
    {
        if (Game.Config.useGlidePaths)
        {
            return Game.Config.redguardPath + "/fxart/";
        }
        else
        {
            return Game.Config.redguardPath + "/3dart/";
        }
    }

    public string GetMapsFolder()
    {
        return Game.Config.redguardPath + "/maps/";
    }

    public bool CheckPaths()
    {
        var rootPath = GetRootFolder();
        if (Directory.Exists(rootPath))
        {
            print("redguardPath does exist on this machine. Tested with path: " + rootPath);
            
            if (Directory.Exists(rootPath + "/3dart"))
            {
                Game.Config.useGlidePaths = false;
                print("Found /3dart directory. Using Non-Glide Paths");
                return true;
            }

            if (Directory.Exists(rootPath + "/fxart"))
            {
                Game.Config.useGlidePaths = true;
                print("Found /fxart directory. Using Glide Paths");
                return true;
            }
        }
        print("redguardPath is invalid. Tested with path: " + rootPath);
        return false;
    }
}
