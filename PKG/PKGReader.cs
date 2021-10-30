using HadesUnpack_test.Entries;
using K4os.Compression.LZ4;
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
            #endregion

            #region Reading PKG File Header
            PKGFile.Header.CompressionAlg = reader.ReadByte();
            PKGFile.Header.Zero = reader.ReadBytes(2);
            PKGFile.Header.PKGVersion = reader.ReadByte();
            #endregion

            #region Reading PKG File
            PKGFile.CompressedFlag = reader.ReadByte();
            PKGFile.CompressedSize = reader.ReadInt32(ByteOrder.BigEndian);
            PKGFile.Compressed_Data = reader.ReadBytes(PKGFile.CompressedSize);

            if (PKGFile.CompressedFlag == 0)
            {
                
            }
            // Decompress The Chunk and Trim
            LZ4Codec.Decode(PKGFile.Compressed_Data, chunk);
            PKGFile.UnCompressed_Data = Utils.TrimEnd(chunk);
            #endregion


            PKGFile.Manifest = ParseManifest(filepath+"_manifest");
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

            Console.WriteLine("Reader Pos: "+ reader.BaseStream.Position);
            return pkgmanifest;
        }
    }
}
