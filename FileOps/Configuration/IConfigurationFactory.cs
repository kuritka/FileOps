using FileOps.Configuration.Entities;
using System.Collections.Generic;
using System.IO;

namespace FileOps.Configuration
{
    internal interface IConfigurationFactory 
    {
        Settings Get(IEnumerable<FileInfo> file);
    }
}
