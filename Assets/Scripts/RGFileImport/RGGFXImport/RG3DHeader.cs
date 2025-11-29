namespace RGFileImport
{
    public class RG3DHeader
    {
        public uint Version { get; set; }
		public uint NumVertices { get; set; }
		public uint NumFaces { get; set; }
		public uint Radius { get; set; }
		public uint NumFrames { get; set; }
		public uint OffsetFrameData { get; set; }
		public uint NumUVOffsets { get; set; }
		public uint OffsetSection4 { get; set; }
		public uint Section4Count { get; set; }
		public uint Unknown4 { get; set; } // 0
		public uint OffsetUVOffsets { get; set; }
		public uint OffsetUVData { get; set; }
		public uint OffsetVertexCoords { get; set; }
		public uint OffsetFaceNormals { get; set; }
		public uint NumUVOffsets2 { get; set; }
		public uint OffsetFaceData { get; set; }
        public override string ToString()
        {
            return $@"###################################
3DHeader
###################################
version: {Version}
numVertices: {NumVertices}
numFaces: {NumFaces}
radius: {Radius}
numFrames: {NumFrames}
offsetFrameData: {OffsetFrameData}
numUVOffsets: {NumUVOffsets}
offsetSection4: {OffsetSection4}
Section4Count: {Section4Count}
unknown1: {Unknown4}
offsetUVOffsets: {OffsetUVOffsets}
offsetUVData: {OffsetUVData}
offsetVertexCoords: {OffsetVertexCoords}
offsetFaceNormals: {OffsetFaceNormals}
numUVOffsets2: {NumUVOffsets2}
offsetFaceData: {OffsetFaceData}
###################################";
        }
    }
}
