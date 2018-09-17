using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public interface IResourceWriter
    {
        bool WriteResource(IWriteArgs data);
    }
}
