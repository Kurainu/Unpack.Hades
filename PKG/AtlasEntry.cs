using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.PKG
{
    class AtlasEntry
    {
        public int Size { get; set; }
        public int Unknown { get; set; }
        public int Version { get; set; }
        public List<SubAtlas> SubAtlases { get; set; }
        public string ReferenceTextureName { get; set; }
    }
}
