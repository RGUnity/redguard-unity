using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RGU.Kitbash.Gerrick
{
    //https://www.codeproject.com/Articles/15725/Tutorial-on-reading-Audio-CDs has been instrumental (harhar) in this reading tool
    public class CDAudio
    {
        ///This class must define a CD Audio file format so that the disc can be directly imported. Hardcoding allowed.
        ///Must interpret all audio from Disc 2 of the game (game.gog in GOG release, REDGUARD.bin in Steam)
        /// Audio is 16 bit PCM Little Endian
        /// Frequencies up to 22050
        /// 44100 samples per second per channel, 8810 bits per second
        /// Each sample is a signed 16 bit integer

        #region Calc utils
        /// <summary>
        /// Converts a timestamp (in byte-array format) to the sector address on disc.
        /// </summary>
        /// <param name="Addr"></param>
        /// <returns>Timestamp address (4 bytes). Byte 0 is hours (unused), 1 is minutes, 2 is seconds.</returns>
        /// <exception cref="FormatException">Thrown if the byte-array is not a length of four.</exception>
        /*
                static ulong AddressToSectors(byte[] Addr) 
                {
                    if (Addr.Length != 4) throw new FormatException();

                    ulong sectors = (ulong)(Addr[1] * 75 * 60 + Addr[2] * 75 + Addr[3]);
                    return sectors - 150;
                }
        */
        /// <summary>
        /// The codeproject sample wants to do this with byte arrays. This will instead use the formatting from the cue sheet.
        /// </summary>
        /// <param name="sec"></param>
        /// <param name="min"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        static ulong GetFrameCount(int min, int sec, int frame)
        {
            ulong frames = (ulong)(min * 75 * 60 + sec * 75 + frame);
            return frames;
        }
        static ulong FramesCounter(int min, int sec, int frame)
        {
            ulong frames = (ulong)(min * 75 * 60 + sec * 75 + frame);
            return frames;
        }
        static ulong FramesToBytes(int frames)
        {
            return (ulong)frames;//todo
        }
        static ulong SectorsToBytes(int sectors)
        {
            return (ulong)sectors * 2352;
        }
        #endregion
        #region Structs
        [System.Serializable]
        public struct CDAudioTableOfContents
        {
            public byte[] length; //should be two bytes
            public byte firstTrack;
            public byte lastTrack;
            public CDAudioTrack[] trackContents; //should be 100 length max, rarely ever reaches that
                                             //contains the TOC info

        }
        //todo: TrackData and Track can probably be one struct in future optimization

        [System.Serializable]
        public struct CDAudioTrack
        {
            public ulong frameAddress;
            public ulong frameLength;
            public ulong byteAddress => frameAddress * 2352;
            public ulong byteLength => frameLength * 2352;
            /*
            Frames/second = 75
            Bytes/frame = 24
            Bytes/second = 
            Sector = 2048 bytes
Second = 176,400 bytes*/
            public void Register(ulong addr, ulong len)
            {
                frameAddress = addr;
                frameLength = len;
                return;
            }
        }
        #endregion
        /// <summary>
        /// Call to read a Table of Contents from a cue sheet and store it into memory. Interprets the frame data to sectors for each track.
        /// This is not meant to be a flexible CD reader, but rather specifically set up for ease of use within the Redguard project. 
        /// For other projects, just make another system and make it better.
        /// </summary>
        /// <param name="filepath">A string pointing to the location of a CUE sheet (game.ins, REDGUARD.ins).</param>
        /// <returns>Returns a CDTOC object.</returns>
        public static CDAudioTableOfContents ReadDiscTOC(string filepath /*Temporary, will use abstracted filepath system later*/)
        {
            CDAudioTableOfContents cdtoc = new CDAudioTableOfContents();
            Debug.Assert(File.Exists(@filepath));
            //byte[] rgcue = System.IO.File.ReadAllBytes(@"E:\Games\Redguard\Redguard\REDGUARD.ins");
            string[] rgtextlines = System.IO.File.ReadAllLines(@filepath);
            //string rgtextcue = System.Convert.ToBase64String(rgcue);
            //split by 0d 0a
            char[] carriagereturn = { (char)0x0d, (char)0x0a };
            //string[] rgtextlines = rgtextcue.Split((char)0x0d);
            //skipping the first line
            Debug.Log(rgtextlines[2]);
            cdtoc.firstTrack = 1;
            cdtoc.lastTrack = (byte)((rgtextlines.Length - 1) / 2);
            cdtoc.trackContents = new CDAudioTrack[cdtoc.lastTrack];
            for (int i = cdtoc.firstTrack; i < cdtoc.lastTrack; i++)
            {
                ///Input parsing cue sheet
                //Includes track num, data type
                //Formatted:    TRACK xx FORMAT
                //rgtextlines[i]      = rgtextlines[i].Remove((char)0x0a).TrimStart((char)0x20);
                //Includes Index num, timestamp
                //Formatted:    INDEX xx mm:ss:ff
                //rgtextlines[i + 1]  = rgtextlines[i + 1].Remove((char)0x0a).TrimStart((char)0x20);
                int l = i * 2 + 1;
                const string tracknumregex = @"TRACK [(0-9)]\w";
                rgtextlines[l] = Regex.Match(rgtextlines[l], tracknumregex).Value;
                const string timestampregex = @"([0-9])\w+:+([0-9])\w+:+([0-9])\w";
                string[] rgtimestamp = Regex.Match(rgtextlines[l + 1], timestampregex).Value.Split(':');
                int trackmins = Convert.ToInt32(rgtimestamp[0]);
                int tracksecs = Convert.ToInt32(rgtimestamp[1]);
                int trackframes = Convert.ToInt32(rgtimestamp[2]);

                /// Register to TOC of cd
                CDAudioTrack rgtrack = new CDAudioTrack();
                ulong addr = GetFrameCount(trackmins, tracksecs, trackframes);
                //len should be calculated in a separate loop, after all track times are registered and known
                rgtrack.frameAddress = addr;
                //rgtrack.Register(addr, len);
                cdtoc.trackContents[i] = rgtrack;
                //todo: register new track with data pulled from cue, then feed to TOC
            }
            for (int i = 0; i < cdtoc.trackContents.Length; i++)
            {
                Debug.Log($"{i} {cdtoc.lastTrack} {cdtoc.trackContents.Length}");


                ulong len = (i >= cdtoc.lastTrack - cdtoc.firstTrack) switch
                {
                    true => /*return end of sectors with magic 70min marker*//*30264764*/ 4180,
                    false => /*standard logic*/ cdtoc.trackContents[i + 1].frameAddress - cdtoc.trackContents[i].frameAddress,
                };
                cdtoc.trackContents[i].frameLength = len;
            }


            return cdtoc;
        }
        
        
    }
}