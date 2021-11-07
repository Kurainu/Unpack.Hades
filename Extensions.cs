using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test
{
    public static class Extensions
    {
        public static int ReadInt32(this BinaryReader reader, ByteOrder ByteOrder)
        {
            byte[] BytesRead = reader.ReadBytes(4);

            if (ByteOrder == ByteOrder.BigEndian && BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(BytesRead.Reverse().ToArray());
            }
            else
            {
                return BitConverter.ToInt32(BytesRead);
            }
        }
        public static int ReadSingle(this BinaryReader reader, ByteOrder ByteOrder)
        {
            byte[] BytesRead = reader.ReadBytes(4);

            if (ByteOrder == ByteOrder.BigEndian && BitConverter.IsLittleEndian)
            {
                return BitConverter.ToInt32(BytesRead.Reverse().ToArray());
            }
            else
            {
                return BitConverter.ToInt32(BytesRead);
            }
        }

        public static byte[] ReadBytes(this BinaryReader reader, int Size,ByteOrder ByteOrder)
        {
            byte[] BytesRead = reader.ReadBytes(Size);

            if (ByteOrder == ByteOrder.BigEndian && BitConverter.IsLittleEndian)
            {
                return BytesRead.Reverse().ToArray();
            }
            else
            {
                return BytesRead;
            }
        }

    }
}
