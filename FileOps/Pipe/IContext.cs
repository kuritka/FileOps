using System.Collections.Generic;
using System.IO;

namespace FileOps.Pipe
{
    public interface IStepContext 
    {
        IEnumerable<FileInfo> Files { get; }
    }
}
