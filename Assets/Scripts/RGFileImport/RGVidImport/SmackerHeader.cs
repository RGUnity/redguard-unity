public class SmackerHeader
{
    uint Signature;
    uint Width {get; set;}
    uint Height{get; set;}
    uint Frames{get;set;}
    float FrameRate{get;set;}
    uint Flags{get;set;}
    uint[] AudioSize{get;set;}
    uint TreesSize{get;set;}
    uint MMap_Size{get;set;}
    uint MClr_Size{get;set;}
    uint Full_Size{get;set;}
    uint Type_Size{get;set;}
    uint[] AudioRate{get;set;}
    uint Dummy{get;set;}
}
