using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML
{
    class FileExtension
    {
        public static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string FilePath = Path.Combine(BasePath, "Files");
    }
}
