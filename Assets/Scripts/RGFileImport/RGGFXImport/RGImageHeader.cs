namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class RGImageHeader
    {
        public ushort XOffset { get; set; }
        public ushort YOffset { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public byte Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public ushort Zero1 { get; set; }
        public ushort Zero2 { get; set; }
        public ushort FrameCount { get; set; }
        public ushort Unknown3 { get; set; }
        public ushort Zero3 { get; set; }
        public ushort Zero4 { get; set; }
        public byte Unknown4 { get; set; }
        public byte Unknown5 { get; set; }
        public ushort Unknown6 { get; set; }
    }
}
