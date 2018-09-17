using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public interface IResourceLoader
    {
        Object LoadResouce(string path);
    }
}