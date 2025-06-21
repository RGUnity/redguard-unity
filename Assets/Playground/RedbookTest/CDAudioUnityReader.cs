using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.ComponentModel.Composition;
using System.Net.NetworkInformation;
using System.Collections;
using UnityEngine.UIElements;
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
    public class CDAudioUnityReader : MonoBehaviour
    {
        public CDAudio.CDAudioTableOfContents TOC;
        void Awake()
        {
            TOC = CDAudio.ReadDiscTOC();
            
        }
        public void SelectDiscTrackAndPlay(int desiredTrack)
        {
            if (ioViolationRisk) return;
            else
            {
                ioViolationRisk = true;
                selectedTrack = (TrackList)desiredTrack;
                AudioSource asour = GetComponent<AudioSource>();
                asour.Stop();
                asour.clip = null;
                songtest = null;
                StartCoroutine(ReadPartialDiscData());
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
        bool enableDebug = true;
        bool ioViolationRisk = false; // a safety check that stops REDGUARD.bin from being accessed when already being streamed
        public void PlayDisc()
        {
            ioViolationRisk = false;
            AudioSource asour = GetComponent<AudioSource>();
            asour.clip = songtest;
            asour.Play();

        }
        public IEnumerator ReadPartialDiscData()
        {
            string rgbin = @"E:\Games\Redguard\Redguard\REDGUARD.bin";
            AudioClip test;
            FileStream fs = File.Open(rgbin, FileMode.Open,FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            ulong trackbytestart = TOC.trackContents[(int)selectedTrack].byteAddress;
            //if the last track is selected, the final length cannot be trusted. Just go to end of file.
            //Debug.Assert(fs.Length != 0);
            ulong trackbytelength = ((int)selectedTrack == TOC.lastTrack) switch
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
                if (i % 60000 == 0)
                {
                    if (enableDebug)
                        Debug.Log($"Reading data - {2 * (float)i * 100 / (float)trackbytelength}%");
                    yield return null;
                }
            /*
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
        public IEnumerator ReadDiscData()
        {
            const int bytesPerFrame = 24;
            const int sectorLength = 2352; //Some sources saying cdroms are 2048
            string rgbin = @"E:\Games\Redguard\Redguard\REDGUARD.bin";
            AudioClip test;
            //ulong trackaddress = TOC.trackContents[(int)selectedTrack].frameAddress;
            //ulong tracklength = TOC.trackContents[(int)selectedTrack].frameLength;

            byte[] data = File.ReadAllBytes(rgbin);

            ulong trackbytestart = TOC.trackContents[(int)selectedTrack].byteAddress;
            //if the last track is selected, the final length cannot be trusted. Just go to end of file.
            ulong trackbytelength = ((int)selectedTrack == TOC.lastTrack) switch
            {
                false => TOC.trackContents[(int)selectedTrack].byteLength,
                true => (ulong)data.Length - TOC.trackContents[(int)selectedTrack].byteAddress
            };
            //int trackbytestart = (int)trackaddress * bytesPerFrame;
            //int trackbytelength = (int)tracklength * bytesPerFrame;
            Debug.Log($"This is the num you want: {data.Length / 24}");
            byte[] datacrop = new byte[trackbytelength];
            Int16[] samples = new short[trackbytelength / 2];
            Array.Copy(data, (int)trackbytestart, datacrop, 0, (long)trackbytelength);
            //Buffer.BlockCopy(data, (int)trackbytestart, samples, 0, (int)trackbytelength);
            //Debug.Log($"Buffer random sample: {datacrop[400000]}");
            Debug.Log(data.Length);
            data = new byte[0];
            //for (int s = 0; s < (int)trackbytelength; s++)
            //{
            for (long i = 0; i < (long)trackbytelength; i++)
            {
                ulong databytemarker = (ulong)/*s * sectorLength*/ +(ulong)i * 2;//+ trackbytestart;
                ulong sampleMarker = (ulong)/*s * sectorLength*/ +(ulong)i;
                if ((int)sampleMarker >= samples.Length) break;
                //Debug.Log(i + s * 2048);
                samples[sampleMarker] = BitConverter.ToInt16(datacrop, (int)databytemarker);
                //samples[sampleMarker] = (short)((datacrop[(int)databytemarker] << 8) | datacrop[(int)databytemarker + 1]);
                //if needed, make a two byte index first
                //samples[i] = BitConverter.ToInt16(nextint);
                if (i % 125000 == 0) yield return null;
            }
            //  if (s % 2500 == 0)
            //{
            //yield return null;
            //  Debug.Log($"Sector {s}");
            //}
            //}

            test = AudioClip.Create($"Song {(int)selectedTrack}", samples.Length/* (int)TOC.trackContents[(int)selectedTrack].length*/, 2, 44100, false);
            float[] samplefloats = new float[samples.Length];
            for (int f = 0; f < samplefloats.Length; f++)
            {
                //samplefloats[f]
                samplefloats[f] = (samples[f] > 0) switch
                {
                    true => (float)((double)samples[f] * 0.00003051850947599719), // for dividing by 32767.0f;
                    false => (float)((double)samples[f] * 0.000030517578125)//for dividing by -32768.0f;
                                                                            //samplefloats[i] = samplefloats[i] / /*32768.0f*/ sizeof(Int16);
                };
            }
            test.SetData(samplefloats, 0);
            songtest = test;
            PlayDisc();
        }
            //using (FileStream str = File.OpenRead(rgbin))



            /*
            FileStream fs = File.Open(rgbin,FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            Int16[] samples = new Int16[tracklength * 1024]; 
            for (int i = 0; i < (int)tracklength*1024; i++)
            {
                int b = 0;
                br.BaseStream.Position = i*2+b;
                Int16 nb = br.ReadInt16();
                samples[i] = nb;
            }
            */

            //Debug.Log($"{trackaddress} {tracklength} {tracklength/4}");
            /*
                {
                    //byte[] str = File.ReadAllBytes(rgbin);

                    //str.Position = (long)trackaddress;
                    //System.IO.BinaryReader br = new BinaryReader(str);
                    //Should pull assuming 2048bytes per sector in a CDROM.
                    //Calc start and end sectors count, and multiply read of bytes by 2048
                    //data = br.ReadBytes((int)TOC.trackContents[(int)selectedTrack].length); // grab the specific byte range of the song
                    byte[] data = new byte[tracklength];
                    Array.Copy(str, (int)trackaddress, data,0,(int)tracklength);
                    //samples = new System.Int16[data.Length / 2];
                    //Buffer.BlockCopy(data, 0, samples, 0, data.Length);
                    samples = new System.Int16[(int)tracklength];
                    Debug.Log(samples.Length);
                    int b = 0;
                    for (int i = 0; i < samples.Length; i++)
                    {

                        samples[i] = BitConverter.ToInt16(data,b);
                        b += 2;
                    }
                    // AudioClip.PCMReaderCallback pcm = new AudioClip.PCMReaderCallback()
                    */
                //samplefloats = Array.ConvertAll<System.Int16, float>(samplefloats),Convert.;

                //br.Close();
                //fs.Close();
            //}
            //    song1test.SetData(br.ReadBytes(3), 0);
        //}
        //public void OnAudioFilterRead(float[] data, int channels)
        //{
         //   float[] audioData;
        //}
    }
}