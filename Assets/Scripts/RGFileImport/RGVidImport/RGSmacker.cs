using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGSmacker
{
    //Request data from a SMACKER file.
    //Data format is property of RAD Game Tools.
    //Created with assistance from https://wiki.multimedia.cx/index.php?title=Smacker

    //Note: Because of Redguard's formatting (use of a Bin/Cue file in digital releases), a CD ROM or mounted BIN/CUE will be mandatory.
 
    //Array of 4x4 pixels with set palettes
    //Convert to bytes then convert to colors then output to Unity

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

    void GetSMKInfo()
    {

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
