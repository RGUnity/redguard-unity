using System;
using System.Collections.Generic;
using UnityEngine;
using RGFileImport;

public static class RGSoundStore
{
    static List<AudioClip> SFXList;

    static RGSoundStore()
    {
        SFXList = new List<AudioClip>();
    }

    public static AudioClip GetSFX(int id)
    {
        // we should have already loaded SFX here
        return SFXList[id];
    }


    public static void LoadSFX(string sfxname)
    {
        try
        {
            string path = new string(Game.pathManager.GetSoundFolder() + sfxname + ".SFX");
            RGSFXFile sfxFile = new RGSFXFile();
            sfxFile.LoadFile(path);

            Debug.Log($"sounds: {sfxFile.FXHD.numSounds}");
            for(int i=0;i<sfxFile.FXHD.numSounds;i++)
            {
                SFXList.Add(SFXToAudio(sfxFile.FXDT.soundEffectList[i], i));
            Debug.Log($"Loaded sound {i}");
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
    private static float[] PCM8ToFloat(sbyte[] datain, int dataLength)
    {
        float[] o = new float[dataLength];
        for(int i=0;i<dataLength;i++)
        {
            o[i] = ((float)datain[i])/128.0f;
        }
        return o;
    }
    private static float[] PCM16ToFloat(sbyte[] datain, int dataLength)
    {
        float[] o = new float[dataLength];
        int j = 0;
        byte[] datain_u = (byte[])(Array)datain;
        for(int i=0;i<dataLength;i+=2)
        {
            o[j] = ((float)BitConverter.ToInt16(datain_u, i))/32768.0f;
            j++;
        }
        return o;
    }

}
