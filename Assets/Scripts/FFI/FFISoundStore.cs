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

    // RTX files live at the install root (next to WORLD.INI), NOT in /sound/.
    // We open one native handle per file via rg_open_rtx, then issue cheap
    // handle-based queries for each entry — the handle owns the parsed file
    // so we do not re-parse on every call. Entries are keyed by their 4-byte
    // ASCII tag (interpreted as little-endian int32), matching the lookup key
    // SOUPDEF scripts pass to RTX(stringId) in the original engine.
    public static void LoadRTX(string rtxName)
    {
        RtxEntries.Clear();

        string rootFolder = Game.pathManager.GetRootFolder();
        string path = FFIPathUtils.ResolveFile(rootFolder, rtxName, ".RTX");

        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            Debug.LogWarning("[FFI] RTX file not found: " + rtxName);
            return;
        }

        IntPtr handle = RgpreBindings.OpenRtx(path);
        if (handle == IntPtr.Zero)
        {
            Debug.LogWarning($"[FFI] Failed to open RTX file '{rtxName}': {RgpreBindings.GetLastErrorMessage()}");
            return;
        }

        try
        {
            int count = RgpreBindings.RtxHandleEntryCount(handle);
            int audioLoaded = 0;
            int textLoaded = 0;
            for (int entryIndex = 0; entryIndex < count; entryIndex++)
            {
                int tag = RgpreBindings.RtxHandleEntryTag(handle, entryIndex);
                if (tag == 0)
                {
                    // Tag fetch failed for this entry — skip rather than collide
                    // with a real tag at key 0.
                    continue;
                }

                string subtitle = string.Empty;
                IntPtr subPtr = RgpreBindings.RtxHandleGetSubtitle(handle, entryIndex);
                if (subPtr != IntPtr.Zero)
                {
                    byte[] subBytes = RgpreBindings.ExtractBytesAndFree(subPtr);
                    if (subBytes.Length > 0)
                        subtitle = System.Text.Encoding.UTF8.GetString(subBytes);
                }

                AudioClip clip = null;
                IntPtr wavPtr = RgpreBindings.RtxHandleConvertEntryToWav(handle, entryIndex);
                if (wavPtr != IntPtr.Zero)
                {
                    byte[] wavBytes = RgpreBindings.ExtractBytesAndFree(wavPtr);
                    clip = WavToAudioClip(wavBytes, "RTX_" + entryIndex);
                    audioLoaded++;
                }
                else
                {
                    textLoaded++;
                }

                RtxEntries[tag] = new RTXEntry(subtitle, clip);
            }

            Debug.Log($"[FFI] Loaded {RtxEntries.Count} RTX entries from {rtxName} ({audioLoaded} audio + {textLoaded} text-only)");
        }
        finally
        {
            RgpreBindings.CloseRtx(handle);
        }
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
        if (!TryReadWav(wavBytes, clipName, out int channels, out int sampleRate, out int bitsPerSample, out byte[] pcmData))
        {
            return null;
        }

        float[] samples = bitsPerSample switch
        {
            8 => Pcm8ToFloat(pcmData),
            16 => Pcm16ToFloat(pcmData),
            _ => null
        };

        if (samples == null)
        {
            Debug.LogWarning($"[FFI] {clipName}: unsupported bits-per-sample {bitsPerSample} (only 8/16 supported).");
            return null;
        }

        if (channels <= 0)
        {
            Debug.LogWarning($"[FFI] {clipName}: invalid channel count {channels}.");
            return null;
        }

        int sampleCount = samples.Length / channels;
        if (sampleCount <= 0)
        {
            Debug.LogWarning($"[FFI] {clipName}: empty PCM data.");
            return null;
        }

        AudioClip clip = AudioClip.Create(clipName, sampleCount, channels, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }

    private static bool TryReadWav(byte[] wavBytes, string clipName, out int channels, out int sampleRate, out int bitsPerSample, out byte[] pcmData)
    {
        channels = 0;
        sampleRate = 0;
        bitsPerSample = 0;
        pcmData = null;

        if (wavBytes == null || wavBytes.Length < WavMinHeaderSize)
        {
            Debug.LogWarning($"[FFI] {clipName}: WAV too small ({wavBytes?.Length ?? 0} bytes, need {WavMinHeaderSize}).");
            return false;
        }

        using var stream = new MemoryStream(wavBytes);
        using var reader = new BinaryReader(stream);

        string riff = new string(reader.ReadChars(4));
        reader.ReadInt32();
        string wave = new string(reader.ReadChars(4));
        if (riff != "RIFF" || wave != "WAVE")
        {
            Debug.LogWarning($"[FFI] {clipName}: not a RIFF/WAVE file (got '{riff}'/'{wave}').");
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
                    Debug.LogWarning($"[FFI] {clipName}: unsupported WAV format code {format} (only PCM=1 supported).");
                    return false;
                }
            }
            else if (chunkId == "data")
            {
                pcmData = reader.ReadBytes(chunkSize);
            }

            reader.BaseStream.Position = nextChunk;
        }

        if (channels <= 0 || sampleRate <= 0 || bitsPerSample <= 0 || pcmData == null)
        {
            Debug.LogWarning($"[FFI] {clipName}: incomplete WAV (channels={channels}, rate={sampleRate}, bits={bitsPerSample}, data={(pcmData == null ? "missing" : pcmData.Length + " bytes")}).");
            return false;
        }

        return true;
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
