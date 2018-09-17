using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceSystem
{
    public class ExcelWriteArgs : IWriteArgs
    {
        public string Path { get; set; }

        public string ExcelName { get; set; }

        public string SheetName { get; set; }

        public string[,] Content { get; set; } 

    }
}
