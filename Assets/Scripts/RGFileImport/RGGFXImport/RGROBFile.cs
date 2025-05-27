using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RGFileImport
{
	public class RGROBFile
	{
		public struct ROBHeader
		{
			public char[] OARC;
            public uint Unknown4;
            public uint NumSegments;
            public uint OARD;
            public uint UnknownId;

			public ROBHeader(MemoryReader memoryReader)
            {
                try
                {
                    OARC = memoryReader.ReadChars(4);
                    Unknown4 = memoryReader.ReadUInt32();
                    NumSegments = memoryReader.ReadUInt32();
                    OARD = memoryReader.ReadUInt32();
                    UnknownId = memoryReader.ReadUInt32();
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load ROB header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"###################################
ROBHeader
###################################
OARC: {new string(OARC)}
Unknown4: {Unknown4:X}
NumSegments: {NumSegments:X}
OARD: {OARD:X}
UnknownId: {UnknownId:X}
###################################";
			}
		}
		public struct ROBSegmentHeader
		{
            public uint Unknown0;
            public string SegmentID; // read as char[]
            public uint UnknownType;
            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint UnknownInt1;
            public uint UnknownInt2;
            public uint UnknownInt3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint UnknownInt4;
            public uint UnknownInt5;
            public uint UnknownInt6;
            public uint UnknownInt7;
            public uint UnknownInt8;
            public uint UnknownInt9;
            public uint Size;
            public byte[] Data;

            public ROBSegmentHeader(MemoryReader memoryReader)
            {
                try
                {
                    char[] SegmentID_char;
                    Unknown0 = memoryReader.ReadUInt32();
                    SegmentID_char = memoryReader.ReadChars(8);
                    string[] segment_strs = new string(SegmentID_char).Split('\0');
                    SegmentID = segment_strs[0];

                    UnknownType = memoryReader.ReadUInt32();
                    Unknown1 = memoryReader.ReadUInt32();
                    Unknown2 = memoryReader.ReadUInt32();
                    Unknown3 = memoryReader.ReadUInt32();
                    UnknownInt1 = memoryReader.ReadUInt32();
                    UnknownInt2 = memoryReader.ReadUInt32();
                    UnknownInt3 = memoryReader.ReadUInt32();
                    Unknown4 = memoryReader.ReadUInt32();
                    Unknown5 = memoryReader.ReadUInt32();
                    Unknown6 = memoryReader.ReadUInt32();
                    UnknownInt4 = memoryReader.ReadUInt32();
                    UnknownInt5 = memoryReader.ReadUInt32();
                    UnknownInt6 = memoryReader.ReadUInt32();
                    UnknownInt7 = memoryReader.ReadUInt32();
                    UnknownInt8 = memoryReader.ReadUInt32();
                    UnknownInt9 = memoryReader.ReadUInt32();
                    Size = memoryReader.ReadUInt32();
                    Data = memoryReader.ReadBytes((int)Size);
                }
                catch(Exception ex)
                {
                    throw new Exception($"Failed to load ROB segment header with error:\n{ex.Message}");
                }
            }
			public override string ToString()
			{
				return $@"SegmentId: {Unknown0},{SegmentID},{UnknownType},{Unknown1},{Unknown2},{Unknown3},{UnknownInt1},{UnknownInt2},{UnknownInt3},{Unknown4},{Unknown5},{Unknown6},{UnknownInt4},{UnknownInt5},{UnknownInt6},{UnknownInt7},{UnknownInt8},{UnknownInt9}, {Size}";
			}

		}


	// data
		public ROBHeader hdr;
        public ROBSegmentHeader[] segments;
        public long fileSize;

		public void LoadFile(string filename)
        {
            try
            {
                byte[] buffer;
                BinaryReader binaryReader = new BinaryReader(File.OpenRead(filename));
                fileSize = binaryReader.BaseStream.Length;
                buffer = binaryReader.ReadBytes((int)fileSize);
                binaryReader.Close();
                LoadMemory(buffer);
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load ROB file {filename} with error:\n{ex.Message}");
            }
        }
		public void LoadMemory(byte[] buffer)
        {
            try
            {
                MemoryReader memoryReader = new MemoryReader(buffer);
                hdr = new ROBHeader(memoryReader);

                segments = new ROBSegmentHeader[hdr.NumSegments];
                const int rob_seg_hdr_size = 80;
                int ptr = 0;
                for(int i=0;i<hdr.NumSegments;i++)
                {
                    ptr = memoryReader.Position + rob_seg_hdr_size;
                    segments[i] = new ROBSegmentHeader(memoryReader);
                    memoryReader.Seek(segments[i].Size, (uint)ptr);
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load ROB file from memory with error:\n{ex.Message}");
            }
        }

		public void PrintROB()
		{
			Console.WriteLine(hdr);
            for(int i=0;i<hdr.NumSegments;i++)
            {
                Console.WriteLine(segments[i]);
            }
		}
	}
}
