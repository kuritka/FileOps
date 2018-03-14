using System.IO;

namespace FileOps.Configuration
{
    public interface IConfigurationFactory
    {
        T Get<T>(FileInfo file);
    }
}
