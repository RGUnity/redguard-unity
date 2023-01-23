using System.Collections;
using UnityEngine;
namespace RGFileImport.VideoPlayback
{

public enum RGVideoFormatFlags
{
    Default=0,
    RingFrame=1,
    YInterlace=2,
    RingFramePlusYInterlace=3,
    YDouble=4,
    RingFramePlusYDouble=5
}
public class RGUnitySmackerPlayer : MonoBehaviour
{
    [SerializeField] RGSmacker SmackerInstance;

    [SerializeField] bool debugHeaderInfo;
    [SerializeField] string path = "D:/Intro.smk";
    void Awake()
    {
        SmackerInstance = new RGSmacker();
        SmackerInstance.LoadFile(path);
        Debug.Log(SmackerInstance);
    }
    void Start()
    {
        if(SmackerInstance.dataHeader!=null)
        Debug.Log(SmackerInstance.dataHeader);
        if(debugHeaderInfo)
        {
            PrintHeaderToDebug();

        }
    }
    void PrintHeaderToDebug()
    {
        Debug.Log("SMACKER Header Statistics for "+path+":\n"+
        "Signature: "+SmackerInstance.dataHeader.Signature+"/SMK"+SmackerInstance.GetSMKVersion()+
        "\nResolution: "+SmackerInstance.dataHeader.Width+"x"+SmackerInstance.dataHeader.Height+
        "\nFrame Count: " + SmackerInstance.dataHeader.Frames+
        "\nFPS: " + SmackerInstance.GetFramerate()+
        "\nFlag: "+(RGVideoFormatFlags)SmackerInstance.dataHeader.Flags+
        "\nAudioSize0: "+SmackerInstance.dataHeader.AudioSize[0]+
        "\nTrees Size: "+SmackerInstance.dataHeader.TreesSize+
        "\nMMap Size: "+SmackerInstance.dataHeader.MMap_Size+
        "\nMClr Size: "+SmackerInstance.dataHeader.MClr_Size+
        "\nFull Size: "+SmackerInstance.dataHeader.Full_Size+
        "\nType Size: "+SmackerInstance.dataHeader.Type_Size);
    }
}
}