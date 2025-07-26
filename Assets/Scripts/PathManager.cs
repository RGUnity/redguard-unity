using System.IO;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static string GetRootFolder()
    {
        return Game.configData.redguardPath;
    }

    public string GetArtFolder()
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

    public string GetMapsFolder()
    {
        return Game.configData.redguardPath + "/maps";
    }

    public static bool RedguardPathExists()
    {
        return Directory.Exists(GetRootFolder());
    }
}
