using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public class AssetBundleWriteArgs : IWriteArgs
    {
        public string Path { get; set; }

        public byte[] Bytes { get; set; }
    }
}
