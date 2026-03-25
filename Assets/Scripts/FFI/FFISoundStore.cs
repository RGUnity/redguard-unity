using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FFISoundStore
{
    private const int WavMinHeaderSize = 44;
    private const float Pcm8Scale = 128f;
    private const float Pcm16Scale = 32768f;

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

    public static List<AudioClip> SfxClips { get; } = new List<AudioClip>();
    public static Dictionary<int, RTXEntry> RtxEntries { get; } = new Dictionary<int, RTXEntry>();

    public static RTXEntry GetRTX(int id)
    {
        if (RtxEntries.TryGetValue(id, out RTXEntry entry))
        {
            return entry;
        }

        throw new KeyNotFoundException("RTX id not loaded: " + id);
    }

    public static AudioClip GetSFX(int id)
    {
        if (id >= 0 && id < SfxClips.Count)
        {
            return SfxClips[id];
        }

        throw new IndexOutOfRangeException("SFX id not loaded: " + id);
    }

    // TODO: RTX loading disabled — each entry currently re-parses the full file, causing a hang.
    // When re-enabled, use path-based APIs:
    // - RgpreBindings.RtxEntryCount(path)
    // - RgpreBindings.ConvertRtxEntryToWav(path, entryIndex)
    public static void LoadRTX(string rtxName)
    {
        Debug.Log("[FFI] RTX loading skipped (not yet implemented with cached FFI)");
    }

    public static void LoadSFX(string sfxName)
    {
        SfxClips.Clear();

        string soundFolder = Game.pathManager.GetSoundFolder();
        string path = FFIPathUtils.ResolveFile(soundFolder, sfxName, ".SFX");

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
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
                SfxClips.Add(null);
                continue;
            }

            byte[] wavBytes = RgpreBindings.ExtractBytesAndFree(wavPtr);
            SfxClips.Add(WavToAudioClip(wavBytes, "SFX_" + effectIndex));
        }
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

        if (wavBytes == null || wavBytes.Length < WavMinHeaderSize)
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
            output[i] = (pcm[i] - Pcm8Scale) / Pcm8Scale;
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
            output[i] = value / Pcm16Scale;
        }

        return output;
    }
}
