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

        public static RGColor[] Load(string path)
        {
            var colors = new RGColor[256];
            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                var fileSize = binaryReader.ReadUInt32();
                var unknown = binaryReader.ReadUInt32();
                const int readSize = 256 * 3;
                var numEntries = fileSize - 8;
                if (numEntries < readSize)
                    return null;
                var readBuffer = new byte[3];
                for (int i = 0; i < 256; i++)
                {
                    if (binaryReader.Read(readBuffer, 0, 3) < 3)
                        return null;
                    colors[i] = new RGColor { r = readBuffer[0], g = readBuffer[1], b = readBuffer[2] };
                }
            }

            return colors;
        }
    }
}
