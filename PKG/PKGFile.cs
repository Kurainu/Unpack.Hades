using K4os.Compression.LZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.PKG
{
    class PKGFile
    {
        public PKGHeader Header { get; set; }
        public int CompressedFlag { get; set; }
        public int CompressedSize { get; set; }
        public byte[] Compressed_Data { get; set; }
        public byte[] UnCompressed_Data { get; set; }

    }
}
