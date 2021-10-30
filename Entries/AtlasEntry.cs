using HadesUnpack_test.Entries;
using HadesUnpack_test.PKG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.Entries
{
    class AtlasEntry
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int Version { get; set; }
        public List<SubAtlas> SubAtlases { get; set; }
        public string ReferenceTextureName { get; set; }
        public bool isReference { get; set; }

        public static AtlasEntry GetEntry(BinaryReader reader)
        {
            List<AtlasEntry> atlasEntries = new List<AtlasEntry>();
            AtlasEntry atlasEntry = new AtlasEntry();

            atlasEntry.Size = reader.ReadInt32(ByteOrder.BigEndian);
            if (reader.ReadInt32(ByteOrder.BigEndian) == 2142336875)
            {
                atlasEntry.SubAtlases = new List<SubAtlas>();
                atlasEntry.Version = reader.ReadInt32(ByteOrder.BigEndian);
                int numSubAtlases = reader.ReadInt32(ByteOrder.BigEndian);

                for (int i = 0; i < numSubAtlases; i++)
                {
                    SubAtlas subAtlas = new SubAtlas();
                    subAtlas.Name =  reader.ReadString();
                    subAtlas.Rect = new Rectangle() { X = reader.ReadInt32(ByteOrder.BigEndian), Y = reader.ReadInt32(ByteOrder.BigEndian), Width = reader.ReadInt32(ByteOrder.BigEndian), Height = reader.ReadInt32(ByteOrder.BigEndian) };
                    subAtlas.TopLeft = new Point() { X = reader.ReadInt32(ByteOrder.BigEndian), Y = reader.ReadInt32(ByteOrder.BigEndian) };
                    subAtlas.OriginalSize = new Point() { X = reader.ReadInt32(ByteOrder.BigEndian), Y = reader.ReadInt32(ByteOrder.BigEndian) };
                    subAtlas.ScaleRatio = new ScaleRatio() { X = reader.ReadSingle(ByteOrder.BigEndian), Y = reader.ReadSingle(ByteOrder.BigEndian) };

                    if (atlasEntry.Version > 0)
                    {
                        subAtlas.Flags = reader.ReadByte();
                        if (atlasEntry.Version > 1)
                        {
                            subAtlas.IsMulti = (subAtlas.Flags & 1) != 0;
                            subAtlas.IsMip = (subAtlas.Flags & 2) != 0;
                        }

                        if (atlasEntry.Version > 3)
                        {
                            subAtlas.IsAlpha8 = (subAtlas.Flags & 4) != 0;
                        }
                    }
                    subAtlas.Hullpoints = new List<Point>();

                    if (atlasEntry.Version > 2)
                    {
                        Console.WriteLine(reader.BaseStream.Position);
                        int hullcount = reader.ReadInt32(ByteOrder.BigEndian);

                        for (int j = 0; j < hullcount; j++)
                        {
                            subAtlas.Hullpoints.Add(new Point() {X =  reader.ReadInt32(ByteOrder.BigEndian), Y = reader.ReadInt32(ByteOrder.BigEndian) });
                        }
                    }
                    atlasEntry.SubAtlases.Add(subAtlas);
                }

                if (reader.ReadByte() == 221 || true)
                {
                    atlasEntry.isReference = true;
                    atlasEntry.ReferenceTextureName = reader.ReadString();
                    atlasEntry.Name = atlasEntry.ReferenceTextureName;
                }
                else
                {
                    //TODO
                    throw new NotImplementedException();
                }

            }
            return atlasEntry;
        }
    }
}
