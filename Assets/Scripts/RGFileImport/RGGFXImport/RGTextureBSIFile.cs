using System;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.RGFileImport.RGGFXImport
{
    public class RGTextureBSIFile
    {
        public const uint RedguardImageHeaderSize = 26;
        public List<RGTextureImage> Images { get; protected set; }
        static private RGPaletteFile.RGColor[] defaultPalette;

        public RGTextureBSIFile(RGPaletteFile.RGColor[] colors)
        {
            if (colors == null)
                defaultPalette = new RGPaletteFile.RGColor[256];
            else
                defaultPalette = colors;
        }

        public void LoadFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();
            using (var binaryReader = new BinaryReader(File.OpenRead(path)))
            {
                Images = new List<RGTextureImage>();
                while (!IsEOF(binaryReader))
                {
                    var image = ReadImage(binaryReader);
                    if (image != null)
                        Images.Add(image);
                }
            }
        }

        protected static RGTextureImage ReadImage(BinaryReader binaryReader)
        {
            const int imageNameLength = 9;
            var texNameBuffer = new char[imageNameLength + 1];
            if (binaryReader.Read(texNameBuffer, 0, imageNameLength) < imageNameLength)
                throw new EndOfStreamException("ReadImage: Couldn't read a full image name.");
            var rgTextureImage = new RGTextureImage();
            // Set Name
            rgTextureImage.Name = new string(texNameBuffer);
            if (IsEOF(binaryReader))
                return null;
            try
            {
                // Set Size
                rgTextureImage.Size = binaryReader.ReadUInt32();
            }
            catch (EndOfStreamException)
            {
                return null;
            }

            if (IsEOF(binaryReader))
                return null;
            rgTextureImage.Palette = defaultPalette;
            // Read subrecords
            const int recordNameLength = 4;
            bool reachedEnd = false;
            do
            {
                var recordNameBuffer = new char[recordNameLength];
                // Read record name
                binaryReader.Read(recordNameBuffer, 0, recordNameLength);
                // Read record size and convert from big to little endian
                var recordSize = ReverseBytes(binaryReader.ReadUInt32());
                var recordName = new string(recordNameBuffer);
                switch (recordName)
                {
                    case "END ":
                        reachedEnd = true;
                        break;
                    case "BSIF":
                        ReadBSIF(rgTextureImage, recordSize, binaryReader);
                        break;
                    case "IFHD":
                        ReadIFHD(rgTextureImage, recordSize, binaryReader);
                        break;
                    case "BHDR":
                        ReadBHDR(rgTextureImage, binaryReader);
                        break;
                    case "CMAP":
                        ReadCMAP(rgTextureImage, binaryReader);
                        break;
                    case "DATA":
                        if (rgTextureImage.HasIFHDData)
                            ReadAnimationData(rgTextureImage, recordSize, binaryReader);
                        else if (rgTextureImage.HasBSIFData)
                            ReadData(rgTextureImage, recordSize, binaryReader);
                        break;
                    default:
                        throw new InvalidDataException("Invalid image subrecord name.");
                }

                if (reachedEnd)
                    break;
            } while (!IsEOF(binaryReader));

            return rgTextureImage;
        }

        static protected bool IsEOF(BinaryReader binaryReader)
        {
            return binaryReader.BaseStream.Position == binaryReader.BaseStream.Length;
        }

        static protected uint ReverseBytes(uint n)
        {
            var bytes = BitConverter.GetBytes(n);
            Array.Reverse(bytes, 0, bytes.Length);
            return BitConverter.ToUInt32(bytes, 0);
        }

        static protected void ReadBSIF(RGTextureImage rgTextureImage, uint recordSize, BinaryReader binaryReader)
        {
            rgTextureImage.HasBSIFData = true;
            rgTextureImage.HasIFHDData = false;
            if (recordSize == 0)
                return;
            rgTextureImage.BSIFData = new byte[recordSize];
            if (binaryReader.Read(rgTextureImage.BSIFData, 0, (int)recordSize) != recordSize)
                throw new EndOfStreamException("Failed to read all specified bytes from BSIF data record.");
        }

        static protected void ReadIFHD(RGTextureImage rgTextureImage, uint recordSize, BinaryReader binaryReader)
        {
            rgTextureImage.HasBSIFData = false;
            rgTextureImage.HasIFHDData = true;
            rgTextureImage.IFHDData = new byte[recordSize];
            if (binaryReader.Read(rgTextureImage.IFHDData, 0, (int)recordSize) != recordSize)
                throw new EndOfStreamException("Failed to read all specified bytes from IFHD data record.");
        }

        static protected void ReadBHDR(RGTextureImage rgTextureImage, BinaryReader binaryReader)
        {
            rgTextureImage.Header = new RGImageHeader
            {
                XOffset = binaryReader.ReadUInt16(),
                YOffset = binaryReader.ReadUInt16(),
                Width = binaryReader.ReadUInt16(),
                Height = binaryReader.ReadUInt16(),
                Unknown1 = binaryReader.ReadByte(),
                Unknown2 = binaryReader.ReadByte(),
                Zero1 = binaryReader.ReadUInt16(),
                Zero2 = binaryReader.ReadUInt16(),
                FrameCount = binaryReader.ReadUInt16(),
                Unknown3 = binaryReader.ReadUInt16(),
                Zero3 = binaryReader.ReadUInt16(),
                Zero4 = binaryReader.ReadUInt16(),
                Unknown4 = binaryReader.ReadByte(),
                Unknown5 = binaryReader.ReadByte(),
                Unknown6 = binaryReader.ReadUInt16()
            };
        }

        static protected void ReadCMAP(RGTextureImage rgTextureImage, BinaryReader binaryReader)
        {
            const int paletteSize = 768;
            const int paletteColorCount = 256;
            var paletteBuffer = new byte[paletteSize];
            if (binaryReader.Read(paletteBuffer, 0, paletteSize) != paletteSize)
                throw new EndOfStreamException("Failed to read a full palette from CMAP record.");
            rgTextureImage.Palette = new RGPaletteFile.RGColor[paletteColorCount];
            for (int i = 0; i < paletteColorCount; i++)
            {
                rgTextureImage.Palette[i] = new RGPaletteFile.RGColor()
                {
                    r = paletteBuffer[i * 3],
                    g = paletteBuffer[i * 3 + 1],
                    b = paletteBuffer[i * 3 + 2]
                };
            }
        }

        static protected void ReadData(RGTextureImage rgTextureImage, uint recordSize, BinaryReader binaryReader)
        {
            if (rgTextureImage.Header.Width * rgTextureImage.Header.Height != recordSize)
                throw new InvalidDataException("Image size does not match size specified in header.");
            rgTextureImage.ImageData.Add(new byte[recordSize]);
            if (binaryReader.Read(rgTextureImage.ImageData[0], 0, (int)recordSize) != recordSize)
                throw new EndOfStreamException("Failed to read all specified data for image.");
        }

        static protected void ReadAnimationData(RGTextureImage rgTextureImage, uint recordSize, BinaryReader binaryReader)
        {
            var offsetDataSize = (uint)(rgTextureImage.Header.Height * rgTextureImage.Header.FrameCount);
            var offsetData = new uint[offsetDataSize];
            var startOffset = binaryReader.BaseStream.Position;
            if (startOffset <= 0)
                throw new EndOfStreamException("Failed to get position in file when reading animation data.");
            for (int i = 0; i < offsetDataSize; i++)
                offsetData[i] = binaryReader.ReadUInt32();
            for (int frame = 0; frame < rgTextureImage.Header.FrameCount; frame++)
            {
                rgTextureImage.ImageData.Add(new byte[rgTextureImage.Header.Width * rgTextureImage.Header.Height]);
                for (int y = 0; y < rgTextureImage.Header.Height; y++)
                {
                    var offsetIndex = rgTextureImage.Header.Height * frame + y;
                    var offset = offsetData[offsetIndex] + startOffset;
                    binaryReader.BaseStream.Seek(offset, SeekOrigin.Begin);
                    if (binaryReader.Read(rgTextureImage.ImageData[frame], y * rgTextureImage.Header.Width, rgTextureImage.Header.Width) != rgTextureImage.Header.Width)
                        throw new EndOfStreamException("Failed to read all data for animated image frame.");
                }
            }

            binaryReader.BaseStream.Seek(startOffset + recordSize, SeekOrigin.Begin);
        }
    }
}
