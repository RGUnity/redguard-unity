using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public static class RGSoundStore
{
    public class RTXEntry
    {
        public string subtitle;
        public AudioClip audio;
        public RTXEntry(string subtitleIn, AudioClip audioIn)
        {
            subtitle = subtitleIn;
            audio = audioIn;
        }
    }

    public static List<AudioClip> SFXList;
    public static Dictionary<int, RTXEntry> RTXDict;

    static RGSoundStore()
    {
        SFXList = new List<AudioClip>();
        RTXDict = new Dictionary<int, RTXEntry>();
    }
    public static RTXEntry GetRTX(int id)
    {
        // we should have already loaded RTX here
        return RTXDict[id];
    }
    public static AudioClip GetSFX(int id)
    {
        // we should have already loaded SFX here
        return SFXList[id];
    }


    public static void LoadRTX(string rtxname)
    {
        try
        {
            string path = new string(Game.pathManager.GetRootFolder() + rtxname + ".RTX");
            RGRTXFile rtxFile = new RGRTXFile();
            rtxFile.LoadFile(path);

            foreach(KeyValuePair<int, RGRTXFile.RTXItem> entry in rtxFile.rtxItemDict)
            {
                // do something with entry.Value or entry.Key
                RTXEntry newRTX = new RTXEntry(entry.Value.subtitle, null);
                RTXDict.Add(entry.Key, newRTX);
            }
        }
        catch(Exception ex)
        {
            Debug.Log($"Failed to read RTX file with error {ex.Message}");
        }
    }

    public static void LoadSFX(string sfxname)
    {
        try
        {
            string path = new string(Game.pathManager.GetSoundFolder() + sfxname + ".SFX");
            RGSFXFile sfxFile = new RGSFXFile();
            sfxFile.LoadFile(path);

            for(int i=0;i<sfxFile.FXHD.numSounds;i++)
            {
                SFXList.Add(SFXToAudio(sfxFile.FXDT.soundEffectList[i], i));
            }
        }
        catch(Exception ex)
        {
            Debug.Log($"Failed to read SFX file with error {ex.Message}");
        }
    }
    private static AudioClip SFXToAudio(RGSFXFile.SoundEffect sfx, int id)
    {
        AudioClip o;
        int channels = 0;
        switch(sfx.typeId)
        {
            case RGSFXFile.AudioType.audiotype_mono8:
            case RGSFXFile.AudioType.audiotype_mono16:
                channels = 1;
                break;
            case RGSFXFile.AudioType.audiotype_stereo8:
            case RGSFXFile.AudioType.audiotype_stereo16:
                channels = 2;
                break;
        }
        float[] PCM2Float;
        switch(sfx.bitDepth)
        {
            case RGSFXFile.AudioBitDepth.audiodepth_8:
                PCM2Float = PCM8ToFloat(sfx.PCMData, sfx.dataLength);
                break;
            case RGSFXFile.AudioBitDepth.audiodepth_16:
                PCM2Float = PCM16ToFloat(sfx.PCMData, sfx.dataLength);
                break;
            default:
                // should never land here; an exception might be more fitting
                PCM2Float = new float[1];
                break;
        }
        o = AudioClip.Create($"SFX_{id}", PCM2Float.Length, channels, sfx.sampleRate, false);
        o.SetData(PCM2Float, 0);

        return o;
    }
    private static float[] PCM8ToFloat(byte[] datain, int dataLength)
    {
        float[] o = new float[dataLength];
        for(int i=0;i<dataLength;i++)
        {
            o[i] = (((float)datain[i])/128.0f)-1.0f;;
        }
        return o;
    }
    private static float[] PCM16ToFloat(byte[] datain, int dataLength)
    {
        float[] o = new float[dataLength/2];
        int j = 0;
        for(int i=0;i<dataLength;i+=2)
        {
            o[j] = ((float)BitConverter.ToInt16(datain, i))/32768.0f;
            j++;
        }
        return o;
    }
}
