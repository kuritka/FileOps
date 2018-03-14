using System.IO;

namespace FileOps.Configuration
{
    internal interface IConfigurationFactory
    {
        T Get<T>(FileInfo file);
    }
}
