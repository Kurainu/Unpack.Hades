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
        public PKGFile Parse(BinaryReader reader)
        {
            #region Initialization
            byte[] chunk = new byte[0x2000000];

            PKGFile PKGFile = new PKGFile();
            PKGFile.Header = new PKGHeader();
            #endregion

            #region Reading Header
            PKGFile.Header.CompressionAlg = reader.ReadByte();
            PKGFile.Header.Zero = reader.ReadBytes(2);
            PKGFile.Header.PKGVersion = reader.ReadByte();
            #endregion

            PKGFile.CompressedFlag = reader.ReadByte();
            PKGFile.CompressedSize = reader.ReadInt32(ByteOrder.BigEndian);
            PKGFile.Compressed_Data = reader.ReadBytes(PKGFile.CompressedSize);

            if (PKGFile.CompressedFlag == 0)
            {
                
            }

            // Decompress The Chunk and Trim
            LZ4Codec.Decode(PKGFile.Compressed_Data, chunk);
            PKGFile.UnCompressed_Data = Utils.TrimEnd(chunk);

            return PKGFile;
        }



    }
}
