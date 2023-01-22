public class SmackerHeader
{
    //All info courtesy of https://wiki.multimedia.cx/index.php?title=Smacker
    uint Signature; // SMK2 or SMK4
    uint Width {get; set;}
    uint Height{get; set;}
    uint Frames{get;set;} //Extra "ring" frame is not counted
    float FrameRate{get;set;} //fps = 1000/Mathf.Abs(FrameRate) or if 0, fps = 10
    uint Flags{get;set;} // 0
    uint[] AudioSize{get;set;} //Size of largest buffer (bytes) per audio track (up to 8)
    uint TreesSize{get;set;} //Size of bytes in huffman trees
    uint MMap_Size{get;set;} //Huffman data
    uint MClr_Size{get;set;}//huffman data
    uint Full_Size{get;set;}//huffman data
    uint Type_Size{get;set;}//huffman data
    uint[] AudioRate{get;set;} 
    uint Dummy{get;set;}
}
