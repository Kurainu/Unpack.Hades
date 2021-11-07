using HadesUnpack_test.Entries;
using K4os.Compression.LZ4;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.PKG
{
    class PKGReader
    {
        public PKGFile Parse(string filepath)
        {
            BinaryReader reader = new(File.OpenRead(filepath));

            #region Initialization
            byte[] chunk = new byte[0x2000000];

            PKGFile PKGFile = new PKGFile();
            PKGFile.Header = new PKGHeader();
            PKGFile.TexturesEntries = new List<TextureEntry>();
            #endregion

            #region Reading PKG File Header
            PKGFile.Header.CompressionAlg = reader.ReadByte();
            PKGFile.Header.Zero = reader.ReadBytes(2);
            PKGFile.Header.PKGVersion = reader.ReadByte();
            #endregion

            #region reading PKG Manifest
            PKGFile.Manifest = ParseManifest(filepath+"_manifest");
            #endregion

            #region Reading PKG File and Decompressing
            PKGFile.CompressedFlag = reader.ReadByte();
            PKGFile.CompressedSize = reader.ReadInt32(ByteOrder.BigEndian);
            byte[] Compressed_Data = reader.ReadBytes(PKGFile.CompressedSize);

            // Decompress The Chunk and Trim it
            LZ4Codec.Decode(Compressed_Data, chunk);
            byte[] UnCompressed_Data = Utils.TrimEnd(chunk);
            #endregion

            #region Read Decompressed File
            // Replace the reader with Uncompressed Data
            reader = new BinaryReader(new MemoryStream(UnCompressed_Data));

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                // Reading Entry Type
                int EntryType = reader.ReadByte();

                //reading Entry 0xAD and 0xAA and converting it to a DDsFile
                if (EntryType == 0xAD || EntryType == 0xAA)
                {
                    PKGFile.TexturesEntries.Add(TextureEntry.ReadEntry(reader));
                }
            }
            #endregion
            return PKGFile;
        }

        private PKGManifest ParseManifest(string manifestpath)
        {
            PKGManifest pkgmanifest = new PKGManifest();
            pkgmanifest.AtlasEntries = new List<AtlasEntry>();
            BinaryReader reader = new(File.OpenRead(manifestpath));

            #region Read Header
            pkgmanifest.CompressionMethod = reader.ReadByte();
            reader.ReadBytes(2); // ignoring 2 Bytes
            pkgmanifest.PackageVersion = reader.ReadByte();
            #endregion

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int EntryType = reader.ReadByte();
                if (EntryType == 0xDE)
                {
                    pkgmanifest.AtlasEntries.Add(AtlasEntry.GetEntry(reader));
                };
            }
            return pkgmanifest;
        }
    }
}
