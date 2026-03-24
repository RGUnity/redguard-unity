using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class FFISoundStore
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

    [Serializable]
    private class RtxMetadataRoot
    {
        public List<RtxMetadataEntry> entries;
        public List<RtxMetadataEntry> rtx_entries;
        public List<RtxMetadataEntry> subtitles;
    }

    [Serializable]
    private class RtxMetadataEntry
    {
        public int id;
        public int index;
        public string subtitle;
        public string text;
    }

    public static List<AudioClip> SFXList { get; } = new List<AudioClip>();
    public static Dictionary<int, RTXEntry> RTXDict { get; } = new Dictionary<int, RTXEntry>();

    public static RTXEntry GetRTX(int id)
    {
        if (RTXDict.TryGetValue(id, out RTXEntry entry))
        {
            return entry;
        }

        throw new KeyNotFoundException("RTX id not loaded: " + id);
    }

    public static AudioClip GetSFX(int id)
    {
        if (id >= 0 && id < SFXList.Count)
        {
            return SFXList[id];
        }

        throw new IndexOutOfRangeException("SFX id not loaded: " + id);
    }

    public static void LoadRTX(string rtxName)
    {
        // TODO: RTX loading disabled — each entry currently re-parses the full file, causing a hang.
        // When re-enabled, use path-based APIs:
        // - RgpreBindings.RtxEntryCount(path)
        // - RgpreBindings.RtxMetadata(path)
        // - RgpreBindings.ConvertRtxEntryToWav(path, entryIndex)
        Debug.Log("[FFI] RTX loading skipped (not yet implemented with cached FFI)");
        return;

        RTXDict.Clear();

        string rootFolder = Game.pathManager.GetRootFolder();
        string uppercasePath = Path.Combine(rootFolder, rtxName + ".RTX");
        string lowercasePath = Path.Combine(rootFolder, rtxName.ToLowerInvariant() + ".rtx");
        string path = File.Exists(uppercasePath) ? uppercasePath : lowercasePath;

        if (!File.Exists(path))
        {
            Debug.LogWarning("[FFI] RTX file not found: " + rtxName);
            return;
        }

        int count = RgpreBindings.RtxEntryCount(path);
        Dictionary<int, RtxMetadataEntry> metadataByIndex = GetRtxMetadataByIndex(path);

        for (int entryIndex = 0; entryIndex < count; entryIndex++)
        {
            IntPtr wavPtr = RgpreBindings.ConvertRtxEntryToWav(path, entryIndex);
            AudioClip clip = null;
            if (wavPtr != IntPtr.Zero)
            {
                byte[] wavBytes = RgpreBindings.ExtractBytesAndFree(wavPtr);
                clip = WavToAudioClip(wavBytes, "RTX_" + entryIndex);
            }

            string subtitle = string.Empty;
            int metadataId = 0;
            if (metadataByIndex.TryGetValue(entryIndex, out RtxMetadataEntry metadata))
            {
                subtitle = !string.IsNullOrEmpty(metadata.subtitle) ? metadata.subtitle : (metadata.text ?? string.Empty);
                metadataId = metadata.id;
            }

            RTXEntry rtxEntry = new RTXEntry(subtitle, clip);
            RTXDict[entryIndex] = rtxEntry;
            if (metadataId != 0)
            {
                RTXDict[metadataId] = rtxEntry;
            }
        }
    }

    public static void LoadSFX(string sfxName)
    {
        SFXList.Clear();

        string soundFolder = Game.pathManager.GetSoundFolder();
        string uppercasePath = Path.Combine(soundFolder, sfxName + ".SFX");
        string lowercasePath = Path.Combine(soundFolder, sfxName.ToLowerInvariant() + ".sfx");
        string path = File.Exists(uppercasePath) ? uppercasePath : lowercasePath;

        if (!File.Exists(path))
        {
            Debug.LogWarning("[FFI] SFX file not found: " + sfxName);
            return;
        }

        int count = RgpreBindings.SfxEffectCount(path);
        for (int effectIndex = 0; effectIndex < count; effectIndex++)
        {
            IntPtr wavPtr = RgpreBindings.ConvertSfxToWav(path, effectIndex);
            if (wavPtr == IntPtr.Zero)
            {
                SFXList.Add(null);
                continue;
            }

            byte[] wavBytes = RgpreBindings.ExtractBytesAndFree(wavPtr);
            SFXList.Add(WavToAudioClip(wavBytes, "SFX_" + effectIndex));
        }
    }

    private static Dictionary<int, RtxMetadataEntry> GetRtxMetadataByIndex(string filePath)
    {
        // TODO: rg_rtx_metadata was removed from DLL. Re-enable when RTX cache API is available.
        return new Dictionary<int, RtxMetadataEntry>();
    }

    private static AudioClip WavToAudioClip(byte[] wavBytes, string clipName)
    {
        if (!TryReadWav(wavBytes, out int channels, out int sampleRate, out int bitsPerSample, out byte[] pcmData))
        {
            return null;
        }

        float[] samples = bitsPerSample switch
        {
            8 => Pcm8ToFloat(pcmData),
            16 => Pcm16ToFloat(pcmData),
            _ => null
        };

        if (samples == null || channels <= 0)
        {
            return null;
        }

        int sampleCount = samples.Length / channels;
        if (sampleCount <= 0)
        {
            return null;
        }

        AudioClip clip = AudioClip.Create(clipName, sampleCount, channels, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static bool TryReadWav(byte[] wavBytes, out int channels, out int sampleRate, out int bitsPerSample, out byte[] pcmData)
    {
        channels = 0;
        sampleRate = 0;
        bitsPerSample = 0;
        pcmData = null;

        if (wavBytes == null || wavBytes.Length < 44)
        {
            return false;
        }

        using var stream = new MemoryStream(wavBytes);
        using var reader = new BinaryReader(stream);

        string riff = new string(reader.ReadChars(4));
        reader.ReadInt32();
        string wave = new string(reader.ReadChars(4));
        if (riff != "RIFF" || wave != "WAVE")
        {
            return false;
        }

        while (reader.BaseStream.Position + 8 <= reader.BaseStream.Length)
        {
            string chunkId = new string(reader.ReadChars(4));
            int chunkSize = reader.ReadInt32();
            long nextChunk = reader.BaseStream.Position + chunkSize;

            if (chunkId == "fmt ")
            {
                short format = reader.ReadInt16();
                channels = reader.ReadInt16();
                sampleRate = reader.ReadInt32();
                reader.ReadInt32();
                reader.ReadInt16();
                bitsPerSample = reader.ReadInt16();
                if (format != 1)
                {
                    return false;
                }
            }
            else if (chunkId == "data")
            {
                pcmData = reader.ReadBytes(chunkSize);
            }

            reader.BaseStream.Position = nextChunk;
        }

        return channels > 0 && sampleRate > 0 && bitsPerSample > 0 && pcmData != null;
    }

    private static float[] Pcm8ToFloat(byte[] pcm)
    {
        float[] output = new float[pcm.Length];
        for (int i = 0; i < pcm.Length; i++)
        {
            output[i] = (pcm[i] - 128f) / 128f;
        }

        return output;
    }

    private static float[] Pcm16ToFloat(byte[] pcm)
    {
        int count = pcm.Length / 2;
        float[] output = new float[count];
        for (int i = 0; i < count; i++)
        {
            short value = BitConverter.ToInt16(pcm, i * 2);
            output[i] = value / 32768f;
        }

        return output;
    }
}
