namespace RGFileImport.VideoPlayback
{
public class SmackerHeader
{
    //All info courtesy of https://wiki.multimedia.cx/index.php?title=Smacker
    public uint Signature {get;set;} // SMK2 or SMK4
    public uint Width {get; set;}
    public uint Height{get; set;}
    public uint Frames{get;set;} //Extra "ring" frame is not counted
    public int FrameRate{get;set;} //fps = 1000/Mathf.Abs(FrameRate) or if 0, fps = 10
    public uint Flags{get;set;} // 0
    public uint[] AudioSize{get;set;} //Size of largest buffer (bytes) per audio track (up to 8)
    public uint TreesSize{get;set;} //Size of bytes in huffman trees
    public uint MMap_Size{get;set;} //Huffman data
    public uint MClr_Size{get;set;}//huffman data
    public uint Full_Size{get;set;}//huffman data
    public uint Type_Size{get;set;}//huffman data
    public uint[] AudioRate{get;set;} 
    public uint Dummy{get;set;}
}
}