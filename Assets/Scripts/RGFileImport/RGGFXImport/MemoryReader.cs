using System;
using System.Collections.Generic;
using System.IO;

namespace RGFileImport
{
    // like binaryreader, but reads from memory instead of a file
    // useful for multi-file archives like the ROB files
    public class MemoryReader
    {
        public int Position;
        public int Length;
        byte[] buffer;

        public MemoryReader(byte[] buffer)
        {
            this.buffer = buffer;
            Position = 0;
            Length = buffer.Length;
        }

        public void Seek(uint adr, uint ofs)
        {
            Position = (int)adr+(int)ofs;
        }

        public uint ReadUInt32()
        {
            
            uint o = BitConverter.ToUInt32(buffer, Position);
            Position+=4;
            return o;
        }
        public byte ReadByte()
        {
            byte o = buffer[Position];
            Position+=1;
            return o;
        }
        public char[] ReadChars(int cnt)
        {
            List<char> o = new List<char>();
            for(int i=0;i<cnt;i++)
            {
                o.Add((char)buffer[Position]);
                Position+=1;
            }
            return o.ToArray();
        }
        public byte[] ReadBytes(int cnt)
        {
            List<byte> o = new List<byte>();
            for(int i=0;i<cnt;i++)
            {
                o.Add((byte)buffer[Position]);
                Position+=1;
            }
            return o.ToArray();
        }


        public ushort ReadUInt16()
        {
            ushort o = BitConverter.ToUInt16(buffer, Position);
            Position+=2;
            return o;
        }
        public short ReadInt16()
        {
            short o = BitConverter.ToInt16(buffer, Position);
            Position+=2;
            return o;
        }
        public int ReadInt32()
        {
            int o = BitConverter.ToInt32(buffer, Position);
            Position+=4;
            return o;
        }
        public float ReadSingle() 
        {
            float o = BitConverter.ToSingle(buffer, Position);
            Position+=4;
            return o;
        }


    }
}
