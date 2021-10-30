using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.PKG
{
    class SubAtlas
    {
        public string Name { get; set; }
        public Rectangle Rect { get; set; }
        public Point TopLeft { get; set; }
        public Point OriginalSize { get; set; }
        public ScaleRatio ScaleRatio { get; set; }
        public int Flags { get; set; }
        public bool IsMulti { get; set; }
        public bool IsMip { get; set; }
        public bool IsAlpha8 { get; set; }
        public List<Point> Hullpoints { get; set; }
    }
}
