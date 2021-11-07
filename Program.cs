using System;
using System.IO;
using System.Linq;
using K4os.Compression.LZ4;
using HadesUnpack_test.PKG;

namespace HadesUnpack_test
{
    class Program
    {
        static void Main(string[] args)
        {
            PKGReader pkgreader = new PKGReader();

            var Files = Directory.GetFiles(@"E:\Hades\Content\Win\Packages\","*.pkg");

            foreach (var item in Files)
            {
                Console.WriteLine(item);
                PKGFile pkgfile = pkgreader.Parse(@"E:\Hades\Content\Win\Packages\Erebus.pkg");
            }
        }
    }
}
