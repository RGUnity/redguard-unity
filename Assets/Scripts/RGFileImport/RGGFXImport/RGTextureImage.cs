using System.Collections.Generic;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class RGTextureImage
    {
        public RGTextureImage()
        {
            ImageData = new List<byte[]>();
        }

        public string Name { get; set; }
        public uint Size { get; set; }
        public RGImageHeader Header { get; set; }
        public RGPaletteFile.RGColor[] Palette { get; set; }
        public bool HasBSIFData { get; set; }
        public byte[] BSIFData { get; set; }
        public bool HasIFHDData { get; set; }
        public byte[] IFHDData { get; set; }
        // Each list item is a frame for animated images.
        public List<byte[]> ImageData { get; set; }
    }
}
