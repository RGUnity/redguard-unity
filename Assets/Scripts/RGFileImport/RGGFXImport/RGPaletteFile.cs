using System;
using System.IO;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class RGPaletteFile
    {
        public struct RGColor
        {
            public byte r;
            public byte g;
            public byte b;
        };
        public RGColor[] colors;
		public void LoadFile(string filename)
        {
            try
            {
                colors = new RGColor[256];
                using (var binaryReader = new BinaryReader(File.OpenRead(filename)))
                {
                    var fileSize = binaryReader.ReadUInt32();
                    var unknown = binaryReader.ReadUInt32();
                    var numEntries = fileSize - 8;
                    var readBuffer = new byte[3];
                    for (int i = 0; i < 256; i++)
                    {
                        binaryReader.Read(readBuffer, 0, 3);
                        colors[i] = new RGColor { r = readBuffer[0], g = readBuffer[1], b = readBuffer[2] };
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to load COL file {filename} with error:\n{ex.Message}");
            }
        }
    }
}
