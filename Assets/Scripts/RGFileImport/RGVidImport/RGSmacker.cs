using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum RGVideos
{
    A_ruins1,
    A_ruins2,
    B_ruins1,
    B_ruins2,
    C_ruins1,
    C_ruins2,
    Intro,
    SCENE3,
    Scene7,
    Scene10,
    WIN
}
public class RGSmacker
{
    ///<summary>
    ///Request data from a SMACKER file.
    ///</summary>
    //Data format is property of RAD Game Tools.
    //Created with assistance from https://wiki.multimedia.cx/index.php?title=Smacker
    //Current status: barely started

    //Note: Because of Redguard's formatting (use of a Bin/Cue file in digital releases), the original Play Disc or mounted BIN/CUE is required at this moment. This is not ideal.
 
    //Array of 4x4 pixels with set palettes
    //Convert to bytes then convert to colors then output to Unity
    string filepath = "D:" + System.IO.Path.DirectorySeparatorChar;

    int[] FrameSizes;

    SmackerHeader dataHeader;
    BinaryReader binaryReader;



    void LoadFile()
    {
        if(File.Exists(filepath))
        {
            binaryReader = new BinaryReader(File.OpenRead(filepath));
            dataHeader = GetHeader(binaryReader);
        }
        else throw new System.Exception("The file "+filepath+ " could not be found. The video was not loaded.");
        dataHeader = GetHeader(binaryReader);
        ReadFrameSizes();
        ReadFrameTypes();
        GetHuffmanTrees();
        GetFramesData();
    }


    public struct PaletteChunk
    {
        byte Length;
        byte[] Blocks;
    }
    public struct AudioTrackChunk
    {
        uint Length;
        uint UnpackedLength; // only needed if the file is compressed
        byte[] Data;
    }

    float GetFramerate()
    {
        float fps = 10;
        if(dataHeader.FrameRate != 0)
            fps = 1000f / Mathf.Abs(dataHeader.FrameRate);
        return fps;
    }

    private SmackerHeader GetHeader(BinaryReader binaryReader)

    {
        var newHeader = new SmackerHeader
        {
            Signature = binaryReader.ReadUInt32(), // SMK2 or SMK4
            Width = binaryReader.ReadUInt32(), //most videos will return 640
            Height = binaryReader.ReadUInt32(), //most videos will return 240
            Frames = binaryReader.ReadUInt32(), //Extra "ring" frame is not counted; Intro is about 6895 frames
            FrameRate = binaryReader.ReadUInt32(), //fps = 1000/Mathf.Abs(FrameRate) or if 0, fps = 10
            Flags = binaryReader.ReadUInt32(),// 0
            AudioSize = new uint[7]{binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32()},//Size of largest buffer (bytes) per audio track (up to 8)... this isn't how it works and needs to be fixed
            TreesSize = binaryReader.ReadUInt32(),//Size of bytes in huffman trees
            MMap_Size = binaryReader.ReadUInt32(),//Huffman data
            MClr_Size = binaryReader.ReadUInt32(),//huffman data
            Full_Size = binaryReader.ReadUInt32(),//huffman data
            Type_Size = binaryReader.ReadUInt32(),//huffman data
            AudioRate = new uint[7]{binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32(),binaryReader.ReadUInt32()},
            Dummy = binaryReader.ReadUInt32(),
        };

        return (SmackerHeader)newHeader;
    }

    private void ReadFrameSizes()
    {
        //should be done after reading Header
        FrameSizes = new int[dataHeader.Frames];
        for (int i = 0; i < dataHeader.Frames; i++)
        {
            FrameSizes[i] = (int)binaryReader.ReadUInt32();
        }
    }
    int[] FrameTypes;
    private void ReadFrameTypes()
    {
        for (int i = 0; i < dataHeader.Frames;i++)
        {
            FrameTypes[i] = (int)binaryReader.ReadByte();

        //do after reading framesizes
        //if bit 0 != 0, palette record is included
        //if bit 1 != 0, audio data is included for track 0
        //other six bits are unused for these videos because there's only one audio track included
            
        }
    }

    private void GetHuffmanTrees()
    {
        //Includes tree data for the four decoding tables listed in the header
         
    }

    private void GetFramesData()
    {

    }
    int version;

    void GetSMKVersion()
    {
        version = dataHeader.Signature switch
        {
            0x534D4B32 => 2, //SMK2
            0x534D4B34 => 4, //SMK4
            _ => 0 // null, will never happen in a valid file
            
        };
    }
    
    public struct HuffmanFlag
    {
        public uint Tag;
        public byte Flag;
    }
    void UnpackHuffman(HuffmanFlag Next)
    {
//          1: Read Tag
//          2: If Tag is zero, finish
        if(Next.Tag==0) return;
//          3: Read Flag
//          4a: If Flag is non-zero:
        if(Next.Flag!=0)
        {
//          5a: Remember current tree node
//          5b: Advance to its '0' branch
//          5c: Repeat recursively from step 3 (one level down)

        }

//          4b: If flag is zero:
        else
        {
//          5a: Read Leaf from stream
//          5b: Assign Leaf value to current node (convert it to leaf)
        }
//          6: If no node previously remembered, finish (one level up)
//          7: Use node's '1' branch from step 5a as current tree position
//          8: Repeat from step 3



    }


    
}
