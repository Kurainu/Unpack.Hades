﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesUnpack_test.PKG
{
    class PKGHeader
    {
        public byte CompressionAlg { get; set; }
        public byte[] Zero  { get; set; }
        public byte PKGVersion  { get; set; }
    }
}
