using System;
using System.IO;

namespace RGFileImport
{
    public sealed class MemoryReader
    {
        private readonly byte[] data;
        private int position;

        public MemoryReader(byte[] source)
        {
            data = source ?? Array.Empty<byte>();
            position = 0;
        }

        public int Position
        {
            get => position;
            set => position = Math.Clamp(value, 0, data.Length);
        }

        public void Seek(uint offset, int origin)
        {
            int basePos = origin switch
            {
                1 => position,
                2 => data.Length,
                _ => 0
            };

            Position = basePos + unchecked((int)offset);
        }

        public byte ReadByte()
        {
            EnsureAvailable(1);
            return data[position++];
        }

        public byte[] ReadBytes(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            EnsureAvailable(count);
            byte[] output = new byte[count];
            Buffer.BlockCopy(data, position, output, 0, count);
            position += count;
            return output;
        }

        public char[] ReadChars(int count)
        {
            byte[] bytes = ReadBytes(count);
            char[] chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = (char)bytes[i];
            }

            return chars;
        }

        public short ReadInt16()
        {
            EnsureAvailable(2);
            short value = BitConverter.ToInt16(data, position);
            position += 2;
            return value;
        }

        public ushort ReadUInt16()
        {
            EnsureAvailable(2);
            ushort value = BitConverter.ToUInt16(data, position);
            position += 2;
            return value;
        }

        public int ReadInt32()
        {
            EnsureAvailable(4);
            int value = BitConverter.ToInt32(data, position);
            position += 4;
            return value;
        }

        public short PeekInt16()
        {
            EnsureAvailable(2);
            return BitConverter.ToInt16(data, position);
        }

        private void EnsureAvailable(int count)
        {
            if (position < 0 || position + count > data.Length)
            {
                throw new EndOfStreamException("Attempted to read past end of MemoryReader data.");
            }
        }
    }
}
