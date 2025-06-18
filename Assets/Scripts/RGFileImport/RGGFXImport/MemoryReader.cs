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
            if(Position > Length)
                throw new Exception($"MemoryReader: seek address 0x{Position:X} is greater than buffer length 0x{Length:X}");
        }

        public uint ReadUInt32()
        {
            
            if(Position+4 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {4} is greater than buffer length 0x{Length:X}");
            uint o = BitConverter.ToUInt32(buffer, Position);
            Position+=4;
            return o;
        }
        public int ReadInt24()
        {
            
            if(Position+3 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {4} is greater than buffer length 0x{Length:X}");

            List<byte> b = new List<byte>();
            b.Add((byte)0x00);
            for(int i=0;i<3;i++)
            {
                b.Add((byte)buffer[Position]);
                Position+=1;
            }
            int o = BitConverter.ToInt32(b.ToArray(), 0);
            return o;
        }

        public byte ReadByte()
        {
            if(Position+1 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {1} is greater than buffer length 0x{Length:X}");
            byte o = buffer[Position];
            Position+=1;
            return o;
        }
        public char[] ReadChars(int cnt)
        {
            if(Position+cnt > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {cnt} (0x{cnt:X}) is greater than buffer length 0x{Length:X}");
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
            if(Position+cnt > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {cnt} (0x{cnt:X}) is greater than buffer length 0x{Length:X}");
            List<byte> o = new List<byte>();
            for(int i=0;i<cnt;i++)
            {
                o.Add((byte)buffer[Position]);
                Position+=1;
            }
            return o.ToArray();
        }
        public int[] ReadInt32s(int cnt)
        {
            if(Position+(cnt*4) > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {4}*{cnt} (0x{4*cnt}) is greater than buffer length 0x{Length:X}");
            List<int> o = new List<int>();
            for(int i=0;i<cnt;i++)
            {
                o.Add((int)buffer[Position]);
                Position+=4;
            }
            return o.ToArray();
        }
        public short[] ReadInt16s(int cnt)
        {
            if(Position+(cnt*2) > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {2}*{cnt} (0x{2*cnt}) is greater than buffer length 0x{Length:X}");
            List<short> o = new List<short>();
            for(int i=0;i<cnt;i++)
            {
                o.Add((short)buffer[Position]);
                Position+=2;
            }
            return o.ToArray();
        }

        public ushort ReadUInt16()
        {
            if(Position+2 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {2} is greater than buffer length 0x{Length:X}");
            ushort o = BitConverter.ToUInt16(buffer, Position);
            Position+=2;
            return o;
        }
        public short ReadInt16()
        {
            if(Position+2 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {2} is greater than buffer length 0x{Length:X}");
            short o = BitConverter.ToInt16(buffer, Position);
            Position+=2;
            return o;
        }
        public int ReadInt32()
        {
            if(Position+4 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {4} is greater than buffer length 0x{Length:X}");
            int o = BitConverter.ToInt32(buffer, Position);
            Position+=4;
            return o;
        }
        public float ReadSingle() 
        {
            if(Position+4 > Length)
                throw new Exception($"MemoryReader: read address 0x{Position:X} + {4} is greater than buffer length 0x{Length:X}");
            float o = BitConverter.ToSingle(buffer, Position);
            Position+=4;
            return o;
        }

        public short PeekInt16()
        {
            short o = ReadInt16();
            Position-=2;
            return o;
        }

        public static uint ReverseBytes(uint n)
        {
            var bytes = BitConverter.GetBytes(n);
            Array.Reverse(bytes, 0, bytes.Length);
            return BitConverter.ToUInt32(bytes, 0);
        }

    }
}
