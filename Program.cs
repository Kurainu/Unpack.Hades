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

            PKGFile pkgfile = pkgreader.Parse(new BinaryReader(File.OpenRead(@"E:\Hades\Content\Win\Packages\Launch.pkg")));

            Console.WriteLine(pkgfile);
        }
    }
}
