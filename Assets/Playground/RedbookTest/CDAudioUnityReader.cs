using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
namespace RGU.Kitbash.Gerrick
{
    public enum TrackList
    {
        Track01 = 1,
        Track02 = 2,
        Track03 = 3,
        Track04 = 4,
        Track05 = 5,
        Track06 = 6
    }
    [RequireComponent(typeof(AudioSource))]
    public class CDAudioUnityReader : MonoBehaviour
    {
        [Tooltip("This should be a string address to a valid BIN file (GOG: game.gog, Steam: REDGUARD.bin). \nMake sure to only use single slashes and respect case sensitivity.")]
        public string RGBinPath = @"E:\Games\Redguard\Redguard\REDGUARD.bin"; //This should be set to your Redguard directory and point to either game.gog or REDGUARD.bin.
        
        [Tooltip("This should be a string address to a valid CUE file (GOG: game.ins, Steam: REDGUARD.ins). \nMake sure to only use single slashes and respect case sensitivity.")]
        public string RGCuePath = @"E:\Games\Redguard\Redguard\REDGUARD.ins"; //This should point to the cue file instead-- game.ins or REDGUARD.ins.
        public CDAudio.CDAudioTableOfContents TOC;
        AudioSource asour;
        void Awake()
        {
            TOC = CDAudio.ReadDiscTOC(RGCuePath);
            asour = GetComponent<AudioSource>();

        }
        public void SelectDiscTrackAndPlay(int desiredTrack)
        {
            if (ioViolationRisk) return;
            else
            {
                ioViolationRisk = true;
                selectedTrack = (TrackList)desiredTrack;
                asour.Stop();
                asour.clip = null;
                songtest = null;
                StartCoroutine(ReadPartialDiscData(RGBinPath));
            }
        }
        ///This class must use the CD Audio class to read music into AudioClips. Hardcoding allowed.
        ///Must interpret all audio from Disc 2 of the game (game.gog in GOG release, REDGUARD.bin in Steam)
        ///For now, hardcode to two potential addresses: 
        /// GOG: game-data/game.ins+game-data/game.gog
        /// Steam: game-data/REDGUARD.bin + game-data/REDGUARD.cue
        [SerializeField] AudioClip songtest;
        //[SerializeField] byte[] data;
        //[SerializeField] System.Int16[] samples;
        //[SerializeField] float[] samplefloats;
        [SerializeField] TrackList selectedTrack;
        [Tooltip("This bool enables extra debug logs as well as some safer behaviours. It is no longer necessary to keep this enabled as the default behavior is stable, but enable for extra progress meters. This will increase load times by a few seconds.")]
        [SerializeField] bool enableDebug = true; // Enables some extra logging and safer behaviors; no longer needed as 
        bool ioViolationRisk = false; // a safety check that stops REDGUARD.bin from being accessed when already being streamed
        public void PlayDisc()
        {
            ioViolationRisk = false;
            AudioSource asour = GetComponent<AudioSource>();
            asour.clip = songtest;
            asour.Play();

        }
        //Todo: Improve performance on public async IAsyncEnumerator<AudioClip>
        public IEnumerator ReadPartialDiscData(string rgbinlocation)
        {
            string rgbin = @rgbinlocation;
            AudioClip test;
            FileStream fs = File.Open(rgbin, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            ulong trackbytestart = TOC.trackContents[(int)selectedTrack].byteAddress;
            //if the last track is selected, the final length cannot be trusted. Just go to end of file.
            //Debug.Assert(fs.Length != 0);
            ulong trackbytelength = ((int)selectedTrack + 1 == TOC.lastTrack) switch
            {
                false => TOC.trackContents[(int)selectedTrack].byteLength,
                true => (ulong)fs.Length - TOC.trackContents[(int)selectedTrack].byteAddress
            };
            if (TOC.lastTrack == (int)selectedTrack) Debug.LogWarning("Last track on list!");
            //Debug.Assert(fs.CanSeek);
            fs.Seek((long)trackbytestart, SeekOrigin.Begin);
            long tracksamplelength = (long)trackbytelength / 2;
            //short[] samples = new short[tracksamplelength];
            float[] samplefloats = new float[tracksamplelength];
            for (int i = 0; i < (long)tracksamplelength; i++)
            {
                if (enableDebug)
                {
                    if (fs.Position == fs.Length)
                    {

                        Debug.LogError($"At {i} the file stream ended. (track length was {trackbytelength}) Ending audioclip here!");
                        break;
                    }
                }
                //fs.Seek((long)trackbytestart + (long)i * 2, SeekOrigin.Begin);
                short sample = br.ReadInt16();
                samplefloats[i] = (sample > 0) switch
                {
                    true => (float)(sample * 0.00003051850947599719f), // for dividing by 32767.0f;
                    false => (float)(sample * 0.000030517578125f)//for dividing by -32768.0f;
                };
                if (i == (int)tracksamplelength * .25f || i == (int)tracksamplelength * .50f || i == (int)tracksamplelength * .25f || i == (int)tracksamplelength * .75f)
                {
                    if (enableDebug)
                        Debug.Log($"Reading data - {2 * (float)i * 100 / (float)trackbytelength}%");
                    yield return null;
                }
                /* Originally the remapping phase was its own loop; leaving for now in case it's needed
                for (int i = 0; i < tracksamplelength; i++)
                {
                    samplefloats[i] = (samples[i] > 0) switch
                    {
                        true => (float)(samples[i] * 0.00003051850947599719f), // for dividing by 32767.0f;
                        false => (float)(samples[i] * 0.000030517578125f)//for dividing by -32768.0f;
                    };
                    if (i % 60000 == 0)
                    {
                        //Debug.Log(i);
                        if (enableDebug)
                            Debug.Log($"Remapping audio - {(float)i / (float)samples.Length * 100}%");
                        yield return null;

                    }
                */
                //samplefloats[i] = remap(samples[i], Int16.MinValue, Int16.MaxValue, -1.0f, 1.0f);
            }
            test = AudioClip.Create($"Song {(int)selectedTrack}", (int)tracksamplelength, 2, 44100, false/*true,OnRedguardMusicPlay,OnRGMusicSetPosition*/);
            test.SetData(samplefloats, 0);
            songtest = test;
            br.Close();
            fs.Close();
            PlayDisc();


        }
        void OnRedguardMusicPlay(float[] data)
        {

        }
        void OnRGMusicSetPosition(int newPosition)
        {
            //https://docs.unity3d.com/ScriptReference/AudioClip.PCMReaderCallback.html
        }
        public static float remap(float val, float in1, float in2, float out1, float out2)
        {
            return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
        }
    }
}