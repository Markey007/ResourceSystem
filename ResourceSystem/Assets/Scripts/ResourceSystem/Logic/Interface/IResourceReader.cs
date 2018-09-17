using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public interface IResourceReader
    {
        Object ReadResouce(string path);
    }
}
