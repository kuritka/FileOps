using FileOps.Pipe;
using System.Collections.Generic;
using System.IO;

namespace FileOps
{
    public interface IFileOpsBuilder
    {
        LinkedList<IStep<IAggregate, IAggregate>> Build();

        IFileOpsBuilder AddConfiguration(FileInfo jsonFile);
    }
}
