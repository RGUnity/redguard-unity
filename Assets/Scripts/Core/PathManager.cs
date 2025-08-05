using System.IO;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public string GetRootFolder()
    {
        return Game.Config.redguardPath;
    }

    public bool SetPath(string rootPath)
    {
        if (Directory.Exists(rootPath))
        {
            print("This rootPath is a valid path on this machine: " + rootPath);
            
            if (Directory.Exists(rootPath + "/3dart"))
            {
                Game.Config.redguardPath = rootPath;
                Game.Config.useGlidePaths = false;
                print("Found /3dart directory. Using Non-Glide Paths");
                return true;
            }

            if (Directory.Exists(rootPath + "/fxart"))
            {
                Game.Config.redguardPath = rootPath;
                Game.Config.useGlidePaths = true;
                print("Found /fxart directory. Using Glide Paths");
                return true;
            }
            else
            {
                print("This rootPath exists, but there is no art folder inside");
                return false;
            }
        }
        print("rootPath not found. Tested with path: " + rootPath);
        return false;
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

    public bool ValidatePath(string rootPath)
    {
        if (Directory.Exists(rootPath))
        {
            print("redguardPath is a valid path on this machine. Tested with path: " + rootPath);
            
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
            else
            {
                print("This rootPath exists, but there is no art folder inside");
                return false;
            }
        }
        print("rootPath not found. Tested with path: " + rootPath);
        return false;
    }
}
