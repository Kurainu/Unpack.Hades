using BCnEncoder.Decoder;
using BCnEncoder.Encoder;
using BCnEncoder.ImageSharp;
using BCnEncoder.Shared;
using BCnEncoder.Shared.ImageFiles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unpack.Hades.Entries
{
    public class TextureEntry
    {
        public string TextureName { get; set; }
        public int Imageformat { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MipLevel { get; set; }
        public byte[] ImageData { get; set; }

        public static TextureEntry ReadEntry(BinaryReader reader)
        {
            TextureEntry texture = new TextureEntry();

            texture.TextureName =  reader.ReadString();

            //Skipping 4 Bytes
            reader.ReadBytes(4);

            string Magic = Encoding.UTF8.GetString(reader.ReadBytes(4));

            if (Magic != "XNBw")
            {
                throw new Exception("Incorrect Header Found. Expected Header was XNBw");
            }

            int XnbVer =  reader.ReadByte();
            if (XnbVer != 5 && XnbVer != 6 )
            {
                throw new Exception("Unknown XNB Version");
            }

            int flags = reader.ReadByte();
            if (flags != 0x0)
            {
                throw new Exception("Cannot export compressed XNB data.");
            }

            //Skipping 4 Bytes
            reader.ReadBytes(4);

            //Ignoring XNB version 5 NonSense
            if (XnbVer == 5)
            {
                int num = reader.Read7BitEncodedInt();
                for (int i = 0; i < num; i++)
                {
                    reader.ReadString();
                    reader.ReadInt32();
                }
                reader.Read7BitEncodedInt();
                reader.Read7BitEncodedInt();
            }

            texture.Imageformat = reader.ReadInt32();
            texture.Width = reader.ReadInt32();
            texture.Height = reader.ReadInt32();
            texture.MipLevel = reader.ReadInt32();
            texture.ImageData = reader.ReadBytes(reader.ReadInt32());

            return texture;
        }

        public DdsFile ConvertToDDS()
        {
            BcDecoder decoder = new();
            BcEncoder encoder = new();

            switch (Imageformat)
            {
                case 28:
                    //Decoding with Bc7 Compression und encoding it.
                    return encoder.EncodeToDds(decoder.DecodeRawToImageRgba32(ImageData, Width, Height, CompressionFormat.Bc7));
                case 6:
                    //Decoding with Bc3 Compression und encoding it.
                    return encoder.EncodeToDds(decoder.DecodeRawToImageRgba32(ImageData, Width, Height, CompressionFormat.Bc3));
                case 0:
                    // No Compression so for it can encode directly to dds without Decoding First
                    return encoder.EncodeToDds(ImageData, Width, Height, PixelFormat.Bgra32);
                default:
                    return null;
            }
        }

        public void Save(string path,DdsFile ddsfile)
        {
            ddsfile.Write(new FileStream(path, FileMode.CreateNew));
        }
    }
}
