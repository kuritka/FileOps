using System.Collections.Generic;
using System.IO;

namespace FileOps.Configuration
{
    internal interface IConfigurationFactory
    {
        T Get<T>(IEnumerable<FileInfo> file);
    }
}
