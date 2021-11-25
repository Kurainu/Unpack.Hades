using Unpack.Hades.Entries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unpack.Hades.PKG
{
    public class PKGManifest
    {
        public int CompressionMethod { get; set; }
        public int PackageVersion { get; set; }
        public int Version { get; set; }
        public List<AtlasEntry> AtlasEntries { get; set; }
    }
}
